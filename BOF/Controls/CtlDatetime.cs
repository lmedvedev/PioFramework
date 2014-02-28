using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public enum CtlDateTimeType { DateOnly, TimeOnly, DateAndTime }
    public partial class CtlDateTime : DateTimePicker, IDataMember
    {
        public CtlDateTime()
        {
            base.Format = DateTimePickerFormat.Custom;
            base.Size = new Size(80, 20);
            //base.MinDate = new DateTime(1900, 1, 1);
            Value = DateTime.MinValue;
        }

        #region Fields
        private CtlDateTimeType _Type = CtlDateTimeType.DateOnly;
        private string _DataMember = null;
        #endregion

        [Category("BOF")]
        [DefaultValue(CtlDateTimeType.DateOnly)]
        public CtlDateTimeType Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                PaintDateTime();
            }
        }

        //[ReadOnly(true), Browsable(false)]
        //new public DateTime MinDate
        //{
        //    get
        //    {
        //        return base.MinDate;
        //    }
        //    set
        //    {
        //        base.MinDate = value;
        //    }
        //}

        [ReadOnly(true), Browsable(false)]
        new public string CustomFormat
        {
            get
            {
                return base.CustomFormat;
            }
            set
            {
                base.CustomFormat = value;
            }
        }

        [ReadOnly(true), Browsable(false)]
        new public DateTimePickerFormat Format
        {
            get
            {
                return base.Format;
            }
            set
            {
                if (base.Format != DateTimePickerFormat.Custom)
                    base.Format = DateTimePickerFormat.Custom;
            }
        }

        [ReadOnly(true), Browsable(false)]
        new public bool Checked
        {
            get
            {
                return base.Checked;
            }
            set
            {
                base.Checked = value;
            }
        }

        [Category("BOF")]
        [Bindable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [DefaultValue(0)]
        new public DateTime Value
        {
            get
            {
                DateTime dt;
                if (!Checked)
                    dt = DateTime.MinValue;
                else
                    dt = base.Value;
                if (Type == CtlDateTimeType.DateOnly)
                    dt = dt.Date;
                return dt;
            }
            set
            {
                bool dateok = (value != DateTime.MinValue);
                if (dateok)
                {
                    Checked = true;
                    if (Type == CtlDateTimeType.DateOnly)
                    {
                        if (base.Value != value.Date)
                            base.Value = value.Date;
                    }
                    else
                    {
                        if (base.Value != value)
                            base.Value = value;
                    }
                }
                else
                {
                    Checked = (Type == CtlDateTimeType.TimeOnly);
                    //if (base.Value != MinDate)
                    //    base.Value = MinDate;
                }
                //PaintDateTime();            

            }
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            WriteValue();
            //PaintDateTime();
        }
        protected override void OnDropDown(EventArgs eventargs)
        {
            if (!Checked)
            {
                Checked = true;
                WriteValue();
                PaintDateTime();
            }
             base.OnDropDown(eventargs);
        }
        private void PaintDateTime()
        {
            //if (base.Format != DateTimePickerFormat.Custom)
            //    base.Format = DateTimePickerFormat.Custom;
            if (/*base.Value == MinDate || */!Checked)
            {
                switch (Type)
                {
                    case CtlDateTimeType.DateOnly:
                        if (base.CustomFormat != "??.??.????")
                            base.CustomFormat = "??.??.????";
                        break;
                    case CtlDateTimeType.TimeOnly:
                        if (base.CustomFormat != "HH:mm")
                            base.CustomFormat = "HH:mm";
                        break;
                    case CtlDateTimeType.DateAndTime:
                        if (base.CustomFormat != "??.??.???? ??:??")
                            base.CustomFormat = "??.??.???? ??:??";
                        break;
                }
            }
            else
            {
                switch (Type)
                {
                    case CtlDateTimeType.DateOnly:
                        //if (base.Format != DateTimePickerFormat.Short)
                        //    base.Format = DateTimePickerFormat.Short;
                        if (base.CustomFormat != "dd.MM.yyyy")
                            base.CustomFormat = "dd.MM.yyyy";
                        break;
                    case CtlDateTimeType.TimeOnly:
                        //if (base.Format != DateTimePickerFormat.Custom)
                        //    base.Format = DateTimePickerFormat.Custom;
                        if (base.CustomFormat != "HH:mm")
                            base.CustomFormat = "HH:mm";
                        break;
                    case CtlDateTimeType.DateAndTime:
                        //if (base.Format != DateTimePickerFormat.Custom)
                        //    base.Format = DateTimePickerFormat.Custom;
                        if (base.CustomFormat != "dd.MM.yyyy HH:mm")
                            base.CustomFormat = "dd.MM.yyyy HH:mm";
                        break;
                }
            }
        }
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            base.WndProc(ref m);
            if (m.Msg == WM_PAINT)
            {
                PaintDateTime();
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.D0:
                        Value = DateTime.MinValue;
                        WriteValue();
                        PaintDateTime();
                        break;
                    case Keys.Space:
                        Value = DateTime.Now;
                        WriteValue();
                        PaintDateTime();
                        break;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
            }
            base.OnKeyDown(e);
        }

        #region IDataMember Members

        [ReadOnly(true), Browsable(false)]
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))// && this.DataBindings["Value"] == null)
                this.DataBindings.Add("Value", datasource, DataMember);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings.Remove(this.DataBindings["Value"]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings["Value"] != null)
                this.DataBindings["Value"].WriteValue();
        }
        #endregion


        #region HelpLabel
        
        const string _helplabeltext = "Ввод даты. Ctrl+0 - сброс; Ctrl+пробел - тек.дата. Enter - следующее поле.";
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
