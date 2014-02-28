using BO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public class FormController
    {
        public ListForm FForm;
        public FormController(Form mdiForm, string name, string caption, Icon icon)
        {
            FForm = new ListForm();
            FForm.Name = name;
            FForm.Text = caption;
            FForm.Icon = icon;
            FForm.WindowState = FormWindowState.Maximized;
        }
        public virtual void InitFilter(BaseLoadFilter loadfilter) { }
        public virtual void Refresh() { }
        public virtual void Show()
        {
            if (FForm != null)
            {
                FForm.WindowState = FormWindowState.Normal;
                FForm.Show();
                FForm.WindowState = FormWindowState.Maximized;
            }
        }
        public virtual void MergeMenu()
        {
        }
        public ToolStrip FToolBar
        {
            get { return FForm.FToolBar; }
        }
    }
}
