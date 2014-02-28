using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public class CtlTextBox : TextBox, IValue 
    {
        public CtlTextBox()
        {
            //AutoSize = false;
            BorderStyle = BorderStyle.FixedSingle;
            Width = 72;
            Height = 18;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private ToolTip _ToolTip = new ToolTip();
        public ToolTip ToolTip
        {
            get { return _ToolTip; }
            set { _ToolTip = value; }
        }

        private string _NoMask = "";
        private string _Mask = "";
        public virtual string Mask
        {
            get { return _Mask; }
            set { _Mask = value; }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (ValueEventDisabled) return;
            if (Mask != "")
            {
                string new_value = "";
                _NoMask = "";
                int pos = SelectionStart;
                char[] c_Text = Text.ToCharArray();
                char[] cMask = Mask.ToCharArray();
                for (int i = 0; i < cMask.Length; i++)
                {
                    if (i >= c_Text.Length) break;

                    if (c_Text[i] == cMask[i])
                        new_value += cMask[i];
                    else if (c_Text[i] == ' ')
                    {
                        if (i > 0 && i < c_Text.Length - 1 && char.IsDigit(c_Text[i - 1])) new_value += " ";
                    }
                    else if (c_Text[i] == '.' || c_Text[i] == ',')
                    {
                        if (Mask.Contains(".") && !_NoMask.Contains("."))
                        {
                            new_value += ".";
                            _NoMask += ".";
                        }
                    }
                    else if (cMask[i] == 'N' || cMask[i] == '#')
                    {
                        if (char.IsDigit(c_Text[i]) || (cMask[i] == '#' && (c_Text[i] == '-')))
                        {
                            new_value += c_Text[i];
                            _NoMask += c_Text[i];
                        }
                    }
                    else if (cMask[i] == '0')
                    {
                        new_value += c_Text[i];
                        if (i < pos) pos++;
                    }
                    else
                    {
                        new_value += cMask[i];
                        new_value += c_Text[i];
                        if (i < pos) pos++;
                    }
                }
                Text = new_value;
                if (pos > 0) SelectionStart = pos;
            }
            FireValueChanged();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            SelectAll();
        }

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_PAINT = 0x000F;
        //    Color pcolor = (Parent == null) ? SystemColors.Control : Parent.BackColor;
        //    base.WndProc(ref m);
        //    if (m.Msg == WM_PAINT && BorderStyle == BorderStyle.FixedSingle)
        //    {
        //        Graphics g = Graphics.FromHwnd(m.HWnd);
        //        g.PageUnit = GraphicsUnit.Pixel;
        //        Rectangle rect = ClientRectangle;
        //        ControlPaint.DrawBorder(g, rect, pcolor, ButtonBorderStyle.Solid);
        //        rect.Inflate(-1, -1);
        //        ControlPaint.DrawBorder(g, rect, Color.Gray, ButtonBorderStyle.Solid);
        //        Pen pen = new Pen(Color.DarkGray, 1);
        //        g.DrawLine(pen, 1, 0, Width - 2, 0);
        //        g.DrawLine(pen, Width - 1, 1, Width - 1, Height - 2);
        //        g.DrawLine(pen, 1, Height - 1, Width - 2, Height - 1);
        //        g.DrawLine(pen, 0, 1, 0, Height - 2);
        //        g.Dispose();
        //    }
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return || (keyData == Keys.Down && !Multiline))
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                if (keyData == Keys.Return) return true;
            }
            else if (keyData == Keys.Up)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, false, true, true, true);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        #region IValue Members

        public event ValueEventHandler ValueChanged;

        private bool _ValueEventDisabled;
        [Browsable(false)]
        public bool ValueEventDisabled
        {
            get { return _ValueEventDisabled; }
            set { _ValueEventDisabled = value; }
        }

        private object _Value;
        [Browsable(false)]
        public virtual object Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                if (_Value == null) return;
                string val = ToString();
                Text = Multiline ? val : val.Replace("\n", "; ");
                ToolTip.SetToolTip(this, val);
            }
        }

        public virtual void FireValueChanged()
        {
            if (ValueChanged != null && !ValueEventDisabled) ValueChanged(this, new ValueEventArgs(Value));
        }

        #endregion
    }
}
