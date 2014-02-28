using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class ListForm : Form
    {
        public ListForm()
        {
            InitializeComponent();
            statusBar.ShowItemToolTips = true;
        }
        public ToolStripContentPanel ContentPanel
        {
            get { return this.tsContainer.ContentPanel; }
        }
        public StatusStrip StatusBar
        {
            get { return this.statusBar; }
        }
        public ToolStrip FToolBar
        {
            get { return this.toolBar; }
        }
        public ToolStripContainer TSContainer
        {
            get { return this.tsContainer; }
        }

        private FormController _formController;
        public FormController FormController
        {
            get { return _formController; }
            set { _formController = value; }
        }
    }
}