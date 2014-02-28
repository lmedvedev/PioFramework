using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace BOF
{
    public class CtlCaptionPanel : CtlPanel
    {
        private class Consts
        {
            public const int DefaultHeight = 26;
            public const string DefaultFontName = "Tahoma";
            public const int DefaultFontSize = 12;
            public const int PosOffset = 4;
        }

        public CtlCaptionPanel() : base()
        {
            this.Name = "CaptionPanel";
            this.Size = new Size(150, 30);

            // set double buffer styles
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            // init the height
            Height = Consts.DefaultHeight;

            // format used when drawing the text
            m_format.FormatFlags = StringFormatFlags.NoWrap;
            m_format.LineAlignment = StringAlignment.Center;
            m_format.Trimming = StringTrimming.EllipsisCharacter;

            // init the font
            Font = new Font("Tahoma", 12.0F, FontStyle.Bold);

            // create gdi objects
            ActiveTextColor = m_colorActiveText;
            InactiveTextColor = m_colorInactiveText;
            
            ActiveGradientHighColor = Color.PowderBlue;
            ActiveGradientLowColor = Color.SteelBlue;
            InactiveGradientHighColor = Color.SlateBlue;
            InactiveGradientLowColor = Color.RoyalBlue;

        }

        private bool m_antiAlias = true;
        private string m_text = string.Empty;

        private Color m_colorActiveText = Color.Black;
        private Color m_colorInactiveText = Color.White;
        
        private SolidBrush m_brushActiveText;
        private SolidBrush m_brushInactiveText;

        private StringFormat m_format = new StringFormat();
        [CategoryAttribute("Appearance")]
        [DescriptionAttribute("If should draw the text as antialiased.")]
        [DefaultValueAttribute(true)]
        public bool AntiAlias
        {
            get
            {
                return m_antiAlias;
            }

            set
            {
                m_antiAlias = value;
                Invalidate();
            }
        }

        [DefaultValueAttribute(typeof(Color), "Black")]
        [DescriptionAttribute("Color of the text when active.")]
        [CategoryAttribute("Appearance")]
        public Color ActiveTextColor
        {
            get
            {
                return m_colorActiveText;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.Black;
                }
                m_colorActiveText = value;
                m_brushActiveText = new SolidBrush(m_colorActiveText);
                Invalidate();
            }
        }

        [CategoryAttribute("Appearance")]
        [DefaultValueAttribute(typeof(Color), "White")]
        [DescriptionAttribute("Color of the text when inactive.")]
        public Color InactiveTextColor
        {
            get
            {
                return m_colorInactiveText;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.White;
                }
                m_colorInactiveText = value;
                m_brushInactiveText = new SolidBrush(m_colorInactiveText);
                Invalidate();
            }
        }

        // brush used to draw the caption
        private SolidBrush TextBrush
        {
            get
            {
                if (Active)
                    return m_brushActiveText;
                else
                    return m_brushInactiveText;
            }
        }

        public override string Text
        {
            get
            {
                return Caption;
            }

            set
            {
                Caption = value;
            }
        }
        [DescriptionAttribute("Text displayed in the caption.")]
        [DefaultValueAttribute("")]
        [CategoryAttribute("Appearance")]
        public string Caption
        {
            get
            {
                return m_text;
            }

            set
            {
                m_text = value;
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            if (m_antiAlias)
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //SizeF sizeText = g.MeasureString(m_text, Font);

            // need a rectangle when want to use ellipsis
            RectangleF bounds = new RectangleF(Consts.PosOffset, Consts.PosOffset
                , this.DisplayRectangle.Width - Consts.PosOffset, Font.Height);

            g.DrawString(m_text, this.Font, this.TextBrush, bounds, m_format);
            //g.DrawRectangle(new Pen(TextBrush), Consts.PosOffset, Consts.PosOffset
            //    , this.DisplayRectangle.Width - Consts.PosOffset, sizeText.Height);
        }


    }
}
