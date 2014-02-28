using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Xml.Serialization;
using BO.Xml;
using System.Xml.Schema;

namespace BO.Reports
{
    public class ReportDocument : PrintDocument
    {
        public const string HEADER = "Header";
        public const string FOOTER = "Footer";
        public const string PAGENUMBER = "PageNumber";
        public const string ROWNUMBER = "RowNumber";
        public const string BARCODEKEYSTRING = "BARCODE:";
        static ReportDocument()
        {
            xc = new XmlConverter();
            xc.AddSchema(BO.Common.GetReportsInfoSchema());
        }
        public class repPage
        {
            public reportInfo Report;
            public int Count;
            public bool IsPrint;

            public repPage(reportInfo ri, int count, bool isprint)
            {
                Report = ri;
                Count = count;
                IsPrint = isprint;
            }
        }

        static XmlConverter xc;

        //List<XmlDocument> pages = new List<XmlDocument>();
        List<repPage> repPages = new List<repPage>();
        List<reportInfo> range = new List<reportInfo>();
        int CurrentPage = 0;
        int CurrentCount = 0;
        public string Title = "Предварительный просмотр";

        protected string errors;

        string image_dir = null;
        public ReportDocument()
        {
            errors = "";
        }
        public int CountPages
        {
            get
            {
                if (repPages == null)
                    return 0;
                else
                    return repPages.Count;
            }
        }

        public itemInfo[] AppendGroups(itemInfo[] items, object dat)
        {
            List<itemInfo> new_items = new List<itemInfo>(items);
            Dictionary<string, ReportGroup> groups = new Dictionary<string, ReportGroup>();
            foreach (itemInfo item in new_items)
            {
                foreach (KeyValuePair<string, ReportGroup> group in groups)
                {
                    if (item.group != FOOTER && item.group != HEADER && (Common.IsNullOrEmpty(item.group) || !groups.ContainsKey(item.group)))
                        group.Value.RestItems.Add(item);
                }
                if (!Common.IsNullOrEmpty(item.group) /*&& dat is IDatInfo*/ && item.group != FOOTER && item.group != HEADER)
                {
                    try
                    {
                        if (!groups.ContainsKey(item.group))
                        {
                            object vals = ProxyInfo.GetPropValue(dat, item.group);
                            List<object> lst = new List<object>();
                            if (vals is BaseSet)
                                lst.AddRange(((BaseSet)vals).ToArray());
                            else if (vals is Array)
                                lst.AddRange(vals as object[]);
                            else
                                lst.Add(vals);
                            groups.Add(item.group, new ReportGroup(item.group, new_items, lst));
                        }
                        groups[item.group].TemplateItems.Add(item);
                    }
                    catch (Exception exp)
                    {
                        string str = exp.Message;
                    }
                }
            }
            foreach (KeyValuePair<string, ReportGroup> group in groups)
            {
                group.Value.Fill();
            }
            return new_items.ToArray();
        }

