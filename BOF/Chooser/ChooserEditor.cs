using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using BO;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace BOF
{
    /// <summary>
    /// Control for view/edit Dat-classes in a form.
    /// ≈сли просто его бросить на форму - то он позвол€ет только отображать Dat-класс на форме. 
    /// ƒл€ св€зи с Set-классом используютс€ контроллеры.
    /// ChooserController - дл€ выбора из списка без DropDown. 
    /// DropDownController - дл€ выбора из простого DropDown-списка.
    /// DropDownTreeCardsController - дл€ выбора из древовидного списка.
    /// </summary>
    [Designer(typeof(LineDesigner))]
    public class ChooserEditor : UserControl, IDataMember
    {

        #region Controls
        private TextBox txtMain = new TextBox();
        ChooserButton btnDialog = new ChooserButton(ChooserButtonType.Dialog, "btnDialog");
        ChooserButton btnDropDown = new ChooserButton(ChooserButtonType.DropDown, "btnDropDown");
        ChooserButton btnReset = new ChooserButton(ChooserButtonType.Reset, "btnReset");
        #endregion

        public ChooserEditor()
        {
            InitializeComponent();

            btnDropDown.Click += btnDropDown_Click;
            btnDialog.Click += btnDialog_Click;
            btnReset.Click += btnReset_Click;

            txtMain.ReadOnlyChanged += txtMain_ReadOnlyChanged;
            txtMain.Resize += txtMain_Resize;
            txtMain.TextChanged += new EventHandler(txtMain_TextChanged);

        }

        void InitializeComponent()
        {
            this.Padding = new Padding(2, 1, 0, 0);
            this.Size = new Size(87, 19);
            this.AutoScaleMode = AutoScaleMode.None;
            this.BackColor = SystemColors.Window;
            this.Margin = new Padding(0);

            this.txtMain.BackColor = this.BackColor;
            this.txtMain.BorderStyle = BorderStyle.None;
            this.txtMain.Dock = DockStyle.Fill;
            this.txtMain.Location = new Point(2, 1);
            this.txtMain.Name = "txtMain";
            this.txtMain.TabIndex = 0;
            this.ReadOnly = true;
            txtMain.AutoCompleteMode = AutoCompleteMode.None;

            this.btnDialog.Visible = false;
            this.btnDropDown.Visible = false;

            this.Controls.AddRange(new Control[] { txtMain, btnDialog, btnDropDown, btnReset });
        }

        #region Fields
        //private bool _CanReset = true;

        private string _Format;

        private BaseDat _Value;

        //private bool valueEventDisabled = false;

        #endregion

        #region Members
        bool _Enabled = true;
        new public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                txtMain.ReadOnly = !_Enabled;
                btnReset.Enabled = btnDialog.Enabled = btnDropDown.Enabled = _Enabled;
                BackColor = txtMain.BackColor = (_Enabled) ? SystemColors.Window : SystemColors.InactiveCaptionText;
                TabStop = txtMain.TabStop = btnReset.TabStop = btnDialog.TabStop = btnDropDown.TabStop = _Enabled;
            }
        }        /// <summary>
        /// “екстовое отображение Dat-класса. Ћучше использовать свойство Value. 
        /// </summary>
        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return txtMain.Text; }
            set { txtMain.Text = value; }
        }
        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get { return txtMain.SelectedText; }
            set { txtMain.SelectedText = value; }
        }
        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get { return txtMain.SelectionStart; }
            set { txtMain.SelectionStart = value; }
        }
        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get { return txtMain.SelectionLength; }
            set { txtMain.SelectionLength = value; }
        }
        /// <summary>
        /// ѕозвол€ет запретить изменение значени€ клавиатурой или мышкой.
        /// </summary>
        [Category("Chooser")
        , DefaultValue(true)]
        public bool ReadOnly
        {
            get { return txtMain.ReadOnly; }
            set
            {
                txtMain.ReadOnly = value;
                btnReset.Visible = !value;
            }
        }

        /// <summary>
        /// ‘ормат в том виде, как его поймет Dat.ToString(format)
        /// </summary>
        [Category("Chooser")]
        public string Format
        {
            get { return _Format; }
            set { _Format = value; }
        }

        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual BaseDat Value
        {
            get { return _Value; }
            set
            {
                bool changed = _Value != value;
                if (changed)
                {
                    _Value = value;
                    FireValueChanged();
                }
                WriteDat();
            }
        }

        [Category("Chooser")]
        new public BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set
            {
                base.BorderStyle = value;
                Padding = new Padding(Padding.Left, ((value == BorderStyle.Fixed3D) ? 0 : 1), Padding.Right, Padding.Bottom);
                switch (value)
                {
                    case BorderStyle.Fixed3D:
                        btnDialog.ButtonStyle
                            = btnDropDown.ButtonStyle
                            = btnReset.ButtonStyle
                            = ButtonState.Normal;
                        break;
                    case BorderStyle.FixedSingle:
                    case BorderStyle.None:
                        btnDialog.ButtonStyle
                            = btnDropDown.ButtonStyle
                            = btnReset.ButtonStyle
                            = ButtonState.Flat;
                        break;
                }
            }
        }

        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        #region AutoComplete
        //
        // Summary:
        //     Gets or sets a custom System.Collections.Specialized.StringCollection to
        //     use when the System.Windows.Forms.TextBox.AutoCompleteSource property is
        //     set to CustomSource.
        //
        // Returns:
        //     A System.Collections.Specialized.StringCollection to use with System.Windows.Forms.TextBox.AutoCompleteSource.
        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(0)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return txtMain.AutoCompleteCustomSource; }
            set { txtMain.AutoCompleteCustomSource = value; }
        }
        //
        // Summary:
        //     Gets or sets an option that controls how automatic completion works for the
        //     System.Windows.Forms.TextBox.
        //
        // Returns:
        //     One of the values of System.Windows.Forms.AutoCompleteMode. The values are
        //     System.Windows.Forms.AutoCompleteMode.Append, System.Windows.Forms.AutoCompleteMode.None,
        //     System.Windows.Forms.AutoCompleteMode.Suggest, and System.Windows.Forms.AutoCompleteMode.SuggestAppend.
        //     The default is System.Windows.Forms.AutoCompleteMode.None.
        //
        // Exceptions:
        //   System.ComponentModel.InvalidEnumArgumentException:
        //     The specified value is not one of the values of System.Windows.Forms.AutoCompleteMode.
        [EditorBrowsable(0)]
        [Browsable(true)]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return txtMain.AutoCompleteMode; }
            set { txtMain.AutoCompleteMode = value; }
        }

        //
        // Summary:
        //     Gets or sets a value specifying the source of complete strings used for automatic
        //     completion.
        //
        // Returns:
        //     One of the values of System.Windows.Forms.AutoCompleteSource. The options
        //     are AllSystemSources, AllUrl, FileSystem, HistoryList, RecentlyUsedList,
        //     CustomSource, and None. The default is None.
        //
        // Exceptions:
        //   System.ComponentModel.InvalidEnumArgumentException:
        //     The specified value is not one of the values of System.Windows.Forms.AutoCompleteSource.
        [EditorBrowsable(0)]
        [Browsable(true)]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return txtMain.AutoCompleteSource; }
            set { txtMain.AutoCompleteSource = value; }
        }
        #endregion
        #endregion

        public void WriteText(string text, int selstart, int sellength)
        {
            if (txtMain.Text != text)
                txtMain.Text = text;
            txtMain.SelectionStart = selstart;
            txtMain.SelectionLength = sellength;
        }
        public void WriteText(string text)
        {
            WriteText(text, 0, 0);
        }
        public void WriteDat()
        {
            string text = BaseDat.ToString(_Value, Format);

            WriteText(text, 0, 0);
            txtMain.ForeColor = Color.Black;
        }

        #region Events Handlers
        private EventHandler<EventArgs<BaseDat>> _ValueDropDownHandler = null;
        public event EventHandler<EventArgs<BaseDat>> ValueDropDown
        {
            add
            {
                _ValueDropDownHandler += value;
                btnDropDown.Visible = (_ValueDropDownHandler != null);
            }
            remove
            {
                _ValueDropDownHandler -= value;
                btnDropDown.Visible = (_ValueDropDownHandler != null);
            }
        }
        public bool FireValueDropDown(BaseDat dat)
        {
            if (_ValueDropDownHandler != null)
            {
                _ValueDropDownHandler(this, new EventArgs<BaseDat>(dat));
                return true;
            }
            else
                return false;
        }

        private EventHandler<EventArgs<BaseDat>> _ValueOpenDialogHandler = null;
        public event EventHandler<EventArgs<BaseDat>> ValueOpenDialog
        {
            add
            {
                _ValueOpenDialogHandler += value;
                btnDialog.Visible = (_ValueOpenDialogHandler != null);
            }
            remove
            {
                _ValueOpenDialogHandler -= value;
                btnDialog.Visible = (_ValueOpenDialogHandler != null);
            }
        }
        public bool FireValueOpenDialog(BaseDat dat)
        {
            if (_ValueOpenDialogHandler != null)
            {
                _ValueOpenDialogHandler(this, new EventArgs<BaseDat>(dat));
                return true;
            }
            else
                return false;
        }

        public event EventHandler ValueChanged;
        public void FireValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);

            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");
        }

        new public event EventHandler TextChanged;
        void txtMain_TextChanged(object sender, EventArgs e)
        {
            //            Console.WriteLine("Editor Text=" + Text);
            if (BaseDat.ToString(Value, Format) != Text)
            {
                txtMain.ForeColor = Color.Brown;
                if (_Value != null)
                {
                    _Value = null;
                    FireValueChanged();
                }
            }
            else
                txtMain.ForeColor = Color.Black;
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
        }

        public event EventHandler NextValue;
        public event EventHandler PrevValue;

        private void txtMain_ReadOnlyChanged(object sender, EventArgs e)
        {
            this.BackColor = txtMain.BackColor;
        }
        private void txtMain_Resize(object sender, EventArgs e)
        {
            Height = txtMain.Height + 6;
            Invalidate();
        }

        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
                //Console.WriteLine("OnEnter");
            }
            base.OnEnter(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    if (Value != null || txtMain.Text.Length == 0 || !FireValueDropDown(null))
                    {
                        Form frm = this.FindForm();
                        if (frm != null)
                            frm.SelectNextControl(this, true, true, true, true);
                        return true;
                    }
                    break;
                case Keys.Down | Keys.Control:
                case Keys.Down | Keys.Alt:
                    if (FireValueDropDown(Value)) return true;
                    break;

                case Keys.Up | Keys.Control:
                case Keys.Up | Keys.Alt:
                    if (FireValueOpenDialog(Value)) return true;
                    break;
                case Keys.Delete | Keys.Control:
                    Value = null;
                    break;
                case Keys.Up:
                    if (PrevValue != null)
                    {
                        PrevValue(this, EventArgs.Empty);
                        return true;
                    }
                    break;
                case Keys.Down:
                    if (NextValue != null)
                    {
                        NextValue(this, EventArgs.Empty);
                        return true;
                    }
                    break;
                case Keys.Back:
                    if (AutoCompleteMode == AutoCompleteMode.None && SelectionLength > 0 && SelectionStart + SelectionLength == Text.Length)
                    {
                        string s = Text.Substring(0, SelectionStart - 1);
                        Text = s;
                        return true;
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ChangeSize();
        }
        private void ChangeSize()
        {
            int h = FontHeight + (SystemInformation.BorderSize.Height * 4) + 3;
            this.Height = h;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!this.txtMain.Focused)
                txtMain.Select();
            Value = null;
        }

        private void btnDropDown_Click(object sender, EventArgs e)
        {
            if (!this.txtMain.Focused)
                txtMain.Select();
            txtMain.SelectionStart = 0;
            txtMain.SelectionLength = 0;
            FireValueDropDown(Value);
        }

        void btnDialog_Click(object sender, EventArgs e)
        {
            if (!this.txtMain.Focused)
                txtMain.Select();
            txtMain.SelectionStart = 0;
            txtMain.SelectionLength = 0;
            FireValueOpenDialog(Value);
        }


        #endregion

        #region IDataMember Members

        private string _DataMember;
        [Category("Binding")]
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
        const string _helplabeltext = "Shift+Tab - предыдущее поле. Enter, Tab - следующее поле.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("“екст, который будет показыватьс€ при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }

        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null) frm.WriteHelp("");
            //Console.WriteLine("OnLeave");
            base.OnLeave(e);
            WriteDat();
        }

        #endregion


        #region Buttons
        enum ChooserButtonType { DropDown, Reset, Dialog }
        class ChooserButton : PictureBox
        {
            public ChooserButton(ChooserButtonType type, string name)
            {
                _type = type;
                this.Cursor = Cursors.Hand;
                this.Dock = DockStyle.Right;
                this.Name = name;
                this.Size = new Size(16, 16);
                this.TabStop = false;
            }

            ChooserButtonType _type;
            ButtonState _ButtonStyle = ButtonState.Flat;

            public ButtonState ButtonStyle
            {
                get { return _ButtonStyle; }
                set
                {
                    _ButtonStyle = value;
                    Invalidate();
                }
            }
            protected override void OnMouseDown(MouseEventArgs e)
            {
                if (ButtonStyle == ButtonState.Normal)
                    drawButton(this.CreateGraphics(), this.ClientRectangle, ButtonState.Pushed);
                base.OnMouseDown(e);
            }

            protected override void OnMouseUp(MouseEventArgs e)
            {
                if (ButtonStyle == ButtonState.Normal)
                    drawButton(this.CreateGraphics(), this.ClientRectangle, ButtonStyle);
                base.OnMouseUp(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                if (e.ClipRectangle.Width > 0 && e.ClipRectangle.Height > 0)
                    drawButton(e.Graphics, e.ClipRectangle, ButtonStyle);
                base.OnPaint(e);
            }

            private void drawButton(Graphics g, Rectangle r, ButtonState bstate)
            {
                Rectangle rc = Rectangle.Empty;
                try
                {
                    if (bstate == ButtonState.Normal)
                        rc = new Rectangle(r.X, r.Y, r.Width, r.Height);
                    else
                        rc = new Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1);

                    switch (_type)
                    {
                        case ChooserButtonType.DropDown:
                            ControlPaint.DrawComboButton(g, rc, bstate);
                            break;
                        case ChooserButtonType.Reset:
                            ControlPaint.DrawCaptionButton(g, rc, CaptionButton.Close, bstate);
                            break;
                        case ChooserButtonType.Dialog:
                            ControlPaint.DrawCaptionButton(g, rc, CaptionButton.Maximize, bstate);
                            break;
                    }
                    //чтобы по€вилась сера€ рамочка вокруг кнопок
                    //ControlPaint.DrawBorder(g, rc, Color.White, ButtonBorderStyle.Solid);
                }
                catch (Exception exp)
                {
                    Console.WriteLine("---\n" + rc.Size.ToString() + "\n" + Common.ExMessage(exp) + "\n---");
                }
            }

        }

        #endregion

        public T GetChooserCard<T>()
            where T : class
        {
            ICardDat cd=this.Value as ICardDat;
            if (cd!=null)
            {
                T ret = (T)Activator.CreateInstance(typeof(T), cd);
                return ret;
            }
            else
                return null;
        }

    }
}

