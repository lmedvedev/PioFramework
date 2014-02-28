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
    public partial class DictForm : OKCancelDatForm
    {
        public DictForm()
        {
            InitializeComponent();
        }

        private void DictForm_Load(object sender, EventArgs e)
        {
            this.Icon = global::BOF.Properties.Resources.document;
            txtSCode.DataMember = "SCode";
            txtName.DataMember = "Name";
            if (NewValue != null && Common.IsNullOrEmpty(this.Text))
                this.Text = NewValue.ToString();
        }
    }
}

