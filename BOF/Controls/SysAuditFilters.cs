using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DA;
using BO;

namespace BOF
{
    public partial class SysAuditFilters : Form, ILoadFilterForm
    {
        public SysAuditFilters()
        {
            InitializeComponent();

            ResetToDefault();
            
            this.ShowInTaskbar = false;

            btnApply.Click += new EventHandler(btnApply_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDefault.Click += new EventHandler(btnDefault_Click);

            cmbTable.DataSource = SysAuditSet.GetTablesList();
            cmbTable.SelectedIndex = -1;
        }

        void btnDefault_Click(object sender, EventArgs e)
        {
            ResetToDefault();
            //if (CloseApply != null)
            //    CloseApply(this, EventArgs.Empty);
            //this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CloseCancel != null)
                CloseCancel(this, EventArgs.Empty);
            this.Close();
        }

        void btnApply_Click(object sender, EventArgs e)
        {
            if (CloseApply != null)
                CloseApply(this, EventArgs.Empty);
            this.Close();
        }

        public BaseLoadFilter GetFilter()
        {
            SysAuditFilter flt = new SysAuditFilter();

            flt.DateFrom = ctlFilterDate.DateFrom;
            flt.DateTo = ctlFilterDate.DateTo;
            
            flt.Tbl = (string)cmbTable.SelectedItem;
            if (string.IsNullOrEmpty(flt.Tbl)) flt.Tbl = cmbTable.Text;

            flt.ID = ctlFilterID.Value;
            flt.User = ctlFilterUser.Value;
            flt.Host = ctlFilterHost.Value;
            flt.Application = ctlFilterApplication.Value;
            flt.Server = ctlFilterServer.Value;
            flt.Base = ctlFilterBase.Value;
            
            return flt;
        }

        public event EventHandler CloseCancel;

        public event EventHandler CloseApply;

        new public void Show()
        {
            this.ShowDialog();
        }
        public void ResetToDefault()
        {
            ctlFilterDate.DateFrom = DateTime.Today;
            ctlFilterDate.DateTo = DateTime.Today;
            
            cmbTable.SelectedIndex = -1;
            ctlFilterID.Value = 0;
            ctlFilterUser.Value = "";
            ctlFilterHost.Value = "";
            ctlFilterApplication.Value = "";
            ctlFilterServer.Value = "";
            ctlFilterBase.Value = "";
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == 0x101 && (int)m.WParam == 0xD)
                btnApply.PerformClick();
            return base.ProcessKeyPreview(ref m);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbTable.SelectedIndex = -1;
        }

        public void Init(BaseLoadFilter fl)
        {
            SysAuditFilter flt = fl as SysAuditFilter;
            if (flt != null)
            {
                cmbTable.SelectedText = flt.Tbl;
                ctlFilterID.Value = flt.ID;
                ctlFilterUser.Value =flt.User;
                ctlFilterHost.Value = flt.Host;
                ctlFilterApplication.Value = flt.Application;
                ctlFilterServer.Value = flt.Server;
                ctlFilterBase.Value = flt.Base;
            }
            else
                ResetToDefault();
        }
    }
}