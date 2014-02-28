using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class ListController2
    {
        #region Constructors
        public ListController2()
        {
            _ToolBar = new ToolStrip();
            _ToolBar.Items.Add(ButtonNew);
            _ToolBar.Items.Add(ButtonEdit);
            _ToolBar.Items.Add(ButtonDel);
            _ToolBar.Items.Add(ButtonPreview);
            _ToolBar.Items.Add(ButtonPrint);
            //_ToolBar.Items.Add(ButtonListPreview);
            //_ToolBar.Items.Add(ButtonListPrint);
            ButtonPrint.Enabled = false;
            ButtonPreview.Enabled = false;

            _Grid = new CtlGrid();
            SetGridStyles();
            //_Grid.MultiSelect = true;
            // _Grid.AllowDrop = true;
        }
        public ListController2(string caption)
            : this()
        {
            _caption = caption;
        }
        public ListController2(OKCancelForm2 entForm)
            : this()
        {
            _EntityForm = entForm;
        }
        #endregion

        #region Fields
        private string _caption = "";
        private OKCancelForm2 _EntityForm = null;

        public OKCancelForm2 EntityForm
        {
            get { return _EntityForm; }
            set { _EntityForm = value; }
        }
        public ToolStrip _ToolBar;
        private CtlGrid _Grid;
        private bool _ReadOnly = false;

        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set
            {
                _ReadOnly = value;
                _ToolBar.Enabled = !_ReadOnly;
            }
        }

        public CtlGrid Grid
        {
            get { return _Grid; }
            set { _Grid = value; }
        }
        private Control _ParentControl;

        public ToolStripButton ButtonNew = new ToolStripButton("Создать", global::BOF.Properties.Resources.NewItem.ToBitmap());
        public ToolStripButton ButtonEdit = new ToolStripButton("Исправить", global::BOF.Properties.Resources.EditItem.ToBitmap());
        public ToolStripButton ButtonDel = new ToolStripButton("Удалить", global::BOF.Properties.Resources.DeleteItem.ToBitmap());
        public ToolStripButton ButtonPreview = new ToolStripButton("Просмотр", global::BOF.Properties.Resources.preview, null, "Preview");
        public ToolStripButton ButtonPrint = new ToolStripButton("Печать", Icons.printer, null, "Print");
        //public ToolStripButton ButtonListPreview = new ToolStripButton("Просмотр списка", global::BOF.Properties.Resources.pio_docspreview.ToBitmap(), null, "PreviewList");
        //public ToolStripButton ButtonListPrint = new ToolStripButton("Печать списка", global::BOF.Properties.Resources.pio_docsprint.ToBitmap(), null, "PrintList");

        //private bool _dragItem = false;
        #endregion

        #region Properties
        public CtlCaptionPanel captList = new CtlCaptionPanel();
        public Control ParentControl
        {
            get { return _ParentControl; }
            //set { _ParentControl = value; }
        }
        #endregion

        #region Events
        public event ObjEventDelegate EntityNew;
        public event ObjEventDelegate EntityDeleted;
        public event ObjEventDelegate EntityChanged;
        public event ObjEventDelegate EntitySelected;
        public event ObjEventDelegate EntityPrinted;
        public event ObjEventDelegate EntityPreviewed;



        public void FireEntityNew() { }
        public void FireEntityDeleted() { }
        public void FireEntityChanged() { }
        public void FireEntitySelected() { }

        void Grid_ValueSelected(object sender, ValueEventArgs e)
        {
            if (!ReadOnly)
                Edit((BaseDat)e.Value);
        }
        void Grid_ValueInserted(object sender, ValueEventArgs e)
        {
            if (!ReadOnly)
                Add();
        }
        void Grid_ValueDeleted(object sender, ValueEventArgs e)
        {
            if (!ReadOnly)
                Delete((BaseDat)e.Value);
        }
        void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Edit(Grid.Value);
        }
        void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Delete(Grid.Value);
        }
        void ButtonPrint_Click(object sender, EventArgs e)
        {
            if (EntityPrinted != null && Grid.Value != null)
                EntityPrinted(this, new ValueEventArgs(Grid.Value));
        }
        void ButtonPreview_Click(object sender, EventArgs e)
        {
            if (EntityPreviewed != null && Grid.Value != null)
                EntityPreviewed(this, new ValueEventArgs(Grid.Value));
        }
        //void ButtonListPrint_Click(object sender, EventArgs e)
        //{
        //    if (EntityListPrinted != null && Grid.Value != null)
        //        EntityListPrinted(this, new DatEventArgs(Grid.Value as BaseDat));
        //}
        //void ButtonListPreview_Click(object sender, EventArgs e)
        //{
        //    if (EntityListPreviewed != null && Grid.Value != null)
        //        EntityListPreviewed(this, new DatEventArgs(Grid.Value as BaseDat));
        //}

        void EntityNew_Handler(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Add();
        }
        void EntityChanged_Handler(object sender, ValueEventArgs e)
        {
            object dat = _Grid.Value;
            ButtonPreview.Enabled = (EntityPreviewed != null && dat != null);
            ButtonPrint.Enabled = (EntityPrinted != null && dat != null);
            if (EntityChanged != null)
                EntityChanged(this, new ValueEventArgs(dat));
        }

        protected virtual void Add()
        {
            if (_EntityForm == null)
            {
                if (EntityNew != null)
                    EntityNew(this, new ValueEventArgs(null));
            }
            else
            {
                try
                {
                    _EntityForm.BindControls(EntityForm);
                    if (_EntityForm.ShowDialog() == DialogResult.OK)
                        ReloadList();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        protected virtual void Edit(object dat)
        {
            if (_EntityForm == null)
            {
                if (EntitySelected != null)
                    EntitySelected(this, new ValueEventArgs(dat));
            }
            else
            {
                try
                {
                    if (dat != null)
                    {
                        _EntityForm.Value = dat;
                        _EntityForm.BindControls(EntityForm);
                        //_EntityForm.ShowDialog();
                        if (_EntityForm.ShowDialog() == DialogResult.OK)
                            ReloadList();

                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        protected virtual void Delete(object dat)
        {
            if (EntityDeleted != null)
                EntityDeleted(this, new ValueEventArgs(dat));
            //else
            //{
            //    try
            //    {
            //        DataGridViewSelectedRowCollection rows = Grid.SelectedRows;
            //        if (rows.Count > 1)
            //        {
            //            if (MessageBox.Show(string.Format("Вы действительно хотите удалить {0} строк?", rows.Count), "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            //            {
            //                foreach (DataGridViewRow row in rows)
            //                {
            //                    BaseDat dt = row.DataBoundItem as BaseDat;
            //                    if (dt != null)
            //                    {
            //                        IDat idat = dt as IDat;
            //                        if (idat != null && idat.ID > 0)
            //                            dt.Delete();
            //                    }
            //                }
            //                ReloadList();
            //            }
            //        }
            //        else if (rows.Count == 1)
            //        {
            //            //BaseDat dat = rows[0].DataBoundItem as BaseDat;
            //            if (dat != null)
            //            {
            //                if (dat is IDat && ((IDat)dat).ID > 0)
            //                {
            //                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить \r\n{0}?", dat), "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            //                    {
            //                        //dat.Delete();
            //                        ReloadList();
            //                    }
            //                }
            //                else
            //                {
            //                    MessageBox.Show(string.Format("Ветки дерева нужно удалять из дерева."), "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception Ex)
            //    {
            //        MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
        }

        #endregion

        #region Methods
        public T GetCurrent<T>(int rowIndex)
            where T:class
        {
            try
            {
                if (Grid.ValueEventDisabled
                    || Grid.RowCount == 0
                    || rowIndex < 0
                    || rowIndex >= Grid.RowCount) return null;

                DataGridViewRow row = Grid.Rows[rowIndex];

                if (row == null
                    || row.DataGridView.DataSource == null) return null;

                T dat = row.DataBoundItem as T;
                return dat;
            }
            catch
            {
                return null;
            }
        }
        protected virtual void ReloadList()
        {
            Grid.ValueEventDisabled = true;
            try
            {
                Grid.Reload();
                ISetReload set = Grid.DataSource as ISetReload;
                if (set != null)
                    set.FireSetReloaded();
            }
            finally
            {
                Grid.ValueEventDisabled = false;
            }
            Grid.FireValueChanged();
        }
        public void Init(Control parentControl)
        {
            _ParentControl = parentControl;

            ParentControl.SuspendLayout();
            if (!string.IsNullOrEmpty(_caption))
            {
                captList.Active = false;
                captList.Caption = _caption;
                captList.Dock = DockStyle.Top;

                ParentControl.Controls.Add(captList);
            }
            if (_ToolBar != null)
            {
                ParentControl.Controls.Add(_ToolBar);
                _ToolBar.Dock = DockStyle.Top;
                ButtonNew.Click += new EventHandler(EntityNew_Handler);
                ButtonEdit.Click += new EventHandler(ButtonEdit_Click);
                ButtonDel.Click += new EventHandler(ButtonDelete_Click);
                ButtonPrint.Click += new EventHandler(ButtonPrint_Click);
                ButtonPreview.Click += new EventHandler(ButtonPreview_Click);
                //ButtonListPrint.Click += new EventHandler(ButtonListPrint_Click);
                //ButtonListPreview.Click += new EventHandler(ButtonListPreview_Click);
                // ParentControl.Controls.Add(_ToolBar);
            }
            if (_Grid != null)
            {
                ParentControl.Controls.Add(_Grid);
                _Grid.Dock = DockStyle.Fill;
                _Grid.BringToFront();
                _Grid.ValueChanged += new ValueEventHandler(EntityChanged_Handler);
                _Grid.ValueInserted += new ValueEventHandler(Grid_ValueInserted);
                _Grid.ValueSelected += new ValueEventHandler(Grid_ValueSelected);
                _Grid.ValueDeleted += new ValueEventHandler(Grid_ValueDeleted);
            }
            //captList.BringToFront();
            ParentControl.ResumeLayout();
        }
        protected virtual void SetGridStyles()
        {
            Grid.AutoGenerateColumns = false;
        }
        #endregion
    }
}
