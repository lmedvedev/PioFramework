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
    public abstract class ListFormControllerBase<S, D> : ListFormController<S, D>
        where S : BaseSet<D, S>, new()
        where D : BaseDat<D>, new()
    {
        public ListFormControllerBase(Form mdiForm, string name, string caption, Icon icon)
            : this(mdiForm, name, caption, icon, false, false, null) { }
        public ListFormControllerBase(Form mdiForm, string name, string caption, Icon icon, bool hasTree, bool hasEntity, BaseLoadFilter filter) 
            : base(mdiForm, name, caption, icon)
        {
            PaintPanels(hasTree ? 70 : 0, hasEntity ? 70 : 0);
            gridMain.Init(PanelList, FForm);
            FToolBar.Visible = false;
            gridMain.Grid.ShowContextMenu = true;
            gridMain.Grid.TabStop = false;
            Show();
            Application.DoEvents();
            Refresh();
        }

        protected override void AddGridEvents()
        {
            gridMain.Grid.ValueChanged += delegate(object sender, ValueEventArgs e)
            {
                SetStatusText(e.Value.ToString());
            };
            gridMain.Grid.DataSourceChanged += delegate(object sender, EventArgs e)
            {
                SetStatusCount(gridMain.Grid.RowCount);
            };
        }

        protected override void AddToolBarButtons()
        {
            gridMain._ToolBar.Items.Clear();
            gridMain._ToolBar.GripStyle = ToolStripGripStyle.Hidden;
            gridMain._ToolBar.Items.Add("Создать", new Icon(Properties.Resources.create, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { gridMain.Grid.FireValueInserted(); });
            ToolStripItem btnEdit = gridMain._ToolBar.Items.Add("Исправить", new Icon(Properties.Resources.pencil, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { gridMain.Grid.FireValueSelected(); });
            ToolStripItem btnDel = gridMain._ToolBar.Items.Add("Удалить", new Icon(Properties.Resources.delete, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { gridMain.Grid.FireValueDeleted(); });
            btnEdit.Enabled = btnDel.Enabled = false;
            gridMain.Grid.ValueChanged += delegate(object sender, ValueEventArgs e) 
            { 
                btnEdit.Enabled = btnDel.Enabled = e.Value != null; 
            };
        }

        public override void Refresh()
        {
            FForm.Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                D dat = CurrentValue;
                SetList.Load();
                gridMain.Grid.SetDataSource(SetList);
                gridMain.Grid.Value = dat;
                gridMain.Grid.Focus();
            }
            finally
            {
                Application.DoEvents();
                FForm.Cursor = Cursors.Default;
            }
        }
    }
}
