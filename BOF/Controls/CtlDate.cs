using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace BOF
{
    [Designer(typeof(LineDesigner))]
    public partial class CtlDate : UserControl, IDataMember
    {
        private static ErrorProvider ErrDate = new ErrorProvider();
        private DropDownCalendarService service;
        //private ToolTip toolTip = new ToolTip();

        public CtlDate()
        {
            InitializeComponent();
            this.txtDate.TypeValidationCompleted += new TypeValidationEventHandler(txtDate_TypeValidationCompleted);
            this.txtDate.KeyPress += new KeyPressEventHandler(txtDate_KeyPress);
            this.txtDate.KeyDown += txtDate_KeyDown;
            this.btn.Click += new System.EventHandler(this.btnDropDown_Click);

            //this.txtDate.Resize += txtDate_Resize;
            //toolTip.IsBalloon = true;
            btn.Image = global::BOF.Properties.Resources.Calendar.ToBitmap();
            btn.SizeMode = PictureBoxSizeMode.CenterImage;

            ChangeSize();
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn = new System.Windows.Forms.PictureBox();
            this.txtDate = new BOF.CtlDate.MaskDate();
            ((System.ComponentModel.ISupportInitialize)(this.btn)).BeginInit();
            this.SuspendLayout();
            // 
            // btn
            // 
            this.btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn.Location = new System.Drawing.Point(63, 1);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(24, 18);
            this.btn.TabIndex = 4;
            this.btn.TabStop = false;
            // 
            // txtDate
            // 
            this.txtDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDate.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite;
            this.txtDate.Location = new System.Drawing.Point(0, 0);
            this.txtDate.Margin = new System.Windows.Forms.Padding(0);
            //this.txtDate.Mask = "90/90/0099";
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(61, 13);
            this.txtDate.TabIndex = 0;
            this.txtDate.ValidatingType = typeof(System.DateTime);
            // 
            // CtlDate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtDate);
            this.Controls.Add(this.btn);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CtlDate";
            this.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.Size = new System.Drawing.Size(87, 19);
            ((System.ComponentModel.ISupportInitialize)(this.btn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        new public bool Enabled
        {
            get { return txtDate.Enabled; }
            set 
            {
                txtDate.Enabled = btn.Enabled = value;
                this.BackColor = value ? txtDate.BackColor : SystemColors.Control;
            }
        }

        private MaskDate txtDate;
        private PictureBox btn;

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ChangeSize();
        }

        private void ChangeSize()
        {
            int h = FontHeight + (SystemInformation.BorderSize.Height * 4) + 3;
            btn.Width = 18;
            int w = txtDate.PreferredSize.Width + btn.Width;
            this.Size = new Size(w, h);
        }

        void txtDate_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Enter:
                    Form frm = this.FindForm();
                    if (frm != null)
                        frm.SelectNextControl(this, true, true, true, true);
                    break;
                case Keys.Down | Keys.Control:
                case Keys.Down | Keys.Alt:
                    btnDropDown_Click(this, EventArgs.Empty);
                    break;
                case Keys.Left:
                    int start = txtDate.SelectionStart;

                    if (txtDate.IsKeyBack && start > 0)
                    {
                        char[] chars = new string('_', 10).ToCharArray();
                        txtDate.Text.CopyTo(0, chars, 0, txtDate.Text.Length);
                        char ch = chars[start - 1];
                        if (char.IsDigit(ch))
                        {
                            chars[start - 1] = ' ';
                            string s = new string(chars);
                            txtDate.Text = s;
                            txtDate.SelectionStart = start;
                        }
                    }
                    break;
            }
        }
        void txtDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            ErrDate.Clear();
        }

        private void SetError()
        {
            //toolTip.ToolTipTitle = "Ошибка при вводе дате";
            //toolTip.Show("Сброс даты - Ctrl+Del, \nТекущая - Ctrl+Пробел.", txtDate, Width, Height, 5000);
            ErrDate.SetError(this, "Ошибка при вводе даты!\r\nСброс даты - Ctrl+Del,\r\nТекущая - Ctrl+Пробел.");
        }

        void txtDate_TypeValidationCompleted(object sender, TypeValidationEventArgs e)
        {
            ErrDate.Clear();
            if (!e.IsValidInput)
            {
                if (txtDate.Text.Replace(txtDate.DateSeparator, "").Trim() == "")
                {
                    Value = DateTime.MinValue;
                }
                else
                {
                    SetError();
                    e.Cancel = true;
                }
            }
            else
            {
                Value = (DateTime)e.ReturnValue;
            }
        }

        DateTime _Value = DateTime.MinValue;

        [Category("BOF")]
        [Bindable(true)]
        public DateTime Value
        {
            get { return _Value; }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    if (value == DateTime.MinValue || value == new DateTime(1900, 1, 1)) 
                        txtDate.ResetText();
                    else
                    {
                        txtDate.Text = Value.ToShortDateString();
                        //SendKeys.Send("{HOME}");
                    }
                    FireValueChanged();
                }
            }
        }

        [Category("BOF"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }
        public void FireValueChanged()
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");

            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }

        #region IDataMember Members
        public event EventHandler ValueChanged;

        private string _DataMember = null;

        [ReadOnly(true), Browsable(false)]
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember))// && this.DataBindings["Value"] == null)
                this.DataBindings.Add("Value", datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
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

        const string _helplabeltext = "Ввод даты. Ctrl+Del - сброс; Ctrl+пробел - тек.дата. Enter - следующее поле.";
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

        private void btnDropDown_Click(object sender, EventArgs e)
        {
            ErrDate.Clear();
            if (service == null)
            {
                service = new DropDownCalendarService(this);
            }

            DateTime dt = DateTime.Today;
            DateTime.TryParse(txtDate.Text, out dt);
            if (dt < service.Calendar.MinDate)
                dt = DateTime.Today;
            service.Calendar.SetDate(dt);
            service.DropDownControl(service.dropDownForm);
        }

        public class MaskDate : MaskedTextBox
        {
            public MaskDate()
            {
                System.Globalization.DateTimeFormatInfo dtfi = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
                string pattern = dtfi.ShortDatePattern;

                if (!string.IsNullOrEmpty(pattern)
                    && (pattern.ToLower().IndexOf("yyyy") > 0
                        || pattern.ToLower().IndexOf("гггг") > 0
                        || pattern.Length > 8
                        )
                    )
                    Mask = "90/90/0099";
                else
                    Mask = "90/90/99";
                ValidatingType = typeof(System.DateTime);
                DateSeparator = dtfi.DateSeparator;
            }
            public string DateSeparator;
            public bool IsKeyBack = false;
            protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
            {
                if (keyData == Keys.Delete)
                {
                    msg.Msg = 258;
                    msg.WParam = (IntPtr)32;
                    keyData = Keys.Space;
                }
                else if (keyData == (Keys.Delete | Keys.Control))
                {
                    this.ResetText();
                    return true;
                }
                else if (keyData == (Keys.Space | Keys.Control))
                {
                    this.Text = DateTime.Today.ToShortDateString();
                    SendKeys.Send("{HOME}");
                    return true;
                }
                else if (keyData == Keys.Back)
                {
                    msg.WParam = (IntPtr)37;
                    keyData = Keys.Left;
                    IsKeyBack = true;
                }
                else if (keyData == Keys.Left)
                {
                    IsKeyBack = false;
                }
                return base.ProcessCmdKey(ref msg, keyData);
            }
            protected override void OnEnter(EventArgs e)
            {
                base.OnEnter(e);
                SendKeys.Send("{HOME}");
            }
        }

        public class DropDownCalendarService : IWindowsFormsEditorService
        {
            public MonthCalendar Calendar;
            private CtlDate editor;
            public DropDownForm dropDownForm;

            public DropDownCalendarService(CtlDate ctl)
            {
                editor = ctl;
                Calendar = new MonthCalendar();

                dropDownForm = new DropDownForm(Calendar);
                Calendar.DateSelected += new DateRangeEventHandler(DropDownCtl_DateSelected);
                Calendar.KeyDown += new KeyEventHandler(DropDownCtl_KeyDown);
                dropDownForm.Deactivate += new EventHandler(dropDownForm_Deactivate);
            }

            void dropDownForm_Deactivate(object sender, EventArgs e)
            {
                CloseDropDown();
            }


            #region IWindowsFormsEditorService Members

            public void DropDownControl(Control ctl)
            {
                Form frm = ctl as Form;

                // location in screen coordinate
                Point location = editor.Parent.PointToScreen(new Point(editor.Left, editor.Bottom));

                // check the form is in the screen working area
                Rectangle swa = Screen.FromControl(editor).WorkingArea;
                location.X = Math.Min(swa.Right - dropDownForm.Width, Math.Max(swa.X, location.X));

                if (frm.Height + location.Y + editor.Height > swa.Bottom)
                    location.Y = location.Y - frm.Height - editor.Height - 1;

                frm.Location = location;
                frm.Show();
            }

            public virtual void CloseDropDown()
            {
                if (dropDownForm != null)
                {
                    dropDownForm.Hide();

                    if (editor.Visible)
                        editor.Focus();
                }
            }

            public DialogResult ShowDialog(Form dialog)
            {
                return DialogResult.Cancel;
            }

            #endregion


            void DropDownCtl_KeyDown(object sender, KeyEventArgs e)
            {
                switch (e.KeyData)
                {
                    case Keys.Escape:
                        CloseDropDown();
                        break;
                    case Keys.Enter:
                        DateTime dt = Calendar.SelectionEnd;
                        editor.Select();
                        editor.Value = dt;
                        CloseDropDown();
                        break;
                }
            }

            void DropDownCtl_DateSelected(object sender, DateRangeEventArgs e)
            {
                DateTime dt = e.End;
                editor.Select();
                editor.Value = dt;
                CloseDropDown();
            }
        }

        public class DropDownForm : Form
        {
            private Control currentControl;
            public DropDownForm(Control control)
            {
                StartPosition = FormStartPosition.Manual;
                FormBorderStyle = FormBorderStyle.FixedSingle;
                ShowInTaskbar = false;
                KeyPreview = true;
                ControlBox = false;
                MinimizeBox = false;
                MaximizeBox = false;
                Text = "";
                currentControl = control;
                currentControl.Dock = DockStyle.Fill;
                Controls.Add(currentControl);
                TopMost = true;
                this.ClientSize = currentControl.Size;
            }

            protected override void OnLoad(EventArgs e)
            {
                this.ClientSize = currentControl.Size;
                this.MaximumSize = this.Size;
                this.MinimumSize = this.Size;
                base.OnLoad(e);
            }
        }

    }
}