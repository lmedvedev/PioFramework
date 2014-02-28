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
    public partial class OKCancelForm2 : Form, IValue, IHelpLabel
    {
        public OKCancelForm2()
        {
            InitializeComponent();
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
        protected virtual void CloseForm(FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                GetEntity();
                //if (_Value != null && _Value is BaseDat) ((BaseDat)_Value).Save();
            }
        }
        private void OKCancelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UnbindControls(this);
                CloseForm(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Common.ExMessage(ex), "Œ¯Ë·Í‡", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BindControls(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember && ctl.DataBindings.Count == 0)
                {
                    ((IDataMember)ctl).AddBinding(Value);
                }
                else
                    BindControls(ctl);
            }
            Application.DoEvents();
        }
        public void MarkErrorControls(Control ctlEntity, DatErrorList eList)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember)
                {
                    DatErrorList.DatError error = eList.Find(delegate(DatErrorList.DatError de) { return de.PropertyName == ((IDataMember)ctl).DataMember; });
                    if (error != null)
                        SetError(ctl, error.Description);
                }
                else
                    MarkErrorControls(ctl, eList);
            }
            Application.DoEvents();
        }
        public void UnbindControls(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember)
                {
                    ((IDataMember)ctl).RemoveBinding();
                }
                else
                    UnbindControls(ctl);
            }
        }
        public void WriteBindingValues(Control ctlEntity)
        {
            foreach (Control ctl in ctlEntity.Controls)
            {
                if (ctl is IDataMember)
                {
                    ((IDataMember)ctl).WriteValue();
                }
                else
                    WriteBindingValues(ctl);
            }
        }
        public void WriteBindingValue(Control ctl)
        {
            if (ctl is IDataMember)
            {
                ((IDataMember)ctl).WriteValue();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //if (keyData == Keys.Enter && Keys.Control )
            //{
            //    this.DialogResult = DialogResult.OK;
            //    OKCancelDatForm_FormClosed(this, FormClosedEventArgs.Empty);
            //    return true;
            //}

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region IHelpLabel Members

        public virtual void WriteHelp(string text)
        {
            lblHelp.Text = text;
        }

        public void SetError(Control ctl, string description)
        {
            DatErrorProvider.SetError(ctl, description);
        }

        #endregion
    }
}