using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Drawing;

namespace BOF
{
    public class ListController
    {
        private DateTime SearchBoxKeyUpTime = DateTime.MinValue;
        #region Constructors
        public ListController()
        {
            _ToolBar = new ToolStrip();
            _ToolBar.Items.Add(ButtonNew);
            _ToolBar.Items.Add(ButtonEdit);
            _ToolBar.Items.Add(ButtonDel);
            _ToolBar.Items.Add(ButtonPreview);
            _ToolBar.Items.Add(ButtonPrint);
            _ToolBar.Items.Add(SearchLabel);
            _ToolBar.Items.Add(SearchBox);
            //_ToolBar.Items.Add(ButtonXml);
            //_ToolBar.Items.Add(ButtonListPreview);
            //_ToolBar.Items.Add(ButtonListPrint);
            //ButtonPrint.Enabled = false;
            //ButtonPreview.Enabled = false;

            SearchBox.BorderStyle = BorderStyle.Fixed3D;
            SearchBox.TextBox.Width = 400;
            //SearchBox.TextBox.Height = 20;
            //SearchBox.Dock = DockStyle.Fill;
            //SearchBox.ImageIndex = 0;
            //SearchBox.Height = 20;

            _Grid = new CtlGrid();
            SetGridStyles();
            //_Grid.MultiSelect = true;
            // _Grid.AllowDrop = true;
        }
        public ListController(string caption)
            : this()
        {
            _caption = caption;
        }
        public ListController(OKCancelDatForm entForm)
            : this()
        {
            _EntityForm = entForm;
        }
        #endregion

        #region Fields
        private string _caption = "";
        private OKCancelDatForm _EntityForm = null;

        public OKCancelDatForm EntityForm
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

        public ToolStripButton ButtonNew = new ToolStripButton("Создать", Properties.Resources.NewDocumentHS);
        public ToolStripButton ButtonEdit = new ToolStripButton("Исправить", global::BOF.Properties.Resources.EditItem.ToBitmap());
        public ToolStripButton ButtonDel = new ToolStripButton("Удалить", global::BOF.Properties.Resources.DeleteItem.ToBitmap());
        public ToolStripButton ButtonPreview = new ToolStripButton("Просмотр", global::BOF.Properties.Resources.preview);
        public ToolStripButton ButtonPrint = new ToolStripButton("Печать", Icons.printer);
        public ToolStripLabel SearchLabel = new ToolStripLabel("Фильтр:", global::BOF.Properties.Resources.icoFilter.ToBitmap());
        public ToolStripTextBox SearchBox = new ToolStripTextBox();

        //public ToolStripButton ButtonXml = new ToolStripButton("XML", global::BOF.Properties.Resources.Web_XML.ToBitmap());
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
        public event DatEventDelegate EntityNew;
        public event DatEventDelegate EntityDeleted;
        public event DatEventDelegate EntityChanged;
        public event DatEventDelegate EntitySelected;
        public event DatEventDelegate EntityPrinted;
        public event DatEventDelegate EntityPreviewed;
        //public event DatEventDelegate EntityXML;

