using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using BO;
using DA;
using System.IO;
using System.Drawing;
using BO.Xml;
using System.Diagnostics;

namespace BOF
{
    public class SysAuditController : ListFormController<SysAuditSet, SysAuditDat>
    {
        public SysAuditController(Form mdiParent)
            : base(mdiParent, "SysAuditController", "Аудит", BOF.Icons.Web_XML, true, true)
        {

            //contLstEnt.SplitterDistance = 430;
            splitTL.Panel1Collapsed = true;


            SysAuditController.SetList.SetReloaded += List_SetReloaded;
        }

        #region Properties
        private CtlCaptionPanel captEntity;
        private CtlPanel pdetails;
        private ListController iidc;

        #endregion

        #region Methods
        protected override void AddGridColumns()
        {
            gridMain.Grid.AutoGenerateColumns = false;

            gridMain.Grid.AddGridColumn(SysAuditColumns.ID, "#", typeof(int)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.Srv, "Сервер", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.Db, "База", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.Tbl, "Таблица", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.TblID, "ID", typeof(int)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.OperationType, "Операция", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.Usr, "Кто", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            gridMain.Grid.AddGridColumn(SysAuditColumns.Dt, "Когда", typeof(DateTime)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;


        }
        protected override void DoGroup(bool open)
        {
            base.DoGroup(open);
            if (open)
            {
                if (treeGroup == null)
                {
                    treeGroup = new TreeGroups();
                    treeGroup.ShowPlusMinus = true;
                    treeGroup.Dock = DockStyle.Fill;
                    treeGroup.SetClass = SetList;
                    treeGroup.GroupChanged += new EventHandler(treeGroup_GroupChanged);
                    PanelTree.Controls.Add(treeGroup);
                }
                treeGroup.Nodes.Clear();
                treeGroup.AddGroupDate<SysAuditDat, SysAuditSet>(SysAuditColumns.DtDate, "Дата");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.ClientApplication, "ClientApplication");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.ClientHost, "ClientHost");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Operation, "Operation");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Db, "Db");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Srv, "Srv");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Tbl, "Tbl");
                treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Usr, "Usr");

                
            }
        }

        protected override void InitEntity()
        {
            CtlPanel panEnt = new CtlPanel();
            panEnt.Active = false;
            panEnt.Dock = DockStyle.Fill;
            contLstEnt.Panel2.Controls.Add(panEnt);

            captEntity = new CtlCaptionPanel();
            captEntity.Active = false;
            captEntity.Caption = "Данные аудита";
            captEntity.Dock = DockStyle.Top;

            pdetails = new CtlPanel();
            pdetails.Active = false;
            pdetails.Dock = DockStyle.Fill;

            iidc = new ListController();
            iidc.Init(pdetails);

            iidc._ToolBar.Items.Clear();
            iidc._ToolBar.Visible = false;

            iidc.Grid.AddGridColumn(SysAuditDetailsColumns.FieldName, "Наименование", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            iidc.Grid.AddGridColumn(SysAuditDetailsColumns.OldValue, "Старое значение", typeof(string), 200);
            iidc.Grid.AddGridColumn(SysAuditDetailsColumns.NewValue, "Новое значение", typeof(string), 200);

            panEnt.Controls.AddRange(new Control[] { pdetails, captEntity });

        }
        protected override void AddGridEvents()
        {
            gridMain.EntityChanged += new DatEventDelegate(grid_ValueChanged);
        }
        protected override void AddToolBarButtons()
        {
            fltController.FilterForm = new SysAuditFilters();
        }        
        #endregion

        #region Events

        void List_SetReloaded(object sender, EventArgs e)
        {
            Refresh();
        }

        void grid_ValueChanged(object sender, DatEventArgs e)
        {
            if (e.DatEntity != null)
            {

                SysAuditDat dat = e.DatEntity as SysAuditDat;
                //dat.FillExtraMembers();
                dat.Details.Load();
                iidc.Grid.ValueEventDisabled = true;
                iidc.Grid.SetDataSource(dat.Details.DetSet);
                iidc.Grid.ValueEventDisabled = false;
                captEntity.Caption = string.Format("Таблица: {0}, ID={1}, Операция: {2}, Кто: {3}, Когда: {4:dd.MM.yyyy HH:mm:ss.ms}", dat.Tbl,dat.Info.id, dat.OperationType, dat.Usr, dat.Dt);
            }
            else
            {
                iidc.Grid.SetDataSource(null);
            }
        }

        void treeGroup_GroupChanged(object sender, EventArgs e)
        {
            if (treeGroup != null)
                treeGroup.FilterSetClass<SysAuditDat, SysAuditSet>();

        }

        #endregion

    }

    //public class SysAuditController_old : ListFormController
    //{
    //    public SysAuditController_old(Form mdiParent)
    //        : base(mdiParent, "SysAudit", "Аудит", BOF.Icons.Web_XML)
    //    {
    //        this.PaintPanels(true, true);

    //        splitTL.Panel1Collapsed = true;
            
    //       // this.ToolBar.Items[2].Visible = false;

    //        gridCont = new ListController("Аудит");
    //        gridCont._ToolBar.Visible = false;
    //        CtlGrid grid = gridCont.Grid;

    //        fltController = new FilterController();
    //        fltController.FilterForm = new SysAuditFilters();
    //        fltController.FilterChanged += new EventHandler<FilterEventsArgs2>(operFilter_FilterChanged);
    //        fltController.Init(ToolBar);

    //        grid.AutoGenerateColumns = false;


    //        grid.AddGridColumn(SysAuditColumns.Srv, "Сервер", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditColumns.Db, "База", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditColumns.Tbl, "Таблица", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditColumns.OperationType, "Операция", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditColumns.Usr, "Кто", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditColumns.Dt, "Когда", typeof(DateTime)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

    //        gridCont.EntityChanged += new DatEventDelegate(grid_ValueChanged);

    //        gridCont.Init(PanelList);

    //        //Refresh();
    //        InitEntity();

    //        grid.ValueEventDisabled = true;
    //        Show();
    //        grid.ValueEventDisabled = false;
    //        grid.FireValueChanged();
    //    }


    //    #region Properties
    //    //private TreeGroups treeGroup;
    //    //FilterController fltController;
    //    private BaseLoadFilter loadFilter;
    //    private ListController gridCont = null;
    //    private ListController gridDetails = null;
    //    static SysAuditSet _SysAudit = new SysAuditSet();
    //    public static SysAuditSet SysAudit
    //    {
    //        get { return _SysAudit; }
    //        set { _SysAudit = value; }
    //    }

    //    private SysAuditDat CurrentSysAudit
    //    {
    //        get
    //        {
    //            if (gridCont.Grid == null)
    //                return null;
    //            else
    //                return gridCont.Grid.Value as SysAuditDat;
    //        }
    //    }

    //    #endregion

    //    #region Methods
    //    protected override void DoGroup(bool open)
    //    {
    //        base.DoGroup(open);
    //        ToolBar.Items[3].Enabled = !open;
    //        ToolBar.Items[4].Enabled = !open;
    //        if (open)
    //        {
    //            if (treeGroup == null)
    //            {
    //                treeGroup = new TreeGroups();
    //                treeGroup.ShowPlusMinus = true;
    //                treeGroup.Dock = DockStyle.Fill;
    //                treeGroup.SetClass = SysAudit;
    //                treeGroup.GroupChanged += new EventHandler(treeGroup_GroupChanged);
    //                PanelTree.Controls.Add(treeGroup);
    //            }
    //            treeGroup.Nodes.Clear();
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.ClientApplication, "Приложение");
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.ClientHost, "Хост");
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Usr, "Пользователь");
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Srv, "Сервер");
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Db, "База");
    //            treeGroup.AddGroup<SysAuditDat, SysAuditSet>(SysAuditColumns.Tbl, "Таблица");
    //            treeGroup.AddGroupDate<SysAuditDat, SysAuditSet>(SysAuditColumns.DtDate, "Дата");
    //        }
    //        else
    //        {
    //            SysAudit.FilterReset();
    //        }
    //    }
    //    protected override void InitEntity()
    //    {
    //        CtlPanel panEnt = new CtlPanel();
    //        panEnt.Active = false;
    //        panEnt.Dock = DockStyle.Fill;
    //        contLstEnt.Panel2.Controls.Add(panEnt);

    //        gridDetails = new ListController("Детали");
    //        CtlGrid grid = gridDetails.Grid;
    //        gridDetails._ToolBar.Visible = false;

    //        gridDetails.EntitySelected += new DatEventDelegate(gridDetails_EntitySelected);


    //        grid.AddGridColumn(SysAuditDetailsColumns.FieldName, "Наименование", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditDetailsColumns.OldValue, "Старое значение", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
    //        grid.AddGridColumn(SysAuditDetailsColumns.NewValue, "Новое значение", typeof(string)).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

    //        gridDetails.Init(panEnt);
    //    }

    //    void gridDetails_EntitySelected(object sender, DatEventArgs e)
    //    {
    //        SysAuditDetailsDat sad = e.DatEntity as SysAuditDetailsDat;
    //        if (sad == null) return;
    //        SysAuditEntityForm frm = new SysAuditEntityForm();
    //        frm.OldValue = sad;
    //        //frm.NewValue = sad;
    //        frm.ShowDialog();
    //    }
    //    public override void Refresh()
    //    {
    //        Reload();
    //    }
    //    private void Reload()
    //    {
    //        try
    //        {
    //            SetStatusCount(0);
    //            Form.Cursor = Cursors.WaitCursor;
    //            SysAudit.FilterReset();
    //            SysAudit.LoadFilter = loadFilter.GetFilter();
    //            SysAudit.Load();
    //            gridCont.Grid.SetDataSource(SysAudit);
    //            SetStatusCount(SysAudit.Count);
    //            //GroupReload();
    //        }
    //        finally
    //        {
    //            Form.Cursor = Cursors.Default;
    //        }
    //    }


    //    #endregion

    //    #region Events
    //    void treeGroup_GroupChanged(object sender, EventArgs e)
    //    {
    //        treeGroup.FilterSetClass<SysAuditDat, SysAuditSet>();
    //    }

    //    void List_SetReloaded(object sender, EventArgs e)
    //    {
    //        Refresh();
    //    }


    //    void operFilter_FilterChanged(object sender, FilterEventsArgs2 e)
    //    {
    //        loadFilter = e.Filter;
    //        Reload();
    //    }

    //    void grid_ValueChanged(object sender, DatEventArgs e)
    //    {
    //        if (e.DatEntity != null)
    //        {
    //            CurrentSysAudit.FillExtraMembers();
    //            CurrentSysAudit.Details.Load();
    //            gridDetails.Grid.SetDataSource(CurrentSysAudit.Details.DetSet);
    //            SetStatusText(e.DatEntity.ToString());
    //        }
    //    }


    //    #endregion

    //}
}
