using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BO;
//using HR;
//using HRF;

namespace BOF
{
    public partial class PassPhraseForm : Form
    {
        private bool _change = false;
        public PassPhraseForm(bool Change)
        {
            _change = Change;
            InitializeComponent();

            labelNewPhrase.Visible = _change;
            textNewPhrase.Visible = _change;

            labelOldPhrase.Text = string.Format("”кажите {0} кодовую фразу", (_change) ? " старую ": " заданную ");
            buttonCancel.Text = DialogResult.Cancel.ToString();
            buttonOK.Text = DialogResult.OK.ToString();
        }

        private void PassPhraseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (_change)
                {
                    CryptHelper.Helper.ChangeKey(textOldPhrase.Text, textNewPhrase.Text);
                }
                else
                {
                    CryptHelper.Helper.SetKey(textOldPhrase.Text);
                }
                this.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "ќшибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}