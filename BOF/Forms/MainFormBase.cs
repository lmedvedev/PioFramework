using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class MainFormBase : Form
    {
        public MainFormBase(bool Mdi)
        {
            InitializeComponent();
            this.IsMdiContainer = Mdi;
            this.DoubleBuffered = true;
        }
    }
}