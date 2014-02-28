using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
//using System.

namespace BOF
{
    public partial class RichTextForm : Form
    {
        public RichTextForm()
        {
            InitializeComponent();
        }

        public RichTextForm(string text)
            : this()
        {
            txtMain.Text = text;
        }

        public bool WordWrap
        {
            get { return txtMain.WordWrap; }
            set { txtMain.WordWrap = value; }
        }

        public string Rtf
        {
            get { return txtMain.Rtf; }
            set { txtMain.Rtf = value; }
        }
    }
}