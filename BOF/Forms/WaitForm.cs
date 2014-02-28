using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BOF
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
        }

        delegate void FormDelegate();
        static bool close = false;
        static public void Start()
        {
            Start(null);
        }

        static public void Start(Control parent)
        {
            try
            {
                Rectangle rect = (parent == null) ? Rectangle.Empty : parent.RectangleToScreen(parent.ClientRectangle);
                close = false;
                FormDelegate d = delegate()
                {
                    WaitForm frm = new WaitForm();
                    if (rect != Rectangle.Empty)
                    {
                        frm.StartPosition = FormStartPosition.Manual;
                        frm.Location = new Point(rect.Left + (rect.Width - frm.Width) / 2, rect.Top + (rect.Height - frm.Height) / 2);
                    }
                    System.Windows.Forms.Timer tm = new System.Windows.Forms.Timer();
                    tm.Interval = 50;
                    tm.Tick += new EventHandler(delegate(object sender, EventArgs e)
                    {
                        if (close)
                            frm.Close();
                        else
                            Application.DoEvents();
                    });
                    tm.Start();
                    frm.ShowDialog();
                };
                d.BeginInvoke(null, null);
            }
            catch(Exception exp)
            {
                throw exp;
            }
        }

        static public void Stop()
        {
            close = true;
        }
    }
}