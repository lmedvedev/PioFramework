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
    public class CtlDecimalEncrypted : NumericUpDown, IDataMember
    {

        private bool RememberReadOnly;
        private Color RememberBackColor;
        private Color RememberForeColor;

        public CtlDecimalEncrypted()
        {
            this.DecimalPlaces = 2;
            this.Increment = 1;
            this.Minimum = decimal.MinValue;
            this.Maximum = decimal.MaxValue;
            this.BackColor = Color.Khaki;

            if (!CryptHelper.KeyIsValid())
            {
                this.ReadOnly = true;
                //this.BackColor = SystemColors.Control;
                this.BackColor = Color.Black;
                this.ForeColor = Color.Black;
            }

            RememberReadOnly = this.ReadOnly;
            RememberBackColor = this.BackColor;
            RememberForeColor = this.ForeColor;
        }
        new public bool ReadOnly
        {
            get { return !this.Enabled; }
            set { this.Enabled = !value; }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return || (keyData == Keys.Down))
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

        #region IDataMember Members
        private string _DataMember;
        //private decimal _ValueEnc;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public decimal ValueEnc
        {
            get { return Value; }
            set 
            { 
                Value = value;
                if (value == CryptHelper.HiddenValue)
                {
                    ReadOnly = true;
                    this.BackColor = Color.Black;
                    this.ForeColor = Color.Black;
                }
                else
                {
                    ReadOnly = RememberReadOnly;
                    this.BackColor = RememberBackColor;
                    this.ForeColor = RememberForeColor;
                }
            }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))// && this.DataBindings["Value"] == null)
                this.DataBindings.Add("ValueEnc", datasource, DataMember);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["ValueEnc"] != null)
                this.DataBindings.Remove(this.DataBindings["ValueEnc"]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["ValueEnc"] != null)
                this.DataBindings["ValueEnc"].WriteValue();
        }
        #endregion

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // CtlDecimalEncrypted
            // 
            this.DecimalPlaces = 2;
            this.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
