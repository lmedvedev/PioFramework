using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class CtlInt : NumericUpDown, IDataMember
    {
        public CtlInt() 
        {
            this.DecimalPlaces = 0;
            this.Increment = 1;
            this.Minimum = int.MinValue;
            this.Maximum = int.MaxValue;
        }
        new public int Value
        {
            get
            {
                return (int)base.Value;
            }
            set
            {
                base.Value = value ;
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            base.OnValueChanged(e);
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");
        }

        new public bool ReadOnly 
        {
            get {return !this.Enabled;}
            set {this.Enabled = !value ;} 
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
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }
        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
        }
        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))// && this.DataBindings["Value"] == null)
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

        //public void RemoveBinding()
        //{
        //    OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
        //    if (frm != null)
        //        RemoveBinding(frm.NewValue);
        //}
        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings["Value"].WriteValue();
        }
        #endregion
    }
}
