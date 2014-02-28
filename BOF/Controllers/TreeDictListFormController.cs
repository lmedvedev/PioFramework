using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BO;
using System.Xml;
using DA;
//using System.Linq;
using System.Collections;

namespace BOF
{
    public abstract class TreeDictListFormController<S, D> : ListFormControllerBase<S, D>
        where S : BaseSet<D, S>, new()
        where D : BaseDat<D>, ITreeDictDat, new()
    {
        CtlTreeDict<S, D> TreeList = new CtlTreeDict<S, D>();
        public TreeDictListFormController(Form mdiForm, string name, string caption, Icon icon) 
            : base(mdiForm, name, caption, icon, true, false, null)
        {
        }

        public override void PaintPanels(bool hasTree, bool hasEntity)
        {
            base.PaintPanels(hasTree, hasEntity);
            splitTL.Panel1Collapsed = false;
            TreeList.Dock = DockStyle.Fill;
            TreeList.BorderStyle = BorderStyle.None;
            TreeList.ShowContextMenu = true;
            TreeList.ShowExpanded = true;
            TreeList.ValueChanged += new ValueEventHandler(TreeList_ValueChanged);
            PanelTree.Controls.Add(TreeList);
        }

        public override void Refresh()
        {
            FForm.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                SetList.Load();
                TreeList.DataSource = SetList;
            }
            finally
            {
                Application.DoEvents();
                FForm.Cursor = Cursors.Default;
            }
        }

        void TreeList_ValueChanged(object sender, ValueEventArgs e)
        {
            FForm.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                gridMain.Grid.SetDataSource(TreeList.GetChildren(true));
            }
            finally
            {
                Application.DoEvents();
                FForm.Cursor = Cursors.Default;
            }
        }
    }
}
