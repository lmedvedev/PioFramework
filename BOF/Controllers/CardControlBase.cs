using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class CardControlBase : UserControl, IValue
    {
        public CardControlBase()
        {
            InitializeComponent();
        }

        #region IValue Members

        public event ValueEventHandler ValueChanged;

        private object _Value;
        public virtual object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }


        public void FireValueChanged()
        {
            if (ValueChanged != null) ValueChanged(this, new ValueEventArgs(Value));
        }

        #endregion

        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            if (m.Msg == WM_PAINT)
            {
                Graphics g = Graphics.FromHwnd(m.HWnd);
                ControlPaint.DrawBorder(g, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
                g.Dispose();
            }
            base.WndProc(ref m);
        }

        #region IValue Members

        public bool ValueEventDisabled
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion
    }
}
