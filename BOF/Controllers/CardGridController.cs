using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Windows.Forms;
using System.Collections;

namespace BOF
{
    public class CardGridController<CS, CD>
        where CS : BaseSet<CD, CS>, new()
        where CD : BaseDat<CD>, ICardDat, new()
    {
        #region Constructors
        public CardGridController(DataGridView grd)
        {
            cardSet = Activator.CreateInstance(typeof(CS)) as CS;
            grid = grd;
            SetGridStyles();
            grid.DataSource = cardSet;
            grid.SelectionChanged += new EventHandler(grid_SelectionChanged);
            grid.CellDoubleClick += new DataGridViewCellEventHandler(grid_CellDoubleClick);

            grid.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CellFormatting);
            grid.PreviewKeyDown += new PreviewKeyDownEventHandler(grid_PreviewKeyDown);
        }

        #endregion

        #region Fields
        protected DataGridView grid;
        protected CS cardSet;
        #endregion

        #region Events
        public event DatEventDelegate DatValueSelected;
        public event DatEventDelegate DatValueChanged;
        
        void grid_SelectionChanged(object sender, EventArgs e)
        {
            BaseDat dat = GetCard();
            //Console.WriteLine(dat);
            OnValueChanged(new DatEventArgs(dat));
        }
        void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                CD dat = cardSet[e.RowIndex] as CD;
                OnValueSelected(new DatEventArgs(dat));
            }
        }

        public void FireDatValueSelected(BaseDat dat)
        {
            if (DatValueSelected != null)
                DatValueSelected(this, new DatEventArgs(dat));
        }
        public void FireDatValueChanged(BaseDat dat)
        {
            if (DatValueChanged != null)
                DatValueChanged(this, new DatEventArgs(dat));
        }
        
        
        protected virtual void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return :
                    CD dat = GetCard();
                    FireDatValueSelected(dat);
                    break;
                case Keys.Delete:
                    RemoveSelected();
                    break;
            }
        }
        void grid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            OnPreviewKeyDown(e);
        }
        
        protected virtual void OnValueSelected(DatEventArgs e)
        {
            FireDatValueSelected(e.DatEntity);
        }
        protected virtual void OnValueChanged(DatEventArgs e)
        {
            FireDatValueChanged(e.DatEntity);
        }

        void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            switch (grid.Columns[e.ColumnIndex].Name)
            {
                case "Icon":
                    if (e.Value is int)
                    {
                        if (BaseDat.O2Int32(e.Value) <= 0)
                            e.Value = global::BOF.Properties.Resources.Folder;
                        else
                            e.Value = global::BOF.Properties.Resources.textdoc;
                    }
                    break;
                case "Code":
                    if (e.Value is int && BaseDat.O2Int32(e.Value) == 0)
                        e.Value = "..";
                    break;
                case "FP":
                    CD dat = cardSet[e.RowIndex] as CD;
                    if (dat.ID < 0)
                        e.Value = PathCard2Tree(dat);
                    break;
            }
        }
        #endregion

        protected virtual Comparison<CD> CardsComparison
        {
            get { return CardsFPComparison; }
        }
        protected virtual DataGridViewColumn CodeGridColumn
        {
            get
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "FP";
                col.DataPropertyName = "FP";
                col.HeaderText = "Код";
                col.ValueType = typeof(string);
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                return col;
            }
        }
        protected virtual void Refresh()
        {
            cardSet.Sort(CardsComparison);
        }

        private void SetGridStyles()
        {
            grid.AutoGenerateColumns = false;
            grid.Columns.Clear();

            DataGridViewColumn col = new DataGridViewImageColumn(true);
            col.HeaderText = "";
            col.Name = "Icon";
            col.DataPropertyName = "ID";
            col.Width = 20;
            grid.Columns.Add(col);

            grid.Columns.Add(CodeGridColumn);

            col = new DataGridViewTextBoxColumn();
            col.Name = "Name";
            col.DataPropertyName = "Name";
            col.HeaderText = "Название";
            col.ValueType = typeof(string);
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            grid.Columns.Add(col);
        }

        public virtual CD GetCard()
        {
            if (grid.CurrentRow == null) return null;
            int index = grid.CurrentRow.Index;

            CS set = grid.DataSource as CS;
            if (set == null) return null;

            return set[index] as CD;
        }
        //public object GetPath()
        //{
        //    CD dat = GetValue();
        //    if (dat == null) return null;

        //    if (dat.ID > 0)
        //        return dat.FP;
        //    else if (dat.ID < 0)
        //        return new PathTree(dat.Parent_FP, dat.Code);
        //    else
        //        return null;
        //}
        
        public CD[] GetSelectedValues()
        {
            List<CD> list = new List<CD>();
            foreach (DataGridViewRow row in grid.SelectedRows)
            {
                CD dat = row.DataBoundItem as CD;
                if (dat != null)
                    list.Add(dat);
            }
            return list.ToArray();
        }
        public object[] GetData()
        {
            Dictionary<int, ArrayList> dict = new Dictionary<int, ArrayList>();
            foreach (CD dat in cardSet)
            {
                int root;
                if (dat.ID < 0)
                {
                    PathTree tree = new PathTree(dat.Parent_FP, dat.Code);
                    root = tree.Root;
                    if (!dict.ContainsKey(root))
                        dict[root] = new ArrayList();
                    dict[root].Add(tree);
                }
                else if (dat.ID > 0)
                {
                    root = dat.FP.Root;
                    if (!dict.ContainsKey(root))
                        dict[root] = new ArrayList();
                    dict[root].Add(dat.ID);
                }
            }
            object[] retList = new object[dict.Count];
            int i = 0;
            foreach (ArrayList al in dict.Values)
            {
                if (al.Count > 1)
                    retList[i++] = al.ToArray();
                else
                    retList[i++] = al[0];
            }
            return retList;
        }
        public void Add(IEnumerable<CD> list)
        {
            foreach (CD dat in list)
                Add(dat);
        }
        public virtual void Add(CD card)
        {
            if (card != null && card.ID != 0)
            {
                if (findParent(card)) return;

                if (card.ID < 0)
                {
                    PathTree addedTree = new PathTree(card.Parent_FP, card.Code);
                    List<CD> deletedList = new List<CD>();
                    foreach (CD dat in cardSet)
                    {
                        PathTree tree = null;
                        if (dat.ID < 0)
                            tree = new PathTree(dat.Parent_FP, dat.Code);
                        else
                            tree = dat.Parent_FP;
                        if (tree != null && tree.IsChildOf(addedTree))
                            deletedList.Add(dat);
                    }
                    foreach (CD dat in deletedList)
                    {
                        cardSet.Remove(dat);
                    }
                }
                cardSet.Add(card);
                Refresh();
            }
        }
        public void Add(ITreeDat tree)
        {
            CD dat = Activator.CreateInstance(typeof(CD)) as CD;
            dat.ID = -tree.ID;
            dat.Parent_FP = tree.FP.Parent;
            dat.Code = tree.FP.Code;
            dat.Name = tree.Name;
            Add(dat);
        }
        public void AddDat(BaseDat dat)
        {
            if (dat is CD)
                Add((CD)dat);
            else if (dat is ITreeDat)
                Add((ITreeDat)dat);
        }

        public void Remove(CD card)
        {
            if (card != null)
                cardSet.Remove(card);
        }

        public void Remove(CD[] cards)
        {
            foreach (CD card in cards)
            {
                if (card != null)
                    cardSet.Remove(card);
            }
        }
        public void RemoveSelected()
        {
            Remove(GetSelectedValues());
        }

        private bool findParent(CD card)
        {
            PathTree parent = card.Parent_FP;
            if (parent == null) return false;
            foreach (CD dat in cardSet)
            {
                if (dat.ID == card.ID) return true;
                if (dat.ID < 0)
                {
                    PathTree tree = new PathTree(dat.Parent_FP, dat.Code);
                    if (parent == tree || parent.IsChildOf(tree))
                        return true;
                }
            }
            return false;
        }

        protected string PathCard2Tree(CD card)
        {
            if (card == null)
                return "";
            PathTree tree = new PathTree(card.Parent_FP, card.Code);
            return tree.ToString();
        }

        public string[] GetCardsString()
        {
            List<string> cards = new List<string>();
            foreach (CD dat in cardSet)
            {
                if (dat.ID < 0)
                {
                    PathTree tree = new PathTree(dat.Parent_FP, dat.Code);
                    cards.Add(string.Format("{0}: {1}", tree, dat.Name));

                }
                else if (dat.ID > 0)
                {
                    cards.Add(dat.ToString());
                }
            }
            return cards.ToArray();
        }

        public static int CardsFPComparison(ICardDat card1, ICardDat card2)
        {
            if (card1 == null && card2 == null) return 0;
            if (card1 == null) return -1;
            if (card2 == null) return 1;

            if (card1.ID == 0 && card2.ID == 0) return 0;
            if (card1.ID == 0) return -1;
            if (card2.ID == 0) return 1;
            if (card1.ID < 0 && card2.ID > 0) return -1;
            if (card1.ID > 0 && card2.ID < 0) return 1;

            if (card1.ID > 0)
                return card1.FP.CompareTo(card2.FP);
            else
            {
                PathTree tree1 = new PathTree(card1.Parent_FP, card1.Code);
                PathTree tree2 = new PathTree(card2.Parent_FP, card2.Code);
                return tree1.CompareTo(tree2);
            }
        }
    }
}
