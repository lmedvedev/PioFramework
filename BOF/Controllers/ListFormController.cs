using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BO;
using System.Xml;
using DA;
using System.Collections;
using System.Xml.Xsl;

namespace BOF
{
    public class ListFormController : FormController
    {
        public ListFormController(Form mdiParent, BaseLoadFilter loadfilter) : this(mdiParent, "", "", null) { }
        public ListFormController(Form mdiForm, string name, string caption, Icon icon)
            : base(mdiForm, name, caption, icon)
        {
            //Form = new ListForm();
            //Form.Name = name;
            //Form.Text = caption;
            //Form.Icon = icon;
            //Form.WindowState = FormWindowState.Maximized;
            _PanelList = FForm.ContentPanel;

            Caption = caption;
            gridMain = new ListController(caption);
            gridMain.EntityChanged += new DatEventDelegate(grid_ValueChanged);

            AddGridColumns();
            AddGridEvents();
            //AddToolBarButtons();


            ToolStripButton btnClose = new ToolStripButton("Закрыть", global::BOF.Properties.Resources.CloseForm.ToBitmap());
            FToolBar.Items.Add(btnClose);
            btnClose.Click += new EventHandler(btnClose_Click);

            ToolStripButton btnRefresh = new ToolStripButton("Обновить", global::BOF.Properties.Resources.Refresh.ToBitmap());
            FToolBar.Items.Add(btnRefresh);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);

            AddToolBarButtons();

            sbCount = new ToolStripStatusLabel();
            sbCount.BorderStyle = Border3DStyle.Flat;
            sbCount.BorderSides = ToolStripStatusLabelBorderSides.Left;
            sbCount.DisplayStyle = ToolStripItemDisplayStyle.Text;
            sbCount.Name = "sbCount";
            sbCount.ToolTipText = "Количество строк в списке";
            sbCount.AutoSize = true;
            sbCount.Height = 20;
            sbCount.Text = "0";

            sbID = new ToolStripStatusLabel();
            sbID.BorderStyle = Border3DStyle.Flat;
            sbID.BorderSides = ToolStripStatusLabelBorderSides.Left;
            sbID.DisplayStyle = ToolStripItemDisplayStyle.Text;
            sbID.ToolTipText = "Идентификатор выбранного элемента";
            sbID.Name = "sbID";
            sbID.AutoSize = true;
            sbID.Height = 20;
            sbID.Visible = false;

            sbText = new ToolStripStatusLabel();
            sbText.BorderStyle = Border3DStyle.Flat;
            sbText.BorderSides = ToolStripStatusLabelBorderSides.Left;
            sbText.DisplayStyle = ToolStripItemDisplayStyle.Text;
            sbText.Spring = true;
            sbText.Name = "sbText";
            sbText.AutoSize = false;
            sbText.Height = 20;
            sbText.TextAlign = ContentAlignment.MiddleLeft;
            sbText.Visible = false;

            FStatusBar.Items.AddRange(new ToolStripItem[] { sbCount, sbID, sbText });
            FForm.MdiParent = mdiForm;
            FForm.FormController = this;
        }

        private Panel _PanelTree = null;
        public Panel PanelTree
        {
            get { return _PanelTree; }
        }
        private Panel _PanelList = null;
        public Panel PanelList
        {
            get { return _PanelList; }
        }
        private Panel _PanelEntity = null;
        public Panel PanelEntity
        {
            get { return _PanelEntity; }
        }
        private string _Caption;
        public string Caption
        {
            get { return _Caption; }
            set { _Caption = value; }
        }

        private bool _AllowEmptyLoadFilter = false;
        public bool AllowEmptyLoadFilter
        {
            get { return _AllowEmptyLoadFilter; }
            set { _AllowEmptyLoadFilter = value; }
        }

        private BaseLoadFilter _LoadFilter;
        public BaseLoadFilter LoadFilter
        {
            get { return _LoadFilter; }
            set { _LoadFilter = value; }
        }

        protected ListController gridMain;
        protected TreeGroups treeGroup;
        public StatusStrip FStatusBar
        {
            get { return FForm.StatusBar; }
        }

        public int EntityHeight
        {
            get
            {
                if (contLstEnt == null)
                    return 0;
                else
                    return contLstEnt.Height - contLstEnt.SplitterDistance;
            }
            set
            {
                if (contLstEnt != null)
                    contLstEnt.SplitterDistance = contLstEnt.Height - value;
            }
        }
        //public ListForm Form;
        protected SplitContainer contLstEnt = null;
        protected SplitContainer splitTL = null;
        private ToolStripStatusLabel sbCount;
        private ToolStripStatusLabel sbID;
        private ToolStripStatusLabel sbText;

