using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Collections;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;

/*
 * Init:
 * {
 *      treeGroup.SetClass = setClass;
 *      treeGroup.AddGroup<TestDat, TestSet>("Client", "Клиенты");
 *      treeGroup.AddGroup<TestDat, TestSet>("Account", "Счета");
 *      treeGroup.AddGroup<TestDat, TestSet>("Date", "Даты");
 * }
 *      private void treeGroup_GroupChanged(object sender, EventArgs e)
        {
            treeGroup.FilterSetClass<TestDat, TestSet>();
        }

*/

namespace BOF
{
    [DefaultEvent("GroupChanged")]
    public class TreeGroups : TreeView
    {
        public TreeGroups()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            CheckBoxes = true;
            imgList.Images.Add(global::BOF.Properties.Resources.document);
            imgList.Images.Add(global::BOF.Properties.Resources.Folder);

            ImageList = this.imgList;
            ImageIndex = 1;
            SelectedImageIndex = 1;
            Indent = 19;
            LineColor = System.Drawing.Color.Blue;
            Name = "treeGroup";
            ShowNodeToolTips = true;
            ShowPlusMinus = true;
            ShowLines = true;
        }

        private ImageList imgList = new ImageList();
        private bool inCheckingMode = false;
        private BaseSet setClass = null;

        [Browsable(false), ReadOnly(true)]
        public BaseSet SetClass
        {
            get { return setClass; }
            set { setClass = value; }
        }

        public event EventHandler GroupChanged;

