using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class DifferenceForm : Form
    {
        private string _Value1;
        public string Value1
        {
            get { return _Value1; }
            set { _Value1 = value; }
        }

        private string _Value2;
        public string Value2
        {
            get { return _Value2; }
            set { _Value2 = value; }
        }

        public DifferenceForm()
        {
            InitializeComponent();
        }

        private void ShowValues()
        {
            if (!checkBoxHEX.Checked)
            {
                label1.Text = _Value1;
                label2.Text = _Value2;
            }
            else
            {
                label1.Text = ToHEX(_Value1);
                label2.Text = ToHEX(_Value2);
            }
        }

        private string ToHEX(string s)
        {
            string ret = "";
            char[] a = s.ToCharArray();
            for (int i = 0; i < s.Length; i++)
			{
                ret += string.Format("{0:X} ", Char.ConvertToUtf32(s, i)).PadLeft(4, '0');
            }
            return ret;
        }

        private void checkBoxHEX_CheckedChanged(object sender, EventArgs e)
        {
            ShowValues();
        }

        private void DifferenceForm_Load(object sender, EventArgs e)
        {
            ShowValues();
        }
    }
}