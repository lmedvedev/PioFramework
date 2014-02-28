using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Drawing;

namespace BOF
{
    public class TreeNController<TS, TD> 
        where TS : BaseSet<TD, TS>, new()
        where TD : BaseDat<TD>, ITreeNDat, new()
    {
        public event DatEventDelegate TreeNew;
        public event DatEventDelegate TreeDeleted;
        public event DatEventDelegate TreeChanged;
        public event DatEventDelegate TreeSelected;
        //public event EventHandler TreeRefreshed;

        private ToolStrip _ToolBar = new ToolStrip();
        public ToolStrip ToolBar
        {
            get { return _ToolBar; }
            //set { _ToolBar = value; }
        }

        private CtlTreeN _TreeV = new CtlTreeN();
        public CtlTreeN TreeV
        {
            get { return _TreeV; }
            //set { _TreeV = value; }
        }

        private CardTreeNForm _EntityForm = null;
        public CardTreeNForm EntityForm
        {
            get { return _EntityForm; }
            set { _EntityForm = value; }
        }

        //private TS _SetTree = new TS();//null;
        private TS _SetTree = null;
        public TS SetTree
        {
            get { return _SetTree; }
            // set { _setTree = value; }
        }

        private Control _ParentControl;
        public Control ParentControl
        {
            get { return _ParentControl; }
            //set { _ParentControl = value; }
        }

        public TreeNController()
        {
            _ToolBar.Items.Add("Добавить ветку", global::BOF.Properties.Resources.NewItem.ToBitmap(), ValueNew);
            _ToolBar.Items.Add("Редактировать", global::BOF.Properties.Resources.EditItem.ToBitmap(), ValueEdit);
            _ToolBar.Items.Add("Удалить", global::BOF.Properties.Resources.DeleteItem.ToBitmap(), ValueDelete);
            _ToolBar.Items.Add("Обновить", global::BOF.Properties.Resources.Refresh.ToBitmap(), TreeRefreshed);

            _ToolBar.Dock = DockStyle.Top;
            _TreeV.ValueChanged += new ValueEventHandler(ValueChanged);
            _TreeV.ValueSelected += new ValueEventHandler(ValueEdit);
            _TreeV.AllowDrop = true;
            _TreeV.Dock = DockStyle.Fill;
        }

        public void Init(TS set_tree, Control parent, CardTreeNForm entity_form)
        {
            _EntityForm = entity_form;
            _ParentControl = parent;
            _SetTree = set_tree;
            ParentControl.Controls.Add(_ToolBar);
            ParentControl.Controls.Add(_TreeV);
            _TreeV.BringToFront();
            Reload();
        }

        void ValueEdit(object sender, EventArgs e)
        {
            if (_EntityForm == null)
            {
                TD dat = (TD)_TreeV.Value;
                if (TreeSelected != null)
                    TreeSelected(this, new DatEventArgs(dat));
            }
            else
            {
                try
                {
                    TD dat = (TD)_TreeV.Value;
                    _EntityForm.OldValue = dat;
                    if (_EntityForm.ShowDialog() == DialogResult.OK)
                        Reload();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void TreeRefreshed(object sender, EventArgs e)
        {
            Reload();
        }

        void ValueDelete(object sender, EventArgs e)
        {
            if (_EntityForm == null)
            {
                TD dat = (TD)_TreeV.Value;
                if (TreeDeleted != null)
                    TreeDeleted(this, new DatEventArgs(dat));
            }
            else
            {
                try
                {
                    TD dat = (TD)_TreeV.Value;
                    if (MessageBox.Show(string.Format("Вы действительно хотите удалить ветку \r\n{0}?", dat), "Удаление ветки", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                    {
                        dat.Delete();
                        Reload((dat.Parent_FPn == null) ? "" : dat.Parent_FPn.ToString());
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void ValueNew(object sender, EventArgs e)
        {
            if (_EntityForm == null)
            {
                if (TreeNew != null)
                    TreeNew(this, new DatEventArgs(null));
            }
            else
            {
                try
                {
                    TD ndat = new TD();
                    if (_TreeV.Value != null)
                    {
                        ITreeNDat dat = (ITreeNDat)_TreeV.Value;
                        ndat.FPn = new PathTreeN(dat.FPn, "10");
                    }
                    else
                    {
                        ndat.FPn = new PathTreeN("1");
                    }
                    _EntityForm.OldValue = ndat;

                    if (_EntityForm.ShowDialog() == DialogResult.OK)
                    {
                        Reload(ndat.FPn.ToString());
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        void ValueChanged(object sender, ValueEventArgs e)
        {
            TD dat = (TD)e.Value;
            if (TreeChanged != null)
                TreeChanged(this, new DatEventArgs(dat));
        }

        public void Reload()
        {
            Reload((_TreeV.SelectedNode == null) ? "" : _TreeV.SelectedNode.Name);
        }

        public void Reload(string path)
        {
            if (SetTree != null)
            {
                bool is_cash = SetTree.IsCashSet;
                SetTree.IsCashSet = false;
                SetTree.Load();
                SetTree.IsCashSet = is_cash;
            }
            TreeV.Nodes.Clear();
            TreeV.Fill(SetTree, null);
            TreeNode[] nodes = _TreeV.Nodes.Find(path, true);
            if (nodes.Length > 0)
                _TreeV.SelectedNode = nodes[0];
        }
    }
}
