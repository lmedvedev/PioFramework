using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using BO;

namespace BOF
{

    public class CtlTreeN : TreeView
    {
        public event ValueEventHandler ValueSelected;
        public event ValueEventHandler ValueChanged;

        //private System.Windows.Forms.StatusStrip StatusBar;
        //private System.Windows.Forms.ToolStripStatusLabel stCount;
        //private System.Windows.Forms.ToolStripStatusLabel stComment;

        public CtlTreeN()
        {
            //this.StatusBar = new System.Windows.Forms.StatusStrip();
            //this.stCount = new System.Windows.Forms.ToolStripStatusLabel();
            //this.stComment = new System.Windows.Forms.ToolStripStatusLabel();
            //// 
            //// StatusBar
            //// 
            //this.StatusBar.GripMargin = new System.Windows.Forms.Padding(0);
            //this.StatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.stCount,
            //this.stComment});
            //this.StatusBar.Location = new System.Drawing.Point(0, 314);
            //this.StatusBar.Name = "StatusBar";
            //this.StatusBar.Size = new System.Drawing.Size(309, 22);
            //this.StatusBar.TabIndex = 2;
            //this.StatusBar.Text = "StatusBar";
            //// 
            //// stCount
            //// 
            //this.stCount.BackColor = System.Drawing.Color.Transparent;
            //this.stCount.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            //this.stCount.Name = "stCount";
            //this.stCount.Size = new System.Drawing.Size(23, 17);
            //this.stCount.Text = "0/0";
            //// 
            //// stComment
            //// 
            //this.stComment.BackColor = System.Drawing.Color.Transparent;
            //this.stComment.Name = "stComment";
            //this.stComment.Size = new System.Drawing.Size(240, 17);
            //this.stComment.Spring = true;
            //this.stComment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //this.Controls.Add(this.StatusBar);

            this.AfterSelect += new TreeViewEventHandler(CtlTree_AfterSelect);
            this.DoubleClick += new EventHandler(CtlTree_DoubleClick);
            this.BorderStyle = BorderStyle.None;
            this.FullRowSelect = true;
            this.HideSelection = false;
            this.ImageList = new ImageList();
            this.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.ImageList.Images.Add(global::BOF.Properties.Resources.Folder);
            this.ImageList.Images.Add(global::BOF.Properties.Resources.textdoc);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        private BaseSet _DataSource;
        public BaseSet DataSource
        {
            get { return _DataSource; }
            set
            {
                _DataSource = value;
                Nodes.Clear();
                Fill(_DataSource, null);
            }
        }

        public BaseDat Value
        {
            get
            {
                return (SelectedNode == null) ? null : (BaseDat)SelectedNode.Tag;
            }
            set
            {
                string path = "";
                if (value == null)
                    SelectedNode = null;
                else if (value is ITreeNDat)
                    path = ((ITreeNDat)value).FPn.ToString();
                else if (value is ICardDat)
                    path = ((ICardDat)value).FP.ToString();
                SelectNode(path);
            }
        }

        public void SelectNode(string path)
        {
            TreeNode[] nodes = Nodes.Find(path, true);
            if (nodes.Length > 0)
                SelectedNode = nodes[0];
        }

        void CtlTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //SetState();
            if (ValueChanged != null)
                ValueChanged(this, new ValueEventArgs(Value));
        }

        void CtlTree_DoubleClick(object sender, EventArgs e)
        {
            if (ValueSelected != null)
                ValueSelected(this, new ValueEventArgs(Value));
        }

        List<int> _IncludeList;
        public List<int> IncludeList
        {
            get { return _IncludeList; }
            set { _IncludeList = value; }
        }

        private BaseSet setTree = null;
        private BaseSet setCard = null;

        public void Fill(BaseSet trees, BaseSet cards)
        {
            setTree = trees;
            setCard = cards;
            IncludeList = null;
            Fill();
        }

        public void Fill()
        {
            BaseDat old_val = Value;
            Nodes.Clear();
            List<BaseDat> lst = new List<BaseDat>();
            if (setTree != null)
            {
                List<ITreeNDat> tree_lst = ((ISet)setTree).ConvertType<ITreeNDat>();
                tree_lst.Sort(BaseSet.FPComparison);
                lst.AddRange(tree_lst.ConvertAll<BaseDat>(delegate(ITreeNDat tree) { return tree as BaseDat; }));
            }

            if (setCard != null)
            {
                List<ICardNDat> card_lst = ((ISet)setCard).ConvertType<ICardNDat>();
                card_lst.Sort(BaseSet.FPComparison);
                lst.AddRange(card_lst.ConvertAll<BaseDat>(delegate(ICardNDat card) { return card as BaseDat; }));
            }

            if (setTree != null)
                FillTree(Nodes, lst, IncludeList, null);
            else
                Fill(Nodes, lst, IncludeList, null);

            Value = old_val;
            //SetState();
        }