        public virtual void PaintPanels(int tree_width, int entity_height)
        {
            PaintPanels(tree_width > 0, entity_height > 0);
            if (tree_width > 0)
                splitTL.SplitterDistance = tree_width;
            if (entity_height > 0)
                contLstEnt.SplitterDistance = contLstEnt.Height - entity_height;
        }
        public override void MergeMenu()
        {
            ToolStripManager.Merge(FToolBar, gridMain._ToolBar);
            FToolBar.Visible = false;
        }
        public virtual void PaintPanels(bool hasTree, bool hasEntity)
        {
            FForm.SuspendLayout();
            FForm.ContentPanel.Controls.Clear();
            Panel panel = FForm.ContentPanel;
            if (hasEntity)
            {
                contLstEnt = new SplitContainer();
                contLstEnt.BorderStyle = BorderStyle.None;
                contLstEnt.SplitterDistance = 60;

                //contLstEnt.BackColor = Color.WhiteSmoke;
                contLstEnt.Panel1.BackColor = Color.WhiteSmoke;
                contLstEnt.Panel2.BackColor = Color.WhiteSmoke;
                panel.Controls.Add(contLstEnt);
                contLstEnt.Dock = DockStyle.Fill;
                contLstEnt.Name = "contLstEnt";
                contLstEnt.Orientation = Orientation.Horizontal;
                panel = contLstEnt.Panel1;
                _PanelEntity = contLstEnt.Panel2;
            }
            if (hasTree)
            {
                splitTL = new SplitContainer();
                splitTL.BorderStyle = BorderStyle.None;
                splitTL.Panel1.BackColor = Color.WhiteSmoke;
                splitTL.Panel2.BackColor = Color.WhiteSmoke;
                //splitTL.BackColor = Color.White;
                panel.Controls.Add(splitTL);
                splitTL.Orientation = Orientation.Vertical;
                splitTL.Dock = DockStyle.Fill;
                splitTL.Panel1Collapsed = true;
                panel = splitTL.Panel2;
                _PanelTree = splitTL.Panel1;

                ToolStripButton ButtonGrouping = new ToolStripButton("Группировка", BOF.Icons.GroupPanel);
                ButtonGrouping.Click += new EventHandler(ButtonGrouping_Click);
                FToolBar.Items.Add(ButtonGrouping);
            }

            FToolBar.GripStyle = ToolStripGripStyle.Hidden;

            _PanelList = panel;
            FForm.ResumeLayout();
        }
        protected virtual void AddGridColumns() { }
        protected virtual void AddGridEvents() 
        {
            gridMain.Grid.DataSourceChanged += delegate(object sender, EventArgs e)
            {
                SetStatusCount();
            };
        }
        protected virtual void AddToolBarButtons() { }
        protected virtual void InitEntity() { }
        protected virtual void SortList() { }
        protected virtual void DoGroup(bool open) { }
        protected virtual Icon GetStatusIcon(DocStatus status)
        {
            switch (status)
            {
                case DocStatus.PARTIAL:
                    return Icons.iconCYellow;
                case DocStatus.ALL:
                    return Icons.iconCGreen;
                case DocStatus.NONE:
                    return Icons.iconCWhite;
                case DocStatus.UNKNOWN:
                default:
                    return Icons.iconCRed;
            }
        }
        //public virtual void Show()
        //{
        //    if (FForm != null)
        //    {
        //        FForm.WindowState = FormWindowState.Normal;
        //        FForm.Show();
        //        FForm.WindowState = FormWindowState.Maximized;
        //    }
        //}
        



        #region datProgressBar Form
        private static frmProgress _frmProgress = null;
        protected void InitProgressForm(string text, bool allowcancel)
        {
            _frmProgress = new frmProgress(text, allowcancel);
            datProgressBar.ProgressEvent += new ProgressEventHandler(hndlProgressEvent);
            _frmProgress.Show();
            Application.DoEvents();
        }

        protected void CloseProgressForm()
        {
            if (_frmProgress != null) _frmProgress.Close();
            _frmProgress = null;
            //this.Activate();
            datProgressBar.ProgressEvent -= new ProgressEventHandler(hndlProgressEvent);
        }

