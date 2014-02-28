using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace BOF
{
    public class CtlPanel : Panel
    {
        public CtlPanel() : base()
        {
            ActiveGradientLowColor = Color.Empty;
            ActiveGradientHighColor = Color.Empty;
            InactiveGradientLowColor = Color.Empty;
            InactiveGradientHighColor = Color.Empty;
        }
        private bool m_active = false;

        private Color m_colorActiveLow = Color.Empty;//FromArgb(255, 165, 78);
        private Color m_colorActiveHigh = Color.Empty;//FromArgb(255, 225, 155);
        private Color m_colorInactiveLow = Color.Empty;//FromArgb(3, 55, 145);
        private Color m_colorInactiveHigh = Color.Empty;//FromArgb(90, 135, 215);

        private LinearGradientBrush m_brushActive;
        private LinearGradientBrush m_brushInactive;

        [DescriptionAttribute("The active state of the caption, draws the caption with different gradient colors.")]
        [DefaultValueAttribute(false)]
        [CategoryAttribute("GradientColors")]
        public bool Active
        {
            get
            {
                return m_active;
            }

            set
            {
                m_active = value;
                Invalidate();
            }
        }

        [DescriptionAttribute("Low color of the active gradient.")]
        [CategoryAttribute("GradientColors")]
        [DefaultValueAttribute(typeof(Color), "255, 165, 78")]
        public Color ActiveGradientLowColor
        {
            get
            {
                return m_colorActiveLow;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(255, 165, 78);
                }
                m_colorActiveLow = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [CategoryAttribute("GradientColors")]
        [DescriptionAttribute("High color of the active gradient.")]
        [DefaultValueAttribute(typeof(Color), "255, 225, 155")]
        public Color ActiveGradientHighColor
        {
            get
            {
                return m_colorActiveHigh;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(255, 225, 155);
                }
                m_colorActiveHigh = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [DefaultValueAttribute(typeof(Color), "165, 199, 255")]
        [DescriptionAttribute("Low color of the inactive gradient.")]
        [CategoryAttribute("GradientColors")]
        public Color InactiveGradientLowColor
        {
            get
            {
                return m_colorInactiveLow;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(165, 199, 255);
                }
                m_colorInactiveLow = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }

        [CategoryAttribute("GradientColors")]
        [DescriptionAttribute("High color of the inactive gradient.")]
        [DefaultValueAttribute(typeof(Color), "239, 247, 255")]
        public Color InactiveGradientHighColor
        {
            get
            {
                return m_colorInactiveHigh;
            }

            set
            {
                if (value == Color.Empty)
                {
                    value = Color.FromArgb(239, 247, 255);
                }
                m_colorInactiveHigh = value;
                CreateGradientBrushes();
                Invalidate();
            }
        }
        private void CreateGradientBrushes()
        {
            // can only create brushes when have a width and height
            if (Width > 0 && Height > 0)
            {
                if (m_brushActive != null)
                {
                    m_brushActive.Dispose();
                }

                m_brushActive = new LinearGradientBrush(ClientRectangle, m_colorActiveHigh, m_colorActiveLow, LinearGradientMode.Vertical);

                if (m_brushInactive != null)
                {
                    m_brushInactive.Dispose();
                }

                m_brushInactive = new LinearGradientBrush(ClientRectangle, m_colorInactiveHigh, m_colorInactiveLow, LinearGradientMode.Vertical);
            }
        }

        // gradient brush for the background
        private LinearGradientBrush BackBrush
        {
            get
            {
                if (m_active)
                    return m_brushActive;
                else
                    return m_brushInactive;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //DrawCaption(e.Graphics);
            if (this.BackBrush != null)
                e.Graphics.FillRectangle(this.BackBrush, this.ClientRectangle);
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            CreateGradientBrushes();
            base.OnResize(e);
        }
        [ReadOnly(true)]
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return Color.Transparent;
            }
        }
    }
}
