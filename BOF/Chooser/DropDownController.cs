using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
using BO;
using System.ComponentModel;

namespace BOF
{
    /// <summary>
    /// Контроллер для выбора Dat-класса при помощи DropDown-списка.
    /// <example>
    /// ddc = new DropDownController<BaseAccountsSet, BaseAccountsDat>(this.accChooser2, new DropDownGridStatus());
    ///
    /// DataGridViewColumn col1 = new DataGridViewTextBoxColumn();
    /// col1.HeaderText = "Путь";
    /// col1.Name = BaseAccountsColumns.FP;
    /// col1.DataPropertyName = BaseAccountsColumns.FP;
    /// col1.ValueType = typeof(PathCard);
    /// col1.Width = 50;
    ///
    /// DataGridViewColumn col2 = new DataGridViewTextBoxColumn();
    /// col2.HeaderText = "Название";
    /// col2.Name = BaseAccountsColumns.Name;
    /// col2.DataPropertyName = BaseAccountsColumns.Name;
    /// col2.ValueType = typeof(string);
    /// col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

    /// ddc.ColumnStyles(new DataGridViewColumn[] { col1, col2 });
    /// </example>
    /// </summary>
    /// <typeparam name="CS"></typeparam>
    /// <typeparam name="CD"></typeparam>
    public class DropDownController<CS, CD> : ChooserController<CS, CD>, IWindowsFormsEditorService
        where CS : BaseSet<CD, CS>, new()
        where CD : BaseDat<CD>, new()
    {
        #region Fields
        //public enum DropDownAlignment { Fit, Left, Right }
        protected bool closingDropDown = false;

        protected DropDownFormBase ddForm;
        protected ChooserEditor Editor;
        protected IDropDownControl ddControl;
        #endregion

        #region Constructors
        protected DropDownController() { }

        /// <summary>
        /// Конструктор со стандартным выпадающим DropDownGrid
        /// </summary>
        /// <param name="editor"></param>
        public DropDownController(ChooserEditor editor)
            : this(editor, new DropDownGrid()) { }

        public DropDownController(ChooserEditor editor, CS bset)
            : this(editor, new DropDownGrid(), bset) { }
        
        /// <summary>
        /// Конструктор с заданным IDropDownControl - выпадающим контролом
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="ctl"></param>
        //public DropDownController(ChooserEditor editor, IDropDownControl ctl)
        //    : base(editor)
        //{
        //    ddControl = ctl;
        //    ddForm = new DropDownFormBase(this, ddControl);
        //    ddControl.Grid.DataSource = set;
        //    ddControl.RowEntered += new EventHandler<EventArgs<int>>(ddControl_RowEntered);
        //    ddControl.RowSelected += new EventHandler<EventArgs<int>>(ddControl_RowSelected);
        //    ddControl.DropDownClosed += new EventHandler(ddControl_DropDownClosed);

        //    Editor = editor;
        //    Editor.ValueDropDown += Editor_ValueDropDown;
        //    //Editor.ValueChanged += new EventHandler(Editor_ValueChanged);
        //}

        public DropDownController(ChooserEditor editor, IDropDownControl ctl)
            : this(editor, ctl, null)
        { }
        public DropDownController(ChooserEditor editor, IDropDownControl ctl, bool reload)
            : this(editor, ctl, null, reload)
        { }
        public DropDownController(ChooserEditor editor, IDropDownControl ctl, CS bset)
            : this(editor, ctl, bset, true)
        { }
        public DropDownController(ChooserEditor editor, IDropDownControl ctl, CS bset, bool reload)
            : base(editor, bset, reload)
        {
            ddControl = ctl;
            ddForm = new DropDownFormBase(this, ddControl);
            ddControl.Grid.DataSource = set;
            ddControl.RowEntered += new EventHandler<EventArgs<int>>(ddControl_RowEntered);
            ddControl.RowSelected += new EventHandler<EventArgs<int>>(ddControl_RowSelected);
            ddControl.DropDownClosed += new EventHandler(ddControl_DropDownClosed);

            Editor = editor;
            Editor.ValueDropDown += Editor_ValueDropDown;
            //Editor.ValueChanged += new EventHandler(Editor_ValueChanged);
        }

        #endregion

        public FormBorderStyle DropDownFormBorderStyle
        {
            get 
            {
                return (ddForm != null) ? ddForm.FormBorderStyle : FormBorderStyle.SizableToolWindow;
            }
            set 
            {
                if (ddForm != null)
                    ddForm.FormBorderStyle = value;
            }
        }

        public int DropDownFormWidth
        {
            get
            {
                return (ddForm != null) ? ddForm.Width : 0;
            }
            set
            {
                if (ddForm != null)
                    ddForm.Width = value;
            }
        }

        public bool ColumnHeadersVisible
        {
            get
            {
                return (ddControl != null && ddControl.Grid != null) ? ddControl.Grid.ColumnHeadersVisible : false;
            }
            set
            {
                if (ddControl != null && ddControl.Grid != null)
                    ddControl.Grid.ColumnHeadersVisible = value;
            }
        }

        #region Methods
        public void ColumnStyles(DataGridViewColumn[] columns)
        {
            DataGridView grid = ddControl.Grid;
            grid.Columns.Clear();
            grid.AutoGenerateColumns = (columns == null);
            if (columns != null)
                grid.Columns.AddRange(columns);
        }

        #region Private functions
        PropertyDescriptor[] GetColumnDescriptors()
        {
            if (set == null)
                return new PropertyDescriptor[] { };
            else
                return Array.ConvertAll<string, PropertyDescriptor>(GetColumnNames()
                   , delegate(string name)
                    {
                        return set.GetDescriptor(name);
                    });
        }
        string[] GetColumnNames()
        {
            string[] slist = new string[ddControl.Grid.Columns.Count];
            int i = 0;
            foreach (DataGridViewColumn column in ddControl.Grid.Columns)
            {
                slist[i++] = column.DataPropertyName;
            }
            return slist;
        }
        #endregion
        #endregion

        #region Events
        void ddControl_RowSelected(object sender, EventArgs<int> e)
        {
            OnRowSelected(sender, e);
        }
        protected virtual void OnRowSelected(object sender, EventArgs<int> e)
        {
            int index = e.Data;
            CD dat = set[index] as CD;
            CloseDropDown();
            Application.DoEvents();
            Editor.Value = dat;
        }

        void ddControl_RowEntered(object sender, EventArgs<int> e)
        {
            OnRowEntered(sender, e);
        }
        protected virtual void OnRowEntered(object sender, EventArgs<int> e)
        {
            OnRowSelected(sender, e);
        }

        void ddControl_DropDownClosed(object sender, EventArgs e)
        {
            OnDropDownClosed(sender, e);
        }
        protected virtual void OnDropDownClosed(object sender, EventArgs e)
        {
            CloseDropDown();
        }

        protected override void OnValueChanged(object sender, EventArgs e)
        {
            if (ddControl != null && Editor != null)
                ddControl.WriteText(BaseDat.ToString(Editor.Value));
            base.OnValueChanged(sender, e);
        }

        void Editor_ValueDropDown(object sender, EventArgs<BaseDat> e)
        {
            OnValueDropDown(sender, e);
        }
        protected virtual void OnValueDropDown(object sender, EventArgs<BaseDat> e)
        {
            if (set != null)
            {
                set.FilterReset();
                if (e.Data == null && !string.IsNullOrEmpty(Editor.Text))
                    set.FilterApply(Editor.Text, GetColumnDescriptors());
            }
            DropDownControl((Control)ddControl);
        }
        #endregion

        #region IWindowsFormsEditorService Members

        public void DropDownControl(Control ctl)
        {
            Point location = new Point(Editor.Left, Editor.Bottom);

            // location in screen coordinate
            location = Editor.Parent.PointToScreen(location);

            if (ddForm == null)
                ddForm = new DropDownFormBase(this, (IDropDownControl)ctl);
            ddForm.Visible = false;

            Size size = new Size(Editor.Width, ctl.Height);
            if (DropDownFormBorderStyle == FormBorderStyle.FixedSingle || DropDownFormBorderStyle == FormBorderStyle.FixedToolWindow)
                size.Width = ddForm.Width;


            // check the form is in the screen working area
            Rectangle screenWorkingArea = Screen.FromControl(Editor).WorkingArea;

            location.X = Math.Min(screenWorkingArea.Right - size.Width,
                                  Math.Max(screenWorkingArea.X, location.X));

            if (size.Height + location.Y + Editor.Height > screenWorkingArea.Bottom)
                location.Y = location.Y - size.Height - Editor.Height - 1;

            ddForm.Location = location;
            ddForm.Size = size;
            ddForm.Visible = true;
            ctl.Visible = true;
            if (set != null)
            {
                int index = -1;
                if (Editor.Value != null)
                    index = set.IndexOf(Editor.Value);
                BindingManagerBase cm = ddControl.Grid.BindingContext[set];
                if (cm != null)
                    cm.Position = index;
            }
            ((IDropDownControl)ctl).Grid.Focus();
        }

        public virtual void CloseDropDown()
        {
            if (closingDropDown)
                return;
            try
            {
                closingDropDown = true;
                if (ddForm != null && ddForm.Visible)
                {
                    ddForm.Visible = false;

                    if (Editor.Visible)
                        Editor.Focus();
                }
            }
            finally
            {
                closingDropDown = false;
            }
        }

        public DialogResult ShowDialog(Form dialog)
        {
            return DialogResult.Cancel;
        }

        #endregion
    }
}
