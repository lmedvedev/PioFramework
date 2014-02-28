using System;
using System.Collections.Generic;
using System.Text;
using BO;
using System.Windows.Forms;
using DA;

namespace BOF
{
    public class CardGridChooserController<TS, TD, CS, CD> : CardGridController<CS, CD>
        where TS : BaseSet<TD, TS>, new()
        where CS : BaseSet<CD, CS>, new()
        where TD : BaseDat<TD>, ITreeDat, IDat, new()
        where CD : BaseDat<CD>, ICardDat, IDat, new()
    {
        #region Constructors
        public CardGridChooserController(DataGridView grd)
            : this(new TS(), null, grd)
        {
            treeSet.Load();
        }
        public CardGridChooserController(TS tree, TD root, DataGridView grd)
            : base(grd)
        {
            treeSet = tree;
            treeRoot = (root != null) ? root.FP : null;
            currentParentFP = treeRoot;
            currentCardFP = null;
        }


        #endregion

        #region Fields
        private PathTree currentParentFP;
        private PathTree currentCardFP;
        private TS treeSet;

        public TS TreeSet
        {
            get { return treeSet; }
            set { treeSet = value; }
        }
        private PathTree treeRoot = null;
        #endregion

        #region Events
        public event EventHandler<EventArgs<TD>> ListReloaded;

        protected override void OnValueSelected(DatEventArgs e)
        {
            Select(e.DatEntity as CD);
        }
        protected override void OnValueChanged(DatEventArgs e)
        {
            base.OnValueChanged(e);
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    if (e.Control)
                        FireDatValueSelected(GetValue());
                    else
                        Select(GetCard());
                    break;
                case Keys.Back:
                    if (currentParentFP != null)
                    {
                        currentCardFP = currentParentFP;
                        currentParentFP = currentCardFP.Parent;
                    }
                    ReloadList();
                    break;
            }
        }
        #endregion

        #region Public Methods

        public BaseDat GetValue()
        {
            CD card = GetCard() as CD;
            if (card == null) return null;

            if (card.ID == 0)
            {
                foreach (TD tree in treeSet)
                {
                    if (tree.FP == card.Parent_FP)
                        return tree;
                }
            }
            else if (card.ID > 0)
                return card;
            else if (card.ID < 0)
            {
                int id = -card.ID;
                foreach (TD tree in treeSet)
                {
                    if (tree.ID == id)
                        return tree;
                }
            }
            return null;
        }

        public void Reload()
        {
            ReloadList();
        }
        public void Reload(PathTree root)
        {
            treeRoot = root;
            currentParentFP = treeRoot;
            ReloadList();
        }
        public void ReloadSearch(string search)
        {
            currentParentFP = treeRoot;
            treeSet.FilterApply(delegate(TD dat)
                {
                    return (BaseDat.ToString(dat).Contains(search));
                });

            cardSet.LoadFilter.Reset();
            if (currentParentFP != null)
                cardSet.LoadFilter.AddWhere(new FilterTree(currentParentFP, true));
            List<FilterUnitBase> lf = new List<FilterUnitBase>();
            if (PathCard.IsPathCard(search))
                lf.Add(new FilterCard(new PathCard(search)));
            else if (PathTree.IsPathTree(search))
                lf.Add(new FilterTree(search, true));
            lf.Add(new FilterString("Name", '%' + search + '%', true));
            cardSet.LoadFilter.AddWhere(new FilterOR(lf.ToArray()));
            cardSet.Load();

            AddTreeSet(treeSet, cardSet);
            treeSet.FilterReset();
            Refresh();
        }
        #endregion

        #region Private Methods
        private TD getParent()
        {
            if (currentParentFP != null)
            {
                foreach (TD tree in treeSet)
                {
                    if (tree.FP == currentParentFP)
                        return tree;
                }
            }
            return null;
        }
        private void ReloadList()
        {
            if (treeRoot != null && (currentParentFP == null || !currentParentFP.IsChildOf(treeRoot)))
                currentParentFP = treeRoot;
            treeSet.FilterApply(delegate(TD dat)
                {
                    return (dat.FP.Parent == currentParentFP);
                }
            );

            cardSet.LoadFilter.Reset();
            cardSet.LoadFilter.AddWhere(new FilterTree(currentParentFP, false));
            cardSet.Load();
            grid.DataSource = null;
            AddTreeSet(treeSet, cardSet);
            grid.DataSource = cardSet;
            treeSet.FilterReset();
            Refresh();
        }

        protected override Comparison<CD> CardsComparison
        {
            get { return BaseSet.CardsCodeComparison; }
        }
        protected override DataGridViewColumn CodeGridColumn
        {
            get
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = "Code";
                col.DataPropertyName = "Code";
                col.HeaderText = "Код";
                col.ValueType = typeof(int);
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "#,##0";
                col.Width = 40;
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                return col;
            }
        }
        protected override void Refresh()
        {
            cardSet.Sort(CardsComparison);
            if (currentCardFP != null)
            {
                for (int i = 0; i < cardSet.Count; i++)
                {
                    CD card = cardSet[i] as CD;
                    if (card != null && card.Code == currentCardFP.Code)
                    {
                        BindingManagerBase cm = grid.BindingContext[cardSet];
                        if (cm != null)
                            cm.Position = i;
                        break;
                    }
                }
            }
            if (ListReloaded != null)
                ListReloaded(this, new EventArgs<TD>(getParent()));
        }

        private void AddTreeSet(TS treeSet, CS cardSet)
        {
            AddParent(currentParentFP);

            foreach (TD tree in treeSet)
            {
                Add(tree);
            }
        }

        private void Select(CD dat)
        {
            try
            {
                if (dat == null) return;
                if (dat.ID == 0)
                {
                    currentCardFP = dat.Parent_FP;
                    currentParentFP = currentCardFP.Parent;
                    ReloadList();
                }
                else if (dat.ID < 0)
                {
                    currentCardFP = null;
                    currentParentFP = new PathTree(PathCard2Tree(dat));
                    ReloadList();
                }
                else if (dat.ID > 0)
                {
                    FireDatValueSelected(dat);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        public override void Add(CD card)
        {
            if (card != null)
                cardSet.Add(card);
        }
        public void AddParent(PathTree parent)
        {
            if (parent != null)
            {
                CD datNull = Activator.CreateInstance(typeof(CD)) as CD;
                datNull.ID = 0;
                datNull.Parent_FP = parent;
                datNull.Code = 0;
                datNull.Name = "";
                cardSet.Add(datNull);
            }
        }

    }
}
