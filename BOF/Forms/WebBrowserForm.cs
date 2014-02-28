using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;
using BO;
using DA;
using System.Xml;
using System.Data.SqlTypes;

namespace BOF
{
    public partial class WebBrowserForm : Form
    {
        ToolStripButton btnNext;
        ToolStripButton btnPrev;
        ToolStripButton btnSave;
        public WebBrowserForm()
        {
            InitializeComponent();

            ToolStripButton btnClose = new ToolStripButton("Закрыть", new Icon(global::BOF.Properties.Resources.CloseForm, 16, 16).ToBitmap());
            btnClose.Click +=new EventHandler(btnClose_Click);
            ToolBar.Items.Add(btnClose);

            btnSave = new ToolStripButton("Сохранить", global::BOF.Properties.Resources.save, null, "Save");
            btnSave.Enabled = false;
            btnSave.Click += new EventHandler(btnSave_Click);
            ToolBar.Items.Add(btnSave);

            ToolStripButton btnPrint = new ToolStripButton("Печать", Icons.printer, null, "Print");
            btnPrint.Click += new EventHandler(btnPrint_Click);
            ToolBar.Items.Add(btnPrint);

            btnPrev = new ToolStripButton("", global::BOF.Properties.Resources.arrow_left, null, "Prev");
            btnPrev.Enabled = false;
            btnPrev.Click += new EventHandler(btnPrev_Click);
            ToolBar.Items.Add(btnPrev);

            btnNext = new ToolStripButton("", global::BOF.Properties.Resources.arrow_right, null, "Next");
            btnNext.Enabled = false;
            btnNext.Click += new EventHandler(btnNext_Click);
            ToolBar.Items.Add(btnNext);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Введите названия сохраняемого файла";
            dlg.FileName = "*.xml";
            dlg.Filter = "XmlData (*.xml)| *.xml|All files (*.*)| *.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter str = new StreamWriter(dlg.FileName))
                {
                    str.Write(btnSave.Tag.ToString());
                }
            }
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            Web.ShowPrintDialog();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Web.GoBack();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Web.GoForward();
        }

        public void AddPage(string html)
        {
            Web.AddPage(html);
        }

        public void AddPage(XmlDocument xml)
        {
            Web.AddPage(xml);
            string str_xml = xml.OuterXml;
            btnSave.Enabled = (str_xml != "");
            if (btnSave.Enabled)
                btnSave.Tag = str_xml;
        }

        public void AddPage(WebBrowserPage page)
        {
            Web.AddPage(page);
        }

        public void AddPage(IXSLTemplate dat)
        {
            Web.AddPage(dat);
        }

        private void Web_PageChanged(object sender, EventArgs e)
        {
            btnPrev.Enabled = Web.CanGoBack;
            btnNext.Enabled = Web.CanGoForward;
            Text = Web.Title;
        }
    }
}