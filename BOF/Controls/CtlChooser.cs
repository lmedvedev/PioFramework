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
    [Designer(typeof(LineDesigner))]
    public partial class CtlChooser : UserControl, IDataMember
    {
        public enum DropDownAlignment { Fit, Left, Right }
        public enum GridColumnStyles { All, Default, Custom }

        public CtlChooser()
        {
            InitializeComponent();
            this.txtMain.KeyDown += new KeyEventHandler(txtMain_KeyDown);
            this.txtMain.LostFocus += new EventHandler(txtMain_LostFocus);
            this.txtMain.GotFocus += new EventHandler(txtMain_GotFocus);
            this.txtMain.ReadOnlyChanged += new EventHandler(txtMain_ReadOnlyChanged);
            this.txtMain.TextChanged += new EventHandler(txtMain_TextChanged);
            //this.txtMain.Resize += new EventHandler(txtMain_Resize);
            this.btnClear.Cursor = Cursors.Hand;
            this.btnDropDown.Cursor = Cursors.Hand;
            ChangeSize();
        }

        private void InitializeComponent()
        {
            this.txtMain = new System.Windows.Forms.TextBox();
            this.btnClear = new BOF.NotFocusedButton();
            this.btnDropDown = new BOF.NotFocusedButton();
            this.SuspendLayout();
            // 
            // txtMain
            // 
            this.txtMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMain.Location = new System.Drawing.Point(2, 1);
            this.txtMain.Name = "txtMain";
            this.txtMain.ReadOnly = true;
            this.txtMain.Size = new System.Drawing.Size(276, 13);
            this.txtMain.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.SystemColors.Control;
            this.btnClear.CausesValidation = false;
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.SystemColors.Control;
            this.btnClear.Location = new System.Drawing.Point(294, 1);
            this.btnClear.MaximumSize = new System.Drawing.Size(16, 36);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(16, 19);
            this.btnClear.TabIndex = 1;
            this.btnClear.TabStop = false;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnDropDown
            // 
            this.btnDropDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnDropDown.CausesValidation = false;
            this.btnDropDown.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnDropDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDropDown.ForeColor = System.Drawing.SystemColors.Control;
            this.btnDropDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDropDown.Location = new System.Drawing.Point(278, 1);
            this.btnDropDown.MaximumSize = new System.Drawing.Size(16, 36);
            this.btnDropDown.Name = "btnDropDown";
            this.btnDropDown.Size = new System.Drawing.Size(16, 19);
            this.btnDropDown.TabIndex = 2;
            this.btnDropDown.TabStop = false;
            this.btnDropDown.UseVisualStyleBackColor = false;
            this.btnDropDown.Click += new System.EventHandler(this.btnDropDown_Click);
            // 
            // CtlChooser
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtMain);
            this.Controls.Add(this.btnDropDown);
            this.Controls.Add(this.btnClear);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "CtlChooser";
            this.Padding = new System.Windows.Forms.Padding(2, 1, 0, 0);
            this.Size = new System.Drawing.Size(310, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #region Fields
        private System.Windows.Forms.TextBox txtMain;
        private NotFocusedButton btnClear;
        private NotFocusedButton btnDropDown;

        private bool _ReadOnly;
        private DropDownAlignment _DropDownAlign = DropDownAlignment.Fit;
        private int _DropDownWidth = 100;
        private GridColumnStyles _GridColumnStyle = GridColumnStyles.Default;
        private bool _CanReset = true;
        private bool _CanHideButtons = false;
        private BaseSet _SetClass;
        private DropDownService _Service;
        private string _Format;
        private bool _CanDropDown = true;
        private int _DropDownHeight = 100;
        private Dictionary<string, string> _Columns = new Dictionary<string, string>();
        private DataGridViewColumn[] _GridColumns = null;

        private BaseDat _Value;
        private bool _CloseDropDownByDoubleClick = true;
        private int position = -1;
        private bool ValueEventDisabled;
        #endregion

        #region Members
        [Category("Chooser")]
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set { _ReadOnly = value; }
        }

        [Category("Chooser")]
        public DropDownAlignment DropDownAlign
        {
            get { return _DropDownAlign; }
            set { _DropDownAlign = value; }
        }

        [Category("Chooser")]
        public int DropDownWidth
        {
            get { return (DropDownAlign == DropDownAlignment.Fit) ? Width : _DropDownWidth; }
            set { _DropDownWidth = value; }
        }

        [Category("Chooser")]
        public int DropDownHeight
        {
            get { return _DropDownHeight; }
            set { _DropDownHeight = value; }
        }

        [Category("Chooser")]
        public GridColumnStyles GridColumnStyle
        {
            get { return _GridColumnStyle; }
            set { _GridColumnStyle = value; }
        }
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
                    txtMain.Text = (_Value == null) ? "" : ((string.IsNullOrEmpty(Format)) ? _Value.ToString() : _Value.ToString(Format, null));
                    position = (SetClass == null) ? -1 : SetClass.IndexOf(_Value);
                    btnClear.Visible = CanReset && ((_Value != null) || !CanHideButtons);
                    if (changed) FireValueChanged();
                    SendKeys.Send("{END}+{HOME}");

                }
            }
        }

        [Category("Chooser")]
        new public BorderStyle BorderStyle
        {
            get { return base.BorderStyle; }
            set
            {
                base.BorderStyle = value;
                btnClear.FlatStyle = btnDropDown.FlatStyle = (base.BorderStyle == BorderStyle.Fixed3D) ? FlatStyle.Standard : FlatStyle.Flat;
                //this.Padding = (base.BorderStyle == BorderStyle.None) ? new Padding(1, 2, 1, 2) : ((base.BorderStyle == BorderStyle.Fixed3D) ? new Padding(0) : new Padding(1));
            }
        }

        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public Padding Padding
        {
            get { return base.Padding; }
            set { base.Padding = value; }
        }

        [Category("Chooser")]
        public bool CanReset
        {
            get { return _CanReset; }
            set { _CanReset = value; }
        }

        [Category("Chooser")]
        public bool CanHideButtons
        {
            get { return _CanHideButtons; }
            set { _CanHideButtons = value; }
        }

        [Category("Chooser"), Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual BaseSet SetClass
        {
            get { return _SetClass; }
            set
            {
                _SetClass = value;
                txtMain.ReadOnly = ReadOnly || (_SetClass == null);
            }
        }

        [Category("Chooser")]
        public bool CanCall
        {
            get { return (ValueOpenForm != null); }
        }

        [Category("Chooser")]
        public bool CloseDropDownByDoubleClick
        {
            get { return _CloseDropDownByDoubleClick; }
            set { _CloseDropDownByDoubleClick = value; }
        }

        [Category("Chooser")]
        public bool CanDropDown
        {
            get { return _CanDropDown; }
            set { _CanDropDown = value; }
        }

        [Category("Chooser"), ReadOnly(true), Browsable(false)]
        public Dictionary<string, string> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }

        [Category("Chooser"), ReadOnly(true), Browsable(false)]
        public DataGridViewColumn[] GridColumns
        {
            get { return _GridColumns; }
            set
            {
                _GridColumns = value;

                if (value != null)
                {
                    //_GridColumns = new DataGridViewColumn[value.Length];
                    //for (int i = 0; i < value.Length; i++)
                    //{
                    //    _GridColumns[i] = (DataGridViewColumn)value[i].Clone();
                    //}
                    GridColumnStyle = GridColumnStyles.Custom;
                }
                //else
                //    _GridColumns = null;
            }
        }
        #endregion

        #region Events Handlers

        public event ValueEventHandler ValueOpenForm = null;

        public void FireValueChanged()
        {
            //if (DatValueChanged != null)
            //    DatValueChanged(this, new ValueEventArgs(Value));
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);

            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");
        }
        public void FireValueOpenForm(ValueEventArgs e)
        {
            if (ValueOpenForm != null) ValueOpenForm(this, e);
        }

        private void txtMain_ReadOnlyChanged(object sender, EventArgs e)
        {
            this.BackColor = txtMain.BackColor;
        }
        private void txtMain_Resize(object sender, EventArgs e)
        {
            Height = txtMain.Height + 6;
            btnClear.Height = btnDropDown.Height = ClientRectangle.Height - Padding.Top - Padding.Bottom;
        }
        private void txtMain_GotFocus(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        private void txtMain_LostFocus(object sender, EventArgs e)
        {
            OnLostFocus(e);
        }
        private void txtMain_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        private void txtMain_TextChanged(object sender, EventArgs e)
        {
            txtMain.ForeColor = Color.Black;
            if (!ValueEventDisabled && txtMain.ContainsFocus) SetText(false);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (btnClear.ContainsFocus || btnDropDown.ContainsFocus) return;
            btnDropDown.Visible = CanDropDown && !CanHideButtons;
            btnClear.Visible = CanReset && !CanHideButtons;

            txtMain.Text = (_Value == null) ? "" : ((string.IsNullOrEmpty(Format)) ? _Value.ToString() : _Value.ToString(Format, null));
        }
        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
            }
            base.OnEnter(e);
            btnClear.Visible = CanReset && ((Value != null) || !CanHideButtons);
            btnDropDown.Visible = CanDropDown && ((SetClass != null) || !CanHideButtons);
            SendKeys.Send("{END}+{HOME}");
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (txtMain.Text != "")
            {
                switch (e.KeyData)
                {
                    case Keys.Delete:
                        ValueEventDisabled = true;
                        if (txtMain.SelectionLength > 0) txtMain.Text = txtMain.Text.Substring(0, txtMain.SelectionStart);
                        txtMain.SelectionStart = txtMain.Text.Length;
                        ValueEventDisabled = false;
                        SetText(true);
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Back:
                        ValueEventDisabled = true;
                        if (txtMain.SelectionLength > 0) txtMain.Text = txtMain.Text.Substring(0, txtMain.SelectionStart);
                        if (txtMain.Text.Length > 0) txtMain.Text = txtMain.Text.Substring(0, txtMain.Text.Length - 1);
                        txtMain.SelectionStart = txtMain.Text.Length;
                        ValueEventDisabled = false;
                        e.SuppressKeyPress = true;
                        SetText(false);
                        break;

                }
            }
            if (SetClass != null)
            {
                switch (e.KeyData)
                {
                    case Keys.Up:
                        e.SuppressKeyPress = true;
                        if (position <= 0) return;
                        position--;
                        ValueEventDisabled = true;
                        Value = (BaseDat)SetClass[position];
                        ValueEventDisabled = false;
                        //txtMain.SelectAll();
                        break;
                    case Keys.Down:
                        e.SuppressKeyPress = true;
                        if (position >= SetClass.Count - 1) return;
                        position++;
                        ValueEventDisabled = true;
                        Value = (BaseDat)SetClass[position];
                        ValueEventDisabled = false;
                        //txtMain.SelectAll();
                        break;
                }
            }
            if (btnDropDown.Visible)
            {
                switch (e.KeyData)
                {
                    case Keys.Enter:
                        if (Value == null && txtMain.Text.Length > 0)
                            btnDropDown_Click(this, EventArgs.Empty);
                        else
                        {
                            Form frm = this.FindForm();
                            if (frm != null)
                                frm.SelectNextControl(this, true, true, true, true);
                        }
                        break;
                    case Keys.Down | Keys.Control:
                    case Keys.Down | Keys.Alt:
                        btnDropDown_Click(this, EventArgs.Empty);
                        break;
                }
            }
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Value = null;
            btnClear.Visible = CanReset && !CanHideButtons;
            //btnClear.Visible = false;
        }

        private void btnDropDown_Click(object sender, EventArgs e)
        {
            if (_Service == null) _Service = new DropDownService(this);
            buildColumns();
            ChooserDropDown ctl = NewChooserDropDown();

            ctl.CloseByDoubleClick = CloseDropDownByDoubleClick;
            
            ctl.DataSource = SetClass;

            if (SetClass != null)
            {
                if (SetClass.Count == 0)
                    SetClass.Load();
                
                SetClass.FilterApply(((Value == null) ? txtMain.Text : ""), ctl.GetColumnDescriptors());
            }
            ctl.Value = Value;
            _Service.DropDownControl(ctl);
        }
        #endregion

        #region Methods
        protected void SetText(string text)
        {
            txtMain.Focus();
            txtMain.Text = text;
            txtMain.Select(text.Length, text.Length);
        }
        protected void SetText(bool strict)
        {
            if (SetClass == null || SetClass.SortProperty == null || SetClass.SortProperty.PropertyType != typeof(string))
            {
                txtMain.ForeColor = Color.Brown;
                if (Value != null)
                {
                    bool ved = ValueEventDisabled;
                    string txt = txtMain.Text;

                    ValueEventDisabled = true;
                    Value = null;

                    txtMain.Text = txt;
                    ValueEventDisabled = ved;
                    SendKeys.Send("{RIGHT}");
                }
            }
            else
            {
                string find = txtMain.Text;
                position = SetClass.FindIndex(find, strict);
                if (position < 0)
                {
                    txtMain.ForeColor = Color.Brown;
                    Value = null;
                }
                else
                {
                    txtMain.ForeColor = Color.Black;
                    ValueEventDisabled = true;
                    Value = (BaseDat)SetClass[position];
                    txtMain.Text = SetClass.SortProperty.GetValue(Value).ToString();
                    int i = txtMain.Text.ToLower().IndexOf(find.ToLower()) + find.Length;
                    txtMain.Select(i, txtMain.Text.Length - i);
                    ValueEventDisabled = false;
                }
            }
        }

        public void AddColumn(string name, string caption)
        {
            Columns.Add(name, caption);
            GridColumnStyle = GridColumnStyles.Custom;
        }

        protected virtual ChooserDropDown NewChooserDropDown()
        {
            if (GridColumns != null)
                return new ChooserDropDown(GridColumns);
            else
                return new ChooserDropDown(Columns);

        }
        private void buildColumns()
        {
            switch (GridColumnStyle)
            {
                case GridColumnStyles.All:
                    Columns.Clear();
                    break;
                case GridColumnStyles.Default:
                    if (SetClass != null)
                    {
                        if (SetClass.IsDatDerivedFrom(typeof(ITreeCustomDisplayName)))
                        {
                            SetClass.Sort("FP");
                            Columns.Clear();
                            AddColumn("DisplayName", "Название");
                        }
                        else if (SetClass.IsDatDerivedFrom(typeof(IDictDat)))
                        {
                            SetClass.Sort("SCode");
                            Columns.Clear();
                            AddColumn("SCode", "Код");
                            AddColumn("Name", "Название");
                        }
                        else if (SetClass.IsDatDerivedFrom(typeof(ICardDat)))
                        {
                            SetClass.Sort("Name");
                            Columns.Clear();
                            AddColumn("Name", "Название");
                        }
                        else if (SetClass.IsDatDerivedFrom(typeof(ITreeDat)))
                        {
                            SetClass.Sort("FP");
                            Columns.Clear();
                            AddColumn("FP", "Путь");
                            AddColumn("Name", "Название");
                        }
                    }
                    break;
                case GridColumnStyles.Custom:
                    break;
            }

        }

        #endregion

        #region IDataMember Members
        public event EventHandler ValueChanged;

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

        //public void RemoveBinding()
        //{
        //    OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
        //    if (frm != null)
        //        RemoveBinding(frm.NewValue);
        //}
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
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }

        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null) frm.WriteHelp("");
            base.OnLeave(e);
        }

        #endregion

    }

    #region ChooserDropDown
    public partial class ChooserDropDown : UserControl
    {
        private System.Windows.Forms.Panel StatusPanel;
        private System.Windows.Forms.ToolStrip Status;
        private System.Windows.Forms.ToolStripButton ButtonClose;
        private System.Windows.Forms.ToolStripButton ButtonSelect;
        private System.Windows.Forms.ToolStripButton ButtonCall;
        private System.Windows.Forms.ToolStripLabel Description;
        private PictureBox ResizeLeft;
        private PictureBox ResizeRight;
        private System.Windows.Forms.DataGridView Grid;

        public event ValueEventHandler ValueOpenForm = null;
        public event ValueEventHandler ValueSelected = null;
        public event EventHandler ValueEscaped = null;
        public ChooserDropDown()
        {
            InitializeComponent();
            this.Grid.DoubleClick += new System.EventHandler(this.Grid_DoubleClick);
            this.Grid.CellClick += new DataGridViewCellEventHandler(Grid_CellClick);
        }

        public ChooserDropDown(Dictionary<string, string> columns)
            : this()
        {
            Grid.AutoGenerateColumns = (columns.Count == 0);
            foreach (KeyValuePair<string, string> col in columns)
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = col.Key;
                column.Name = col.Key;
                column.HeaderText = col.Value;
                Grid.Columns.Add(column);
            }
            if (Grid.Columns.Count > 0) Grid.Columns.GetLastColumn(DataGridViewElementStates.None, DataGridViewElementStates.None).AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public ChooserDropDown(params DataGridViewColumn[] gridcolumns)
            : this()
        {
            Grid.AutoGenerateColumns = false;
            Grid.Columns.Clear();
            //Grid.Columns.AddRange(gridcolumns);
            foreach (DataGridViewColumn col in gridcolumns)
            {
                Grid.Columns.Add(col.Clone() as DataGridViewColumn);
            }
        }

        public string[] GetColumnNames()
        {
            string[] slist = new string[Grid.Columns.Count];
            int i = 0;
            foreach (DataGridViewColumn column in Grid.Columns)
            {
                slist[i++] = column.DataPropertyName;
            }
            return slist;
        }
        public PropertyDescriptor[] GetColumnDescriptors()
        {
            BaseSet dset = Grid.DataSource as BaseSet;
            if (dset == null)
                return new PropertyDescriptor[] { };
            else
                return Array.ConvertAll<string, PropertyDescriptor>(GetColumnNames()
                   , delegate(string name)
                    {
                        return dset.GetDescriptor(name);
                    });
        }

        private void InitializeComponent()
        {
            this.StatusPanel = new System.Windows.Forms.Panel();
            this.Status = new System.Windows.Forms.ToolStrip();
            this.ButtonCall = new System.Windows.Forms.ToolStripButton();
            this.Description = new System.Windows.Forms.ToolStripLabel();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.ResizeLeft = new System.Windows.Forms.PictureBox();
            this.ButtonClose = new System.Windows.Forms.ToolStripButton();
            this.ButtonSelect = new System.Windows.Forms.ToolStripButton();
            this.ResizeRight = new System.Windows.Forms.PictureBox();
            this.StatusPanel.SuspendLayout();
            this.Status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeRight)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusPanel
            // 
            this.StatusPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.StatusPanel.Controls.Add(this.ResizeLeft);
            this.StatusPanel.Controls.Add(this.ResizeRight);
            this.StatusPanel.Controls.Add(this.Status);
            this.StatusPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.StatusPanel.Location = new System.Drawing.Point(0, 156);
            this.StatusPanel.Name = "StatusPanel";
            this.StatusPanel.Size = new System.Drawing.Size(309, 16);
            this.StatusPanel.TabIndex = 2;
            // 
            // Status
            // 
            this.Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Status.AutoSize = false;
            this.Status.BackColor = Color.FromArgb(192, 192, 192);
            this.Status.Dock = System.Windows.Forms.DockStyle.None;
            this.Status.GripMargin = new System.Windows.Forms.Padding(0);
            this.Status.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.Status.ImageScalingSize = new System.Drawing.Size(13, 13);
            this.Status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ButtonClose,
            this.ButtonSelect,
            this.ButtonCall,
            this.Description});
            this.Status.Location = new System.Drawing.Point(11, 1);
            this.Status.Name = "Status";
            this.Status.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Status.Size = new System.Drawing.Size(290, 16);
            this.Status.Stretch = true;
            this.Status.TabIndex = 0;
            this.Status.Text = "toolStrip1";
            // 
            // ButtonCall
            // 
            this.ButtonCall.AutoSize = false;
            this.ButtonCall.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ButtonCall.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ButtonCall.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ButtonCall.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonCall.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonCall.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonCall.Name = "ButtonCall";
            this.ButtonCall.Size = new System.Drawing.Size(13, 13);
            this.ButtonCall.Text = "...";
            this.ButtonCall.ToolTipText = "Открыть форму";
            this.ButtonCall.Click += new System.EventHandler(this.ButtonCall_Click);
            // 
            // Description
            // 
            this.Description.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.Description.Name = "Description";
            this.Description.Size = new System.Drawing.Size(0, 16);
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.AllowUserToResizeRows = false;
            this.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 0);
            this.Grid.MultiSelect = false;
            this.Grid.Name = "Grid";
            this.Grid.ReadOnly = true;
            this.Grid.RowHeadersVisible = false;
            this.Grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.Grid.RowTemplate.Height = 18;
            this.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Grid.ShowCellErrors = false;
            this.Grid.ShowCellToolTips = false;
            this.Grid.ShowEditingIcon = false;
            this.Grid.ShowRowErrors = false;
            this.Grid.Size = new System.Drawing.Size(309, 156);
            this.Grid.TabIndex = 1;
            this.Grid.VirtualMode = true;
            this.Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Grid_KeyDown);
            this.Grid.SelectionChanged += new System.EventHandler(this.Grid_SelectionChanged);
            // 
            // ResizeLeft
            // 
            this.ResizeLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ResizeLeft.Image = global::BOF.Properties.Resources.resize_left;
            this.ResizeLeft.Location = new System.Drawing.Point(0, 0);
            this.ResizeLeft.Name = "ResizeLeft";
            this.ResizeLeft.Size = new System.Drawing.Size(12, 16);
            this.ResizeLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ResizeLeft.TabIndex = 0;
            this.ResizeLeft.TabStop = false;
            this.ResizeLeft.MouseDown += new MouseEventHandler(Resize_MouseDown);
            this.ResizeLeft.MouseUp += new MouseEventHandler(Resize_MouseUp);
            this.ResizeLeft.MouseMove += new MouseEventHandler(Resize_MouseMove);
            this.ResizeLeft.MouseHover += new EventHandler(ResizeLeft_MouseHover);
            this.ResizeLeft.MouseLeave += new EventHandler(Resize_MouseLeave);
            // 
            // ButtonClose
            // 
            this.ButtonClose.AutoSize = false;
            this.ButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonClose.Image = global::BOF.Properties.Resources.XIcon1.ToBitmap();
            this.ButtonClose.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(13, 13);
            this.ButtonClose.ToolTipText = "Закрыть список";
            this.ButtonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // ButtonSelect
            // 
            this.ButtonSelect.AutoSize = false;
            this.ButtonSelect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ButtonSelect.Image = global::BOF.Properties.Resources.Select.ToBitmap();
            this.ButtonSelect.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ButtonSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ButtonSelect.Name = "ButtonSelect";
            this.ButtonSelect.Size = new System.Drawing.Size(13, 13);
            this.ButtonSelect.ToolTipText = "Выбрать значение";
            this.ButtonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // ResizeRight
            // 
            this.ResizeRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ResizeRight.Image = global::BOF.Properties.Resources.resize_right;
            this.ResizeRight.Location = new System.Drawing.Point(297, 0);
            this.ResizeRight.Name = "ResizeRight";
            this.ResizeRight.Size = new System.Drawing.Size(12, 16);
            this.ResizeRight.TabIndex = 1;
            this.ResizeRight.TabStop = false;
            this.ResizeRight.MouseDown += new MouseEventHandler(Resize_MouseDown);
            this.ResizeRight.MouseUp += new MouseEventHandler(Resize_MouseUp);
            this.ResizeRight.MouseMove += new MouseEventHandler(Resize_MouseMove);
            this.ResizeRight.MouseHover += new EventHandler(ResizeRight_MouseHover);
            this.ResizeRight.MouseLeave += new EventHandler(Resize_MouseLeave);
            // 
            // ChooserDropDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.StatusPanel);
            this.VisibleChanged += new EventHandler(ChooserDropDown_VisibleChanged);
            this.DoubleBuffered = true;
            this.Name = "ChooserDropDown";
            this.Size = new System.Drawing.Size(309, 170);
            this.StatusPanel.ResumeLayout(false);
            this.Status.ResumeLayout(false);
            this.Status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResizeRight)).EndInit();
            this.ResumeLayout(false);

        }

        private bool _CloseByDoubleClick = true;
        public bool CloseByDoubleClick
        {
            get { return _CloseByDoubleClick; }
            set { _CloseByDoubleClick = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseDat Value
        {
            get
            {
                return (DataSource == null || Grid.CurrentRow == null) ? null : DataSource[Grid.CurrentRow.Index] as BaseDat;
            }
            set
            {
                //int position = (value == null) ? 0 : DataSource.IndexOf(value);
                ////bool changed = Grid.CurrentCell.RowIndex != position;
                //if (DataSource != null && Grid.Rows.Count > 0 && position > -1) Grid.CurrentCell = Grid.Rows[position].Cells[0];

                if (DataSource != null)
                {
                    int position = (value == null) ? 0 : DataSource.IndexOf(value);
                    if (Grid.Rows.Count > 0 && position > -1) Grid.CurrentCell = Grid.Rows[position].Cells[0];
                }
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public BaseSet DataSource
        {
            get { return Grid.DataSource as BaseSet; }
            set { Grid.DataSource = value; }
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            Description.Text = (Value == null) ? "" : Value.ToString();
        }

        void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!CloseByDoubleClick && e.RowIndex >= 0 && e.ColumnIndex >= 0)
                FireValueSelected();
        }
        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            if (CloseByDoubleClick)
                FireValueSelected();
        }

        private void FireValueSelected()
        {
            if (ValueSelected != null) ValueSelected(this, new ValueEventArgs(Value));
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            if (ValueEscaped != null) ValueEscaped(this, EventArgs.Empty);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            FireValueSelected();
        }

        private bool _DontClose;
        public bool DontClose
        {
            get { return _DontClose; }
            set { _DontClose = value; }
        }

        private void ButtonCall_Click(object sender, EventArgs e)
        {
            try
            {
                DontClose = true;
                ValueOpenForm(this, new ValueEventArgs(Value));
            }
            catch { }
            finally
            {
                DontClose = false;
            }
        }

        void ChooserDropDown_VisibleChanged(object sender, EventArgs e)
        {
            ButtonCall.Enabled = (ValueOpenForm != null);
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                FireValueSelected();
            else if (e.KeyData == Keys.Escape)
                ButtonClose_Click(this, EventArgs.Empty);
        }

        void Resize_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        void ResizeLeft_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.SizeNESW;
        }

        void ResizeRight_MouseHover(object sender, EventArgs e)
        {
            Cursor = Cursors.SizeNWSE;
        }

        int start_y = 0;
        bool resizing = false;
        int start_x = 0;
        private void Resize_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            start_x = e.X;
            start_y = e.Y;
            resizing = true;
        }

        private void Resize_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            resizing = false;
        }

        private void Resize_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (!resizing) return;
            int delta_x = e.X - start_x;
            if (((Control)sender).Name == "ResizeRight")
                TopLevelControl.Width += delta_x;
            else
            {
                if (delta_x < 0)
                {
                    TopLevelControl.Left += delta_x;
                    TopLevelControl.Width -= delta_x;
                }
                else
                {
                    int width = TopLevelControl.Width;
                    TopLevelControl.Width -= delta_x;
                    TopLevelControl.Left += width - TopLevelControl.Width;
                }
            }
            TopLevelControl.Height += e.Y - start_y;
        }
    }
    #endregion

    #region NotFocusedButton
    public class NotFocusedButton : Button
    {
        public NotFocusedButton()
            : base()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            Rectangle rc = new Rectangle(pevent.ClipRectangle.X, pevent.ClipRectangle.Y - 1, pevent.ClipRectangle.Width, pevent.ClipRectangle.Height + 1);
            if (this.Name == "btnDropDown")
                ControlPaint.DrawComboButton(pevent.Graphics, rc, ButtonState.Flat);
            else if (this.Name == "btnClear")
                ControlPaint.DrawCaptionButton(pevent.Graphics, rc, CaptionButton.Close, ButtonState.Flat);
            ControlPaint.DrawBorder(pevent.Graphics, rc, Color.White, ButtonBorderStyle.Solid);
        }
    }
    #endregion
}
