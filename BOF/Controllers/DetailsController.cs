using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class DetailsController<DD,DS,HD>
        where DD : BaseDat<DD>, IDetailDat<HD>, new()
        where DS : BaseSet<DD, DS>, new()
        where HD : BaseDat<HD>, IDat, new()
    {
        public DetailsController(DetailsWrapper<DD, DS, HD> header)
            : this()
        {
            _header = header;
            _header.DetSet.AllowEdit = true;
            _Grid.DataSource = _header.DetSet;
        }
        private DetailsWrapper<DD, DS, HD> _header = null;

        #region Constructors
        public DetailsController()
        {
            _ToolBar = new ToolStrip();
            _ToolBar.Items.Add(ButtonNew);
            _ToolBar.Items.Add(ButtonEdit);
            _ToolBar.Items.Add(ButtonDel);
            _Grid = new CtlGrid();
            SetGridStyles();
        }


        #endregion

        #region Fields

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

        private ToolStripButton ButtonNew = new ToolStripButton("Создать", global::BOF.Properties.Resources.NewItem.ToBitmap());
        private ToolStripButton ButtonEdit = new ToolStripButton("Исправить", global::BOF.Properties.Resources.EditItem.ToBitmap());
        private ToolStripButton ButtonDel = new ToolStripButton("Удалить", global::BOF.Properties.Resources.DeleteItem.ToBitmap());
        #endregion

        #region Properties
        public Control ParentControl
        {
            get { return _ParentControl; }
        }
        #endregion

        #region Events
        public event DatEventDelegate EntityChanged;
        //public event DatEventDelegate EntityNew;
        //public event DatEventDelegate EntityDeleted;
        //public event DatEventDelegate EntitySelected;
        //public event DatEventDelegate EntityPrinted;
        //public event DatEventDelegate EntityPreviewed;

        public void FireEntityNew() { }
        public void FireEntityDeleted() { }
        public void FireEntityChanged() { }
        public void FireEntitySelected() { }

        void Grid_ValueSelected(object sender, ValueEventArgs e)
        {
            if(!ReadOnly)
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
                Delete();
        }
        void ButtonEdit_Click(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Edit((BaseDat)Grid.Value);
        }
        void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (!ReadOnly) 
                Delete();
        }

        void EntityNew_Handler(object sender, EventArgs e)
        {
            if (!ReadOnly)
                Add();
        }
        void EntityChanged_Handler(object sender, ValueEventArgs e)
        {
            BaseDat dat = _Grid.Value as BaseDat;
            if (EntityChanged != null)
                EntityChanged(this, new DatEventArgs(dat));
        }

        protected virtual void Add()
        {
            _header.Add(new DD());
        }
        protected virtual void Edit(BaseDat dat)
        {

        }
        protected virtual void Delete()
        {

            BaseDat dat = (BaseDat)_Grid.Value;
            try
            {
                if (dat != null)
                {
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить \r\n{0}?", dat), "Удаление", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        _header.DetSet.Remove(dat);
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Methods
        public virtual void ReloadList()
        {
           _header.DetSet.LoadByHeaderDat(_header.Header);
        }
        public void Init(Control parentControl)
        {
            _ParentControl = parentControl;

            ParentControl.SuspendLayout();
            if (_ToolBar != null)
            {
                ParentControl.Controls.Add(_ToolBar);
                _ToolBar.Dock = DockStyle.Top;
                ButtonNew.Click += new EventHandler(EntityNew_Handler);
                ButtonEdit.Click += new EventHandler(ButtonEdit_Click);
                ButtonDel.Click += new EventHandler(ButtonDelete_Click);
                //ButtonPrint.Click += new EventHandler(ButtonPrint_Click);
                //ButtonPreview.Click += new EventHandler(ButtonPreview_Click);
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
                //ParentControl.Controls.Add(_Grid);
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
