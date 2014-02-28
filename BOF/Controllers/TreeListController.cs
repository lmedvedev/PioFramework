using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Collections;
using DA;

namespace BOF
{
    public class TreeListController<TS, TD, CS, CD> : ListController
        where TS : BaseSet<TD, TS>, new()
        where CS : BaseSet<CD, CS>, new()
        where TD : BaseDat<TD>, ITreeDat, new()
        where CD : BaseDat<CD>, ICardDat, new()
    {
        public TreeListController():base()
        {
        }

        private bool _IsChooserMode = false;

        public bool IsChooserMode
        {
            get { return _IsChooserMode; }
            set { _IsChooserMode = value; }
        }

        public PathTree pt = null;


        public void Refresh()
        {
            ReloadList();
        }
        protected override void ReloadList()
        {
            TS treeSet = Activator.CreateInstance(typeof(TS)) as TS;
            treeSet.Load();
            treeSet.FilterApply(delegate(TD dat)
                {
                    return (dat.FP.Parent == pt);
                }
            );

            CS cardSet = Activator.CreateInstance(typeof(CS)) as CS;
            cardSet.Load();

            AddTreeSet(treeSet, cardSet);
            cardSet.Sort(BaseSet.CardsCodeComparison);


            //AddGridStyles();

            cardSet.FilterApply(delegate(CD dat)
               {
                   return (dat.Parent_FP == pt);
               }
           );

            Grid.SetDataSource(cardSet);

        }
        private void AddTreeSet(TS treeSet, CS cardSet)
        {
            if (pt != null && IsChooserMode)
            {
                CD datNull = Activator.CreateInstance(typeof(CD)) as CD;
                datNull.ID = 0;
                datNull.Parent_FP = pt;
                datNull.Code = 0;
                datNull.Name = "...";
                cardSet.Add(datNull);
            }
            
            foreach (TD tree in treeSet)
            {
                CD dat = Activator.CreateInstance(typeof(CD)) as CD;
                dat.ID = -tree.ID;
                dat.Parent_FP = tree.FP.Parent;
                dat.Code = tree.FP.Code;
                dat.Name = tree.Name;
                cardSet.Add(dat);
            }
        }

        protected override void SetGridStyles()
        {
            base.SetGridStyles();
            Grid.CellFormatting += new DataGridViewCellFormattingEventHandler(grid_CellFormatting);

            DataGridViewColumn col = new DataGridViewImageColumn(true);
            col.HeaderText = "";
            col.Name = "Icon";
            col.DataPropertyName = "ID";
            col.Width = 20;

            Grid.AddGridColumn(col);


            //Grid.AddGridColumn("Icon", "", null, "ID");
            Grid.AddGridColumn("Parent_FP", "Parent", typeof(string));
            Grid.AddGridColumn("Code", "Code", typeof(int));
            //Grid.AddGridColumn("Name", "Name", typeof(string));

            col = new DataGridViewTextBoxColumn();
            col.Name = "Name";
            col.DataPropertyName = "Name";
            col.ValueType = typeof(string);
            setFmt(col);
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Grid.AddGridColumn(col);
        }
        void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (Grid.Columns[e.ColumnIndex].Name == "Icon")
            {
                if ((e.Value is int) && (BaseDat.O2Int32(e.Value) < 0))
                    e.Value = global::BOF.Properties.Resources.Folder;
                else if ((e.Value is int) && (BaseDat.O2Int32(e.Value) == 0))
                    e.Value = global::BOF.Properties.Resources.Folder;
                else
                    e.Value = global::BOF.Properties.Resources.document;
            }
        }
        protected static void setFmt(DataGridViewColumn col)
        {
            Type t = col.ValueType;
            if (t == null) return;

            if (t.Equals(typeof(int)))
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "#,##0";
                col.Width = 55;
            }
            else if (t.Equals(typeof(decimal)))
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Format = "#,##0.00;-#,##0.00;-";
                col.Width = 90;
            }
            else if (t.Equals(typeof(DateTime)))
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.Width = 80;
            }
        }

        protected override void Edit(BaseDat dat)
        {
            try
            {
                if (dat == null) return;
                if (((IDat)dat).ID == 0)
                {
                    string path = "";
                    path = ((ICardDat)dat).Parent_FP.Parent == null ? "" : ((ICardDat)dat).Parent_FP.Parent.ToString();
                    if (path != "")
                        pt = new PathTree(path);
                    else
                        pt = null;
                    ReloadList();
                }
                if (((IDat)dat).ID < 0 && IsChooserMode)
                {
                    string path = "";
                    path = ((ICardDat)dat).Parent_FP == null ? "" : ((ICardDat)dat).Parent_FP.ToString() + ".";
                    path += ((ICardDat)dat).Code;
                    pt = new PathTree(path);
                    ReloadList();
                }
                else
                    base.Edit(dat);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Îøèáêà", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}