        class ReportGroup
        {
            string Name = "";
            List<itemInfo> Parent = null;
            List<object> Values = null;
            public List<itemInfo> TemplateItems = null;
            public List<itemInfo> RestItems = null;
            public ReportGroup(string name, List<itemInfo> parent, List<object> values)
            {
                Name = name;
                Parent = parent;
                Values = values;
                TemplateItems = new List<itemInfo>();
                RestItems = new List<itemInfo>();
            }
            public void Fill()
            {
                if (Values == null)
                    return;
                int counter = 1;
                float offset = 0;
                float delta = TemplateItems[TemplateItems.Count - 1].top + TemplateItems[TemplateItems.Count - 1].height - TemplateItems[0].top;
                int index = Parent.IndexOf(TemplateItems[0]);
                foreach (object val in Values)
                {
                    if (val == null)
                        continue;
                    List<itemInfo> tmpls = new List<itemInfo>();
                    foreach (itemInfo tmpl in TemplateItems)
                    {
                        itemInfo new_tmpl = tmpl.Clone() as itemInfo;
                        new_tmpl.top += offset;
                        tmpls.Add(new_tmpl);
                    }
                    Parent.InsertRange(index, tmpls);
                    foreach (itemInfo tmpl in tmpls)
                    {
                        if (tmpl.Value != null && tmpl.Value.StartsWith("[#" + Name + "."))
                        {
                            if (tmpl.Value.StartsWith("[#" + Name + "." + ROWNUMBER))
                                tmpl.Value = counter.ToString();
                            else
                            {
                                string propname = Common.CreateFormatString(tmpl.Value.Replace(Name + ".", ""));
                                tmpl.Value = String.Format(Common.Formatter, propname, val);
                            }
                        }

                        //{
                        //    string[] pars = propname.Replace(Name + ".", "").Split(':');
                        //    object oval = null;
                        //    if (val is IPropValue)
                        //        oval = ((IPropValue)val).GetValue(pars[0]);
                        //    else
                        //        oval = ExtraRepDataInfo.GetValue(val, pars[0]);

                        //    string frmt = "";
                        //    for (int i = 1; i < pars.Length; i++)
                        //    {
                        //        frmt += ":" + pars[i];
                        //    }
                        //    if (oval == null)
                        //        tmpl.Value = "!" + pars[0] + "!";
                        //    else
                        //        tmpl.Value = (frmt == "") ? oval.ToString() : string.Format(Common.Formatter, @"{0" + frmt + @"}", oval);
                        //}
                    }
                    offset += delta;
                    counter++;
                }
                offset -= delta;
                Parent.RemoveRange(Parent.IndexOf(TemplateItems[0]), TemplateItems.Count);
                foreach (itemInfo rest in RestItems)
                {
                    rest.top += offset;
                }
            }
        }

