using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class ExceptionForm : Form
    {
        Bitmap img_close = new Icon(Properties.Resources.Symbol_Collapse_Vertical, 16, 16).ToBitmap();
        Bitmap img_open = new Icon(Properties.Resources.Symbol_Expand_Vertical, 16, 16).ToBitmap();

        public ExceptionForm(string header, Exception exp) :
            this(header, null, exp, MessageBoxIcon.Error) { }

        public ExceptionForm(string header, string text, Exception exp, MessageBoxIcon icon)
        {
            InitializeComponent();
            Text = header;
            lblText.Text = (text == null) ? "Для более подробной информации раскройте детали сообщения об ошибке." : text;

            string exMessage = (exp.InnerException != null) ? exp.InnerException.Message : exp.Message;

            if (exMessage.StartsWith("<!DOCTYPE html PUBLIC"))
            {
                errorHTML.AddPage(exMessage);
                tabControlError.TabPages.Remove(tabPageText);
            }
            else
            {
                errorPlain.Value = Common.ExMessage(exp);

                if (!Common.IsNullOrEmpty(exp.Source))
                    errorPlain.Value += "\r\nИсточник:\r\n" + exp.Source;

                if (!Common.IsNullOrEmpty(exp.StackTrace))
                    errorPlain.Value += "\r\nСтэк вызова:\r\n" + exp.StackTrace;

                tabControlError.TabPages.Remove(tabPageHtml);
            }
            
            switch (icon)
            {
                //case MessageBoxIcon.Asterisk:
                //    break;
                case MessageBoxIcon.Error:
                    imgBox.Image = new Icon(Properties.Resources.error, 48, 48).ToBitmap();
                    break;
                //case MessageBoxIcon.Exclamation:
                //    break;
                //case MessageBoxIcon.Hand:
                //    break;
                case MessageBoxIcon.Information:
                    imgBox.Image = new Icon(Properties.Resources.INFO, 48, 48).ToBitmap();
                    break;
                case MessageBoxIcon.None:
                    imgBox.Image = new Icon(Properties.Resources.none, 48, 48).ToBitmap();
                    break;
                case MessageBoxIcon.Question:
                    imgBox.Image = new Icon(Properties.Resources.question2, 48, 48).ToBitmap();
                    break;
                //case MessageBoxIcon.Stop:
                //    break;
                //case MessageBoxIcon.Warning:
                //    break;
                default:
                    imgBox.Image = null;
                    break;
            }
        }

        private void ExceptionForm_Load(object sender, EventArgs e)
        {
            btnDetails_Click(this, EventArgs.Empty);
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            tabControlError.Visible = !tabControlError.Visible;
            btnDetails.Text = (tabControlError.Visible) ? "Скрыть детали" : "Показать детали";
            btnDetails.Image = (tabControlError.Visible) ? img_close : img_open;

            if (tabControlError.Visible && tabControlError.Height < 20)
                tabControlError.Height = 400;

            ClientSize = new Size(ClientSize.Width, tabControlError.Top + (tabControlError.Visible ? tabControlError.Height : 0) + 5);
        }
    }
}
