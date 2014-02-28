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
    public partial class CtlDecimal : NumericUpDown, IDataMember
    {
        public CtlDecimal() 
        {
            this.DecimalPlaces = 2;
            this.Increment = 1;
            this.Minimum = decimal.MinValue;
            this.Maximum = decimal.MaxValue;
        }
        new public bool ReadOnly
        {
            get { return !this.Enabled; }
            set { this.Enabled = !value; }
        }

        new public decimal Value
        {
            get { return Common.Round(base.Value, DecimalPlaces); }
            set { base.Value = value; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return)// || (keyData == Keys.Down))
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                /*if (keyData == Keys.Return) */return true;
            }
            //Console.WriteLine(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //Console.WriteLine(e.KeyChar);
            char dot = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
            if (e.KeyChar == ',' || e.KeyChar == '.')
                e.KeyChar = dot;
            base.OnKeyPress(e);
        }

        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");
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
            if (!string.IsNullOrEmpty(DataMember))// && this.DataBindings["Value"] == null)
                this.DataBindings.Add("Value", datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if(frm !=null)
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

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CtlDecimal
            // 
            this.DecimalPlaces = 2;
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
        #region HelpLabel
        const string _helplabeltext = "Shift+Tab - предыдущее поле. Enter, Tab - следующее поле.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("“екст, который будет показыватьс€ при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }

        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
            }
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null) frm.WriteHelp("");
            base.OnLeave(e);
        }

        #endregion
    }
}
