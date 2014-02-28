using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace BO
{
    public class ExcelXml : BaseXmlDoc
    {
        public enum PaperSizeIndexEnum
        {
            DMPAPER_LETTER = 1,             //	    US Letter 8 1/2 x 11 in
            DMPAPER_LETTERSMALL = 2,        //		US Letter Small 8 1/2 x 11 in
            DMPAPER_TABLOID = 3,            //		US Tabloid 11 x 17 in
            DMPAPER_LEDGER = 4,             //		US Ledger 17 x 11 in
            DMPAPER_LEGAL = 5,              //		US Legal 8 1/2 x 14 in
            DMPAPER_STATEMENT = 6,          //		US Statement 5 1/2 x 8 1/2 in
            DMPAPER_EXECUTIVE = 7,          //		US Executive 7 1/4 x 10 1/2 in
            DMPAPER_A3 = 8,                 //		A3 297 x 420 mm
            DMPAPER_A4 = 9,                 //		A4 210 x 297 mm
            DMPAPER_A4SMALL = 10,           //		A4 Small 210 x 297 mm
            DMPAPER_A5 = 11,                //		A5 148 x 210 mm
            DMPAPER_B4 = 12,                //		B4 (JIS) 257 x 364 mm
            DMPAPER_B5 = 13,                //		B5 (JIS) 182 x 257 mm
            DMPAPER_FOLIO = 14,             //		Folio 8 1/2 x 13 in
            DMPAPER_QUARTO = 15,            //		Quarto 215 x 275 mm
            DMPAPER_10X14 = 16,             //		10 x 14 in
            DMPAPER_11X17 = 17,             //		11 x 17 in
            DMPAPER_NOTE = 18,              //		US Note 8 1/2 x 11 in
            DMPAPER_ENV_9 = 19,             //		US Envelope #9 3 7/8 x 8 7/8
            DMPAPER_ENV_10 = 20,            //		US Envelope #10 4 1/8 x 9 1/2
            DMPAPER_ENV_11 = 21,            //		US Envelope #11 4 1/2 x 10 3/8
            DMPAPER_ENV_12 = 22,            //		US Envelope #12 4 3/4 x 11 in
            DMPAPER_ENV_14 = 23,            //		US Envelope #14 5 x 11 1/2
            DMPAPER_CSHEET = 24,            //		C size sheet
            DMPAPER_DSHEET = 25,            //		D size sheet
            DMPAPER_ESHEET = 26,            //		E size sheet
            DMPAPER_ENV_DL = 27,            //		Envelope DL 110 x 220mm
            DMPAPER_ENV_C5 = 28,            //		Envelope C5 162 x 229 mm
            DMPAPER_ENV_C3 = 29,            //		Envelope C3 324 x 458 mm
            DMPAPER_ENV_C4 = 30,            //		Envelope C4 229 x 324 mm
            DMPAPER_ENV_C6 = 31,            //		Envelope C6 114 x 162 mm
            DMPAPER_ENV_C65 = 32,           //		Envelope C65 114 x 229 mm
            DMPAPER_ENV_B4 = 33,            //		Envelope B4 250 x 353 mm
            DMPAPER_ENV_B5 = 34,            //		Envelope B5 176 x 250 mm
            DMPAPER_ENV_B6 = 35,            //		Envelope B6 176 x 125 mm
            DMPAPER_ENV_ITALY = 36,         //		Envelope 110 x 230 mm
            DMPAPER_ENV_MONARCH = 37,       //		US Envelope Monarch 3.875 x 7.5 in
            DMPAPER_ENV_PERSONAL = 38,      //		6 3/4 US Envelope 3 5/8 x 6 1/2 in
            DMPAPER_FANFOLD_US = 39,        //		US Std Fanfold 14 7/8 x 11 in
            DMPAPER_FANFOLD_STD_GERMAN = 40, //		German Std Fanfold 8 1/2 x 12 in
            DMPAPER_FANFOLD_LGL_GERMAN = 41, //		German Legal Fanfold 8 1/2 x 13 in
            DMPAPER_ISO_B4 = 42,            //		B4 (ISO) 250 x 353 mm
            DMPAPER_JAPANESE_POSTCARD = 43, //		Japanese Postcard 100 x 148 mm
            DMPAPER_9X11 = 44,              //		9 x 11 in
            DMPAPER_10X11 = 45,             //		10 x 11 in
            DMPAPER_15X11 = 46,             //		15 x 11 in
            DMPAPER_ENV_INVITE = 47,        //		Envelope Invite 220 x 220 mm
            DMPAPER_RESERVED_48 = 48,       //		RESERVED--DO NOT USE
            DMPAPER_RESERVED_49 = 49,       //		RESERVED--DO NOT USE
            DMPAPER_LETTER_EXTRA = 50,      //		US Letter Extra 9 1/2 x 12 in
            DMPAPER_LEGAL_EXTRA = 51,       //		US Legal Extra 9 1/2 x 15 in
            DMPAPER_TABLOID_EXTRA = 52,     //		US Tabloid Extra 11.69 x 18 in
            DMPAPER_A4_EXTRA = 53,          //		A4 Extra 9.27 x 12.69 in
            DMPAPER_LETTER_TRANSVERSE = 54, //		Letter Transverse 8 1/2 x 11 in
            DMPAPER_A4_TRANSVERSE = 55,     //		A4 Transverse 210 x 297 mm
            DMPAPER_LETTER_EXTRA_TRANSVERSE = 56, //		Letter Extra Transverse 9 1/2 x 12 in
            DMPAPER_A_PLUS = 57,            //		SuperA/SuperA/A4 227 x 356 mm
            DMPAPER_B_PLUS = 58,            //		SuperB/SuperB/A3 305 x 487 mm
            DMPAPER_LETTER_PLUS = 59,       //		US Letter Plus 8.5 x 12.69 in
            DMPAPER_A4_PLUS = 60,           //		A4 Plus 210 x 330 mm
            DMPAPER_A5_TRANSVERSE = 61,     //		A5 Transverse 148 x 210 mm
            DMPAPER_B5_TRANSVERSE = 62,     //		B5 (JIS) Transverse 182 x 257 mm
            DMPAPER_A3_EXTRA = 63,          //		A3 Extra 322 x 445 mm
            DMPAPER_A5_EXTRA = 64,          //		A5 Extra 174 x 235 mm
            DMPAPER_B5_EXTRA = 65,          //		B5 (ISO) Extra 201 x 276 mm
            DMPAPER_A2 = 66,                //		A2 420 x 594 mm
            DMPAPER_A3_TRANSVERSE = 67,     //		A3 Transverse 297 x 420 mm
            DMPAPER_A3_EXTRA_TRANSVERSE = 68, //		A3 Extra Transverse 322 x 445 mm
            DMPAPER_DBL_JAPANESE_POSTCARD = 69, //		Japanese Double Postcard 200 x 148 mm
            DMPAPER_A6 = 70,                //		A6 105 x 148 mm
            DMPAPER_JENV_KAKU2 = 71,        //		Japanese Envelope Kaku #2
            DMPAPER_JENV_KAKU3 = 72,        //		Japanese Envelope Kaku #3
            DMPAPER_JENV_CHOU3 = 73,        //		Japanese Envelope Chou #3
            DMPAPER_JENV_CHOU4 = 74,        //		Japanese Envelope Chou #4
            DMPAPER_LETTER_ROTATED = 75,    //		Letter Rotated 11 x 8 1/2 11 in
            DMPAPER_A3_ROTATED = 76,        //		A3 Rotated 420 x 297 mm
            DMPAPER_A4_ROTATED = 77,        //		A4 Rotated 297 x 210 mm
            DMPAPER_A5_ROTATED = 78,        //		A5 Rotated 210 x 148 mm
            DMPAPER_B4_JIS_ROTATED = 79,    //		B4 (JIS) Rotated 364 x 257 mm
            DMPAPER_B5_JIS_ROTATED = 80,    //		B5 (JIS) Rotated 257 x 182 mm
            DMPAPER_JAPANESE_POSTCARD_ROTATED = 81, //		Japanese Postcard Rotated 148 x 100 mm
            DMPAPER_DBL_JAPANESE_POSTCARD_ROTATED = 82, //		Double Japanese Postcard Rotated 148 x 200 mm
            DMPAPER_A6_ROTATED = 83,        //		A6 Rotated 148 x 105 mm
            DMPAPER_JENV_KAKU2_ROTATED = 84, //		Japanese Envelope Kaku #2 Rotated
            DMPAPER_JENV_KAKU3_ROTATED = 85, //		Japanese Envelope Kaku #3 Rotated
            DMPAPER_JENV_CHOU3_ROTATED = 86, //		Japanese Envelope Chou #3 Rotated
            DMPAPER_JENV_CHOU4_ROTATED = 87, //		Japanese Envelope Chou #4 Rotated
            DMPAPER_B6_JIS = 88, //	B6	 (JIS) 128 x 182 mm
            DMPAPER_B6_JIS_ROTATED = 89, //		B6 (JIS) Rotated 182 x 128 mm
            DMPAPER_12X11 = 90, //		12 x 11 in
            DMPAPER_JENV_YOU4 = 91, //		Japanese Envelope You #4
            DMPAPER_JENV_YOU4_ROTATED = 92, //		Japanese Envelope You #4 Rotated
            DMPAPER_P16K = 93, //		PRC 16K 146 x 215 mm
            DMPAPER_P32K = 94, //		PRC 32K 97 x 151 mm
            DMPAPER_P32KBIG = 95, //		PRC 32K(Big) 97 x 151 mm
            DMPAPER_PENV_1 = 96, //		PRC Envelope #1 102 x 165 mm
            DMPAPER_PENV_2 = 97, //		PRC Envelope #2 102 x 176 mm
            DMPAPER_PENV_3 = 98, //		PRC Envelope #3 125 x 176 mm
            DMPAPER_PENV_4 = 99, //		PRC Envelope #4 110 x 208 mm
            DMPAPER_PENV_5 = 100, //		PRC Envelope #5 110 x 220 mm
            DMPAPER_PENV_6 = 101, //		PRC Envelope #6 120 x 230 mm
            DMPAPER_PENV_7 = 102, //		PRC Envelope #7 160 x 230 mm
            DMPAPER_PENV_8 = 103, //		PRC Envelope #8 120 x 309 mm
            DMPAPER_PENV_9 = 104, //		PRC Envelope #9 229 x 324 mm
            DMPAPER_PENV_10 = 105, //		PRC Envelope #10 324 x 458 mm
            DMPAPER_P16K_ROTATED = 106, //		PRC 16K Rotated
            DMPAPER_P32K_ROTATED = 107, //		PRC 32K Rotated
            DMPAPER_P32KBIG_ROTATED = 108, //		PRC 32K(Big) Rotated
            DMPAPER_PENV_1_ROTATED = 109, //		PRC Envelope #1 Rotated 165 x 102 mm
            DMPAPER_PENV_2_ROTATED = 110, //		PRC Envelope #2 Rotated 176 x 102 mm
            DMPAPER_PENV_3_ROTATED = 111, //		PRC Envelope #3 Rotated 176 x 125 mm
            DMPAPER_PENV_4_ROTATED = 112, //		PRC Envelope #4 Rotated 208 x 110 mm
            DMPAPER_PENV_5_ROTATED = 113, //		PRC Envelope #5 Rotated 220 x 110 mm
            DMPAPER_PENV_6_ROTATED = 114, //		PRC Envelope #6 Rotated 230 x 120 mm
            DMPAPER_PENV_7_ROTATED = 115, //		PRC Envelope #7 Rotated 230 x 160 mm
            DMPAPER_PENV_8_ROTATED = 116, //		PRC Envelope #8 Rotated 309 x 120 mm
            DMPAPER_PENV_9_ROTATED = 117, //		PRC Envelope #9 Rotated 324 x 229 mm
            DMPAPER_PENV_10_ROTATED = 118 //		PRC Envelope #10 Rotated 458 x 324 mm
        }
        public const string ssNameSpace = "urn:schemas-microsoft-com:office:spreadsheet";
        public const string xNameSpace = "urn:schemas-microsoft-com:office:excel";
        public const string oNameSpace = "urn:schemas-microsoft-com:office:office";
        public ExcelXml()
            : base()
        {
            WorkBook = this.AppendChild("Workbook", "xmlns", ssNameSpace);
            this.AppendAttribute(WorkBook, "xmlns:o", oNameSpace);
            this.AppendAttribute(WorkBook, "xmlns:x", xNameSpace);
            this.AppendAttribute(WorkBook, "xmlns:ss", ssNameSpace);
            this.AppendAttribute(WorkBook, "xmlns:html", "http://www.w3.org/TR/REC-html40");
            XmlDeclaration xmldecl = this.CreateXmlDeclaration("1.0", null, null);
            this.InsertBefore(xmldecl, WorkBook);
            Styles = this.AppendChild(WorkBook, "Styles");
            //WorksheetOptions = this.AppendChild(WorkBook,"WorksheetOptions","xmlns","urn:schemas-microsoft-com:office:excel");
        }
        public ExcelXml(XlsResourceType type, string xls)
        {
            switch (type)
            {
                case XlsResourceType.Embedded:
                    System.IO.Stream stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream(xls);
                    if (stream != null)
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                        {
                            xls = sr.ReadToEnd();
                            this.LoadXml(xls);
                        }
                    else
                        throw new Exception(string.Format("Ресурс XML-шаблона \"{0}\" не найден", xls));
                    break;
                case XlsResourceType.FileName:
                    this.Load(xls);
                    break;
                case XlsResourceType.IsString:
                    this.LoadXml(xls);
                    break;
            }
        }

        #region Members
        private XmlNode _WorkBook = null;
        public XmlNode WorkBook { get { return _WorkBook; } set { _WorkBook = value; } }
        private XmlNode _Styles = null;
        public XmlNode Styles { get { return _Styles; } set { _Styles = value; } }

        private XmlNode _CurrentStyle = null;
        public XmlNode CurrentStyle { get { return _CurrentStyle; } set { _CurrentStyle = value; } }

        private XmlNode _Names = null;
        public XmlNode Names { get { return _Names; } set { _Names = value; } }

        private XmlNode _CurrentWorksheet = null;
        public XmlNode CurrentWorksheet { get { return _CurrentWorksheet; } set { _CurrentWorksheet = value; } }
        private string _CurrentWorksheetName = "";
        public string CurrentWorksheetName { get { return _CurrentWorksheetName; } set { _CurrentWorksheetName = value; } }

        private XmlNode _CurrentPageSetup = null;
        public XmlNode CurrentPageSetup { get { return _CurrentPageSetup; } set { _CurrentPageSetup = value; } }
        private XmlNode _CurrentRowBreaks = null;
        public XmlNode CurrentRowBreaks { get { return _CurrentRowBreaks; } set { _CurrentRowBreaks = value; } }
        private XmlNode _CurrentTable = null;
        public XmlNode CurrentTable { get { return _CurrentTable; } set { _CurrentTable = value; } }
        private XmlNode _CurrentRow = null;
        public XmlNode CurrentRow { get { return _CurrentRow; } set { _CurrentRow = value; } }
        private XmlNode _CurrentCell = null;
        public XmlNode CurrentCell { get { return _CurrentCell; } set { _CurrentCell = value; } }
        private int _RowNumber = 0;
        private int _RowSpan = 0;
        public int RowNumber { get { return _RowNumber; } }
        #endregion

        #region Methods
        public XmlNode CreateNodeFromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            xml = @"<?xml version='1.0'?><Workbook xmlns='urn:schemas-microsoft-com:office:spreadsheet' xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:x='urn:schemas-microsoft-com:office:excel' xmlns:ss='urn:schemas-microsoft-com:office:spreadsheet' xmlns:html='http://www.w3.org/TR/REC-html40'>" + xml;
            //doc.DocumentElement.InnerXml = xml;
            doc.LoadXml(xml + "</Workbook>");
            //XmlNode node = CreateElement("test");
            //Styles.InnerXml = xml;
            //this.AddStyle();
            //node.InnerXml = xml;
            XmlNode node = CreateElement(doc.DocumentElement.FirstChild.Name);
            node.InnerXml = doc.DocumentElement.FirstChild.InnerXml;
            foreach (XmlAttribute attr in doc.DocumentElement.FirstChild.Attributes)
            {
                if (attr.Name.StartsWith("ss:"))
                    AppendSSAttribute(node, attr.Name.Replace("ss:", ""), attr.Value);
                else
                    AppendAttribute(node, attr.Name, attr.Value);
            }
            return node;
        }

        public void CreateRecursiveNode()
        {
        }

        #region Styles
        public enum StyleHorisontalAlignment { Automatic, Left, Center, Right, Justify };
        public enum StyleVerticalAlignment { Automatic, Top, Bottom, Center };

        public XmlNode AddXmlStyle(string xml)
        {
            return Styles.AppendChild(CreateNodeFromXml(xml));
        }

        public XmlNode AddStyle(string id)
        {
            return AddStyle(id, "", "");
        }
        public XmlNode AddStyle(string id, string name)
        {
            return AddStyle(id, name, "");
        }
        public XmlNode AddStyle(string id, string name, string parent)
        {
            CurrentStyle = this.AppendSSChild(Styles, "Style", @"ID", id);
            if (name.Length > 0) this.AppendSSAttribute(CurrentStyle, "Name", name);
            if (parent.Length > 0) this.AppendSSAttribute(CurrentStyle, "Parent", parent);
            return CurrentStyle;
        }
        public XmlNode AddStyleAlignment(StyleHorisontalAlignment horisontal, int indent, StyleVerticalAlignment vertical, bool wrap)
        {
            return AddStyleAlignment(CurrentStyle, horisontal, indent, vertical, wrap);
        }
        public XmlNode AddStyleAlignment(XmlNode nodeStyle, StyleHorisontalAlignment horisontal, int indent, StyleVerticalAlignment vertical, bool wrap)
        {
            XmlNode alignment = this.AppendChild(nodeStyle, "Alignment");
            string s = "";
            switch (horisontal)
            {
                case StyleHorisontalAlignment.Left: s = "Left"; break;
                case StyleHorisontalAlignment.Center: s = "Center"; break;
                case StyleHorisontalAlignment.Right: s = "Right"; break;
            }
            this.AppendSSAttribute(alignment, "Horizontal", s);

            if (indent > 0)
                this.AppendSSAttribute(alignment, "Indent", indent.ToString());

            s = "";
            switch (vertical)
            {
                case StyleVerticalAlignment.Bottom: s = "Bottom"; break;
                case StyleVerticalAlignment.Center: s = "Center"; break;
                case StyleVerticalAlignment.Top: s = "Top"; break;
            }
            this.AppendSSAttribute(alignment, "Vertical", s);
            if (wrap)
                this.AppendSSAttribute(alignment, "WrapText", "1");

            return alignment;
        }
        public void AddStyleRotate(XmlNode nodeAlignment, int degrees)
        {
            this.AppendSSAttribute(nodeAlignment, "Rotate", degrees.ToString());
        }



        public XmlNode AddStyleBorders(params Border[] borders)
        {
            return AddStyleBorders(CurrentStyle, borders);
        }
        public XmlNode AddStyleBorders(XmlNode nodeStyle, params Border[] borders)
        {
            XmlNode node = this.AppendChild(nodeStyle, "Borders");
            for (int i = 0; i < borders.Length; i++)
            {
                Border border = borders[i];
                XmlNode nodeBorder = this.AppendSSChild(node, "Border", "Position", border.Position);
                if (border.Weight > 0)
                    this.AppendSSAttribute(nodeBorder, "Weight", ToStr(border.Weight));
                if (border.LineStyle.Length > 0)
                    this.AppendSSAttribute(nodeBorder, "LineStyle", border.LineStyle);
                if (border.Color.Length > 0)
                    this.AppendSSAttribute(nodeBorder, "Color", border.Color);
            }
            return node;
        }
        public XmlNode AddStyleBorders()
        {
            return AddStyleBorders(CurrentStyle, Border.BorderLineStyle.Continuous);
        }
        public XmlNode AddStyleBorders(Border.BorderLineStyle lineStyle)
        {
            return AddStyleBorders(CurrentStyle, lineStyle);
        }
        public XmlNode AddStyleBorders(XmlNode nodeStyle, Border.BorderLineStyle lineStyle)
        {
            return AddStyleBorders(
                 new Border(Border.BorderPosition.Left, lineStyle)
                , new Border(Border.BorderPosition.Bottom, lineStyle)
                , new Border(Border.BorderPosition.Right, lineStyle)
                , new Border(Border.BorderPosition.Top, lineStyle));
        }

        public XmlNode AddStyleFont(string name, Double size)
        {
            return AddStyleFont(CurrentStyle, name, size, false, false, false);
        }
        public XmlNode AddStyleFont(XmlNode nodeStyle, string name, Double size)
        {
            return AddStyleFont(nodeStyle, name, size, false, false, false);
        }
        public XmlNode AddStyleFont(string name, Double size, bool bold, bool italic, bool underline)
        {
            return AddStyleFont(CurrentStyle, name, size, bold, italic, underline);
        }
        public XmlNode AddStyleFont(XmlNode nodeStyle, string name, Double size, bool bold, bool italic, bool underline)
        {
            return AddStyleFont(nodeStyle, name, size, bold, italic, underline, "");
        }
        public XmlNode AddStyleFont(string name, Double size, bool bold, bool italic, bool underline, string color)
        {
            return AddStyleFont(CurrentStyle, name, size, bold, italic, underline, color);
        }
        public XmlNode AddStyleFont(XmlNode nodeStyle, string name, Double size, bool bold, bool italic, bool underline, string color)
        {
            XmlNode nodeFont = this.AppendChild(nodeStyle, "Font");
            this.AppendSSAttribute(nodeFont, "FontName", name);
            this.AppendSSAttribute(nodeFont, "Size", ToStr(size));
            if (bold) this.AppendSSAttribute(nodeFont, "Bold", "1");
            if (italic) this.AppendSSAttribute(nodeFont, "Italic", "1");
            if (underline) this.AppendSSAttribute(nodeFont, "Underline", "Single");
            if (color != "") this.AppendSSAttribute(nodeFont, "Color", color);
            return nodeFont;
        }

        public XmlNode AddStyleInterior(string color)
        {
            return AddStyleInterior(CurrentStyle, color);
        }
        public XmlNode AddStyleInterior(XmlNode nodeStyle, string color)
        {
            XmlNode nodeInterior = this.AppendChild(nodeStyle, "Interior");
            this.AppendSSAttribute(nodeInterior, "Color", color);
            this.AppendSSAttribute(nodeInterior, "Pattern", "Solid");
            return nodeInterior;
        }

        public XmlNode AddStyleNumberFormat(string format)
        {
            return AddStyleNumberFormat(CurrentStyle, format);
        }
        public XmlNode AddStyleNumberFormat(XmlNode nodeStyle, string format)
        {
            XmlNode node = this.AppendChild(nodeStyle, "NumberFormat");
            this.AppendSSAttribute(node, "Format", format);
            return node;
        }

        public string addCustomStyle(string Prefix, string HA, string VA, int FontSize, string FontStyle, int LeftBorder, int TopBorder, int RightBorder, int BottomBorder, bool Wrap, string numberFormat)
        {
            string StyleName = Prefix + HA + VA + FontSize.ToString() + FontStyle + LeftBorder.ToString() + TopBorder.ToString() + RightBorder.ToString() + BottomBorder.ToString();// + numberFormat;

            AddStyle(StyleName);
            if (numberFormat != "") AddStyleNumberFormat(numberFormat);

            bool FontBold = (FontStyle.ToLower().IndexOf("bold") >= 0) ? true : false;
            bool FontItalic = (FontStyle.ToLower().IndexOf("italic") >= 0) ? true : false;
            bool FontUnderline = (FontStyle.ToLower().IndexOf("underline") >= 0) ? true : false;

            StyleHorisontalAlignment tha = StyleHorisontalAlignment.Left;
            StyleVerticalAlignment tva = StyleVerticalAlignment.Center;

            if (HA.ToLower().IndexOf("left") >= 0)
            {
                tha = StyleHorisontalAlignment.Left;
            }
            else if (HA.ToLower().IndexOf("right") >= 0)
            {
                tha = StyleHorisontalAlignment.Right;
            }
            else if (HA.ToLower().IndexOf("center") >= 0)
            {
                tha = StyleHorisontalAlignment.Center;
            }

            if (VA.ToLower().IndexOf("top") >= 0)
            {
                tva = StyleVerticalAlignment.Top;
            }
            else if (VA.ToLower().IndexOf("bottom") >= 0)
            {
                tva = StyleVerticalAlignment.Bottom;
            }
            else if (VA.ToLower().IndexOf("center") >= 0)
            {
                tva = StyleVerticalAlignment.Center;
            }

            AddStyleFont(FontName, FontSize, FontBold, FontItalic, FontUnderline);
            AddStyleAlignment(tha, 0, tva, Wrap);

            Border.BorderLineStyle LeftBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle TopBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle RightBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle BottomBorderStyle = Border.BorderLineStyle.None;

            double LeftBorderWeight = 1;
            double TopBorderWeight = 1;
            double RightBorderWeight = 1;
            double BottomBorderWeight = 1;

            if (LeftBorder <= 0)
            {
                LeftBorderStyle = Border.BorderLineStyle.None;
                LeftBorderWeight = 0;
            }
            else
            {
                LeftBorderStyle = Border.BorderLineStyle.Continuous;
                LeftBorderWeight = LeftBorder / 2;
            }

            if (TopBorder <= 0)
            {
                TopBorderStyle = Border.BorderLineStyle.None;
                TopBorderWeight = 0;
            }
            else
            {
                TopBorderStyle = Border.BorderLineStyle.Continuous;
                TopBorderWeight = TopBorder / 2;
            }

            if (RightBorder <= 0)
            {
                RightBorderStyle = Border.BorderLineStyle.None;
                RightBorderWeight = 0;
            }
            else
            {
                RightBorderStyle = Border.BorderLineStyle.Continuous;
                RightBorderWeight = RightBorder / 2;
            }

            if (BottomBorder <= 0)
            {
                BottomBorderStyle = Border.BorderLineStyle.None;
                BottomBorderWeight = 0;
            }
            else
            {
                BottomBorderStyle = Border.BorderLineStyle.Continuous;
                BottomBorderWeight = BottomBorder / 2;
            }

            AddStyleBorders(
                new Border(Border.BorderPosition.Left, LeftBorderStyle, LeftBorderWeight),
                new Border(Border.BorderPosition.Top, TopBorderStyle, TopBorderWeight),
                new Border(Border.BorderPosition.Right, RightBorderStyle, RightBorderWeight),
                new Border(Border.BorderPosition.Bottom, BottomBorderStyle, BottomBorderWeight)
                );
            return StyleName;
        }

        public string addCustomStyle(string Prefix, string HA, string VA, int FontSize, string FontStyle, int LeftBorder, int TopBorder, int RightBorder, int BottomBorder, bool Wrap)
        {
            string StyleName = Prefix + HA + VA + FontSize.ToString() + FontStyle + LeftBorder.ToString() + TopBorder.ToString() + RightBorder.ToString() + BottomBorder.ToString();

            AddStyle(StyleName);
            bool FontBold = (FontStyle.ToLower().IndexOf("bold") >= 0) ? true : false;
            bool FontItalic = (FontStyle.ToLower().IndexOf("italic") >= 0) ? true : false;
            bool FontUnderline = (FontStyle.ToLower().IndexOf("underline") >= 0) ? true : false;

            StyleHorisontalAlignment tha = StyleHorisontalAlignment.Left;
            StyleVerticalAlignment tva = StyleVerticalAlignment.Center;

            if (HA.ToLower().IndexOf("left") >= 0)
            {
                tha = StyleHorisontalAlignment.Left;
            }
            else if (HA.ToLower().IndexOf("right") >= 0)
            {
                tha = StyleHorisontalAlignment.Right;
            }
            else if (HA.ToLower().IndexOf("center") >= 0)
            {
                tha = StyleHorisontalAlignment.Center;
            }

            if (VA.ToLower().IndexOf("top") >= 0)
            {
                tva = StyleVerticalAlignment.Top;
            }
            else if (VA.ToLower().IndexOf("bottom") >= 0)
            {
                tva = StyleVerticalAlignment.Bottom;
            }
            else if (VA.ToLower().IndexOf("center") >= 0)
            {
                tva = StyleVerticalAlignment.Center;
            }

            AddStyleFont(FontName, FontSize, FontBold, FontItalic, FontUnderline);
            AddStyleAlignment(tha, 0, tva, Wrap);

            Border.BorderLineStyle LeftBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle TopBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle RightBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle BottomBorderStyle = Border.BorderLineStyle.None;

            double LeftBorderWeight = 1;
            double TopBorderWeight = 1;
            double RightBorderWeight = 1;
            double BottomBorderWeight = 1;

            if (LeftBorder <= 0)
            {
                LeftBorderStyle = Border.BorderLineStyle.None;
                LeftBorderWeight = 0;
            }
            else
            {
                LeftBorderStyle = Border.BorderLineStyle.Continuous;
                LeftBorderWeight = LeftBorder / 2;
            }

            if (TopBorder <= 0)
            {
                TopBorderStyle = Border.BorderLineStyle.None;
                TopBorderWeight = 0;
            }
            else
            {
                TopBorderStyle = Border.BorderLineStyle.Continuous;
                TopBorderWeight = TopBorder / 2;
            }

            if (RightBorder <= 0)
            {
                RightBorderStyle = Border.BorderLineStyle.None;
                RightBorderWeight = 0;
            }
            else
            {
                RightBorderStyle = Border.BorderLineStyle.Continuous;
                RightBorderWeight = RightBorder / 2;
            }

            if (BottomBorder <= 0)
            {
                BottomBorderStyle = Border.BorderLineStyle.None;
                BottomBorderWeight = 0;
            }
            else
            {
                BottomBorderStyle = Border.BorderLineStyle.Continuous;
                BottomBorderWeight = BottomBorder / 2;
            }

            AddStyleBorders(
                new Border(Border.BorderPosition.Left, LeftBorderStyle, LeftBorderWeight),
                new Border(Border.BorderPosition.Top, TopBorderStyle, TopBorderWeight),
                new Border(Border.BorderPosition.Right, RightBorderStyle, RightBorderWeight),
                new Border(Border.BorderPosition.Bottom, BottomBorderStyle, BottomBorderWeight)
                );
            return StyleName;
        }

        public string addCustomStyle(string Parent, string Prefix, string HA, string VA, int FontSize, string FontStyle, int LeftBorder, int TopBorder, int RightBorder, int BottomBorder, bool Wrap)
        {
            string StyleName = Prefix + HA + VA + FontSize.ToString() + FontStyle + LeftBorder.ToString() + TopBorder.ToString() + RightBorder.ToString() + BottomBorder.ToString();

            AddStyle(StyleName);
            bool FontBold = (FontStyle.ToLower().IndexOf("bold") >= 0) ? true : false;
            bool FontItalic = (FontStyle.ToLower().IndexOf("italic") >= 0) ? true : false;
            bool FontUnderline = (FontStyle.ToLower().IndexOf("underline") >= 0) ? true : false;

            StyleHorisontalAlignment tha = StyleHorisontalAlignment.Left;
            StyleVerticalAlignment tva = StyleVerticalAlignment.Center;

            if (HA.ToLower().IndexOf("left") >= 0)
            {
                tha = StyleHorisontalAlignment.Left;
            }
            else if (HA.ToLower().IndexOf("right") >= 0)
            {
                tha = StyleHorisontalAlignment.Right;
            }
            else if (HA.ToLower().IndexOf("center") >= 0)
            {
                tha = StyleHorisontalAlignment.Center;
            }

            if (VA.ToLower().IndexOf("top") >= 0)
            {
                tva = StyleVerticalAlignment.Top;
            }
            else if (VA.ToLower().IndexOf("bottom") >= 0)
            {
                tva = StyleVerticalAlignment.Bottom;
            }
            else if (VA.ToLower().IndexOf("center") >= 0)
            {
                tva = StyleVerticalAlignment.Center;
            }

            AddStyleFont(FontName, FontSize, FontBold, FontItalic, FontUnderline);
            AddStyleAlignment(tha, 0, tva, Wrap);

            Border.BorderLineStyle LeftBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle TopBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle RightBorderStyle = Border.BorderLineStyle.None;
            Border.BorderLineStyle BottomBorderStyle = Border.BorderLineStyle.None;

            double LeftBorderWeight = 1;
            double TopBorderWeight = 1;
            double RightBorderWeight = 1;
            double BottomBorderWeight = 1;

            if (LeftBorder <= 0)
            {
                LeftBorderStyle = Border.BorderLineStyle.None;
                LeftBorderWeight = 0;
            }
            else
            {
                LeftBorderStyle = Border.BorderLineStyle.Continuous;
                LeftBorderWeight = LeftBorder / 2;
            }

            if (TopBorder <= 0)
            {
                TopBorderStyle = Border.BorderLineStyle.None;
                TopBorderWeight = 0;
            }
            else
            {
                TopBorderStyle = Border.BorderLineStyle.Continuous;
                TopBorderWeight = TopBorder / 2;
            }

            if (RightBorder <= 0)
            {
                RightBorderStyle = Border.BorderLineStyle.None;
                RightBorderWeight = 0;
            }
            else
            {
                RightBorderStyle = Border.BorderLineStyle.Continuous;
                RightBorderWeight = RightBorder / 2;
            }

            if (BottomBorder <= 0)
            {
                BottomBorderStyle = Border.BorderLineStyle.None;
                BottomBorderWeight = 0;
            }
            else
            {
                BottomBorderStyle = Border.BorderLineStyle.Continuous;
                BottomBorderWeight = BottomBorder / 2;
            }

            AddStyleBorders(
                new Border(Border.BorderPosition.Left, LeftBorderStyle, LeftBorderWeight),
                new Border(Border.BorderPosition.Top, TopBorderStyle, TopBorderWeight),
                new Border(Border.BorderPosition.Right, RightBorderStyle, RightBorderWeight),
                new Border(Border.BorderPosition.Bottom, BottomBorderStyle, BottomBorderWeight)
                );
            if (Parent.Length > 0) this.AppendSSAttribute(CurrentStyle, "Parent", Parent);
            return StyleName;
        }


        #endregion

        #region Names
        public XmlNode AddName(string name, string refersTo)
        {
            if (Names == null) Names = this.AppendChild(CurrentWorksheet, "Names");

            XmlNode node = this.AppendSSChild(Names, "NamedRange", @"Name", name);
            this.AppendSSAttribute(node, "RefersTo", string.Format("='{0}'!{1}", CurrentWorksheetName, refersTo));
            return node;
        }
        public XmlNode AddRepeatedRows(int start, int end)
        {
            return AddName("Print_Titles", string.Format("R{0}:R{1}", start, end));
        }
        #endregion

        #region Worksheet
        public XmlNode GetWorksheet(string name)
        {
            return this.SelectSingleNode(string.Format("Workbook/Worksheet[@ss:Name='{0}']", name));
        }

        public XmlNode AddWorksheet(string name)
        {
            CurrentWorksheetName = (name.Length > 31) ? name.Substring(0, 31) : name;
            CurrentWorksheet = this.AppendChild(WorkBook, "Worksheet", "Name", CurrentWorksheetName, "ss", ssNameSpace);
            _RowNumber = 0;
            _RowSpan = 0;
            return CurrentWorksheet;
        }

        #endregion

        #region Table
        public XmlNode AddXmlTable(string xml)
        {
            CurrentTable = CurrentWorksheet.AppendChild(CreateNodeFromXml(xml));
            return CurrentTable;
        }
        public XmlNode AddWorksheetTable()
        {
            return AddWorksheetTable(CurrentWorksheet, "");
        }
        public XmlNode AddWorksheetTable(XmlNode nodeWorksheet)
        {
            return AddWorksheetTable(nodeWorksheet, "");
        }
        public XmlNode AddWorksheetTable(string styleID)
        {
            return AddWorksheetTable(CurrentWorksheet, styleID);
        }
        public XmlNode AddWorksheetTable(XmlNode nodeWorksheet, string styleID)
        {
            CurrentTable = this.AppendChild(nodeWorksheet, "Table");
            if (styleID.Length > 0)
                this.AppendSSAttribute(CurrentTable, "StyleID", styleID);
            return CurrentTable;
        }
        #endregion

        #region Columns
        public XmlNode AddXmlColumn(string xml)
        {
            return CurrentTable.AppendChild(CreateNodeFromXml(xml));
        }

        public XmlNode AddColumn(double width, bool isAutoFitWidth, string styleID)
        {
            return AddColumn(CurrentTable, width, isAutoFitWidth, false, -1, 0, styleID);
        }
        public XmlNode AddColumn(double width, bool isAutoFitWidth, int span, string styleID)
        {
            return AddColumn(CurrentTable, width, isAutoFitWidth, false, -1, span, styleID);
        }
        public XmlNode AddColumn(double width, bool isAutoFitWidth, bool isHidden, int index, int span, string styleID)
        {
            return AddColumn(CurrentTable, width, isAutoFitWidth, isHidden, index, span, styleID);
        }
        public XmlNode AddColumn(XmlNode nodeTable, double width, bool isAutoFitWidth, bool isHidden, int index, int span, string styleID)
        {
            XmlNode node = this.AppendChild(nodeTable, "Column");
            if (width >= 0) this.AppendSSAttribute(node, "Width", ToStr(width));
            if (isAutoFitWidth) this.AppendSSAttribute(node, "AutoFitWidth", "1");
            if (isHidden) this.AppendSSAttribute(node, "Hidden", "1");
            if (index > 0) this.AppendSSAttribute(node, "Index", index.ToString());
            if (span > 0) this.AppendSSAttribute(node, "Span", span.ToString());
            if (styleID.Length > 0) this.AppendSSAttribute(node, "StyleID", styleID);
            return node;
        }
        #endregion

        #region Rows
        public void SetAutoRowHeight(XmlNode row, int wrapLength, double wrapHeight)
        {
            SetAutoRowHeight(row, -1, wrapLength, wrapHeight);
        }
        public void SetAutoRowHeight(XmlNode row, int cellIndex, int wrapLength, double wrapHeight)
        {
            if (row.HasChildNodes)
            {
                //                XmlNode cell = null;
                int maxLen = 0;
                if (cellIndex == -1)
                {

                    for (int i = 0; i < row.ChildNodes.Count; i++)
                    {
                        XmlNode cell = row.ChildNodes[i];
                        if (cell.InnerText.Length > maxLen)
                            maxLen = cell.InnerText.Length;
                    }
                }
                else if (row.ChildNodes.Count > cellIndex)
                    maxLen = row.ChildNodes[cellIndex].InnerText.Length;

                if (maxLen > wrapLength)
                {
                    for (int i = wrapLength; i < maxLen; i += wrapLength)
                        wrapHeight += wrapHeight;

                    this.AppendSSAttribute(row, "Height", ToStr(wrapHeight));

                }

            }
        }

        public XmlNode AddXmlRow(string xml)
        {
            CurrentRow = CurrentTable.AppendChild(CreateNodeFromXml(xml));
            return CurrentRow;
        }

        public XmlNode AddRow()
        {
            return AddRow(CurrentTable, "");
        }
        public XmlNode AddRow(bool isAutoFitHeight)
        {
            return AddRow(CurrentTable, -1, isAutoFitHeight, false, -1, 0, "");
        }
        public XmlNode AddRow(XmlNode nodeTable)
        {
            return AddRow(nodeTable, "");
        }
        public XmlNode AddRow(string styleID)
        {
            return AddRow(CurrentTable, -1, false, false, -1, 0, styleID);
        }
        public XmlNode AddRow(XmlNode nodeTable, string styleID)
        {
            return AddRow(nodeTable, -1, false, false, -1, 0, styleID);
        }
        public XmlNode AddRow(double height, bool isAutoFitHeight, bool isHidden, int index, int span, string styleID)
        {
            return AddRow(CurrentTable, height, isAutoFitHeight, isHidden, index, span, styleID);
        }
        public XmlNode AddRow(int span, string styleID)
        {
            return AddRow(CurrentTable, -1, false, false, -1, span, styleID);
        }
        public XmlNode AddRow(XmlNode nodeTable, double height, bool isAutoFitHeight, bool isHidden, int index, int span, string styleID)
        {
            CurrentRow = this.AppendChild(nodeTable, "Row");
            if (height >= 0) this.AppendSSAttribute(CurrentRow, "Height", ToStr(height));
            if (isAutoFitHeight) this.AppendSSAttribute(CurrentRow, "AutoFitHeight", "1");
            if (isHidden) this.AppendSSAttribute(CurrentRow, "Hidden", "1");
            if (index > 0) this.AppendSSAttribute(CurrentRow, "Index", index.ToString());
            if (span > 0) this.AppendSSAttribute(CurrentRow, "Span", span.ToString());
            if (styleID.Length > 0) this.AppendSSAttribute(CurrentRow, "StyleID", styleID);
            if (index > 0)
                _RowNumber = index;
            else
                _RowNumber += 1 + _RowSpan;
            _RowSpan = span;
            return CurrentRow;
        }
        public int GetRowIndex()
        {
            return GetRowIndex(CurrentRow);
        }
        public int GetRowIndex(XmlNode nodeRow)
        {
            int row = 0;
            while (nodeRow != null)
            {
                int index = getAttrIntValue(nodeRow, "Row", "Index");
                if (index > 0) return row + index;
                nodeRow = prevNode(nodeRow, "Row");
                row += getAttrIntValue(nodeRow, "Row", "Span") + 1;
            }
            return row;
        }
        public int GetColIndex()
        {
            return GetColIndex(CurrentCell);
        }
        public int GetColIndex(XmlNode nodeCell)
        {
            int col = 0;
            while (nodeCell != null)
            {
                int index = getAttrIntValue(nodeCell, "Cell", "Index");
                if (index > 0) return col + index;
                nodeCell = prevNode(nodeCell, "Cell");
                col += getAttrIntValue(nodeCell, "Cell", "MergeAcross") + 1;
            }
            return col;
        }
        public string GetCellAddress()
        {
            return GetCellAddress(CurrentCell);
        }
        public string GetCellAddress(XmlNode nodeCell)
        {
            XmlNode nodeRow = nodeCell.ParentNode;
            int row = GetRowIndex(nodeRow);
            int col = GetColIndex(nodeCell);
            return string.Format("R{0}C{1}", row, col);
        }
        private XmlNode prevNode(XmlNode node, string name)
        {
            while (node != null)
            {
                node = node.PreviousSibling;
                if (node != null && node.Name == name) break;
            }
            return node;
        }

        private int getAttrIntValue(XmlNode node, string tag, string localName)
        {
            if (node == null || node.Name != tag) return 0;
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.LocalName == localName)
                    return int.Parse(attr.Value);
            }
            return 0;
        }
        #endregion

        #region Cells

        public XmlNode AddXmlCell(string xml)
        {
            CurrentCell = CurrentRow.AppendChild(CreateNodeFromXml(xml));
            return CurrentCell;
        }
        public XmlNode AddCell()
        {
            return AddCell(CurrentRow, "", -1, 0, 0, "", "");
        }
        public XmlNode AddCell(XmlNode nodeRow)
        {
            return AddCell(nodeRow, "", -1, 0, 0, "", "");
        }
        public XmlNode AddCell(int mergeAcross, string styleID)
        {
            return AddCell(CurrentRow, "", -1, mergeAcross, 0, styleID, "");
        }
        public XmlNode AddCell(string formula, int index, int mergeAcross, int mergeDown, string styleID, string href)
        {
            return AddCell(CurrentRow, formula, index, mergeAcross, mergeDown, styleID, href);
        }
        public XmlNode AddCell(XmlNode nodeRow, string formula, int index, int mergeAcross, int mergeDown, string styleID, string href)
        {
            CurrentCell = this.AppendChild(nodeRow, "Cell");
            if (formula.Length > 0) this.AppendSSAttribute(CurrentCell, "Formula", formula);
            if (index > 0) this.AppendSSAttribute(CurrentCell, "Index", index.ToString());
            if (mergeAcross > 0) this.AppendSSAttribute(CurrentCell, "MergeAcross", mergeAcross.ToString());
            if (mergeDown > 0) this.AppendSSAttribute(CurrentCell, "MergeDown", mergeDown.ToString());
            if (styleID.Length > 0) this.AppendSSAttribute(CurrentCell, "StyleID", styleID);
            if (href.Length > 0) this.AppendSSAttribute(CurrentCell, "HRef", href);
            return CurrentCell;
        }


        public XmlNode AddCell(CellType celltype, string valCell)
        {
            return AddCell(CurrentRow, celltype, valCell);
        }

        public XmlNode AddCell(XmlNode nodeRow, CellType celltype, string valCell)
        {
            AddCell(nodeRow, "", -1, 0, 0, "", "");
            AddCellData(CurrentCell, celltype, valCell);
            return CurrentCell;
        }

        #region Cell = DateTime
        public XmlNode AddCell(DateTime dt)
        {
            return AddCell(CurrentRow, dt, "");
        }
        public XmlNode AddCell(DateTime dt, string styleID)
        {
            return AddCell(CurrentRow, dt, styleID);
        }
        public XmlNode AddCell(XmlNode nodeRow, DateTime dt)
        {
            return AddCell(nodeRow, dt, "");
        }
        public XmlNode AddCell(XmlNode nodeRow, DateTime dt, string styleID)
        {
            XmlNode nodeCell = AddCell(nodeRow, "", -1, 0, 0, styleID, "");
            AddCellData(nodeCell, CellType.DateTime, dt.ToString("yyyy-MM-dd"));
            return nodeCell;
        }
        #endregion
        #region Cell = Decimal
        public XmlNode AddCell(decimal number)
        {
            return AddCell(CurrentRow, number, "");
        }
        public XmlNode AddCell(decimal number, string styleID)
        {
            return AddCell(CurrentRow, number, styleID);
        }
        public XmlNode AddCell(XmlNode nodeRow, decimal number)
        {
            return AddCell(nodeRow, number, "");
        }
        public XmlNode AddCell(XmlNode nodeRow, decimal number, string styleID)
        {
            AddCell(nodeRow, "", -1, 0, 0, styleID, "");
            AddCellData(CurrentCell, CellType.Number, ToStr(number));
            return CurrentCell;
        }
        #endregion
        #region Cell = String
        public XmlNode AddCell(string str)
        {
            return AddCell(CurrentRow, str, "");
        }
        public XmlNode AddCell(string str, string styleID)
        {
            return AddCell(CurrentRow, str, styleID);
        }
        public XmlNode AddCell(XmlNode nodeRow, string str)
        {
            return AddCell(nodeRow, str, "");
        }
        public XmlNode AddCell(XmlNode nodeRow, string str, string styleID)
        {
            AddCell(nodeRow, "", -1, 0, 0, styleID, "");
            AddCellData(CurrentCell, CellType.String, str);
            return CurrentCell;
        }
        #endregion

        #region Cells - Only add data tag
        public XmlNode AddCellData(string valCell)
        {
            return AddCellData(CurrentCell, valCell);
        }
        public XmlNode AddCellData(XmlNode nodeCell, string valCell)
        {
            return AddCellData(nodeCell, CellType.String, valCell);
        }

        public XmlNode AddCellData(DateTime valCell)
        {
            return AddCellData(CurrentCell, valCell);
        }
        public XmlNode AddCellData(XmlNode nodeCell, DateTime valCell)
        {
            return AddCellData(nodeCell, CellType.DateTime, valCell.ToString("yyyy-MM-dd"));
        }

        public XmlNode AddCellData(decimal valCell)
        {
            return AddCellData(CurrentCell, valCell);
        }
        public XmlNode AddCellData(XmlNode nodeCell, decimal valCell)
        {
            return AddCellData(nodeCell, CellType.Number, ToStr(valCell));
        }

        public XmlNode AddCellData(XmlNode nodeCell, CellType celltype, string valCell)
        {
            string[] stype = new string[] { "Number", "DateTime", "Boolean", "String", "Error" };
            XmlNode node = this.AppendSSChild(nodeCell, "Data", "Type", stype[(int)celltype]);
            node.InnerText = valCell;
            return node;
        }
        public XmlNode AddCellFormula(XmlNode nodeCell, CellType celltype, string formula)
        {
            AddCellFormula(nodeCell, formula);
            string[] stype = new string[] { "Number", "DateTime", "Boolean", "String", "Error" };
            if (nodeCell.SelectNodes("Data").Count == 0)
                this.AppendSSChild(nodeCell, "Data", "Type", stype[(int)celltype]);
            return nodeCell;
        }
        public XmlNode AddCellFormula(XmlNode nodeCell, string formula)
        {
            this.AppendSSAttribute(nodeCell, "Formula", formula);
            return nodeCell;
        }
        public XmlNode AddCellFormula(string formula)
        {
            return AddCellFormula(CurrentCell, formula);
        }

        public XmlNode AddCellComment(string comment)
        {
            return AddCellComment(CurrentCell, comment);
        }
        public XmlNode AddCellComment(XmlNode nodeCell, string comment)
        {
            XmlNode nodeComment = this.AppendSSChild(nodeCell, "Comment", "Author", "");
            XmlNode node = this.AppendSSChild(nodeComment, "Data", "xmlns", @"http://www.w3.org/TR/REC-html40");
            node.InnerXml = comment;

            return nodeComment;
        }

        #endregion

        #endregion

        #region PageSetup
        public XmlNode AddWorksheetPageSetup()
        {
            return AddWorksheetPageSetup(CurrentWorksheet, 0, 0, PaperSizeIndexEnum.DMPAPER_A4);
        }
        public XmlNode AddWorksheetPageSetup(XmlNode nodeWorksheet, PaperSizeIndexEnum paperSize)
        {
            return AddWorksheetPageSetup(nodeWorksheet, 0, 0, paperSize);
        }
        public XmlNode AddWorksheetPageSetup(bool gridLines, PaperSizeIndexEnum paperSize)
        {
            return AddWorksheetPageSetup(CurrentWorksheet, 0, 0, gridLines, paperSize);
        }
        public XmlNode AddWorksheetPageSetup(int fitHeight, int fitWidth, PaperSizeIndexEnum paperSize)
        {
            return AddWorksheetPageSetup(CurrentWorksheet, fitHeight, fitWidth, paperSize);
        }
        public XmlNode AddWorksheetPageSetup(XmlNode nodeWorksheet, int fitHeight, int fitWidth, PaperSizeIndexEnum paperSize)
        {
            return AddWorksheetPageSetup(nodeWorksheet, fitHeight, fitWidth, false, paperSize);
        }
        public XmlNode AddWorksheetPageSetup(int fitHeight, int fitWidth, bool gridLines, PaperSizeIndexEnum paperSize)
        {
            return AddWorksheetPageSetup(CurrentWorksheet, fitHeight, fitWidth, gridLines, paperSize);
        }
        public XmlNode AddWorksheetPageSetup(XmlNode nodeWorksheet, int fitHeight, int fitWidth, bool gridLines, PaperSizeIndexEnum paperSize)
        {

           //<Print>
           // <FitHeight>100</FitHeight>
           // <ValidPrinterInfo/>
           // <PaperSizeIndex>9</PaperSizeIndex>
           // <Scale>66</Scale>
           // <HorizontalResolution>600</HorizontalResolution>
           // <VerticalResolution>0</VerticalResolution>
           //</Print>

            XmlNode nodeWO = this.AppendChild(nodeWorksheet, "WorksheetOptions");
            this.AppendAttribute(nodeWO, "xmlns", xNameSpace);
            XmlNode nodePrint = this.AppendChild(nodeWO, "Print");
            if (fitHeight > 0 || fitWidth > 0)
            {
                this.AppendChild(nodeWO, "FitToPage");
                if (fitHeight > 0)
                {
                    XmlNode node = this.AppendChild(nodePrint, "FitHeight");
                    node.InnerText = fitHeight.ToString();
                }
                if (fitWidth > 0)
                {
                    XmlNode node = this.AppendChild(nodePrint, "FitWidth");
                    node.InnerText = fitWidth.ToString();
                }
            }

            XmlNode nodePaperSize = this.AppendChild(nodePrint, "PaperSizeIndex");
            nodePaperSize.InnerText = ((int)paperSize).ToString();

            if (gridLines) this.AppendChild(nodePrint, "Gridlines");

            CurrentPageSetup = this.AppendChild(nodeWO, "PageSetup");
            return CurrentPageSetup;
        }

        public XmlNode AddWorksheetPageBreaks()
        {
            return AddWorksheetPageBreaks(CurrentWorksheet);
        }
        public XmlNode AddWorksheetPageBreaks(XmlNode nodeWorksheet)
        {
            XmlNode node = this.AppendChild(nodeWorksheet, "PageBreaks");
            this.AppendAttribute(node, "xmlns", xNameSpace);
            CurrentRowBreaks = this.AppendChild(node, "RowBreaks");
            return CurrentRowBreaks;
        }

        public XmlNode AddWorksheetRowBreak()
        {
            return AddWorksheetRowBreak(GetRowIndex());
        }
        public XmlNode AddWorksheetRowBreak(int row)
        {
            return AddWorksheetRowBreak(row, 0);
        }
        public XmlNode AddWorksheetRowBreak(int row, int col)
        {
            if (CurrentRowBreaks == null) AddWorksheetPageBreaks();
            return AddWorksheetRowBreak(CurrentRowBreaks, row, col);
        }
        public XmlNode AddWorksheetRowBreak(XmlNode nodeRowBreaks, int row, int col)
        {
            XmlNode nodeRowBreak = this.AppendChild(nodeRowBreaks, "RowBreak");
            if (row > 0)
            {
                XmlNode node = this.AppendChild(nodeRowBreak, "Row");
                node.InnerText = row.ToString();
            }
            if (col > 0)
            {
                XmlNode node = this.AppendChild(nodeRowBreak, "ColEnd");
                node.InnerText = col.ToString();
            }
            return nodeRowBreak;
        }


        public XmlNode AddWorksheetHeader()
        {
            return AddWorksheetHeader(CurrentPageSetup, 0.5, string.Format("&L{0}&R{1}", CompanyName, MCName));
        }
        public XmlNode AddWorksheetHeader(string left, string center, string right)
        {
            return AddWorksheetHeader(CurrentPageSetup, 0.5, string.Format("&L{0}&C{1}&R{2}", left, center, right));
        }
        public XmlNode AddWorksheetHeader(double margin, string data)
        {
            return AddWorksheetHeader(CurrentPageSetup, margin, data);
        }
        public XmlNode AddWorksheetHeader(XmlNode nodePageSetup, double margin, string data)
        {
            XmlNode node = this.AppendChild(nodePageSetup, "Header");
            this.AppendXAttribute(node, "Margin", ToStr(margin));
            this.AppendXAttribute(node, "Data", data);
            return node;
        }

        public XmlNode AddWorksheetFooter()
        {
            return AddWorksheetFooter(CurrentPageSetup, 0.3, "&L&D &T&C&A&RPage &P of &N");
        }
        public XmlNode AddWorksheetFooter(string left, string center, string right)
        {
            return AddWorksheetFooter(CurrentPageSetup, 0.3, string.Format("&L{0}&C{1}&R{2}", left, center, right));
        }
        public XmlNode AddWorksheetFooter(double margin, string data)
        {
            return AddWorksheetFooter(CurrentPageSetup, margin, data);
        }
        public XmlNode AddWorksheetFooter(XmlNode nodePageSetup, double margin, string data)
        {
            XmlNode node = this.AppendChild(nodePageSetup, "Footer");
            this.AppendXAttribute(node, "Margin", ToStr(margin));
            this.AppendXAttribute(node, "Data", data);
            return node;
        }
        public XmlNode AddWorksheetLayout(bool isLandscape)
        {
            return AddWorksheetLayout(CurrentPageSetup, isLandscape);
        }
        public XmlNode AddWorksheetLayout(XmlNode nodePageSetup, bool isLandscape)
        {
            XmlNode node = this.AppendChild(nodePageSetup, "Layout");
            this.AppendXAttribute(node, "Orientation", (isLandscape ? "Landscape" : "Portrait"));
            return node;
        }
        public XmlNode AddWorksheetMargins(double left, double top, double right, double bottom)
        {
            return AddWorksheetMargins(CurrentPageSetup, left, top, right, bottom);
        }
        public XmlNode AddWorksheetMargins(XmlNode nodePageSetup, Double left, Double top, Double right, Double bottom)
        {
            XmlNode node = this.AppendChild(nodePageSetup, "PageMargins");
            this.AppendXAttribute(node, "Left", ToStr(left));
            this.AppendXAttribute(node, "Top", ToStr(top));
            this.AppendXAttribute(node, "Right", ToStr(right));
            this.AppendXAttribute(node, "Bottom", ToStr(bottom));
            return node;
        }
        #endregion

        #endregion

        #region Functions
        public XmlNode AppendSSChild(XmlNode node, string nameElement, string attrName, string attrValue)
        {
            return this.AppendChild(node, nameElement, attrName, attrValue, "ss", ssNameSpace);
        }
        public XmlAttribute AppendSSAttribute(XmlNode node, string attrName, string attrValue)
        {
            return this.AppendAttribute(node, attrName, attrValue, "ss", ssNameSpace);
        }
        public XmlAttribute AppendXAttribute(XmlNode node, string attrName, string attrValue)
        {
            return this.AppendAttribute(node, attrName, attrValue, "x", xNameSpace);
        }
        public string ToStr(decimal val)
        {
            return val.ToString().Replace(',', '.');
        }
        public string ToStr(double val)
        {
            return val.ToString().Replace(',', '.');
        }
        #endregion

        #region Pioglobal Defaults
        #region Static Members
        private static string _CompanyName = "";
        public static string CompanyName { get { return _CompanyName; } set { _CompanyName = value; } }

        private static string _MCName = "Pioglobal Asset Management";
        public static string MCName { get { return _MCName; } set { _MCName = value; } }

        private static string _FontName = "Verdana";
        public static string FontName { get { return _FontName; } set { _FontName = value; } }
        #endregion

        public void AddDefaultStyles()
        {
            AddStyle("Default", "Normal");
            AddStyleFont(FontName, 10);

            AddStyle("Types");
            AddStyleFont(FontName, 8);

            AddStyle("TypesWrap");
            AddStyleFont(FontName, 8);
            AddStyleAlignment(StyleHorisontalAlignment.Left, 0, StyleVerticalAlignment.Bottom, true);

            AddStyle("CenterText");
            AddStyleFont(FontName, 8);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, false);

            AddStyle("CenterTextLargeTop");
            AddStyleFont(FontName, 20, true, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, false);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous, 2)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.None, 0)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous, 2)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous, 2));

            AddStyle("CenterTextLargeCenter");
            AddStyleFont(FontName, 20, true, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, false);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.None, 0)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.None, 0)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous, 2)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous, 2));

            AddStyle("CenterTextLargeBottom");
            AddStyleFont(FontName, 20, true, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, false);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.None, 0)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous, 2)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous, 2)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous, 2));


            AddStyle("LittleText");
            AddStyleFont(FontName, 7);
            AddStyleAlignment(StyleHorisontalAlignment.Left, 0, StyleVerticalAlignment.Bottom, false);

            AddStyle("styleTable");
            AddStyleFont(FontName, 10);

            AddStyle("styleAmount");
            AddStyleNumberFormat("_.#,##0_.;[Red]-_.#,##0_.;-_.");

            //			AddStyle("styleNumb");
            //			AddStyleNumberFormat("_.#,##0_.;[Red]-_.#,##0_.;-_.");

            AddStyle("styleMoney");
            AddStyleNumberFormat("_.#,##0.00_.;[Red]-_.#,##0.00_.;-_.");

            AddStyle("styleMoney4");
            AddStyleNumberFormat("_.#,##0.0000_.;[Red]-_.#,##0.0000_.;-_.");

            AddStyle("styleMoney5");
            AddStyleNumberFormat("_.#,##0.00000_.;[Red]-_.#,##0.00000_.;-_.");

            AddStyle("stylePrice");
            AddStyleNumberFormat("_.#,##0.0000_.;[Red]-_.#,##0.0000_.;-_.");

            AddStyle("styleMoneyZero");
            AddStyleNumberFormat("_.#,##0.00_.;[Red]-_.#,##0.00_.;0.00_.");

            AddStyle("styleDate");
            AddStyleNumberFormat("Short Date");

            AddStyle("styleText");
            AddStyleFont(FontName, 10, false, false, false);
            AddStyleNumberFormat("@");

            AddStyle("styleTextWord");
            AddStyleFont(FontName, 11, false, false, false);
            AddStyleNumberFormat("@");

            AddStyle("styleTextWordBold");
            AddStyleFont(FontName, 11, true, false, false);
            AddStyleNumberFormat("@");

            AddStyle("styleTextWrap", "WrapText", "styleText");
            AddStyleAlignment(StyleHorisontalAlignment.Left, 0, StyleVerticalAlignment.Bottom, true);

            AddStyle("H1");
            AddStyleFont(FontName, 12, true, false, false);
            AddStyleInterior("#CCFFCC");

            AddStyle("PinkInteger");
            AddStyleInterior("#FF9999");
            AddStyleNumberFormat("_.#,##0_.;[Red]-_.#,##0_.;-_.");

            AddStyle("H1Line", "H1Line", "H1");
            AddStyleFont(FontName, 12, true, false, true);
            AddStyleInterior("#CCFFCC");

            AddStyle("H2");
            AddStyleFont(FontName, 12, true, false, false);

            AddStyle("H3");
            AddStyleFont(FontName, 10, true, false, false);

            AddStyle("H3Line", "H3Line", "H3");
            AddStyleFont(FontName, 10, true, false, true);
            AddStyleInterior("#CCFFCC");

            AddStyle("Title");
            AddStyleFont(FontName, 12, true, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, false);

            AddStyle("TableHeader");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));

            AddStyle("TableHeaderLeft");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Left, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));

            AddStyle("TableHeaderNumber");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));
            AddStyleNumberFormat(Formats.INTEGER);

            AddStyle("TableHeaderMoney");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Right, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));
            AddStyleNumberFormat(Formats.MONEY);

            AddStyle("TableHeaderMoney5");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Right, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));
            AddStyleNumberFormat(Formats.MONEY5);

            AddStyle("TableHeaderAmount");
            AddStyleFont(FontName, 9, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Right, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));
            AddStyleNumberFormat(Formats.AMOUNT);


            AddStyle("TableHeaderSmall");
            AddStyleFont(FontName, 7, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Center, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));

            AddStyle("TableHeaderSmallLeft");
            AddStyleFont(FontName, 7, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Left, 0, StyleVerticalAlignment.Center, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));

            AddStyle("TableHeaderSign");
            AddStyleFont(FontName, 6, false, false, false);
            AddStyleAlignment(StyleHorisontalAlignment.Right, 0, StyleVerticalAlignment.Bottom, true);
            AddStyleBorders(new Border(Border.BorderPosition.Left, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Bottom, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Right, Border.BorderLineStyle.Continuous)
                , new Border(Border.BorderPosition.Top, Border.BorderLineStyle.Continuous));


            AddStyle("stylePercent");
            AddStyleNumberFormat("Percent");
        }
        public XmlNode AddDefaultWorksheet()
        {
            return AddDefaultWorksheet("Report", "", true);
        }
        public XmlNode AddDefaultWorksheet(bool isLandscape)
        {
            return AddDefaultWorksheet("Report", "", true);
        }
        public XmlNode AddDefaultWorksheet(string repName)
        {
            return AddDefaultWorksheet(repName, "", true);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname)
        {
            return AddDefaultWorksheet(repName, worksheetname, true);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, "Report", true);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, bool UseFooter)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, "Report", UseFooter);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, string sheetName)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, sheetName, 0.5, 1, 0.5, 0.7, false, true);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, string sheetName, bool UseFooter)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, sheetName, 0.5, 1, 0.5, 0.7, false, UseFooter);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, string sheetName, double left, double top, double right, double bottom)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, sheetName, left, top, right, bottom, false, true);
        }
        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, bool NoFitPage, bool UseFooter)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, "Report", 0.5, 1, 0.5, 0.7, NoFitPage, UseFooter);
        }

        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, string sheetName, double left, double top, double right, double bottom, bool NoFitPage)
        {
            return AddDefaultWorksheet(repName, worksheetname, isLandscape, sheetName, left, top, right, bottom, NoFitPage, true);
        }

        public XmlNode AddDefaultWorksheet(string repName, string worksheetname, bool isLandscape, string sheetName, double left, double top, double right, double bottom, bool NoFitPage, bool UseFooter)
        {
            AddWorksheet(sheetName);
            AddWorksheetTable("styleTable");

            if (!NoFitPage)
                AddWorksheetPageSetup(100, 1, PaperSizeIndexEnum.DMPAPER_A4);
            else
                AddWorksheetPageSetup();
            AddWorksheetLayout(isLandscape);
            AddWorksheetMargins(left, top, right, bottom);
            //AddWorksheetHeader(CurrentPageSetup, 0.5, string.Format("&L&\"Verdana,Bold\"&12{0}", MCName));
            if (UseFooter)
                AddWorksheetFooter(CurrentPageSetup, 0.3, string.Format("&L{0}&C{1}&RСтраница &P из &N", worksheetname, repName));

            return CurrentWorksheet;
        }
        #endregion
        #region SubClasses
        public class Border
        {
            public enum BorderPosition { Left, Top, Right, Bottom };
            public enum BorderLineStyle { None, Continuous, Dash, Dot, DashDot, DashDotDot };
            public Border(BorderPosition position)
            {
                switch (position)
                {
                    case BorderPosition.Left: Position = "Left"; break;
                    case BorderPosition.Top: Position = "Top"; break;
                    case BorderPosition.Right: Position = "Right"; break;
                    case BorderPosition.Bottom: Position = "Bottom"; break;
                }
                LineStyle = "Continuous";
                Weight = 1;
            }
            public Border(BorderPosition position, BorderLineStyle linestyle)
                : this(position)
            {
                switch (linestyle)
                {
                    case BorderLineStyle.None: LineStyle = "None"; break;
                    case BorderLineStyle.Continuous: LineStyle = "Continuous"; break;
                    case BorderLineStyle.Dash: LineStyle = "Dash"; break;
                    case BorderLineStyle.DashDot: LineStyle = "DashDot"; break;
                    case BorderLineStyle.DashDotDot: LineStyle = "DashDotDot"; break;
                    case BorderLineStyle.Dot: LineStyle = "Dot"; break;
                }
                Weight = 1;
            }
            public Border(BorderPosition position, BorderLineStyle linestyle, Double weight)
                : this(position, linestyle)
            {
                Weight = weight;
            }
            public Border(BorderPosition position, BorderLineStyle linestyle, Double weight, string color)
                : this(position, linestyle, weight)
            {
                Color = color;
            }

            public Double Weight = 0;
            public string Position;
            public string Color = "";
            public string LineStyle = "";
        }
        #endregion

        #region Replace Excel Cell Values
        public XmlNodeList GetCells(XmlNode row, string name)
        {
            return row.SelectNodes("*[contains(.,'[#" + name + "#]')]/*[local-name(.)='Cell']");
        }
        public XmlNode GetData(XmlNode cell)
        {
            return cell.SelectNodes("*[local-name(.)='Data']")[0];
        }
        public void Replace(XmlNode row, string name, string value)
        {
            foreach (XmlNode subnode in GetCells(row, name))
            {
                GetData(subnode).InnerText = subnode.InnerText.Replace("[#" + name + "#]", value);
            }
        }
        public void Replace(XmlNode row, string name, decimal value)
        {
            foreach (XmlNode subnode in GetCells(row, name))
            {
                GetData(subnode).InnerText = subnode.InnerText.Replace("[#" + name + "#]", ToStr(value));
                GetData(subnode).Attributes["ss:Type"].Value = "Number";
            }
        }
        public void Replace(XmlNode row, string name, DateTime value)
        {
            foreach (XmlNode subnode in GetCells(row, name))
            {
                GetData(subnode).InnerText = subnode.InnerText.Replace("[#" + name + "#]", value.ToString("yyyy-MM-dd"));
                GetData(subnode).Attributes["ss:Type"].Value = "DateTime";
            }
        }
        public void ReplaceHTML(string oldVal, string newVal)
        {
            XmlNodeList nodes = this.DocumentElement.SelectNodes(string.Format("//*[local-name()='Cell']/*[local-name()='Data' and contains(.,'{0}')]", oldVal));
            foreach (XmlNode node in nodes)
            {
                string sIn = node.InnerText;
                string sOut = sIn.Replace(oldVal, newVal);
                node.InnerXml = sOut;
                node.ParentNode.ParentNode.Attributes.RemoveNamedItem("Hidden", ssNameSpace);

                XmlAttribute attr = this.CreateAttribute("xmlns");
                attr.Value = @"http://www.w3.org/TR/REC-html40";
                node.Attributes.Append(attr);
            }
        }
        public void Replace(string oldVal, string newVal)
        {
            if (newVal == "Null") newVal = "";
            XmlNodeList nodes = this.DocumentElement.SelectNodes(string.Format("//*[local-name()='Cell']/*[local-name()='Data' and contains(.,'{0}')]", oldVal));
            foreach (XmlNode node in nodes)
            {
                string sIn = node.InnerText;
                string sOut = sIn.Replace(oldVal, newVal);
                node.InnerText = sOut;
                node.ParentNode.ParentNode.Attributes.RemoveNamedItem("Hidden", ssNameSpace);
            }
        }
        public void Replace(string oldVal, decimal newDecVal)
        {
            XmlNodeList nodes = this.DocumentElement.SelectNodes(string.Format("//*[local-name()='Cell']/*[local-name()='Data' and contains(.,'{0}')]", oldVal));
            foreach (XmlNode node in nodes)
            {
                node.InnerText = ToStr(newDecVal);
                //XmlAttribute aType = node.Attributes["ss:Type"];
                node.Attributes["ss:Type"].Value = "Number";
                node.ParentNode.ParentNode.Attributes.RemoveNamedItem("Hidden", ssNameSpace);
            }
        }
        public void Replace(string oldVal, Double newDblVal)
        {
            XmlNodeList nodes = this.DocumentElement.SelectNodes(string.Format("//*[local-name()='Cell']/*[local-name()='Data' and contains(.,'{0}')]", oldVal));
            foreach (XmlNode node in nodes)
            {
                node.InnerText = ToStr(newDblVal);
                node.Attributes["ss:Type"].Value = "Number";
                node.ParentNode.ParentNode.Attributes.RemoveNamedItem("Hidden", ssNameSpace);
            }
        }
        public void Replace(string oldVal, DateTime newDateVal)
        {
            XmlNodeList nodes = this.DocumentElement.SelectNodes(string.Format("//*[local-name()='Cell']/*[local-name()='Data' and contains(.,'{0}')]", oldVal));
            foreach (XmlNode node in nodes)
            {
                node.InnerText = newDateVal.ToString("yyyy-MM-dd");
                node.Attributes["ss:Type"].Value = "DateTime";
                node.ParentNode.ParentNode.Attributes.RemoveNamedItem("Hidden", ssNameSpace);
            }
        }
        #endregion

        #region Show, Print or SaveAs Excel
        private string saveXML()
        {
            string str = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string fileName = str + @"\Report.xml";
            string txtReport = this.OuterXml.Replace(@"\n", @"&#10;");
            using (TextWriter writer = new StreamWriter(fileName))
            {
                writer.Write(txtReport);
            }
            return fileName;
        }
        public void SaveAsXml(string xmlFileName)
        {
            using (TextWriter writer = new StreamWriter(xmlFileName))
            {
                string txtReport = this.OuterXml.Replace(@"\n", @"&#10;");
                writer.Write(txtReport);
            }
        }
        public void Show()
        {
            string file = saveXML();
            ProcessStartInfo si = new ProcessStartInfo("excel.exe");
            si.Arguments = "/e /r \"" + file + "\"";
            Process.Start(si);
        }
        public void Print()
        {
            string fileName = saveXML();
            doProcess("PrintAsExcel", fileName, "", ProcessWindowStyle.Minimized);
        }
        public void SaveAs(string xls)
        {
            string fileName = saveXML();
            doProcess("SaveAsExcel", fileName, "\"" + xls + "\"", ProcessWindowStyle.Hidden);
        }

        private void doProcess(string verbExec, string fileName, string arguments, ProcessWindowStyle windowStyle)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            foreach (string verb in startInfo.Verbs)
                if (verb.Equals(verbExec))
                {
                    startInfo.Verb = verbExec;
                    startInfo.Arguments = arguments;
                    startInfo.UseShellExecute = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.WindowStyle = windowStyle;
                    Process newProcess = new Process();
                    newProcess.StartInfo = startInfo;


                    newProcess.Start();
                    //newProcess.WaitForExit();
                    return;
                }
            throw new Exception("Для xml-файлов не прописана команда " + verbExec);
        }
        #endregion
    }
    public enum CellType : int { Number = 0, DateTime, Boolean, String, Xml, Error };
    public enum XlsResourceType { Embedded, FileName, IsString };
}

