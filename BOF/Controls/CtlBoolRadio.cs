using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class CtlBoolRadio : UserControl, IDataMember
    {
        private RadioButton Check1 = new System.Windows.Forms.RadioButton();
        private RadioButton Check2 = new System.Windows.Forms.RadioButton();

        public CtlBoolRadio() 
        {
            this.Controls.Add(Check1);
            this.Controls.Add(Check2);
            Check1.Checked = true;
            Check1.CheckedChanged += new EventHandler(Check1_CheckedChanged);
            Check2.CheckedChanged += new EventHandler(Check2_CheckedChanged);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                Check1.BackColor = value;
                Check2.BackColor = value;
                base.BackColor = value;
            }
        }
        public FlatStyle FlatStyle
        {
            get
            {
                return Check1.FlatStyle;
            }
            set
            {
                Check2.FlatStyle = Check1.FlatStyle = value;
            }
        }
        void Check1_CheckedChanged(object sender, EventArgs e)
        {
            if (ValueEventDisabled) return;
            ValueEventDisabled = true;
            Check2.Checked = !Check1.Checked;
            ValueEventDisabled = false;
            FireValueChanged();
        }

        void Check2_CheckedChanged(object sender, EventArgs e)
        {
            if (ValueEventDisabled) return;
            ValueEventDisabled = true;
            Check1.Checked = !Check2.Checked;
            ValueEventDisabled = false;
            FireValueChanged();
        }
        [Category("BOF")]
        public bool ReadOnly
        {
            get { return !Check1.Enabled; }
            set { Check1.Enabled = Check2.Enabled = !value; }
        }

        [Category("BOF")]
        public string TextTrue
        {
            get { return Check1.Text; }
            set { Check1.Text = value; }
        }

        [Category("BOF")]
        public string TextFalse
        {
            get { return Check2.Text; }
            set { Check2.Text = value; }
        }

        public enum RadioButtonsCaptionAlignment { Left, Right }
        RadioButtonsCaptionAlignment _RadioButtonsCaptionAlign = RadioButtonsCaptionAlignment.Right;

        [Category("BOF")]
        public RadioButtonsCaptionAlignment RadioButtonsCaptionAlign
        {
            get { return _RadioButtonsCaptionAlign; }
            set
            {
                _RadioButtonsCaptionAlign = value;
                Redraw();
            }
        }

        public enum RadioButtonsPositionType { Vertical, Horisontal }
        RadioButtonsPositionType _RadioButtonsPosition = RadioButtonsPositionType.Vertical;

        [Category("BOF")]
        public RadioButtonsPositionType RadioButtonsPosition
        {
            get { return _RadioButtonsPosition; }
            set 
            { 
                _RadioButtonsPosition = value;
                Redraw();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Redraw();
        }

        void Redraw()
        {
            Check1.RightToLeft = Check2.RightToLeft = (RadioButtonsCaptionAlign == RadioButtonsCaptionAlignment.Right) ? RightToLeft.No : RightToLeft.Yes;
            if (RadioButtonsPosition == RadioButtonsPositionType.Vertical)
            {
                Check1.Height = Check2.Height = (int)(Height / 2);
                Check1.Width = Check2.Width = Width;
                Check1.Location = new Point(0, 0);
                Check2.Location = new Point(0, Check1.Height);
                Check1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                Check2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            }
            else
            {
                Check1.Height = Check2.Height = Height;
                Check1.Width = Check2.Width = (int)(Width / 2);
                Check1.Location = new Point(0, 0);
                Check2.Location = new Point(Check1.Width, 0);
                Check1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
                Check2.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            }
        }

        #region IDataMember Members
        private string _DataMember;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))
                this.DataBindings.Add("Value", datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings.Remove(this.DataBindings["Value"]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings["Value"].WriteValue();
        }
        #endregion

        #region IValue Members
        bool _ValueEventDisabled;
        [Category("BOF")]
        public bool ValueEventDisabled
        {
            get { return _ValueEventDisabled; }
            set { _ValueEventDisabled = value; }
        }

        public event EventHandler ValueChanged;
        [Category("BOF")]
        public bool Value
        {
            get { return Check1.Checked; }
            set { Check1.Checked = value; }
        }

        public void FireValueChanged()
        {
            if (ValueChanged != null && !ValueEventDisabled) ValueChanged(this, new ValueEventArgs(Value));
        }
        #endregion
    }
}
