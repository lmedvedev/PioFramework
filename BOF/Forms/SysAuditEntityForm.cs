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
    public partial class SysAuditEntityForm : OKCancelDatForm
    {
        public SysAuditEntityForm()
        {
            InitializeComponent();

            this.CanSave = false;
        }

        private void SysAuditEntity_Load(object sender, EventArgs e)
        {
            if (NewValue != null)
            {
                SysAuditDetailsDat sd = (SysAuditDetailsDat)NewValue;
                ctlOldValue.Value = sd.OldValue;
                ctlNewValue.Value = sd.NewValue;

                this.Text += " : " + sd.FieldName;
            }
        }
    }
}