using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Linq;
using System.ComponentModel;

namespace BOF
{

    public class CtlTreeDict<S, D> : TreeView
        where S : BaseSet<D, S>, new()
        where D : BaseDat<D>, ITreeDictDat, new()
    {
        public event ValueEventHandler ValueChanged;
        public event ValueEventHandler ValueSelected;

        public CtlTreeDict()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            this.AfterSelect += new TreeViewEventHandler(CtlTree_AfterSelect);
            this.DoubleClick += new EventHandler(CtlTree_DoubleClick);
            this.MouseDown += new MouseEventHandler(CtlTree_MouseDown); ;
            this.AfterLabelEdit += new NodeLabelEditEventHandler(CtlTree_AfterLabelEdit);
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.ImageList = new ImageList();
            this.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.ImageList.Images.Add(global::BOF.Properties.Resources.Folder);
            this.ImageList.Images.Add(global::BOF.Properties.Resources.textdoc);
            this.LabelEdit = true;
            this.TabStop = true;
        }

        private bool _ShowCards = false;
        public bool ShowCards
        {
            get { return _ShowCards; }
            set { _ShowCards = value; }
        }

        private bool _ShowContextMenu = false;
        public bool ShowContextMenu
        {
            get { return _ShowContextMenu; }
            set 
            { 
                _ShowContextMenu = value;
                if (_ShowContextMenu && this.ContextMenu == null)
                {
                    this.ContextMenu = new ContextMenu();
                    this.ContextMenu.MenuItems.Add("Создать новую ветку", CtlTree_Add);
                    MenuItem btnEdit = this.ContextMenu.MenuItems.Add("Исправить ветку", CtlTree_Edit);
                    MenuItem btnDel = this.ContextMenu.MenuItems.Add("Удалить ветку", CtlTree_Delete);
                    btnEdit.Enabled = btnDel.Enabled = false;
                    this.ContextMenu.Popup += new EventHandler(delegate(object snd, EventArgs ev) 
                        { 
                            btnEdit.Enabled = btnDel.Enabled = this.Value != null; 
                        });
                }
                else
                    this.ContextMenu = null;
            }
        }

        private bool _ShowExpanded = false;
        public bool ShowExpanded
        {
            get { return _ShowExpanded; }
            set { _ShowExpanded = value; }
        }

        private S _DataSource;
        public S DataSource
        {
            get{ return _DataSource; }
            set
            {
                _DataSource = value;
                Reload();
            }
        }

        public D Value
        {
            get { return (SelectedNode == null) ? null : SelectedNode.Tag as D; }
            set
            {
                ITreeDictDat val = value as ITreeDictDat;
                SelectedNode = (val == null) ? null : FindNode(val.FP.ToString());
            }
        }

