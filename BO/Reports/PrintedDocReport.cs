using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace BO
{
    public class PrintedDocReport
    {
        public PrintedDocReport()
        {
            string fore_color = "black";

            fore_brush = new SolidBrush(fore_color.StartsWith("#") ? Color.FromArgb(int.Parse(fore_color.Substring(1, 2), NumberStyles.HexNumber), int.Parse(fore_color.Substring(3, 2), NumberStyles.HexNumber), int.Parse(fore_color.Substring(5, 2), NumberStyles.HexNumber)) : Color.FromName(fore_color));
            black_brush = new SolidBrush(Color.FromArgb(0,0,0));
            blackPen = new Pen(fore_brush, 5 / 1000);

            Margin_left = 10f;
            Margin_top = 6f;
            
            pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler
               (this.pd_PrintPage);
            //Margins marg = new Margins(35, 0, 25, 0);
            //pd.DefaultPageSettings.Margins = marg;
            //pd.OriginAtMargins = true;

        }
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            g = ev.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            //ev.PageSettings.Margins = new Margins(35, 0, 25, 0);
            Print(ev);
        }

        protected Pen blackPen;
        protected Brush black_brush;
        protected Brush fore_brush;
        protected Graphics g;
        protected BaseDat Dat;
        public PrintDocument pd;
        float margin_left = 0f;

        public float Margin_left
        {
            get { return margin_left; }
            set { margin_left = value; }
        }
        float margin_top = 0f;

        public float Margin_top
        {
            get { return margin_top; }
            set { margin_top = value; }
        }
        public void Print()
        {
            pd.Print();
        }
        public virtual void Print(PrintPageEventArgs ev)
        {

        }

        public void DrawCheck(bool isChecked, RectangleF rect)
        {
            g.DrawRectangle(blackPen, twipsInUnits(rect.X) + margin_left, twipsInUnits(rect.Y) + margin_top, twipsInUnits(0.2f), twipsInUnits(0.2f));
            if (isChecked)
            {
                g.FillRectangle(black_brush, twipsInUnits(rect.X) + margin_left, twipsInUnits(rect.Y) + margin_top, twipsInUnits(0.2f), twipsInUnits(0.2f));
            }
        }
        public void DrawLabel(string text, Color font_color, Color back_color,  Font font, int TextAlign, float left, float top, float width, float height)
        {
            if (back_color != null && back_color.ToArgb() != Color.FromArgb(255, 255, 255).ToArgb())
            {
                g.FillRectangle(new SolidBrush(back_color), twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
                g.DrawRectangle(blackPen, twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
            }
            StringFormat fmt = new StringFormat();
            fmt.Alignment = GetAlignment(TextAlign);

            g.DrawString(text, font, new SolidBrush(font_color), new RectangleF(twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width) + 2f, twipsInUnits(height)), fmt);


        }
        public void DrawCell(string text, Color font_color, Color border_color, Font font, int TextAlign, float left, float top, float width, float height)
        {
            Pen borderPen = new Pen(new SolidBrush(border_color), 5 / 1000);
            g.DrawRectangle(borderPen, twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
            
            StringFormat fmt = new StringFormat();
            fmt.Alignment = GetAlignment(TextAlign);

            g.DrawString(text, font, new SolidBrush(font_color), new RectangleF(twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height)), fmt);


        }
        public void DrawTextBox(string text, Font font, Brush brush, int TextAlign, int BorderStyle, float left, float top, float width, float height, Color back_color)
        {
            StringFormat fmt = new StringFormat();
            fmt.LineAlignment = GetAlignment(TextAlign);
            //if (BorderStyle == 1)
            //{
            //    g.DrawRectangle(blackPen, twipsInUnits(left) + margin_left - 0.2f, twipsInUnits(top) + margin_top - 0.2f, twipsInUnits(width) + 0.2f, twipsInUnits(height) + 0.2f);
            //}
            g.FillRectangle(new SolidBrush(back_color), twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
            g.DrawString(text, font, brush, new RectangleF(twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height)), fmt);
            if (BorderStyle == 1)
            {
                g.DrawRectangle(blackPen, twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
            }
        }
        public void DrawLine(float left, float top, float width, float height)
        {
            g.DrawLine(blackPen, twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width) + margin_left, twipsInUnits(height) + margin_top);

        }
        public void DrawRectangle(float left, float top, float width, float height, Color back_color)
        {
            if (back_color != null /*&& back_color.ToArgb() != Color.FromArgb(255, 255, 255).ToArgb()*/)
                g.FillRectangle(new SolidBrush(back_color), twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
            g.DrawRectangle(blackPen, twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
        }
        public void DrawImage(string image ,float left, float top, float width, float height)
        {

            System.IO.Stream stream = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceStream("PifBox.Resources." + image);
            g.DrawImage(Image.FromStream(stream), twipsInUnits(left) + margin_left, twipsInUnits(top) + margin_top, twipsInUnits(width), twipsInUnits(height));
        }

        private float twipsInUnits(float num)
        {
            return num *10;
        }
        private RectangleF twipsInUnits(RectangleF rect)
        {
            return new RectangleF(twipsInUnits(rect.X), twipsInUnits(rect.Y), twipsInUnits(rect.Width), twipsInUnits(rect.Height));
        }
        private StringAlignment GetAlignment(int textAlign)
        {
            switch (textAlign)
            {
                case 0:
                case 1:
                case 4:
                    return StringAlignment.Near;
                case 2:
                    return StringAlignment.Center;
                case 3:
                    return StringAlignment.Far;
                default :
                    return StringAlignment.Near;
            }
        }
    }
}