        public TreeNode AddGroup<TD, TS>(string name, ToStringDelegate prop, ToStringDelegate child, ToStringDelegate parent)
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            List<string> chs = new List<string>();
            TreeNode par = null;
            TreeNode chi = null;
            TS set = SetClass as TS;
            //DatDescriptor dd = set.GetDescriptor(prop);
            TreeNode ret = new TreeNode(name);
            ret.Checked = true;
            ret.Tag = prop;
            foreach (TD dat in set)
            {
                string pr = parent(dat);
                if (Common.IsNullOrEmpty(pr))
                    par = ret;
                else
                {
                    par = null;
                    foreach (TreeNode t in ret.Nodes)
                    {
                        if (t.Text == pr)
                            par = (TreeNode)t;
                    }
                    if (par == null)
                    {
                        par = new TreeNode(pr);
                        ret.Nodes.Add(par);
                        par.Checked = true;
                    }
                }
                string ch = child(dat);
                if (Common.IsNullOrEmpty(ch))
                    chi = par;
                else
                {
                    chi = null;
                    foreach (TreeNode t in par.Nodes)
                    {
                        if (t.Text == ch)
                            chi = (TreeNode)t;
                    }
                    if (chi == null)
                    {
                        chi = new TreeNode(ch);
                        par.Nodes.Add(chi); 
                        chi.Checked = true;
                    }
                }
                //if (dd != null)
                //{
                string val = prop(dat);
                if (!Common.IsNullOrEmpty(val) && !chs.Exists(delegate(string s) { return s == val; }))
                {
                    TreeNode node = new TreeNode(val);
                    node.Checked = true;
                    node.Tag = val;
                    chs.Add(val);
                    chi.Nodes.Add(node);
                }
                //}
            }
            Nodes.Add(ret);
            return ret;
        }

        /// <summary>
        /// Добавление новой категории в список только по имени свойства.
        /// Все остальные данные берутся из дескриптора. 
        /// Для свойства с этим именем нужен делегат Get_#Name#, иначе будет работать через Reflection
        /// </summary>
        /// <typeparam name="TD">Тип Dat-класса (BaseDat&lt;TD&gt;)</typeparam>
        /// <typeparam name="TS">Тип Set-класса (BaseSet&lt;TD,TS&gt;)</typeparam>
        /// <param name="name">Название свойства</param>
        /// <param name="text">Видимое название категории</param>
        /// <returns>Ссылку на TreeNode с созданной категорией</returns>
        public TreeNode AddGroup<TD, TS>(string name, string text)
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            return AddGroup<TD, TS>(name, text, delegate(object val)
                            {
                                return (val != null) ? val.ToString() : "";
                            });
        }

        /// <summary>
        /// Добавление новой категории в список только по имени свойства.
        /// Все остальные данные берутся из дескриптора. 
        /// Для свойства с этим именем нужен делегат Get_#Name#, иначе будет работать через Reflection
        /// </summary>
        /// <typeparam name="TD">Тип Dat-класса (BaseDat&lt;TD&gt;)</typeparam>
        /// <typeparam name="TS">Тип Set-класса (BaseSet&lt;TD,TS&gt;)</typeparam>
        /// <param name="name">Название свойства</param>
        /// <param name="text">Видимое название категории</param>
        /// <param name="tostring">Делегат string ToStringDelegate(object value).
        /// Пример:delegate(object val){return "Клиент " + val.ToString();});
        /// </param>
        /// <returns>Ссылку на TreeNode с созданной категорией</returns>
        public TreeNode AddGroup<TD, TS>(string name, string text, ToStringDelegate tostring)
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            TS set = SetClass as TS;
            const int docImageIndex = 0;
            const int folderImageIndex = 1;
            inCheckingMode = true;

            TreeNode root = new TreeNode(text);
            root.Name = name;
            root.ImageIndex = folderImageIndex;
            root.SelectedImageIndex = folderImageIndex;

            Nodes.Add(root);
            DatDescriptor dd = set.GetDescriptor(name);

            root.Tag = dd;

            SortedDictionary<string, object> dict = new SortedDictionary<string, object>();
            foreach (TD dat in set)
            {
                object val;
                if (dd != null)
                    val = dd.GetValue(dat);
                else
                    val = "Не найден дескриптор для " + name;
                string key = tostring(val);
                if (!dict.ContainsKey(key))
                {
                    if (val is IDat)
                        val = ((IDat)val).ID;
                    dict.Add(key, val);
                }
            }

            List<TreeNode> nodes = new List<TreeNode>();
            foreach (KeyValuePair<string, object> de in dict)
            {
                TreeNode node = new TreeNode(de.Key);
                node.Tag = de.Value;
                node.ImageIndex = docImageIndex;
                node.SelectedImageIndex = docImageIndex;
                node.Checked = true;
                nodes.Add(node);
            }
            if (nodes.Count > 0)
                root.Nodes.AddRange(nodes.ToArray());
            root.Checked = true;
            inCheckingMode = false;
            return root;
        }

        /// <summary>
        /// Добавление новой категории в список по делегату для сравнения.
        /// </summary>
        /// <typeparam name="TD">Тип Dat-класса (BaseDat&lt;TD&gt;)</typeparam>
        /// <typeparam name="TS">Тип Set-класса (BaseSet&lt;TD,TS&gt;)</typeparam>
        /// <param name="text">Видимое название категории</param>
        /// <param name="tostring">Делегат string ToStringDelegate&lt;TD&gt;(TD value).
        /// Пример: delegate(TestDat val){return val.Date.ToShortDateString();}
        /// </param>
        /// <param name="compare">Делегат для сравнения двух Dat-классов int Comparison&lt;TD&gt;(TD dat). 
        /// При помощи делегата можно сравнить два Dat-класса по любому признаку. 
        /// По этому признаку будет отсортирован массив значений, и затем будет работать фильтр.
        /// Пример:delegate(TestDat dat1, TestDat dat2){return DateTime.Compare(dat1.Date.Date, dat2.Date.Date);}
        /// В этом примере будет построен список по датам без учета времени.
        /// </param>
        /// <returns>Ссылку на TreeNode с созданной категорией</returns>
        public TreeNode AddGroup<TD, TS>(string text, ToStringDelegate<TD> tostring, Comparison<TD> compare)
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            TS set = SetClass as TS;
            const int docImageIndex = 0;
            const int folderImageIndex = 1;
            inCheckingMode = true;

            TreeNode root = new TreeNode(text);
            root.Name = "";
            root.Tag = compare;
            root.ImageIndex = folderImageIndex;
            root.SelectedImageIndex = folderImageIndex;

            Nodes.Add(root);

            //List<TD> list = set.ConvertType<TD>();
            //list.Sort(compare);
            set.Sort(compare);

            List<TreeNode> nodes = new List<TreeNode>();
            TD prev = null;
            foreach (TD dat in set)
            {
                if (prev == null || compare(prev, dat) != 0)
                {
                    TreeNode node = new TreeNode(tostring(dat));
                    node.Tag = dat;
                    node.ImageIndex = docImageIndex;
                    node.SelectedImageIndex = docImageIndex;
                    node.Checked = true;
                    nodes.Add(node);

                    prev = dat;
                }
            }

            if (nodes.Count > 0)
                root.Nodes.AddRange(nodes.ToArray());
            root.Checked = true;
            inCheckingMode = false;
            return root;
        }

        public TreeNode AddGroupDate<TD, TS>(string name, string text)
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            return AddGroup<TD, TS>(name, text, delegate(object value)
                            {
                                if (value == null)
                                    return "";
                                else if (value is DateTime)
                                    return ((DateTime)value).ToString("yyyy-MM-dd");
                                else
                                    return value.ToString();
                            });
        }

        bool isEqual(object val1, object val2)
        {
            if (val1 is string && val2 is string)
                return (string)val1 == (string)val2;
            if (val1 is DateTime && val2 is DateTime)
            {
                //return (DateTime)val1 == (DateTime)val2;
                return ((DateTime)val1).Date == ((DateTime)val2).Date;
            }
            if (val1 is decimal && val2 is decimal)
                return (decimal)val1 == (decimal)val2;
            if (val1 is int && val2 is int)
                return (int)val1 == (int)val2;
            if (val1 is IDat && val2 is IDat)
                return (val1.GetType() == val2.GetType())
                    && ((IDat)val1).ID == ((IDat)val2).ID;
            if (val1 is Enum && val1 is Enum)
                return (int)val1 == (int)val2;
            return val1 == val2;
        }

        void FillChecksUp(TreeNode sender)
        {
            if (sender.Parent != null)
            {
                int nodechecked = 0;
                int nodeunchecked = 0;
                foreach (TreeNode node in sender.Parent.Nodes)
                {
                    if (!node.Checked)
                        nodeunchecked++;
                    else if (!_indeterminateds.Contains(node))
                        nodechecked++;
                }
                TreeNode parent = (TreeNode)sender.Parent;
                if (nodeunchecked == sender.Parent.Nodes.Count)
                {
                    parent.Checked = false;
                    _indeterminateds.Remove(parent);
                }
                else
                {
                    parent.Checked = true;
                    if (nodechecked == sender.Parent.Nodes.Count)
                        _indeterminateds.Remove(parent);
                    else if (!_indeterminateds.Contains(parent))
                        _indeterminateds.Add(parent);
                }
                FillChecksUp(sender.Parent);
            }
        }

        void FillChecksDown(TreeNode sender)
        {
            if (sender.Nodes.Count > 0)
            {
                foreach (TreeNode node in sender.Nodes)
                {
                    node.Checked = sender.Checked;
                    FillChecksDown(node);
                }
            }
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            base.OnAfterCheck(e);
            if (!inCheckingMode)
            {
                inCheckingMode = true;
                FillChecksDown(e.Node);
                //_indeterminateds.Clear();
                FillChecksUp(e.Node);
                inCheckingMode = false;
                Invalidate();
                if (GroupChanged != null)
                    GroupChanged(this, EventArgs.Empty);
            }
        }

        public void FilterSetClass<TD, TS>()
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>, new()
        {
            TS set = setClass as TS;
            if (set != null)
            {
                set.FilterApply(delegate(TD dat)
                        {
                            bool ret = true;
                            foreach (TreeNode root in this.Nodes)
                            {
                                //if (root.Checked)
                                //    ret &= true;
                                Comparison<TD> cmp = root.Tag as Comparison<TD>;
                                if (cmp != null)
                                    ret &= findNodeByComparison<TD>(dat, root);
                                else
                                {
                                    object val = null;
                                    if (root.Tag is DatDescriptor)
                                        val = ((DatDescriptor)root.Tag).GetValue(dat);
                                    else if (root.Tag is ToStringDelegate)
                                        val = ((ToStringDelegate)root.Tag)(dat);
                                    ret &= findNodeByDescriptor(val, root);
                                }
                            }
                            return ret;
                        });
            }
        }

        private static bool findNodeByComparison<TD>(TD dat, TreeNode root) 
            where TD : BaseDat<TD>, new()
        {
            Comparison<TD> cmp = root.Tag as Comparison<TD>;
            if (cmp != null)
            {
                foreach (TreeNode node in root.Nodes)
                {
                    if (node.Checked && cmp(node.Tag as TD, dat) == 0)
                        return true;
                }
            }
            return false;
        }

        bool findNodeByDescriptor(object value, TreeNode root)
        {
            if (value is IDat)
                value = ((IDat)value).ID;
            foreach (TreeNode node in root.Nodes)
            {
                if (node.Checked)
                {
                    if (node.Nodes.Count == 0)
                    {
                        if (isEqual(node.Tag, value))
                            return true;
                    }
                    else if (findNodeByDescriptor(value, node))
                        return true;
                }
            }
            return false;
        }

        List<TreeNode> _indeterminateds = new List<TreeNode>();
        protected override void WndProc(ref Message m)
        {
            const int WM_Paint = 15;
            base.WndProc(ref m);
            if (m.Msg == WM_Paint)
            {
                foreach (TreeNode nd in _indeterminateds)
                {
                    Graphics g = Graphics.FromHwnd(m.HWnd);
                    g.PageUnit = GraphicsUnit.Pixel;
                    Rectangle rect = new Rectangle(nd.Bounds.X - imgList.ImageSize.Width -17, nd.Bounds.Y + 1, 13, 14);
                    ControlPaint.DrawCheckBox(g, rect, (ButtonState.Checked | ButtonState.Flat | ButtonState.Inactive));
                    rect = new Rectangle(rect.X + 1, rect.Y + 2, rect.Width -2, rect.Height -3);
                    g.DrawRectangle(new Pen(Color.Black, 2), rect);
                    //ControlPaint.DrawBorder(g, rect, Color.Black, ButtonBorderStyle.Solid);
                    //_graphics.DrawImage(_imgIndeterminate, GetCheckRect(nd).Location);
                }
            }
        }
    }
    public delegate string ToStringDelegate(object value);
    public delegate string ToStringDelegate<TD>(TD value) where TD : BaseDat<TD>, new();
}