        private TreeNode NewNode(string key, ITreeNDat dt)
        {
            TreeNode ret = new TreeNode();
            ret.Name = key;
            ret.Tag = dt;

            if (dt is ITreeCustomDisplayName)
                ret.Text = ((ITreeCustomDisplayName)dt).DisplayName;
            else
                ret.Text = string.Format("{0}: {1}", dt.FPn.CodeS, dt.Name);
            
            ret.ImageIndex = ret.SelectedImageIndex = 0;

            return ret;
        }

        //private TreeNode NewNode(string key, string text, ITreeDat dt)
        //{
        //    TreeNode ret = new TreeNode(text);
        //    ret.Name = key;
        //    ret.Tag = dt;
        //    ret.ImageIndex = ret.SelectedImageIndex = 0;

        //    return ret;
        //}
        private void FillTree(TreeNodeCollection nodes, List<BaseDat> lst, List<int> included, ITreeNDat dat)
        {
            List<TreeNode> lastNode = new List<TreeNode>();
            int startDepth = (lst.Count > 0) ? ((ITreeNDat)lst[0]).FPn.Count : 0;

            foreach (ITreeNDat dt in lst)
            {
                bool is_empty = false;


                TreeNode node = NewNode(dt.FPn.ToString(), dt);

                if (lastNode.Count < dt.FPn.Count)
                    lastNode.Add(null);

                lastNode[dt.FPn.Count - startDepth] = node;

                if (dt.FPn.Count == startDepth)
                    nodes.Add(node);
                else if (dt.FPn.Count > startDepth)
                    lastNode[dt.FPn.Count - startDepth - 1].Nodes.Add(node);

                if (node.Nodes.Count == 0)
                    is_empty = true;
                else
                    is_empty = CardsCount(node.Nodes) == 0;
                if (is_empty && included != null)
                    nodes.Remove(node);
            }
        }

        private void Fill(TreeNodeCollection nodes, List<BaseDat> lst, List<int> included, BaseDat dat)
        {
            foreach (BaseDat dt in GetChildren(lst, dat))
            {
                string path = "";
                string name = "";
                string codeN = "";
                if (dt is ITreeNDat)
                {
                    ITreeNDat tr = (ITreeNDat)dt;
                    path = tr.FPn.ToString();
                    name = tr.Name;
                    codeN = tr.FPn.CodeS;
                }
                else if (dt is ICardNDat)
                {
                    ICardNDat cr = (ICardNDat)dt;
                    if (included != null && included.IndexOf(cr.ID) < 0)
                        continue;
                    path = cr.FPn.ToString();
                    name = cr.Name;
                    codeN = cr.CodeN;
                }

                bool is_empty = false;
                TreeNode node = nodes.Add(path, string.Format("{0}: {1}", codeN, name));
                node.Tag = dt;
                if (dt is ITreeNDat)
                {
                    node.ImageIndex = node.SelectedImageIndex = 0;
                    Fill(node.Nodes, lst, included, dt);
                    if (node.Nodes.Count == 0)
                        is_empty = true;
                    else
                        is_empty = CardsCount(node.Nodes) == 0;
                }
                else if (dt is ICardNDat)
                    node.ImageIndex = node.SelectedImageIndex = 1;
                if (is_empty && included != null)
                    nodes.Remove(node);
            }
        }

        public int CardsCount(TreeNodeCollection nodes)
        {
            int ret = 0;
            foreach (TreeNode nd in nodes)
            {
                if (nd.Tag is ICardNDat)
                    ret++;
                else if (nd.Nodes.Count > 0)
                    ret += CardsCount(nd.Nodes);
            }
            return ret;
        }

        private static List<BaseDat> GetChildren(List<BaseDat> lst, BaseDat dat)
        {
            PathTreeN path = (dat is ITreeNDat) ? ((ITreeNDat)dat).FPn : null;
            List<BaseDat> ret = lst.FindAll(delegate(BaseDat dt)
            {
                return (
                    ((dt is ITreeNDat) && (((ITreeNDat)dt).FPn.Parent == path))
                    ||
                    ((dt is ICardNDat) && (((ICardNDat)dt).FPn.Parent == path))
                    );
            });
            return ret;
        }

        //private void SetState()
        //{
        //    int cnt_all = (setCard == null) ? 0 : setCard.Count;
        //    int cnt_inc = (IncludeList == null) ? cnt_all : IncludeList.Count;
        //    stCount.Text = string.Format("{0}/{1}", cnt_inc, cnt_all);
        //    if (SelectedNode == null)
        //        stComment.Text = "";
        //    else
        //    {
        //        if (SelectedNode.Tag is ICardNDat)
        //            stComment.Text = string.Format(((BaseDat)SelectedNode.Tag).ToString());
        //        else
        //        {
        //            int cnt = CardsCount(SelectedNode.Nodes);
        //            stComment.Text = string.Format("карточек в ветке: {0}", cnt);
        //        }
        //    }
        //}
    }
}
