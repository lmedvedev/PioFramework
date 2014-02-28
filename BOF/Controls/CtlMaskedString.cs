using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class CtlMaskedString : MaskedTextBox, IDataMember
    {
        public CtlMaskedString()
        {
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Value
        {
            get { return Text; }
            set
            {
                if (Text != value)
                    Text = value;
            }
        }

        public void FireValueChanged()
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");

            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Return || (keyData == Keys.Down && !Multiline))
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                if (keyData == Keys.Return) return true;
            }
            else if (keyData == Keys.Up && !Multiline)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, false, true, true, true);
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            SelectionStart = 0;
            base.OnLostFocus(e);
        }
        #region IDataMember Members
        public event EventHandler ValueChanged;
        
        private string _DataMember;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))
                this.DataBindings.Add("Value", datasource, DataMember);//, false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            Binding bnd = this.DataBindings["Value"];
            if (!string.IsNullOrEmpty(DataMember) && bnd != null)
                this.DataBindings.Remove(bnd);
        }

        public void WriteValue()
        {
            Binding bnd = this.DataBindings["Value"];
            if (!string.IsNullOrEmpty(DataMember) && bnd != null)
                bnd.WriteValue();
        }
        #endregion

        #region HelpLabel
        const string _helplabeltext = "Стрелка вверх - предыдущее поле. Enter, стрелка вниз - следующее поле.";
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
    }
}