        const string _helplabeltext = "Enter, вправо - к следующему полю, влево - к предыдущему полю.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText + (ShowContextMenu ? "F2 - редактировать, Insert - добавить, Delete - удалить. Правая кнопка мыши - контекстное меню." : ""); }
            set { _HelpLabelText = value; }
        }
        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
                this.Invalidate();
            }
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp("");
                this.Invalidate();
            }
            base.OnLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.Focused && this.Enabled && e.ClipRectangle == this.ClientRectangle)
                ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Black, ButtonBorderStyle.Solid);
        }
        
        public List<D> GetChildren(bool cards_only)
        {
            List<D> ret = Value == null ? null : _DataSource.Where(d => (!cards_only || !d.IsTree) && d.Parent_FP != null && d.Parent_FP.ToString() == Value.FP).ToList();
            return ret;
        }
        
        public TreeNode FindNode(string fp)
        {
            TreeNode[] nodes = Nodes.Find(fp, true);
            return (nodes.Length > 0) ? nodes[0] : null;
        }

        public void Reload()
        {
            Nodes.Clear();
            if (_DataSource != null)
            {
                D old_val = Value;
                _DataSource.Sort(BaseSet.FPComparison);
                foreach (D val in _DataSource)
                {
                    if (val.IsTree || ShowCards)
                    {
                        TreeNode parent = (val.Parent_FP == null) ? null : FindNode(val.Parent_FP.ToString());
                        TreeNode node = (parent == null ? Nodes : parent.Nodes).Add(val.FP.ToString(), val.Name);
                        node.Tag = val;
                        node.ImageIndex = node.SelectedImageIndex = val.IsTree ? 0 : 1;
                    }
                }
                if (ShowExpanded)
                    ExpandAll();
                Value = old_val;
                if (Value == null && Nodes.Count > 0)
                    SelectedNode = Nodes[0];
            }
        }

        void CtlTree_DoubleClick(object sender, EventArgs e)
        {
            if (ValueSelected != null)
                ValueSelected(this, new ValueEventArgs(Value));
        }

        void CtlTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, new ValueEventArgs(e.Node.Tag as IDictDat));
        }

        void CtlTree_MouseDown(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo hit = HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right && hit.Node != SelectedNode)
            {
                SelectedNode = hit.Node;
                if (ValueChanged != null)
                    ValueChanged(this, new ValueEventArgs(null));
            }
        }

        void CtlTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            D dat = e.Node.Tag as D;
            try
            {
                if (dat != null && !Common.IsNullOrEmpty(e.Label))
                {
                    dat.Name = e.Label;
                    dat.Save();
                    if (!e.Node.IsSelected)
                        this.SelectedNode = e.Node;
                }
                else if (dat.IsNew)
                    e.Node.Remove();
                else
                {
                    e.CancelEdit = true;
                    e.Node.Text = dat.Name;
                }
            }
            catch (Exception exp)
            {
                e.CancelEdit = true;
                if (dat.IsNew)
                    e.Node.Remove();
                new ExceptionForm("Ошибка сохранения ветки", "При сохранении ветки возникли ошибки.\nБолее подробная информация - в деталях", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }

        void CtlTree_Add(object sender, EventArgs e)
        {
            try
            {
                int max_code = 0;
                TreeNodeCollection nodes = this.SelectedNode == null ? this.Nodes : this.SelectedNode.Nodes;
                foreach (TreeNode node in nodes)
                {
                    D val = node.Tag as D;
                    int code = 0;
                    string[] codes = val.SCode.Split('.');
                    int.TryParse(codes.Last(), out code);
                    if (val != null && val.IsTree && code >= max_code)
                        max_code = code;
                }
                max_code++;
                D parent = (this.SelectedNode != null) ? this.SelectedNode.Tag as D : null;
                D dat = new D();
                dat.Name = "Новая ветка";
                dat.IsTree = true;
                dat.Parent_FP = (parent == null) ? null : new PathTree(parent.FP);
                dat.SCode = max_code.ToString();
                TreeNode new_node = nodes.Insert(nodes.IndexOf(this.SelectedNode) + 1, dat.Name);
                new_node.Tag = dat;
                if (this.SelectedNode != null)
                    this.SelectedNode.Expand();
                this.SelectedNode = new_node;
                new_node.BeginEdit();
            }
            catch (Exception exp)
            {
                new ExceptionForm("Ошибка создания ветки", "При создании ветки возникли ошибки.\nБолее подробная информация - в деталях", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }

        void CtlTree_Edit(object sender, EventArgs e)
        {
            if (this.SelectedNode != null)
                this.SelectedNode.BeginEdit();
        }

        void CtlTree_Delete(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить ветку '" + Value.Name + "' ?","Внимание", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                return;
            try
            {
                D dat = (this.SelectedNode != null) ? this.SelectedNode.Tag as D : null;
                if (dat != null)
                {
                    dat.Delete();
                    this.SelectedNode.Remove();
                }
            }
            catch (Exception exp)
            {
                new ExceptionForm("Ошибка удаления ветки", "Ветка не может быть удалена.\nВозможно, существуют ссылки на неё.\nБолее подробная информация - в деталях", exp, MessageBoxIcon.Error).ShowDialog();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Return)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, true, true, true, true);
                return true;
            }
            else if (keyData == Keys.Right)
            {
                Form frm = this.FindForm();
                if (frm != null)
                    frm.SelectNextControl(this, false, true, true, true);
            }
            else if (keyData == Keys.Insert)
            {
                CtlTree_Add(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.Delete)
            {
                CtlTree_Delete(this, EventArgs.Empty);
                return true;
            }
            else if (keyData == Keys.F2)
            {
                CtlTree_Edit(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
