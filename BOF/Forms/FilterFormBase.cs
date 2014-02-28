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
    public partial class FilterFormBase: Form, ILoadFilterForm
    {
        public FilterFormBase()
        {
            InitializeComponent();

            btnApply.Click += new EventHandler(btnApply_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnDefault.Click += new EventHandler(btnDefault_Click);
        }

        void btnDefault_Click(object sender, EventArgs e)
        {
            ResetToDefault();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            if (CloseCancel != null)
                CloseCancel(this, EventArgs.Empty);
            this.Close();
        }

        public void btnApply_Click(object sender, EventArgs e)
        {
            //ResetToDefault();
            try
            {
                this.GetFilter();

                this.Close();
                this.Hide();
                Application.DoEvents();
                if (CloseApply != null)
                    CloseApply(this, EventArgs.Empty);
            }
            catch (System.Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public event EventHandler CloseCancel;

        public event EventHandler CloseApply;

        new public void Show()
        {
            this.ShowDialog();
        }

        public virtual void ResetToDefault()
        {
            BaseLoadFilter flt = GetFilter();
            flt.ResetToDefault();
            Init(flt);
        }

        private bool ctrlPressed = false;
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if ((int)m.WParam == 0x11)
                ctrlPressed = (m.Msg == 0x100);

            Console.WriteLine(m);
            if (m.Msg == 0x101 && (int)m.WParam == 0xD)
            {
                if (ctrlPressed)
                    btnApply.PerformClick();
            }
            return base.ProcessKeyPreview(ref m);
        }
        public virtual BaseLoadFilter GetFilter()
        {
            return null; 
        }

        public virtual void Init(BaseLoadFilter flt)
        {
            
        }
    }
}