using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class CtlString : TextBox, IDataMember
    {
        public CtlString()
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

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            BackColor = (Enabled) ? SystemColors.Window : SystemColors.Control;
        }


        private bool _Capitalize = false;
        [Browsable(true)]
        [DefaultValue(false)]
        public bool Capitalize
        {
            get { return _Capitalize; }
            set { _Capitalize = value; }
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            OnValueChanged(e);
        }
        protected virtual void OnValueChanged(EventArgs e)
        {
            FireValueChanged();
        }
        private void FireValueChanged()
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");

            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Capitalize)
            {
                string letter = e.KeyChar.ToString();
                int pos = this.SelectionStart;
                if (Char.IsLetter(e.KeyChar) && Char.IsLower(e.KeyChar))
                {
                    if (pos == Text.Length
                        && (Text.Length == 0 || Text[Text.Length - 1].ToString() == " "))
                        //mustCapitalize = true;
                        e.KeyChar = Char.ToUpper(e.KeyChar);
                }
            }
            base.OnKeyPress(e);
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
            {
                DatBinding bind = new DatBinding("Value", datasource, DataMember);//, false, DataSourceUpdateMode.OnValidation);
                this.DataBindings.Add(bind);

                //Binding bind = this.DataBindings.Add("Value", datasource, DataMember);//, false, DataSourceUpdateMode.OnPropertyChanged);
                //bind.Parse += new ConvertEventHandler(bind_Parse);
            }
        }


        protected override void OnBindingContextChanged(EventArgs e)
        {
            base.OnBindingContextChanged(e);
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
