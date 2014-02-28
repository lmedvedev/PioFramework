using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace BO
{
    public class setReport : PrintedDocReport
    {
        public setReport(): base()
        {

        }

        protected int rowCount;
        protected int pageCount = 1;
        protected float top = 0f;
        
        private List<ReportRow> rows = new List<ReportRow>();
        public List<ReportRow> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        private List<ReportColumn> _columns = new List<ReportColumn>();
        public List<ReportColumn> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }

        private float GetRowHeight()
        {
            float height = 0;
            foreach (ReportColumn col in Columns)
            {
                if (col.Height > height)
                    height = col.Height;
            }
            return height;
        }
        protected virtual float GetFooterHeight()
        {
            return 0;
        }

        public void Print(int count)
        {
            for(int i = 0; i < count; i++)
            {
                Print();
            }
        }

        public override void Print(PrintPageEventArgs ev)
        {
            if (pageCount == 1) PrintReportStart();
            RectangleF printableArea = ev.PageBounds;
            // pd.DefaultPageSettings.PrintableArea;
            
            float header_height = PrintHeader();
            top += header_height;
            top += 0.3f;
            //top += GetFooterHeight();

            float prArea = (float)(printableArea.Height * 2.54 / 100) - (Margin_top / 10) - header_height - GetFooterHeight();
            
            while (this.rowCount < rows.Count)
            {
                ReportRow row = (ReportRow)rows[rowCount];
                float rowHeight = GetRowHeight();
                if (prArea > rowHeight)
                {
                    PrintRow(row,top);
                    top += rowHeight;
                    printableArea.Y = printableArea.Y + rowHeight;
                    //printableArea.Height = printableArea.Height - rowHeight;
                    prArea = prArea - rowHeight;

                    ++rowCount;
                }
                else
                    break;
            }
            PrintFooter(top);
            ++pageCount;
            ev.HasMorePages = (rowCount < rows.Count);
            if (ev.HasMorePages == false) PrintReportEnd();
            //PrintReportEnd();
        }
        protected void PrintRow(ReportRow row,float top)
        {
            float left = 0f;
            int colN = 0;
            foreach (ReportColumn col in Columns)
            {
                Font curFont = col.Font;
                if (row.IsHeader)
                    curFont = new Font(col.Font.FontFamily, col.Font.Size, col.Font.Style | FontStyle.Bold);
                DrawCell(row.Cols[colN], col.Font_color, col.Border_color, curFont, col.TextAlign, left, top, col.Width, col.Height);
                colN++;
                left += col.Width;
            }
        }
        protected virtual float PrintReportStart() { return 0f; }
        protected virtual float PrintReportEnd()
        {
            rowCount = 0;
            pageCount = 1;
            top = 0f;
            return 0f;
        }
        protected virtual float PrintHeader() { return 0f; }
        protected virtual void PrintFooter(float top)
        {
            top = 0f;
        }
    }

    public class ReportColumn
    {
        public ReportColumn() { }
        public ReportColumn(Color font_color, Color border_color, Font font, int TextAlign, float width, float height) 
        {
            _font_color = font_color;
            _border_color = border_color;
            _font = font;
            _TextAlign = TextAlign;
            _width = width;
            _height = height;
        }

        private Color _font_color;

        public Color Font_color
        {
            get { return _font_color; }
            set { _font_color = value; }
        }
        private Color _border_color;

        public Color Border_color
        {
            get { return _border_color; }
            set { _border_color = value; }
        }
        private Font _font;

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }
        private int _TextAlign;

        public int TextAlign
        {
            get { return _TextAlign; }
            set { _TextAlign = value; }
        }

        private float _width;
        private float _height;

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public float Height
        {
            get { return _height; }
            set { _height = value; }
        }
    }
    
    public class ReportRow
    {
        public ReportRow() { }
        private bool _IsHeader = false;

        public bool IsHeader
        {
            get { return _IsHeader; }
            set { _IsHeader = value; }
        }
        private List<string> cols = new List<string>();
        public List<string> Cols
        {
            get { return cols; }
            set { cols = value; }
        }
    }
}