        private void hndlProgressEvent(ProgressEventArgs e)
        {
            if (_frmProgress == null) return;
            if (e.Position < 0)
                _frmProgress.Close();
            else
            {
                datProgressBar level = datProgressBar.Current;
                if (level == null) throw new Exception("При прорисовке datProgressBar произошла ошибка - не задан datProgressBar.Current");
                _frmProgress.Position = level.StartPos + e.Position * (level.EndPos - level.StartPos) / 100;
                _frmProgress.Caption = e.Caption;
                Application.DoEvents();
            }
        }
        #endregion

        //frmProgress _frmProgress;
        public void ProgressInit(string caption, int count)
        {
            _frmProgress = new frmProgress(caption, true);
            _frmProgress.progressBar.Maximum = count;
            _frmProgress.progressBar.Minimum = 0;
            _frmProgress.progressBar.Step = 1;
            _frmProgress.Show(FForm.MdiParent);
            _frmProgress.ProgressCanceled += new EventHandler(_frmProgress_ProgressCanceled);
            Application.DoEvents();
        }

        void _frmProgress_ProgressCanceled(object sender, EventArgs e)
        {
            ProgressClose();
            throw new Exception("Операция прервана пользователем.");
        }

        public void ProgressMove(string text)
        {
            if (_frmProgress == null)
                return;
            if (_frmProgress.progressBar.Value <= _frmProgress.progressBar.Maximum)
            {
                _frmProgress.Caption = text;
                _frmProgress.progressBar.PerformStep();
            }
            Application.DoEvents();
        }

        public void ProgressClose()
        {
            if (_frmProgress != null)
                _frmProgress.Close();
            _frmProgress = null;
        }

        public void SetStatusCount()
        {
            IList ds = gridMain.Grid.DataSource as IList;
            int all = ds == null ? 0 : ds.Count;
            int selected = gridMain.Grid.RowCount;
            if (all != selected)
            {
                sbCount.Text = all.ToString() + " / " + selected.ToString();
                sbCount.ToolTipText = "Количество строк в списке / Всего загружено";
            }
            else
            {
                sbCount.Text = all.ToString();
                sbCount.ToolTipText = "Количество строк в списке";
            }
        }
        public void SetStatusCount(int count_all)
        {
            sbCount.Text = count_all.ToString("0");
            sbCount.ToolTipText = "Количество загруженных строк";
        }
        public void SetStatusCount(int count_all, int count_visible)
        {
            sbCount.Text = count_visible.ToString("") + ((count_visible == count_all) ? "" : "/" + count_all.ToString(""));
            sbCount.ToolTipText = "Показано/Загружено";
        }
        public void SetStatusID(int id)
        {
            sbID.Text = id == 0 ? "" : "#" + id.ToString("");
            sbID.Visible = !Common.IsNullOrEmpty(sbID.Text);
        }
        public void SetStatusText()
        {
            sbText.Text = gridMain.Grid.Value == null ? "" : gridMain.Grid.Value.ToString();
        }
        public void SetStatusText(string text)
        {
            sbText.Text = text;
            sbText.ToolTipText = Common.IsNullOrEmpty(sbText.Text) ? "" : "Описание выбранного элемента";
            sbText.Visible = !Common.IsNullOrEmpty(sbText.Text);
        }

