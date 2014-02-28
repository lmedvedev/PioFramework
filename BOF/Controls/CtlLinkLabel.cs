using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace BOF
{
    public class CtlLinkLabel : LinkLabel
    {
        #region HelpLabel
        const string _helplabeltext = "Стрелка вверх или влево - предыдущее поле. Enter, стрелка вниз или вправо - следующее поле. Пробел - активировать ссылку.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }

        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
            }
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null) frm.WriteHelp("");
            base.OnLeave(e);
        }

        #endregion
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                return true;
            }
            else if(keyData == Keys.Space)
                this.OnClick(EventArgs.Empty);
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