        public void AppendReportInfo(reportInfo ri, string caption, int count, IFormattable dat, Dictionary<string, Image> imagelist)
        {
            Array.Sort<itemInfo>(ri.Items, RepItemComparer);
            ri.Items = AppendGroups(ri.Items, dat);
            if (Common.IsNullOrEmpty(ri.name) || !Common.IsNullOrEmpty(caption))
            {
                try
                {
                    string frmt = Common.CreateFormatString(caption);
                    caption = String.Format(Common.Formatter, frmt, dat);
                }
                catch { }
                ri.name = caption;
            }
            foreach (itemInfo item in ri.Items)
            {
                string value = item.Value == null ? "" : item.Value.Trim();

                try
                {
                    if (item is imgInfo)
                    {
                        if (value.Contains(":BARCODE"))
                        {
                            value = value.Replace("[#", "").Replace("#]", "").Trim();
                            int i = value.IndexOf(":BARCODE");
                            string mask = (value.Length > i + 8) ? value.Substring(i + 9) : "";
                            int height = Convert.ToInt32(item.height * 10);
                            string field = value.Substring(0, i);

                            string val = "";
                            val = dat.ToString(field, null);

                            if (mask != "") val = mask.Replace("%", val);
                            //new code
                            value = BARCODEKEYSTRING + val;
                            item.Value = value;
                        }
                        else
                        {
                            imgInfo ii = item as imgInfo;

                            ii.Value = "";
                            if (!Common.IsNullOrEmpty(ii.src))
                            {
                                string imagename = ii.src.Trim();
                                if (imagelist != null && imagelist.ContainsKey(imagename))
                                {
                                    using (MemoryStream str = new MemoryStream())
                                    {
                                        Image image = imagelist[imagename];
                                        image.Save(str, ImageFormat.Jpeg);
                                        ii.Value = Convert.ToBase64String(str.ToArray());
                                        ii.src = "";
                                    }
                                }
                                else if (!Common.IsNullOrEmpty(image_dir))
                                {
                                    string imagefile = Path.Combine(image_dir, imagename);
                                    if (File.Exists(imagefile))
                                    {
                                        MemoryStream str = new MemoryStream();
                                        Image.FromFile(imagefile).Save(str, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        ii.Value = Convert.ToBase64String(str.ToArray());
                                        ii.src = "";
                                    }
                                }
                            }
                        }
                    }
                    else if (item is checkInfo && value.Contains("=="))
                    {
                        string prop = value.Substring(0, value.IndexOf("==")).Trim();
                        string cond = value.Substring(value.IndexOf("==") + 2).Trim().ToUpper();
                        bool enumval = false;

                        string propval = dat.ToString(prop, null).Trim().ToUpper();
                        //object val = ExtraRepDataInfo.GetValue(dat, prop);
                        //string propval = (val == null) ? "" : val.ToString().Trim().ToUpper();

                        // Заплатка! Для того, чтобы в отчетах значения enum 
                        // можно было использовать как с пробелами, так и без оных.
                        // Андрей 8.8.2007
                        string valnospaces = propval.Replace(" ", "");
                        // Конец заплатки. Ниже значение используется в теле цикла
                        foreach (string s in cond.Split('|'))
                        {
                            enumval |= ((propval == s) || (valnospaces == s));
                        }
                        item.Value = enumval.ToString();
                    }
                    else if (!value.Contains(PAGENUMBER) && (value.Contains("[#") || value.Contains("#]")))
                    {
                        //string frmt = value.Replace("{", @"{{").Replace("}", @"}}").Replace("[#", "{0:").Replace("#]", "}");
                        string frmt = Common.CreateFormatString(value);
                        item.Value = String.Format(Common.Formatter, frmt, dat);
                    }
                }
                catch (Exception ex)
                {
                    errors += string.Format("Ошибка в шаблоне - поле {0}, значение={1}, [{2}]\n ", item.GetType(), item.Value, ex.Message);
                }
            }
            repPages.AddRange(CreatePages(ri, caption, count));
            DefaultPageSettings.Landscape = IsLandscape();
        }

        List<repPage> CreatePages(reportInfo ri, string caption, int count)
        {
            List<repPage> ret = new List<repPage>();
            List<itemInfo> lst = new List<itemInfo>();
            Array.Sort<itemInfo>(ri.Items, RepItemComparer);
            itemInfo[] footer = Array.FindAll<itemInfo>(ri.Items, delegate(itemInfo i) { return !Common.IsNullOrEmpty(i.group) && i.group == FOOTER; });
            itemInfo[] header = Array.FindAll<itemInfo>(ri.Items, delegate(itemInfo i) { return !Common.IsNullOrEmpty(i.group) && i.group == HEADER; });
            if (footer.Length == 0)
            {
                ret.Add(new repPage(ri, count, true));
                return ret;
            }
            float offset = 0;
            if (Common.IsNullOrEmpty(caption))
                caption = ri.name;
            foreach (itemInfo item in ri.Items)
            {
                if (Array.IndexOf(footer, item) >= 0/* || Array.IndexOf(header, item) >= 0*/)
                    continue;
                if (Array.IndexOf(header, item) >= 0)
                    lst.AddRange(CloneBlock(header, ret.Count + 1));
                else
                {
                    if (item.top + item.height - offset > footer[0].top)
                    {
                        lst.AddRange(CloneBlock(footer, ret.Count + 1));
                        ret.Add(new repPage(new reportInfo(string.Format("{0} (стр.{1})", caption, ret.Count + 1)), count, true));
                        ret[ret.Count - 1].Report.Items = lst.ToArray();
                        lst = new List<itemInfo>();
                        offset = item.top;
                        if (header.Length > 0)
                        {
                            offset -= header[0].top + header[header.Length - 1].top + header[header.Length - 1].height;
                            lst.AddRange(CloneBlock(header, ret.Count + 1));
                        }
                    }
                    item.top -= offset;
                    lst.Add(item);
                }
            }
            if (lst.Count > 0)
            {
                lst.AddRange(CloneBlock(footer, ret.Count + 1));
                ret.Add(new repPage(new reportInfo(string.Format("{0} (стр.{1})", caption, ret.Count + 1)), count, true));
                ret[ret.Count - 1].Report.Items = lst.ToArray();
            }
            return ret;
        }

        List<itemInfo> CloneBlock(itemInfo[] items, int pagenum)
        {
            List<itemInfo> ret = new List<itemInfo>();
            foreach (itemInfo item in items)
            {
                itemInfo new_item = (itemInfo)item.Clone();
                //new_item.top += offset;
                if (!Common.IsNullOrEmpty(new_item.Value))
                    new_item.Value = new_item.Value.Replace("[#" + PAGENUMBER + "#]", pagenum.ToString());
                ret.Add(new_item);
            }
            return ret;
        }


        public static int RepItemComparer(itemInfo inf1, itemInfo inf2)
        {
            if (inf1 is imgInfo && inf2 is textInfo)
                return 1;
            else if (inf2 is imgInfo && inf1 is textInfo)
                return -1;
            else if (inf1 is checkInfo && inf2 is textInfo)
                return 1;
            else if (inf2 is checkInfo && inf1 is textInfo)
                return -1;
            else if (inf1.top > inf2.top)
                return 1;
            else if (inf1.top < inf2.top)
                return -1;
            else if (inf1.left > inf2.left)
                return 1;
            else if (inf1.left < inf2.left)
                return -1;
            else
                return 0;
        }

        public void AppendReportInfo(XmlDocument xml, string caption, int count, IFormattable dat, Dictionary<string, Image> imagelist)
        {
            try
            {
                reportInfo ri = xc.Deserialize(typeof(reportInfo), xml.DocumentElement.OuterXml) as reportInfo;
                AppendReportInfo(ri, caption, count, dat, imagelist);

            }
            catch (Exception ex)
            {
                errors += string.Format("Ошибка в шаблоне {0}, [{2}]\n ", caption, ex.Message);
            }
        }
        public void AppendReportInfo(string filename, string caption, string tooltip, int count, IFormattable dat, Dictionary<string, Image> imagelist)
        {
            image_dir = "";
            if (File.Exists(filename))
            {
                try
                {
                    image_dir = Path.GetDirectoryName(filename);
                    reportInfo ri = xc.Read<reportInfo>(filename);
                    ri.Tootip = tooltip;
                    AppendReportInfo(ri, caption, count, dat, imagelist);
                }
                catch (Exception ex)
                {
                    errors += string.Format("Ошибка в файле {0},\nшаблоне {1},\n [{2}]\n ", filename, caption, Common.ExMessage(ex));
                }
            }
            else
                errors += string.Format("Файл с шаблоном отчета {0} не найден\n", filename);

            if (errors != "")
            {
                reportInfo ri = new reportInfo();
                if (Common.IsNullOrEmpty(caption))
                    caption = filename.Substring(filename.LastIndexOf(@"\")).Replace(@"\", "");
                textInfo ii = new textInfo();
                ii.width = 20;
                ii.height = 40;
                ii.Value = errors;
                ri.Items = new itemInfo[] { ii };
                AppendReportInfo(ri, caption, count, dat, imagelist);
                errors = "";
            }
        }

        public bool IsLandscape()
        {
            float max_right = 0;
            foreach (repPage page in repPages)
            {
                foreach (itemInfo item in page.Report.Items)
                {
                    float left = item.left * 10;
                    float width = item.width * 10;
                    if (left + width > max_right)
                    {
                        textInfo ti = item as textInfo;
                        if (ti == null
                            || ti.border == null
                            || ti.border[1] != 0
                            || ti.border[2] != 0
                            || ti.border[3] != 0
                            || !Common.IsNullOrEmpty(ti.Value)
                            )
                            max_right = left + width;
                    }
                }
            }
            return (max_right > 200);
        }

        string CreateGraphicsRI(Graphics g, reportInfo report)
        {
            if (repPages.Count == 0) return "Формат документа не задан !";
            string error = "";
            int counter = 0;
            PointF offset = new PointF(0, 0);
            foreach (itemInfo item in report.Items)
            {
                counter++;
                try
                {
                    float left = item.left * 10;
                    float top = item.top * 10;
                    float width = item.width * 10;
                    float height = item.height * 10;
                    RectangleF rect = new RectangleF(left, top, width, height);
                    string value = item.Value;
                    StringFormat frmt = new StringFormat();
                    frmt.Trimming = StringTrimming.None;

                    Color foreColor = Color.Black;
                    Color backColor = Color.White;
                    if (item is imgInfo)
                    {
                        imgInfo ii = item as imgInfo;

                        foreColor = GetColor(ii.forecolor, Color.Black);
                        Brush fore_brush = new SolidBrush(foreColor);

                        backColor = GetColor(ii.backcolor, Color.White);
                        Brush back_brush = new SolidBrush(backColor);
                        if (backColor != Color.White) g.FillRectangle(back_brush, rect);

                        string font_name = Common.IsNullOrEmpty(ii.fontname) ? "Arial" : ii.fontname;
                        float font_size = ii.fontsize;
                        if (font_size == 0) font_size = 9;

                        FontStyle style = FontStyle.Regular;
                        if (ii.fontbold) style |= FontStyle.Bold;
                        if (ii.fontitalic) style |= FontStyle.Italic;
                        if (ii.fontunderline) style |= FontStyle.Underline;

                        Font font = new Font(font_name, font_size, style, GraphicsUnit.Point);

                        frmt.Alignment = GetStringAlignment(ii.align);
                        if (!Common.IsNullOrEmpty(value))
                            try
                            {
                                RectangleF new_rect = new RectangleF(rect.Location, rect.Size);
                                if (value.StartsWith(BARCODEKEYSTRING))
                                {
                                    string barcode = value.Substring(BARCODEKEYSTRING.Length);
                                    BarCode.DrawBarCodeFromString(barcode, g, new_rect);
                                }
                                else
                                {
                                    Image img = Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
                                    if (ii.valign == valignType.Center)
                                        new_rect.Offset(0, rect.Height / 2 - img.Height);
                                    else if (ii.valign == valignType.Bottom)
                                        new_rect.Offset(0, rect.Height - img.Height);

                                    g.DrawImage(img, new_rect);
                                }
                            }
                            catch (Exception exp)
                            {
                                string exp_str = exp.Message;
                            }
                    }
                    else if (item is textInfo)
                    {
                        textInfo ti = item as textInfo;

                        foreColor = GetColor(ti.forecolor, Color.Black);
                        backColor = GetColor(ti.backcolor, Color.White);
                        Brush fore_brush = new SolidBrush(foreColor);
                        Brush back_brush = new SolidBrush(backColor);
                        if (backColor != Color.White) g.FillRectangle(back_brush, rect);

                        string font_name = Common.IsNullOrEmpty(ti.fontname) ? "Arial" : ti.fontname;
                        float font_size = ti.fontsize;
                        if (font_size == 0) font_size = 9;

                        FontStyle style = FontStyle.Regular;
                        if (ti.fontbold) style |= FontStyle.Bold;
                        if (ti.fontitalic) style |= FontStyle.Italic;
                        if (ti.fontunderline) style |= FontStyle.Underline;

                        Font font = new Font(font_name, font_size, style, GraphicsUnit.Point);

                        if (ti.nowrap)
                            frmt.FormatFlags = StringFormatFlags.NoWrap;
                        frmt.Alignment = GetStringAlignment(ti.align);

                        if (ti.bordercolor != null)
                        {
                            Color bclrLeft = GetColor(ti.bordercolor[0], Color.Black);
                            Color bclrTop = GetColor(ti.bordercolor[1], Color.Black);
                            Color bclrRight = GetColor(ti.bordercolor[2], Color.Black);
                            Color bclrBottom = GetColor(ti.bordercolor[3], Color.Black);
                            float bwidthLeft = ti.border[0];
                            float bwidthTop = ti.border[1];
                            float bwidthRight = ti.border[2];
                            float bwidthBottom = ti.border[3];

                            if (bwidthLeft != 0) g.DrawLine(GetPen(bclrLeft, bwidthLeft), rect.X, rect.Y, rect.X, rect.Y + rect.Height);
                            if (bwidthTop != 0) g.DrawLine(GetPen(bclrTop, bwidthTop), rect.X, rect.Y, rect.X + rect.Width, rect.Y);
                            if (bwidthRight != 0) g.DrawLine(GetPen(bclrRight, bwidthRight), rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
                            if (bwidthBottom != 0) g.DrawLine(GetPen(bclrBottom, bwidthBottom), rect.X, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);
                        }
                        frmt.LineAlignment = GetStringAlignment(ti.valign);

                        SizeF textsize = g.MeasureString(value, font);

                        g.DrawString(value, font, fore_brush, rect, frmt);
                    }
                    else if (item is checkInfo)
                    {
                        checkInfo ci = item as checkInfo;

                        RectangleF ch = new RectangleF(rect.X + 1.3f, rect.Y + (rect.Height - 2.6f) / 2, 2.6f, 2.6f);
                        Brush fore_brush = new SolidBrush(Color.Black);
                        Brush back_brush = new SolidBrush(Color.White);
                        g.FillRectangle(back_brush, ch.X, ch.Y, ch.Width, ch.Height);
                        g.DrawRectangle(new Pen(fore_brush, 0.2f), ch.X, ch.Y, ch.Width, ch.Height);
                        if (value == "1" || value.ToUpper() == "TRUE")
                            g.FillRectangle(fore_brush, ch.X + 0.5f, ch.Y + 0.5f, 1.6f, 1.6f);
                    }
                }
                catch (Exception exp)
                {
                    error += "(строка " + counter.ToString() + "): " + exp.Message + "\n" + ((exp.InnerException == null) ? "" : exp.InnerException.Message) + ";\n";
                }
            }
            return error;
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);
            if (e.PrintAction != PrintAction.PrintToPreview)
            {
                range.Clear();
                foreach (repPage page in repPages)
                {
                    int count = page.IsPrint ? ((CurrentCount > 0 /*&& page.Count > 0*/) ? CurrentCount : page.Count) : 0;
                    for (int i = 0; i < count; i++)
                    {
                        range.Add(page.Report);
                    }
                }
                CurrentPage = 0;
                if (range.Count == 0)
                    e.Cancel = true;
            }
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.PageUnit = GraphicsUnit.Millimeter;
            //e.PageSettings.Margins = new Margins(0, 0, 0, 0);
            g.TranslateTransform(13f, 2f);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            if (range.Count > 0)
            {
                CreateGraphicsRI(g, range[CurrentPage]);
                CurrentPage++; //Здесь CurrentPage - счетчик по range
                e.HasMorePages = (CurrentPage < range.Count);
            }
            else
            {
                CreateGraphicsRI(g, repPages[CurrentPage].Report);
                CurrentPage++;//Здесь CurrentPage - счетчик по repPages
                e.HasMorePages = (CurrentPage < repPages.Count);
            }
            e.PageSettings.Landscape = DefaultPageSettings.Landscape;
        }

        public static string GetFormattedString(string val)
        {
            return val;
        }

        public List<KeyValuePair<string, string>> GetPages(bool validonly)
        {
            List<KeyValuePair<string, string>> ret = new List<KeyValuePair<string, string>>();
            foreach (repPage page in repPages)
            {
                ret.Add(new KeyValuePair<string, string>(page.Report.name, page.Report.Tootip));
            }
            return ret;
        }

        public string DefaultPrinter
        {
            get { return PrinterSettings.PrinterName; }
        }

        public void SetPagePrint(int index, bool print)
        {
            if (index < repPages.Count)
                repPages[index].IsPrint = print;
        }

        private Brush GetBrush(string str)
        {
            Color color = Color.Empty;
            try
            {
                string[] colors = (str.Contains(",")) ? str.Split(',') : new string[] { int.Parse(str.Substring(0, 2), NumberStyles.AllowHexSpecifier).ToString(), int.Parse(str.Substring(2, 2), NumberStyles.AllowHexSpecifier).ToString(), int.Parse(str.Substring(4, 2), NumberStyles.AllowHexSpecifier).ToString() };
                color = Color.FromArgb(int.Parse(colors[0]), int.Parse(colors[1]), int.Parse(colors[2]));
            }
            catch (Exception exp)
            {
                string exp_str = exp.Message;
            }
            return new SolidBrush(color);
        }
        private Color GetColor(Byte[] bytes, int r, int g, int b)
        {
            if (bytes != null)
            {
                int len = bytes.Length;
                if (len >= 1) r = (int)bytes[0];
                if (len >= 2) g = (int)bytes[1];
                if (len >= 3) b = (int)bytes[2];
            }
            return Color.FromArgb(r, g, b);
        }
        private Color GetColor(Byte[] bytes, Color defaultcolor)
        {
            return GetColor(bytes, defaultcolor.R, defaultcolor.G, defaultcolor.B);
        }

        private StringAlignment GetStringAlignment(alignType align)
        {
            switch (align)
            {
                case alignType.Left:
                    return StringAlignment.Near;
                case alignType.Center:
                    return StringAlignment.Center;
                case alignType.Right:
                    return StringAlignment.Far;
            }
            return StringAlignment.Near;
        }
        private StringAlignment GetStringAlignment(valignType valign)
        {
            switch (valign)
            {
                case valignType.Top:
                    return StringAlignment.Near;
                case valignType.Center:
                    return StringAlignment.Center;
                case valignType.Bottom:
                    return StringAlignment.Far;
                default:
                    return StringAlignment.Near;
            }
        }
        private Pen GetPen(string color, string weight)
        {
            Pen ret = new Pen(GetBrush(color), float.Parse(weight, new CultureInfo("en-us")));
            ret.EndCap = LineCap.Round;
            ret.StartCap = LineCap.Round;
            return ret;
        }
        private Pen GetPen(Color color, float weight)
        {
            Pen ret = new Pen(new SolidBrush(color), weight);
            ret.EndCap = LineCap.Round;
            ret.StartCap = LineCap.Round;
            return ret;
        }

        public void Print(int count)
        {
            Print(count, PrinterSettings.PrinterName);
        }

        public void Print(int count, string printer)
        {
            string old_printer = PrinterSettings.PrinterName;
            try
            {
                //if (range.Count > 0) 
                //{
                //    foreach (XmlDocument doc in pages)
                //    {
                //        range.Add(pages.IndexOf(doc));
                //    }
                //}
                PrinterSettings.PrinterName = (printer == "") ? old_printer : printer;
                //List<int> new_range = new List<int>();
                //for (int i = 0; i < count; i++)
                //{
                //    new_range.AddRange(range);
                //}
                //range = new_range;
                CurrentCount = (count > 0) ? count : 0;
                base.Print();
            }
            catch (Exception exp)
            {
                string exp_str = exp.Message;
            }
            finally
            {
                PrinterSettings.PrinterName = old_printer;
            }
        }
    }
}
namespace BO.Reports
{
    [XmlRoot("text")]
    public partial class textInfo : IXmlSerializable
    {
        //static XmlConverter xc;
        //static XmlSchema sch;

        #region IXmlSerializable Members

        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
        {
            //if (sch == null)
            //{
            //    using (TextReader stringReader = new StringReader(BO.Common.GetReportsInfoSchema()))
            //        sch = XmlSchema.Read(stringReader, null);
            //}
            //return sch;
            return null;
        }
        //public void ReadXml(XmlReader reader)
        //{
        //    if (xc == null) 
        //        xc = new XmlConverter();
        //    //string s = reader.ReadOuterXml();
        //    textInfo t = xc.Deserialize(typeof(textInfo), reader.ReadOuterXml()) as textInfo;
        //    this.left = t.left;
        //}
        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            string left, top, width, height, fontsize;

            left = reader.GetAttribute("left");
            top = reader.GetAttribute("top");
            width = reader.GetAttribute("width");
            height = reader.GetAttribute("height");
            fontsize = reader.GetAttribute("font-size");

            if (!string.IsNullOrEmpty(left)) this.left = float.Parse(Common.GlobalDecimal(left)); // "0" 
            if (!string.IsNullOrEmpty(top)) this.top = float.Parse(Common.GlobalDecimal(top)); // "2.953" 
            if (!string.IsNullOrEmpty(width)) this.width = float.Parse(Common.GlobalDecimal(width)); // "18.4756" 
            if (!string.IsNullOrEmpty(height)) this.height = float.Parse(Common.GlobalDecimal(height)); // "0.4704" 
            if (!string.IsNullOrEmpty(fontsize)) this.fontsize = float.Parse(Common.GlobalDecimal(fontsize)); // "8" 
            
            this.align = XmlEnum.StringToXmlEnum<alignType>(reader.GetAttribute("align")); // "Left" 
            this.valign = XmlEnum.StringToXmlEnum<valignType>(reader.GetAttribute("valign")); // "Top" 
            this.fontname = reader.GetAttribute("font-name"); // "Arial" 
            this.fontbold = GetBool(reader.GetAttribute("font-bold")); // "true" 
            this.fontitalic = GetBool(reader.GetAttribute("font-italic")); // "false" 
            this.fontunderline = GetBool(reader.GetAttribute("font-underline")); // "false" 
            this.nowrap = GetBool(reader.GetAttribute("no-wrap"));
            this.group = reader.GetAttribute("group");

            this.forecolor = GetRGBColor(reader.GetAttribute("fore-color"), "000000");
            this.backcolor = GetRGBColor(reader.GetAttribute("back-color"), "FFFFFF");

            this.border = GetBorders(reader.GetAttribute("border")); // "0 0 0 0.1" 
            this.bordercolor = GetBorderColor(reader.GetAttribute("border-color")); // "000000 000000 000000 FF0000"

            this.Value = reader.ReadElementContentAsString();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            //ns.Add(string.Empty, string.Empty);

            //writer.WriteRaw("sdf=\"вапвап\"");
            //writer.WriteStartElement("text");
            //writer.WriteElementString("text", "sdf");
            //writer.WriteRaw(" sdf=\"вапвап\"");
            //writer.

            writer.WriteAttributeString("left", Common.PropValue2String(this.left, typeof(float)));
            writer.WriteAttributeString("top", Common.PropValue2String(this.top, typeof(float)));
            writer.WriteAttributeString("width", Common.PropValue2String(this.width, typeof(float)));
            writer.WriteAttributeString("height", Common.PropValue2String(this.height, typeof(float)));
            writer.WriteAttributeString("align", Common.PropValue2String(this.align, typeof(alignType)));
            writer.WriteAttributeString("valign", Common.PropValue2String(this.valign, typeof(valignType)));
            writer.WriteAttributeString("font-name", this.fontname);
            writer.WriteAttributeString("font-size", Common.PropValue2String(this.fontsize, typeof(float)));
            writer.WriteAttributeString("font-bold", this.fontbold.ToString().ToLower());
            writer.WriteAttributeString("font-italic", this.fontitalic.ToString().ToLower());
            writer.WriteAttributeString("font-underline", this.fontunderline.ToString().ToLower());
            writer.WriteAttributeString("no-wrap", this.nowrap.ToString().ToLower());
            writer.WriteAttributeString("group", this.group);

            writer.WriteAttributeString("fore-color", PutHex(this.forecolor));
            writer.WriteAttributeString("back-color", PutHex(this.backcolor));


            writer.WriteAttributeString("border", PutFloats(border)); // "0 0 0 0.1" 
            writer.WriteAttributeString("border-color", PutHex(bordercolor)); // "000000 000000 000000 FF0000"

            if (Value != null)
                writer.WriteValue(Value);


        }

        #endregion
        string PutFloats(float[] border)
        {
            string[] str = new string[border.GetLength(0)];
            for (int i = 0; i < border.GetLength(0); i++)
            {
                str[i] = Common.PropValue2String(border[i], typeof(float));
            }
            return string.Join(" ", str);
        }
        string PutHex(byte[][] bytes)
        {
            string[] str = new string[bytes.GetLength(0)];
            for (int i = 0; i < bytes.GetLength(0); i++)
            {
                 str[i] = BitConverter.ToString(bytes[i]).Replace("-", "");
            }
            return string.Join(" ", str);
        }

        string PutHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }
        bool GetBool(string boolstr)
        {
            if (string.IsNullOrEmpty(boolstr))
                return false;

            switch (boolstr.ToLower())
            {
                case "true":
                case "1":
                    return true;
                case "false":
                case "0":
                    return false;
                default:
                    throw new Exception(string.Format("bad xml bool value \"{0}\"", boolstr));
            }
        }
        byte[] GetRGBColor(string HexRGB, string defColor)
        {
            byte[] t = BitConverter.GetBytes(int.Parse(defColor, NumberStyles.HexNumber));
            
            if (!string.IsNullOrEmpty(HexRGB))
                t = BitConverter.GetBytes(int.Parse(HexRGB, NumberStyles.HexNumber));
            
            Array.Resize<byte>(ref t, 3);
            return t;
        }
        float[] GetBorders(string borders)
        {
            float[] ret = new float[] { 0, 0, 0, 0 };
            if (string.IsNullOrEmpty(borders))
                return ret;

            string[] sarray = borders.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ret = new float[sarray.Length];
            for (int i = 0; i < sarray.Length; i++)
                ret[i] = float.Parse(Common.GlobalDecimal(sarray[i]));

            return ret;
        }
        byte[][] GetBorderColor(string bordercolor)
        {
            string sdefColor = "000000";
            byte[] defcolor = GetRGBColor("", sdefColor);
            byte[][] ret = new byte[][] { defcolor, defcolor, defcolor, defcolor };
            if (string.IsNullOrEmpty(bordercolor))
                return ret;
            string[] sarray = bordercolor.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sarray.Length; i++)
                ret[i] = GetRGBColor(sarray[i], sdefColor);

            return ret;
        }

    }

    public partial class itemInfo
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    public partial class reportInfo
    {
        private string _tootip;

        public string Tootip
        {
            get { return _tootip; }
            set { _tootip = value; }
        }
        public reportInfo()
        {
        }
        public reportInfo(string rep_name)
            : this()
        {
            name = rep_name;
        }
    }

}