        delegate void RefreshDelegate();
        void btnClose_Click(object sender, EventArgs e)
        {
            FForm.Close();
        }
        void btnRefresh_Click(object sender, EventArgs e)
        {
            Refresh();
        }
        void ButtonGrouping_Click(object sender, EventArgs e)
        {
            FToolBar.Items["FilterShow"].Enabled = !splitTL.Panel1Collapsed;
            FToolBar.Items["FilterReset"].Enabled = !splitTL.Panel1Collapsed;
            splitTL.Panel1Collapsed = !splitTL.Panel1Collapsed;
            ((ToolStripButton)sender).Checked = !splitTL.Panel1Collapsed;
            DoGroup(!splitTL.Panel1Collapsed);
        }
        void grid_ValueChanged(object sender, DatEventArgs e)
        {
            if (e.DatEntity != null)
            {
                SetStatusID(((IDat)e.DatEntity).ID);
                //SetStatusText(e.DatEntity.ToString());
            }
        }
    }

    public abstract class ListFormController<BS, BD> : ListFormController
        where BS : BaseSet<BD, BS>, new()
        where BD : BaseDat<BD>, new()
    {
        public ListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel)
            : this(mdiForm, name, caption, icon, TreePanel, EntityPanel, null) { }

        public ListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel, BaseLoadFilter loadfilter)
            : base(mdiForm, name, caption, icon)
        {
            fltController = new FilterController();
            PaintPanels(TreePanel, EntityPanel);
            fltController.Init(FToolBar);
            fltController.FilterChanged += new EventHandler<FilterEventsArgs2>(operFilter_FilterChanged);
            InitFilter(loadfilter);

            AddListPrintingButtons();

            gridMain.Init(PanelList);

            if (EntityPanel)
                InitEntity();

            gridMain.Grid.ValueEventDisabled = true;
            Show();
            // Refresh(); Сделал Кишик, чтобы работало задание фильтра при открытии формы(Refresh() перенесен внутрь Show())
            gridMain.Grid.ValueEventDisabled = false;
            gridMain.Grid.FireValueChanged();
        }
        public ListFormController(Form mdiForm, string name, string caption, Icon icon)
            : base(mdiForm, name, caption, icon)
        {
        }
        protected FilterController fltController = new FilterController();

        public event DatEventDelegate EntityListPrinted;
        public event DatEventDelegate EntityListPreviewed;

        protected BD CurrentValue
        {
            get
            {
                if (gridMain.Grid == null)
                    return null;
                else
                    return gridMain.Grid.Value as BD;
            }
        }
        private static BS _SetList = new BS();
        public static BS SetList
        {
            get
            {
                return _SetList;
            }
            set
            {
                _SetList = value;
            }
        }
        private bool IsBaseSort = true;

        public override void Refresh()
        {
            try
            {
                gridMain.Grid.ValueEventDisabled = true;
                BD dat = CurrentValue;
                SetStatusCount(0);
                FForm.Cursor = Cursors.WaitCursor;

                Application.DoEvents();

                SetList.FilterReset();
                if (LoadFilter != null)
                {
                    SetList.LoadFilter = LoadFilter.GetFilter();
                    string filter = LoadFilter.ToString();
                    //SetList.LoadFilter = fltController.FilterForm.GetFilter().GetFilter();
                    //string filter = fltController.FilterForm.GetFilter().ToString();

                    gridMain.captList.Caption = Caption + " [" + ((filter == "") ? "все" : filter) + "]";
                }
                try
                {
                    WaitForm.Start(gridMain.Grid);
                    SetList.Load();
                }
                finally
                {
                    WaitForm.Stop();
                }

                //this.Form.Activate();
                Application.DoEvents();

                gridMain.Grid.SetDataSource(SetList);
                SetStatusCount(SetList.CountAll, SetList.Count);
                gridMain.Grid.Value = dat;
                gridMain.Grid.ValueEventDisabled = false;
                if (IsBaseSort)
                {
                    SortList();
                    IsBaseSort = false;
                }
                FForm.Cursor = Cursors.Default;
                gridMain.Grid.FireValueChanged();
                DoGroup(false);
            }
            finally
            {
                FForm.Cursor = Cursors.Default;
                gridMain.Grid.ValueEventDisabled = false;
            }
        }
        protected override void DoGroup(bool open)
        {
            if (treeGroup == null && PanelTree != null)
            {
                treeGroup = new TreeGroups();
                treeGroup.ShowPlusMinus = true;
                treeGroup.Dock = DockStyle.Fill;
                treeGroup.SetClass = SetList;
                treeGroup.GroupChanged += new EventHandler(treeGroup_GroupChanged);
                PanelTree.Controls.Add(treeGroup);
            }
        }

        ToolStripDropDownButton btnExport = new ToolStripDropDownButton("Экспорт", global::BOF.Properties.Resources.export, null, "Export");
        public void AddExportButton(string text, Image image, EventHandler OnClick)
        {
            btnExport.DropDownItems.Add(text, image, OnClick);
        }

        protected virtual void AddListPrintingButtons()
        {
            AddExportButton("Список в Excel", new Icon(global::BOF.Properties.Resources.excel, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { Export("Excel_List"); });
            AddExportButton("Список в HTML", new Icon(global::BOF.Properties.Resources.Report, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { Export("HTML_List"); });
            AddExportButton("Список в XML", new Icon(global::BOF.Properties.Resources.Web_XML, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { Export("XML_List"); });
            AddExportButton("Строку в XML", new Icon(global::BOF.Properties.Resources.Web_XML, 16, 16).ToBitmap(), delegate(object sender, EventArgs e) { Export("XML_Entity"); });
            FToolBar.Items.Add(btnExport);

            //ToolStripDropDownButton Excel = new ToolStripDropDownButton("Excel", global::BOF.Properties.Resources.excel.ToBitmap(), null, "Excel");
            //Excel.DropDownItems.Add("Просмотр", global::BOF.Properties.Resources.preview, new EventHandler(ButtonListPreview_Click));
            //Excel.DropDownItems.Add("Печать", Icons.printer, new EventHandler(ButtonListPrint_Click));
            //ToolBar.Items.Add(Excel);

            
            //ToolStripButton ButtonListPreview = new ToolStripButton("Просмотр", global::BOF.Properties.Resources.pio_docspreview.ToBitmap(), null, "PreviewList");
            //ToolBar.Items.Add(ButtonListPreview);
            //ButtonListPreview.Click += new EventHandler(ButtonListPreview_Click);
            //ToolStripButton ButtonListPrint = new ToolStripButton("Печать", global::BOF.Properties.Resources.pio_docsprint.ToBitmap(), null, "PrintList");
            //ToolBar.Items.Add(ButtonListPrint);
            //ButtonListPrint.Click += new EventHandler(ButtonListPrint_Click);
            //EntityListPrinted += new DatEventDelegate(grid_EntityListPrinted);
            //EntityListPreviewed += new DatEventDelegate(grid_EntityListPreviewed);
         
            //временная кнопка для просмотра сериализованного дат-класса
            //ToolStripButton XMLView = new ToolStripButton("Просмотр XML", global::BOF.Properties.Resources.Web_XML.ToBitmap(), null, "XMLView");
            //ToolBar.Items.Add(XMLView);
            //XMLView.Click += new EventHandler(delegate(object sender, EventArgs e) { ViewXML(CurrentValue.Serialize()); });

            //this.gridMain.EntityXML += new DatEventDelegate(delegate(object sender, DatEventArgs e) { ViewXML(e.DatEntity.Serialize()); });
        }

        public virtual void Export(string type)
        {
            try
            {
                FForm.Cursor = Cursors.WaitCursor;
                switch (type)
                {
                    case "Excel_List":
                        {
                            ExcelXml doc = PrintList();
                            FForm.Cursor = Cursors.Default;
                            doc.Show();
                        }
                        break;
                    case "HTML_List":
                        {
                            ExcelXml doc = PrintList();
                            XslCompiledTransform xsl = new XslCompiledTransform(true);
                            xsl.Load(@"c:\xml2html.xslt");

                            string html = Common.TranslateXSL(doc, xsl);
                            WebBrowserForm frm = new WebBrowserForm();
                            frm.MdiParent = this.FForm.MdiParent;
                            frm.WindowState = FormWindowState.Maximized;
                            //frm.AddPage(new WebBrowserPage(doc, xsl));
                            frm.AddPage(html);
                            FForm.Cursor = Cursors.Default;
                            frm.Show();
                            
                        }
                        break;
                    case "XML_List":
                    case "XML_Entity":
                        {
                            XmlDocument xml = null;
                            if (type == "XML_List")
                            {
                                xml = new XmlDocument();
                                BS lst = gridMain.Grid.GetSelectedSet<BD, BS>();
                                if (lst.Count <= 1)
                                    lst = SetList;
                                XmlElement node = xml.CreateElement(lst.ToString().Replace(" ", ""));
                                xml.AppendChild(node);
                                foreach (BaseDat dat in lst)
                                {
                                    XmlDocument dat_xml = dat.Serialize();
                                    node = xml.CreateElement(dat_xml.DocumentElement.Name);
                                    node.InnerXml = dat_xml.DocumentElement.InnerXml;
                                    xml.DocumentElement.AppendChild(node);
                                }
                            }
                            else if (CurrentValue != null)
                                xml = CurrentValue.Serialize();
                            WebBrowserForm frm = new WebBrowserForm();
                            frm.MdiParent = this.FForm.MdiParent;
                            frm.WindowState = FormWindowState.Maximized;
                            frm.AddPage(xml);
                            FForm.Cursor = Cursors.Default;
                            frm.Show();
                        }
                        break;
                }
            }
            catch (System.Exception Ex)
            {
                FForm.Cursor = Cursors.Default;
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void treeGroup_GroupChanged(object sender, EventArgs e)
        {
            try
            {
                gridMain.Grid.ValueEventDisabled = true;
                FForm.Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                if (treeGroup != null)
                    treeGroup.FilterSetClass<BD, BS>();
                SetStatusCount(SetList.CountAll, SetList.Count);
            }
            catch (System.Exception Ex)
            {
                string s = Ex.Message;
            }
            finally
            {
                gridMain.Grid.ValueEventDisabled = false;
                FForm.Cursor = Cursors.Default;
            }
        }
        void operFilter_FilterChanged(object sender, FilterEventsArgs2 e)
        {
            if (fltController.FilterForm != null)
            {
                LoadFilter = fltController.FilterForm.GetFilter();
                Refresh();
            }
        }
        void ButtonListPrint_Click(object sender, EventArgs e)
        {
            if (EntityListPrinted != null && gridMain.Grid.Value != null)
                EntityListPrinted(this, new DatEventArgs(gridMain.Grid.Value as BaseDat));
        }
        void ButtonListPreview_Click(object sender, EventArgs e)
        {
            if (EntityListPreviewed != null && gridMain.Grid.Value != null)
                EntityListPreviewed(this, new DatEventArgs(gridMain.Grid.Value as BaseDat));
        }
        //void grid_EntityListPreviewed(object sender, DatEventArgs e)
        //{
        //    try
        //    {
        //        ExcelXml doc = PrintList();
        //        doc.Show();
        //    }
        //    catch (System.Exception Ex)
        //    {
        //        MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //void grid_EntityListPrinted(object sender, DatEventArgs e)
        //{
        //    try
        //    {
        //        ExcelXml doc = PrintList();
        //        doc.Print();
        //    }
        //    catch (System.Exception Ex)
        //    {
        //        MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        protected virtual List<PrintColumn> InitPrintColumn()
        {
            List<PrintColumn> ret = new List<PrintColumn>();
            foreach (DataGridViewColumn col in gridMain.Grid.Columns)
            {
                ret.Add(new PrintColumn(col.Name, col.HeaderText, col.ValueType, col.Width));
            } 
            return ret;
        }
        protected virtual ExcelXml PrintList()
        {
            ExcelXml excel = new ExcelXml();
            excel.AddDefaultStyles();
            excel.AddDefaultWorksheet("Report", "", true, "Список");

            Dictionary<string, PrintColumn> dList = new Dictionary<string, PrintColumn>();
            List<PrintColumn> cols = InitPrintColumn();

            foreach (PrintColumn col in cols)
            {
                excel.AddColumn(col.Width, true, col.GetStyleID());
            }

            excel.AddRow("H3");
            excel.AddRepeatedRows(1, 1);

            foreach (PrintColumn col in cols)
            {
                excel.AddCell(col.ColumnCaption);
            }

            BS lst = gridMain.Grid.GetSelectedSet<BD, BS>();
            if (lst.Count <= 1)
                lst = SetList;

            foreach (BD dat in lst)
            {
                excel.AddRow();
                foreach (PrintColumn col in cols)
                {
                    string text = "";
                    try
                    {
                        object val = dat.GetDatValue(col.ColumnName);
                        if (val == null)
                            val = BO.Reports.ExtraRepDataInfo.GetValue(dat, col.ColumnName);
                        if (val != null)
                        {
                            switch (col.GetCellType())
                            {
                                case CellType.Number:
                                    text = (val is decimal) ? ((decimal)val).ToString("###0.#######") : val.ToString();
                                    text = text.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                                    break;
                                case CellType.Boolean:
                                    //text = ((bool)val) ? "'да" : "'нет";
                                    text = ((bool)val) ? "1" : "0";
                                    //text = val;
                                    break;
                                case CellType.Xml:
                                    text = ((XmlDocument)val).OuterXml;
                                    break;
                                case CellType.DateTime:
                                    if (val is DateTime && (DateTime)val != DateTime.MinValue)
                                        text = ((DateTime)val).ToString("yyyy-MM-dd");
                                    break;
                                default:
                                    text = val.ToString();
                                    break;
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        text = "Ошибка: " + Common.ExMessage(exp);
                    }
                    CellType tp = col.GetCellType();
                    if (!(tp == CellType.DateTime && Common.IsNullOrEmpty(text)))
                        excel.AddCell(tp, text);
                }
            }
            return excel;
            
            //if (cols != null)
            //{
            //    foreach (PrintColumn col in cols)
            //    {
            //        if (col.GetValueFromInfo)
            //        {
            //            dList.Add(col.ColumnCaption, col);
            //        }
            //        foreach (DatDescriptor dd in BaseDat<BD>.Dlist)
            //        {
            //            if (dd.Name == col.ColumnName)
            //            {
            //                col.ValueType = dd.PropertyType;
            //                dList.Add(col.ColumnCaption, col);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (DatDescriptor dd in BaseDat<BD>.Dlist)
            //    {
            //        PrintColumn col = new PrintColumn(dd.Name, dd.Name);
            //        col.ValueType = dd.PropertyType;
            //        dList.Add(dd.Name, col);
            //    }
            //}

            //int count = dList.Count;
            //CellType[] celltypes = new CellType[count];

            //int it = 0;
            //foreach (PrintColumn pc in dList.Values)
            //{
            //    celltypes[it] = CellType.Error;
            //    //if (dd.Name != "")
            //    //{
            //    string styleName = string.Format("ColumnStyle{0}", it);
            //    int width = 20;
            //    if (pc.ValueType == typeof(decimal))
            //    {
            //        celltypes[it] = CellType.Number;
            //        width = 80;
            //        styleName = "styleAmount";
            //    }
            //    else if (pc.ValueType == typeof(DateTime))
            //    {
            //        celltypes[it] = CellType.DateTime;
            //        width = 50;
            //        styleName = "styleDate";
            //    }
            //    else if (pc.ValueType == typeof(int))
            //    {
            //        celltypes[it] = CellType.Number;
            //        width = 50;
            //        styleName = "styleAmount";
            //    }
            //    else if (pc.ValueType == typeof(XmlDocument))
            //    {
            //        celltypes[it] = CellType.Xml;
            //        width = 100;
            //        styleName = "styleText";
            //    }
            //    else if (pc.ValueType == typeof(bool))
            //    {
            //        celltypes[it] = CellType.Boolean;
            //        width = 20;
            //        styleName = "styleText";
            //    }
            //    else
            //    {
            //        celltypes[it] = CellType.String;
            //        width = 100;
            //        styleName = "styleText";
            //    }
            //    excel.AddColumn(width, true, styleName);
            //    //}
            //    it++;
            //}

            //excel.AddRow("H3");
            //excel.AddRepeatedRows(1, 1);
            //it = 0;

            //foreach (string cap in dList.Keys)
            //{
            //    if (celltypes[it] != CellType.Error)
            //    {
            //        excel.AddCell(cap);
            //    }
            //    it++;
            //}

            //List<PrintColumn> list = new List<PrintColumn>();
            //foreach (PrintColumn dd in dList.Values)
            //{
            //    list.Add(dd);
            //}

            //foreach (BD dat in SetList)
            //{
            //    excel.AddRow();
            //    for (int i = 0; i < count; i++)
            //    {
            //        PrintColumn dd = list[i];
            //        if (string.IsNullOrEmpty(dd.ColumnName)) continue;

            //        object val = null;
            //        try
            //        {
            //            if (dd.GetValueFromInfo)
            //            {
            //                val = BO.Reports.ExtraRepDataInfo.GetValue(dat, dd.ColumnName);
            //                //val = ProxyInfo.GetPropValue(dat, dd.ColumnName);
            //            }
            //            else
            //            {
            //                val = dat.GetDatValue(dd.ColumnName);
            //            }
            //        }
            //        catch { }
            //        if (val != null)
            //        {
            //            CellType ct;
            //            if (dd.GetValueFromInfo)
            //            {
            //                ct = GetCellType(val);
            //            }
            //            else
            //            {
            //                ct = celltypes[i];
            //            }

            //            switch (ct)
            //            {
            //                case CellType.Error:
            //                    continue;
            //                case CellType.Number:
            //                    excel.AddCell(celltypes[i], val.ToString());
            //                    break;
            //                case CellType.Boolean:
            //                    excel.AddCell((((bool)(val)) == true) ? "+" : "");
            //                    break;
            //                case CellType.Xml:
            //                    excel.AddCell(val != null ? ((XmlDocument)val).OuterXml : "");
            //                    break;
            //                case CellType.DateTime:
            //                    if (!(val is DateTime))
            //                        excel.AddCell("");
            //                    else if (((DateTime)val) == DateTime.MinValue)
            //                        excel.AddCell("");
            //                    else
            //                        excel.AddCell((DateTime)val);
            //                    break;
            //                case CellType.String:
            //                    excel.AddCell(celltypes[i], val.ToString());
            //                    break;
            //            }
            //        }
            //        else
            //            excel.AddCell(CellType.String, "");
            //    }
            //}
            //return excel;
        }
        private CellType GetCellType(object val)
        {
            Type tp = val.GetType();
            if (tp == typeof(decimal))
            {
                return CellType.Number;
            }
            else if (tp == typeof(DateTime))
            {
                return CellType.DateTime;
            }
            else if (tp == typeof(int))
            {
                return CellType.Number;
            }
            else if (tp == typeof(XmlDocument))
            {
                return CellType.Xml;
            }
            else if (tp == typeof(bool))
            {
                return CellType.Boolean;
            }
            else
            {
                return CellType.String;
            }
        }

        public override void Show()
        {
            base.Show();
            //if (fltController != null && fltController.FilterForm != null)
            if (fltController != null && LoadFilter != null)
            {
                //&& !AllowEmptyLoadFilter
                IDAOFilter flt = LoadFilter.GetFilter();
                if (string.IsNullOrEmpty(flt.WhereString().Trim()) && string.IsNullOrEmpty(flt.JoinString().Trim()) && !AllowEmptyLoadFilter)     // если фильтр по-умолчанию не задан
                    fltController.ShowFilterForm(); // то поднимаем FilterForm
                else
                    Refresh();
            }
        }
    }
    public abstract class ListFormController<BS, BD, FF, FL> : ListFormController<BS, BD>
        where BS : BaseSet<BD, BS>, new()
        where BD : BaseDat<BD>, new()
        where FF : FilterFormBase
        where FL : BaseLoadFilter
    {
        public ListFormController(Form mdiForm, string name, string caption, Icon icon, BaseLoadFilter loadfilter)
            : base(mdiForm, name, caption, icon) 
        {
            LoadFilter = loadfilter;
            if (LoadFilter == null)
            {
                LoadFilter = Activator.CreateInstance<FL>();
                LoadFilter.ResetToDefault();
            }
            FilterController<FF> fc = new FilterController<FF>();
            fltController = fc;
            fc.Filter = LoadFilter;
            fc.Init(FToolBar);
            fc.FilterChanged += new EventHandler<FilterEventsArgs2>(FilterChanged);
        }

        public ListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel)
            : this(mdiForm, name, caption, icon, TreePanel, EntityPanel, null, false) { }

        public ListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel, BaseLoadFilter loadfilter)
            : this(mdiForm, name, caption, icon, TreePanel, EntityPanel, loadfilter, true)
        { }
        public ListFormController(Form mdiForm, string name, string caption, Icon icon, bool TreePanel, bool EntityPanel, BaseLoadFilter loadfilter, bool allowEmptyLoadFilter)
            : this(mdiForm, name, caption, icon, loadfilter)
        {
            PaintPanels(TreePanel, EntityPanel);
            //InitFilter(loadfilter);
            AddListPrintingButtons();

            gridMain.Init(PanelList);

            if (EntityPanel)
                InitEntity();

            gridMain.Grid.ValueEventDisabled = true;
            AllowEmptyLoadFilter = allowEmptyLoadFilter;
            Show();
            // Refresh(); Сделал Кишик, чтобы работало задание фильтра при открытии формы(Refresh() перенесен внутрь Show())
            gridMain.Grid.ValueEventDisabled = false;
            gridMain.Grid.FireValueChanged();
        }

        public override void InitFilter(BaseLoadFilter loadfilter)
        {
            LoadFilter = loadfilter;
        }

        void FilterChanged(object sender, FilterEventsArgs2 e)
        {
            InitFilter(e.Filter);
            Refresh();
        }
    }
    public class PrintColumn
    {
        public PrintColumn(string name, string caption)
        {
            ColumnName = name;
            ColumnCaption = caption;
        }

        public PrintColumn(string name, string caption, Type type, int width)
            : this(name, caption)
        {
            ValueType = type;
            Width = width;
        }

        private string _ColumnName;
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        private string _ColumnCaption;
        public string ColumnCaption
        {
            get { return _ColumnCaption; }
            set { _ColumnCaption = value; }
        }

        //private bool _GetValueFromInfo;
        //public bool GetValueFromInfo
        //{
        //    get { return _GetValueFromInfo; }
        //    set { _GetValueFromInfo = value; }
        //}

        private Type _ValueType = typeof(string);
        public Type ValueType
        {
            get { return _ValueType; }
            set { _ValueType = value; }
        }

        private double _Width = 80;
        public double Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        
        public CellType GetCellType()
        {
            if (ValueType == typeof(bool))
                return CellType.Boolean;
            else if (ValueType == typeof(DateTime))
                return CellType.DateTime;
            else if (ValueType == typeof(int))
                return CellType.Number;
            else if (ValueType == typeof(decimal))
                return CellType.Number;
            else if (ValueType == typeof(XmlDocument))
                return CellType.Xml;
            else
                return CellType.String;
        }

        public string GetStyleID()
        {
            if (ValueType == typeof(DateTime))
                return "styleDate";
            else if (ValueType == typeof(int))
                return "styleAmount";
            else if (ValueType == typeof(decimal))
                return "styleAmount";
            else
                return "styleText";
        }
    }
}
