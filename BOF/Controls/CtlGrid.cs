using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Xml;

namespace BOF
{
    public partial class CtlGrid : DataGridView, ISelectable
    {
        object lastvalue = null;
        public CtlGrid()
        {
            this.BorderStyle = BorderStyle.None;
            this.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RowHeadersVisible = false;
            this.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.AutoGenerateColumns = true;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowDrop = false;
            this.VirtualMode = false;
            this.CellDoubleClick += new DataGridViewCellEventHandler(CtlGrid_CellDoubleClick);
            this.SelectionChanged += CtlGrid_SelectionChanged;
            this.Sorted += new EventHandler(CtlGrid_Sorted);
            this.DataSourceChanged += new EventHandler(CtlGrid_DataSourceChanged);
            this.AllowUserToResizeRows = false;
            this.DataError += new DataGridViewDataErrorEventHandler(CtlGrid_DataError);
            this.BackgroundColor = Color.White;
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            this.OnLeave(EventArgs.Empty);
        }

        void CtlGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            this.Cursor = Cursors.Default;
            e.Cancel = true;
        }

        string _DataMember;
        new public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        bool _ShowContextMenu = false;
        public bool ShowContextMenu
        {
            get { return _ShowContextMenu; }
            set
            {
                _ShowContextMenu = value;
                if (!value)
                    this.ContextMenuStrip = null;
                else if (this.ContextMenuStrip == null)
                {
                    this.ContextMenuStrip = new ContextMenuStrip();
                    this.ContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    new ToolStripMenuItem("Добавить элемент", global::BOF.Properties.Resources.NewDataRecord, delegate(object snd, EventArgs ev){ FireValueInserted(); }, Keys.Insert),
                    new ToolStripMenuItem("Удалить элемент", global::BOF.Properties.Resources.DeleteHS, delegate(object snd, EventArgs ev){ FireValueDeleted(); }, Keys.Delete) });
                }
            }
        }

        const string _helplabeltext = "Enter, вправо - к следующему полю, влево - к предыдущему полю. ";
        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _helplabeltext + (ShowContextMenu ? "F2 - редактировать, Insert - добавить, Delete - удалить. Правая кнопка мыши - контекстное меню." : ""); }
            set { }
        }
        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
                this.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                this.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
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
                this.DefaultCellStyle.SelectionBackColor = SystemColors.Info;
                this.DefaultCellStyle.SelectionForeColor = SystemColors.InfoText;
                this.Invalidate();
            }
            if (this.IsCurrentCellInEditMode)
            {
                CommitEdit(DataGridViewDataErrorContexts.LeaveControl);
                EndEdit();
            }
            base.OnLeave(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            DataGridView.HitTestInfo Hti = HitTest(e.X, e.Y);
            if (this.IsCurrentCellInEditMode)
            {
                if (Hti.Type != DataGridViewHitTestType.Cell || Hti.RowIndex != CurrentRow.Index)
                    this.EndEdit();
            }
            else
                this.Focus();

            if (e.Button == MouseButtons.Right)
            {
                if (Hti.Type == DataGridViewHitTestType.Cell || Hti.Type == DataGridViewHitTestType.RowHeader)
                {
                    if (!((DataGridViewRow)(Rows[Hti.RowIndex])).Selected)
                    {
                        ValueEventDisabled = true;
                        CurrentCell = Rows[Hti.RowIndex].Cells[Hti.ColumnIndex];
                        ValueEventDisabled = false;
                        FireValueChanged();
                    }
                }
            }
            base.OnMouseDown(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if ((this.Focused || this.IsCurrentCellInEditMode) && this.Enabled && this.TabStop && this.CurrentCell != null && (e.ClipRectangle == this.ClientRectangle || e.ClipRectangle != this.CurrentCell.ContentBounds))
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        bool issorting = false;
        void CtlGrid_Sorted(object sender, EventArgs e)
        {
            Value = lastvalue;
            issorting = false;
        }

        void CtlGrid_DataSourceChanged(object sender, EventArgs e)
        {
            IBindingList list = this.DataSource as IBindingList;
            if (list != null) list.ListChanged += new ListChangedEventHandler(CtlGrid_ListChanged);
        }

        public event ListChangedEventHandler ListChanged;
        void CtlGrid_ListChanged(object sender, ListChangedEventArgs e)
        {
            //issorting = true;
            if (ListChanged != null)
                ListChanged(sender, e);
        }

        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            issorting = true;
            base.Sort(dataGridViewColumn, direction);
        }

        void CtlGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (CurrentCell != null && !issorting)
            {
                FireValueChanged();
                lastvalue = Value;
            }
        }

        void CtlGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (ValueSelected != null)
                    FireValueSelected();
                else if (this.Enabled)
                    BeginEdit(true);
            }
        }

        #region ISelectable Members

        public event ValueEventHandler ValueSelected;
        public event ValueEventHandler ValueInserted;
        public event ValueEventHandler ValueDeleted;
        public event ValueEventHandler ValueChanged;

        private bool _ValueEventDisabled;
        public bool ValueEventDisabled
        {
            get { return _ValueEventDisabled; }
            set
            {
                _ValueEventDisabled = value;

            }
        }

        public object Value
        {
            get
            {
                if (CurrentRow != null)
                    return CurrentRow.DataBoundItem;
                else
                    return null;

                //if (DataSource == null || CurrentRow == null || !(DataSource is IList))
                //    return null;
                //else
                //{
                //    return ((IList)DataSource)[CurrentRow.Index] ;
                //}
            }
            set
            {
                BaseSet bset = DataSource as BaseSet;

                if (bset != null && BindingContext != null)
                {
                    CurrencyManager cm = (CurrencyManager)BindingContext[DataSource];
                    cm.Position = -1;
                    IDat dat = value as IDat;
                    if (dat != null)
                    {
                        object x = bset.FindByID(dat.ID);
                        if (x != null)
                            cm.Position = bset.IndexOf(x);
                    }
                }
            }
        }
        public int SelectedRowIndex
        {
            get
            {
                return CurrentRow == null ? -1 : CurrentRow.Index;
            }
            set
            {
                if (value >= 0 && RowCount > 0)
                    CurrentCell = Rows[value].Cells[0];
                else
                    CurrentCell = null;
                //else if (DataSource != null && BindingContext != null)
                //{
                //    CurrencyManager cm = (CurrencyManager)BindingContext[DataSource];
                //    cm.Position = value;
                //}
            }
        }

        public void FireValueChanged()
        {
            if (ValueChanged != null && !ValueEventDisabled && !IsCurrentCellInEditMode)
            {
                ValueChanged(this, new ValueEventArgs(Value));
                if (!ValueEventDisabled)
                    this.Invalidate();
            }
        }

        public void FireValueSelected()
        {
            if (ValueSelected != null && !ValueEventDisabled && !IsCurrentCellInEditMode)
            {
                ValueSelected(this, new ValueEventArgs(Value));
                if (!ValueEventDisabled)
                    this.Invalidate();
            }
        }

        public void FireValueInserted()
        {
            if (ValueInserted != null && !ValueEventDisabled && !IsCurrentCellInEditMode)
            {
                ValueInserted(this, new ValueEventArgs(null));
                if (!ValueEventDisabled)
                    this.Invalidate();
            }
        }

        public void FireValueDeleted()
        {
            if (ValueDeleted != null && !ValueEventDisabled && !IsCurrentCellInEditMode)
            {
                ValueDeleted(this, new ValueEventArgs(Value));
                if (!ValueEventDisabled)
                    this.Invalidate();
            }
        }

        public void Reload()
        {
            BaseSet set = this.DataSource as BaseSet;
            if (set != null) set.Load();
        }

        public List<T> GetSelectedValues<T>()
        {
            List<T> ret = new List<T>();
            foreach (DataGridViewRow row in SelectedRows)
            {
                ret.Add((T)row.DataBoundItem);
            }
            ret.Reverse();
            return ret;
        }

        public TS GetSelectedSet<TD, TS>()
            where TS : BaseSet<TD, TS>, new()
            where TD : BaseDat<TD>
        {
            TS ret = new TS();
            foreach (DataGridViewRow row in SelectedRows)
            {
                ret.Add((TD)row.DataBoundItem);
            }
            return ret;
        }
        #endregion

        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_PAINT = 0x000F;
        //    base.WndProc(ref m);
        //    if (m.Msg == WM_PAINT)
        //    {
        //        Graphics g = Graphics.FromHwnd(m.HWnd);
        //        ControlPaint.DrawBorder(g, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
        //        g.Dispose();
        //    }
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.IsCurrentCellInEditMode)
            {
                if (keyData == Keys.Return)
                {
                    //this.EndEdit();
                    if (CurrentCell.ColumnIndex < this.Columns.Count - 1)
                    {
                        CurrentCell = this.Rows[CurrentCell.RowIndex].Cells[CurrentCell.ColumnIndex + 1];
                        BeginEdit(true);
                    }
                    else
                        OnLeave(EventArgs.Empty);
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    this.CancelEdit();
                    this.EndEdit();
                    return true;
                }
            }
            else if (keyData == Keys.F2)
            {
                CurrentCell = this.Rows[CurrentCell.RowIndex].Cells[0];
                BeginEdit(true);
                return true;
            }
            else if (keyData == Keys.Return && ValueSelected != null)
            {
                FireValueSelected();
                return true;
            }
            else if (keyData == Keys.Left || (keyData == Keys.Return && ValueSelected == null))
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
                FireValueInserted();
                return true;
            }
            else if (keyData == Keys.Delete)
            {
                FireValueDeleted();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Добавил Андрей - новые DataSource & GridColumns
        public void SetDataSource(object src)
        {
            if (!(src is BaseSet))
                base.DataSource = null;
            base.DataSource = src;
            //IBindingList list = this.DataSource as IBindingList;
            //if (list != null)
            //    list.ListChanged += delegate(object sender, ListChangedEventArgs e) { if (!this.IsCurrentCellInEditMode) this.OnDataSourceChanged(EventArgs.Empty); };
            FireValueChanged();
        }
        public void SetGridColumns(DataGridViewColumn[] columns)
        {
            base.Columns.Clear();
            if (columns != null) base.Columns.AddRange(columns);
            AutoGenerateColumns = base.Columns.Count > 0;
        }

        public DataGridViewColumn AddGridColumn(DataGridViewColumn column)
        {
            base.Columns.Add(column);
            return column;
        }

        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type)
        {
            return AddGridColumn(name, headerText, type, name);
        }

        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, int columnWidth)
        {
            return AddGridColumn(name, headerText, type, name, columnWidth);
        }
        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, int columnWidth, bool readOnly)
        {
            return AddGridColumn(name, headerText, type, name, columnWidth, null, readOnly);
        }
        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, string dataPropertyName)
        {
            return AddGridColumn(name, headerText, type, dataPropertyName, 0, null, false);
        }

        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, string dataPropertyName, int columnWidth)
        {
            return AddGridColumn(name, headerText, type, dataPropertyName, columnWidth, "", false);
        }

        //public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, string dataPropertyName, bool readOnly)
        //{
        //    DataGridViewColumn col = new DataGridViewTextBoxColumn();
        //    col.HeaderText = headerText;
        //    col.Name = name;
        //    col.DataPropertyName = dataPropertyName;
        //    if (type != null)
        //        col.ValueType = type;
        //    setFmt(col);
        //    col.ReadOnly = readOnly;
        //    AddGridColumn(col);
        //    return col;
        //}
        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, string dataPropertyName, int columnWidth, string format)
        {
            return AddGridColumn(name, headerText, type, dataPropertyName, columnWidth, format, true);
        }

        public DataGridViewColumn AddGridColumn(string name, string headerText, Type type, string dataPropertyName, int columnWidth, string format, bool readOnly)
        {
            DataGridViewColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = headerText;
            col.Name = name;
            col.DataPropertyName = dataPropertyName;
            if (type != null)
                col.ValueType = type;
            setFmt(col);
            col.ReadOnly = readOnly;

            if (!string.IsNullOrEmpty(format))
                col.DefaultCellStyle.Format = format;

            if (columnWidth > 0)
            {
                col.Width = columnWidth;
            }
            else if (columnWidth < 0)
            {
                col.Visible = false;
            }

            AddGridColumn(col);
            return col;
        }

        public DataGridViewComboBoxColumn AddGridComboBoxColumn(string name, string header, string displaymember, BaseSet datasource)
        {
            return AddGridComboBoxColumn(name, header, displaymember, typeof(string), datasource);
        }
        public DataGridViewComboBoxColumn AddGridComboBoxColumn(string name, string header, string displaymember, Type type, BaseSet datasource)
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            col.HeaderText = header;
            col.DataPropertyName = col.Name = name;
            col.ValueType = type;
            col.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            col.DisplayMember = displaymember;
            col.DataSource = datasource;
            col.SortMode = DataGridViewColumnSortMode.NotSortable;
            AddGridColumn(col);
            return col;
        }
        public DataGridViewComboBoxColumn AddGridComboBoxColumn(string name, string header, params object[] items)
        {
            DataGridViewComboBoxColumn col = new DataGridViewComboBoxColumn();
            col.HeaderText = header;
            col.Name = col.DataPropertyName = name;
            col.ValueType = typeof(string);
            col.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            if (items != null && items.Length > 0)
            {
                col.Items.AddRange(items);
            }
            AddGridColumn(col);
            return col;
        }
        public DataGridViewCheckBoxColumn AddGridCheckColumn(string name, string header)
        {
            DataGridViewCheckBoxColumn col = new DataGridViewCheckBoxColumn();
            col.HeaderText = header;
            col.Name = col.DataPropertyName = name;
            col.Width = 20;
            AddGridColumn(col);
            return col;
        }
        public DataGridViewImageColumn AddGridImageColumn(string name, Type type, string dataPropertyName)
        {
            return AddGridImageColumn(name, "", type, dataPropertyName, name);
        }
        public DataGridViewImageColumn AddGridImageColumn(string name, string header, Type type, string dataPropertyName, string tooltip)
        {
            DataGridViewImageColumn col = new DataGridViewImageColumn(true);
            col.HeaderText = header;
            col.Name = name;
            col.DataPropertyName = dataPropertyName;
            if (type != null)
                col.ValueType = type;
            col.Width = 20;
            col.ToolTipText = tooltip;
            AddGridColumn(col);
            return col;
        }

        public static void setFmt(DataGridViewColumn col)
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
                //col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.Width = 65;
            }
        }
        public static string ColumnSource(params string[] args)
        {
            return string.Join("__", args);
        }

        #endregion

        protected virtual List<PrintColumn> InitPrintColumn()
        {
            List<PrintColumn> ret = new List<PrintColumn>();
            foreach (DataGridViewColumn col in this.Columns)
            {
                ret.Add(new PrintColumn(col.Name, col.HeaderText, col.ValueType, col.Width));
            }
            return ret;
        }

        private ExcelXml PrepareXL(out List<PrintColumn> cols)
        {
            ExcelXml excel = new ExcelXml();
            excel.AddDefaultStyles();
            excel.AddDefaultWorksheet("Report", "", true, "Список");

            Dictionary<string, PrintColumn> dList = new Dictionary<string, PrintColumn>();
            cols = InitPrintColumn();

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
            return excel;
        }

        public ExcelXml PrintList()
        {
            List<PrintColumn> cols;
            ExcelXml excel = PrepareXL(out cols);

            foreach (DataGridViewRow item in this.SelectedRows)
            {
                excel.AddRow();
                foreach (PrintColumn col in cols)
                {
                    string text = "";
                    try
                    {
                        object val = item.Cells[col.ColumnName].Value;
                        //if (val == null)
                        //    val = BO.Reports.ExtraRepDataInfo.GetValue(dat, col.ColumnName);
                        if (val != null)
                        {
                            switch (col.GetCellType())
                            {
                                case CellType.Number:
                                    text = (val is decimal) ? ((decimal)val).ToString("###0.#######") : val.ToString();
                                    text = text.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                                    break;
                                case CellType.Boolean:
                                    text = ((bool)val) ? "+" : "";
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
        }

        public ExcelXml PrintList<BD, BS>()
            where BS : BaseSet<BD, BS>, new()
            where BD : BaseDat<BD>, new()
        {
            List<PrintColumn> cols;
            ExcelXml excel = PrepareXL(out cols);

            BS lst = this.GetSelectedSet<BD, BS>();
            if (lst.Count <= 1)
                lst = DataSource as BS;

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
                                    text = ((bool)val) ? "+" : "";
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
        }

    }
}
