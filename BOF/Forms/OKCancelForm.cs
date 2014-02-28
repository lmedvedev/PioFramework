using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class OKCancelForm : Form, IValue
    {
        public OKCancelForm()
        {
            InitializeComponent();
        }

        private bool _ReadOnly = false;
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                this.btnOK.Enabled = !value;
            }
        }

        protected virtual void GetEntity() { }

        #region IValue Members

        public event ValueEventHandler ValueChanged;

        private bool _ValueEventDisabled;
        public bool ValueEventDisabled
        {
            get { return _ValueEventDisabled; }
            set { _ValueEventDisabled = value; }
        }

        private object _Value;
        public virtual object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }

        public virtual void FireValueChanged()
        {
            if (ValueChanged != null && !ValueEventDisabled) ValueChanged(this, new ValueEventArgs(Value));
        }

        #endregion

        //private void OKCancelForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    if (DialogResult == DialogResult.OK)
        //    {
        //        GetEntity();
        //        if (_Value != null && _Value is BaseDat) ((BaseDat)_Value).Save();
        //    }
        //}

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.Modal) Close();
        }
        protected virtual void ValidateEntity(FormClosingEventArgs e) 
        {
            IValidate iv = Value as IValidate;
            if (iv != null)
            {
                DatErrorList eList = iv.ValidateErrors();
                if (eList != null && eList.Count > 0)
                {
                    string ErrorsMessage = "";
                    foreach (DatErrorList.DatError derr in eList)
                    {
                        ErrorsMessage += derr.Description + "\n";
                    }
                    MessageBox.Show(ErrorsMessage, "Обнаружены ошибки, сохранение невозможно", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                DatErrorList wList = iv.ValidateWarnings();
                if (wList != null && wList.Count > 0)
                {
                    string ErrorsMessage = "";
                    foreach (DatErrorList.DatError derr in wList)
                    {
                        ErrorsMessage += derr.Description + "\n";
                    }
                    ErrorsMessage += "\n\nВы по прежнему хотите сохранить запись?\n";

                    DialogResult r = MessageBox.Show(ErrorsMessage, "Обнаружены замечания", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                    switch (r)
                    {
                        case DialogResult.Abort:
                        case DialogResult.Cancel:
                        case DialogResult.None:
                        case DialogResult.No:
                            e.Cancel = true;
                            return;
                        case DialogResult.OK:
                        case DialogResult.Retry:
                        case DialogResult.Yes:
                        case DialogResult.Ignore:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected virtual void CloseForm(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                GetEntity();
                ValidateEntity(e);
                if (_Value != null && _Value is BaseDat) ((BaseDat)_Value).Save();
            }
        }
        private void OKCancelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseForm(e);
        }
    }
}