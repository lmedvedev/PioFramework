using System;
using System.Windows.Forms;
using BO;
//using DAO;

namespace BOF
{
    public class CtlField : CtlTextBox, ILink
    {
        public CtlField()
        {
            ReadOnly = true;
        }

        protected virtual OKCancelForm CreateForm() { return null; }

        #region ILink Members

        public event EventHandler LinkClicked;

        public void FireLinkClicked()
        {
            if (LinkClicked != null) LinkClicked(this, EventArgs.Empty);
            OKCancelForm frm = CreateForm();
            if (frm != null && frm.ShowDialog(this) == DialogResult.OK) this.Value = frm.Value;
        }

        #endregion
    }
}