        public void FireEntityNew() 
        {
            if (EntityNew != null)
                EntityNew(this, new DatEventArgs(Grid.Value as BaseDat));
        }
        public void FireEntityDeleted() 
        {
            if (EntityDeleted != null)
                EntityDeleted(this, new DatEventArgs(Grid.Value as BaseDat));
        }
        public void FireEntityChanged() 
        {
            if (EntityChanged != null)
                EntityChanged(this, new DatEventArgs(Grid.Value as BaseDat));
        }
        public void FireEntitySelected() 
        {
            if (EntitySelected != null)
                EntitySelected(this, new DatEventArgs(Grid.Value as BaseDat));
        }
        void Grid_ValueSelected(object sender, ValueEventArgs e)
        {
            if (!ReadOnly && e.Value is BaseDat)
                Edit((BaseDat)e.Value);
        }
        void Grid_ValueInserted(object sender, ValueEventArgs e)
        {
            if (!ReadOnly)
                Add();
        }
        void Grid_ValueDeleted(object sender, ValueEventArgs e)
        {
            if (!ReadOnly && e.Value is BaseDat)
                Delete((BaseDat)e.Value);
        }
        void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Edit((BaseDat)Grid.Value);
        }
        void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Delete((BaseDat)Grid.Value);
        }
        void ButtonPrint_Click(object sender, EventArgs e)
        {
            if (EntityPrinted != null && Grid.Value != null)
                EntityPrinted(this, new DatEventArgs(Grid.Value as BaseDat));
        }
        void ButtonPreview_Click(object sender, EventArgs e)
        {
            if (EntityPreviewed != null && Grid.Value != null)
                EntityPreviewed(this, new DatEventArgs(Grid.Value as BaseDat));
        }

        void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {

            //if (SearchBoxKeyUpTime > DateTime.MinValue && SearchBoxKeyUpTime.AddSeconds(2.0) > DateTime.Now)
            //{
                FilterList();
            //}
            SearchBoxKeyUpTime = DateTime.Now;
        }

        private void FilterList()
        {
            (this.Grid.DataSource as BaseSet).FilterApply(SearchBox.Text);
        }

        //void ButtonXml_Click(object sender, EventArgs e)
        //{
        //    if (EntityXML != null && Grid.Value != null)
        //        EntityXML(this, new DatEventArgs(Grid.Value as BaseDat));
        //}
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
            BaseDat dat = _Grid.Value as BaseDat;
            ButtonPreview.Enabled = (EntityPreviewed != null && dat != null);
            ButtonPrint.Enabled = (EntityPrinted != null && dat != null);
            if (EntityChanged != null)
                EntityChanged(this, new DatEventArgs(dat));
        }

        protected virtual void Add()
        {
            if (_EntityForm == null)
            {
                if (EntityNew != null)
                    EntityNew(this, new DatEventArgs(null));
            }
            else
            {
                try
                {
                    _EntityForm.BindControls(EntityForm);
                    if (_EntityForm.ShowDialog() == DialogResult.OK)
                        ReloadList();
                }
                catch (Exception exp)
                {
                    new ExceptionForm("Внимание", "Не удалось добавить элемент.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
                }
            }
        }
        protected virtual void Edit(BaseDat dat)
        {
            if (_EntityForm == null)
            {
                if (EntitySelected != null)
                    EntitySelected(this, new DatEventArgs(dat));
            }
            else
            {
                try
                {
                    if (dat != null)
                    {
                        _EntityForm.OldValue = dat;
                        _EntityForm.BindControls(EntityForm);
                        //_EntityForm.ShowDialog();
                        if (_EntityForm.ShowDialog() == DialogResult.OK)
                            ReloadList();

                    }
                }
                catch (Exception exp)
                {
                    new ExceptionForm("Внимание", "Не удалось сохранить элемент.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
                }
            }
        }
        protected virtual void Delete(BaseDat dat)
        {
            if (EntityDeleted != null)
                EntityDeleted(this, new DatEventArgs(dat));
            else
            {
                try
                {
                    DataGridViewSelectedRowCollection rows = Grid.SelectedRows;
                    if (rows.Count > 1)
                    {
                        if (MessageBox.Show(string.Format("Вы действительно хотите удалить {0} строк?", rows.Count), "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                        {
                            foreach (DataGridViewRow row in rows)
                            {
                                BaseDat dt = row.DataBoundItem as BaseDat;
                                if (dt != null)
                                {
                                    IDat idat = dt as IDat;
                                    if (idat != null && idat.ID > 0)
                                        dt.Delete();
                                }
                            }
                            ReloadList();
                        }
                    }
                    else if (rows.Count == 1)
                    {
                        //BaseDat dat = rows[0].DataBoundItem as BaseDat;
                        if (dat != null)
                        {
                            if (dat is IDat && ((IDat)dat).ID > 0)
                            {
                                if (MessageBox.Show(string.Format("Вы действительно хотите удалить \r\n{0}?", dat), "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                                {
                                    dat.Delete();
                                    ReloadList();
                                }
                            }
                            else
                            {
                                MessageBox.Show(string.Format("Ветки дерева нужно удалять из дерева."), "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    new ExceptionForm("Внимание", "Не удалось удалить элемент.\nДля более подробной информации раскройте детали сообщения об ошибке.", exp, MessageBoxIcon.Error).ShowDialog();
                }
            }
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
            Init(parentControl, parentControl);
        }
        public void Init(Control parentControl, Control captionControl)
        {
            _ParentControl = parentControl;

            ParentControl.SuspendLayout();
            if (!string.IsNullOrEmpty(_caption))
            {
                captList.Active = false;
                captList.Caption = _caption;
                captList.Dock = DockStyle.Top;

                captionControl.Controls.Add(captList);
            }
            if (_ToolBar != null)
            {
                ParentControl.Controls.Add(_ToolBar);
                _ToolBar.Dock = DockStyle.Top;
                _ToolBar.GripStyle = ToolStripGripStyle.Hidden;
                ButtonNew.Click += new EventHandler(EntityNew_Handler);
                ButtonEdit.Click += new EventHandler(ButtonEdit_Click);
                ButtonDel.Click += new EventHandler(ButtonDelete_Click);
                ButtonPrint.Click += new EventHandler(ButtonPrint_Click);
                ButtonPreview.Click += new EventHandler(ButtonPreview_Click);
                SearchBox.KeyUp += new KeyEventHandler(SearchBox_KeyUp);
                //ButtonXml.Click += new EventHandler(ButtonXml_Click);
                //ButtonListPrint.Click += new EventHandler(ButtonListPrint_Click);
                //ButtonListPreview.Click += new EventHandler(ButtonListPreview_Click);
                // ParentControl.Controls.Add(_ToolBar);
            }
            if (_Grid != null)
            {
                ParentControl.Controls.Add(_Grid);
                _Grid.Dock = DockStyle.Fill;
                _Grid.TabStop = false;
                _Grid.Focus();
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
            Grid.ReadOnly = true;
        }
        #endregion
    }
}
