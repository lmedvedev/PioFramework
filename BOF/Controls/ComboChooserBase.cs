using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using BO;
using System.Runtime.InteropServices;

namespace BOF
{
    public delegate string Item2Text<T>(T item);
    /// <summary>
    /// Класс для показа в комбобоксе набора из классов.
    /// Полезно для enum-типов, например.
    /// Должна быть функция преобразования 
    /// </summary>
    public class ComboChooserBase : ComboBox, IDataMember
    {
        public ComboChooserBase()
            : base()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            //FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
            MaxDropDownWidth = this.DropDownWidth;
        }
        bool _FitToValues = false;
        public bool FitToValues
        {
            get { return _FitToValues; }
            set { _FitToValues = value; }
        }

        ToolTip _ToolTip = null;
        public ToolTip ToolTip
        {
            get { return _ToolTip; }
            set { _ToolTip = value; }
        }

        int MaxDropDownWidth = 0;


        protected virtual string BindProperty
        {
            get { return "SelectedValue"; }
        }
        //protected const string BIND_PROPERTY = "SelectedValue";
        //protected const string BIND_PROPERTY = "Value";

        /// <summary>
        /// Функция для начальной заливки пар в комбик. 
        /// </summary>
        /// <typeparam name="T">Enum-тип классов, которые будут показываться в комбо</typeparam>
        /// <param name="func">Делегат функции, возвращающий string из enum-класса</param>
        /// <param name="items">Список строк в комбо</param>
        public virtual void FillValues<T>(Item2Text<T> func, params T[] items)
        {
            List<ComboItem> ds = new List<ComboItem>();

            foreach (T item in items)
            {
                ds.Add(new ComboItem(func(item), item));
            }
            FillValues(ds);
        }

        //нельзя просто так добавлять список, не проверив, что уже такой же список загружен
        //public virtual void FillValues<T>(IEnumerable<T> items)
        //{
        //    foreach (T item in items)
        //    {
        //        Items.Add(item);
        //    }
        //}
        
        public virtual void AddValueEnum<T>(T item)
        {
            AddValue<T>(BO.Xml.XmlEnum.GetXmlEnumString(item), item);
        }

        public virtual void AddValue<T>(Item2Text<T> func, T item)
        {
            AddValue<T>(func(item), item);
        }

        public virtual void AddValue<T>(string text, T item)
        {
            if (DataSource == null)
            {
                DataSource = new List<ComboItem>();
                ValueMember = "Value";
                DisplayMember = "Key";
            }
            ((List<ComboItem>)base.DataSource).Add(new ComboItem(text, item));
        }
        
        protected ComboItem GetComboItem(object val)
        {
            return new ComboItem(BO.Xml.XmlEnum.GetXmlEnumString(val), val);
        }
        
        public virtual void FillValues(params ComboItem[] items)
        {
            FillValues(new List<ComboItem>(items));
        }

        public virtual void FillValues(List<ComboItem> items, object value)
        {
            event_disabled = true;
            List<ComboItem> lst = (List<ComboItem>)base.DataSource;
            bool equal = (lst != null && items != null && lst.Count == items.Count);
            if (equal)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    ComboItem item_old = lst[i];
                    ComboItem item_new = items[i];
                    equal &= (item_old.Key == item_new.Key);
                }
            }
            if (!equal)
            {
                base.DataSource = items;
                base.ValueMember = "Value";
                base.DisplayMember = "Key";
                
                if (FitToValues)
                {
                    string max = "";
                    foreach (ComboItem item in items)
                    {
                        if (item.Key.Length > max.Length)
                            max = item.Key;
                    }
                    MaxDropDownWidth = (int)this.CreateGraphics().MeasureString(max, Font).Width;
                }
            }
            event_disabled = false;
            if (value == null)
                SelectedIndex = -1;
            else
                SelectedValue = value;
        }
        #region Interop
        internal const int WM_CTLCOLORLISTBOX = 0x134;

        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;





        }

        [DllImport("user32.dll")]
        internal static extern bool GetWindowRect(IntPtr hwnd, ref RECT lpRect);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);
        #endregion Interop
        //protected override void WndProc(ref Message m)
        //{
        //    const int WM_PAINT = 0x000F;
        //    base.WndProc(ref m);
        //    if (m.Msg == WM_PAINT && this.FlatStyle == FlatStyle.Flat)
        //    {
        //        Graphics g = Graphics.FromHwnd(m.HWnd);
        //        g.PageUnit = GraphicsUnit.Pixel;
        //        Rectangle rect = ClientRectangle;
        //        ControlPaint.DrawBorder(g, rect, Color.Black, ButtonBorderStyle.Solid);
        //        rect = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2);
        //        ControlPaint.DrawBorder(g, rect, Color.White, ButtonBorderStyle.Solid);
        //        g.Dispose();
        //    }
        //}
        protected override void WndProc(ref Message m)
        {
            const int WM_PAINT = 0x000F;
            base.WndProc(ref m);
            if (m.Msg == WM_CTLCOLORLISTBOX)
            {
                RECT dropDownRect = new RECT();
                GetWindowRect(m.LParam, ref dropDownRect);
                Point dropDownPosition = this.PointToScreen(new Point(0, this.Size.Height));


                int new_width = DropDownWidth;
                if (MaxDropDownWidth > this.Size.Width && DropDownWidth != MaxDropDownWidth)
                {
                    Control top = this.TopLevelControl;
                    int width = top.PointToClient(this.Parent.PointToScreen(this.Location)).X + this.Width;
                    new_width = (MaxDropDownWidth > width) ? width : MaxDropDownWidth;
                }
                if (new_width < this.Size.Width)
                    new_width = this.Size.Width;
                if (new_width != DropDownWidth)
                {
                    MoveWindow(
                        m.LParam,
                        dropDownPosition.X - (new_width - this.Size.Width),
                        dropDownPosition.Y,
                        new_width,
                        dropDownRect.bottom - dropDownRect.top,
                        true);
                }
            }
            if (m.Msg == WM_PAINT && this.FlatStyle == FlatStyle.Flat)
            {
                Graphics g = Graphics.FromHwnd(m.HWnd);
                g.PageUnit = GraphicsUnit.Pixel;
                Rectangle rect = ClientRectangle;
                ControlPaint.DrawBorder(g, rect, Color.Black, ButtonBorderStyle.Solid);
                rect = new Rectangle(ClientRectangle.X + 1, ClientRectangle.Y + 1, ClientRectangle.Width - 2, ClientRectangle.Height - 2);
                ControlPaint.DrawBorder(g, rect, Color.White, ButtonBorderStyle.Solid);
                g.Dispose();
            }
        }
        public virtual void FillValues(List<ComboItem> items)
        {
            List<ComboItem> lst = (List<ComboItem>)base.DataSource;
            bool equal = (lst != null && items != null && lst.Count == items.Count);
            if (equal)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    ComboItem item_old = lst[i];
                    ComboItem item_new = items[i];
                    equal &= (item_old.Key == item_new.Key);
                }
            }
            if (!equal)
            {
                ComboItem oldValue = this.SelectedItem as ComboItem;
                base.DataSource = items;
                base.ValueMember = "Value";
                base.DisplayMember = "Key";

                if (oldValue != null)
                {
                    int ind = items.FindIndex(delegate(ComboItem item)
                                                        {
                                                            return (oldValue.Key == item.Key);
                                                        });
                    if (ind >= 0)
                        this.SelectedIndex = ind;
                }
            }
        }
        public virtual void SelectValue(BaseDat Value) // Добавил Медведев.
        {
            int index = SelectedIndex;
            BaseSet s = DataSource as BaseSet;

            if (Value == null || DataSource == null)
                return;

            index = s.IndexOf(Value);
            if (index >= 0)
                SelectedIndex = index;
        }

        [Serializable]
        public class ComboItem
        {
            string _Key;

            public string Key
            {
                get { return _Key; }
                set { _Key = value; }
            }
            object _Value;

            public object Value
            {
                get { return _Value; }
                set
                {
                    if (_Value != value)
                        _Value = value;
                }
            }
            public ComboItem(string key, object value)
            {
                _Key = key;
                _Value = value;
            }
        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    base.OnPaint(e);
        //    Graphics g = e.Graphics;
        //    if (this.FlatStyle == FlatStyle.Flat)
        //        ControlPaint.DrawBorder(g, e.ClipRectangle, Color.Black, ButtonBorderStyle.Solid);
        //}
        #region IDataMember Members
        public event EventHandler ValueChanged;

        private string _DataMember;
        public string DataMember
        {
            get { return _DataMember; }
            set { _DataMember = value; }
        }

        public void AddBinding(object datasource)
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BindProperty] == null)
                this.DataBindings.Add(BindProperty, datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BindProperty] != null)
                this.DataBindings.Remove(this.DataBindings[BindProperty]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BindProperty] != null)
                this.DataBindings[BindProperty].WriteValue();
        }
        #endregion
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!this.DroppedDown)
            {
                if (keyData == Keys.Return || (keyData == Keys.Right))
                {
                    Form frm = this.FindForm();
                    if (frm != null)
                        frm.SelectNextControl(this, true, true, true, true);
                    if (keyData == Keys.Return) return true;
                }
                else if (keyData == Keys.Left)
                {
                    Form frm = this.FindForm();
                    if (frm != null)
                        frm.SelectNextControl(this, false, true, true, false);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region HelpLabel
        const string _helplabeltext = "Alt+вниз - вызов списка. Стрелка вверх/вниз - выбор пред./след. значения из списка. Стрелка влево - предыдущее поле. Enter, стрелка вправо - следующее поле.";
        private string _HelpLabelText = _helplabeltext;

        [Category("BOF")]
        [DefaultValue(_helplabeltext)]
        [Description("Текст, который будет показываться при активации контрола")]
        public string HelpLabelText
        {
            get { return _HelpLabelText; }
            set { _HelpLabelText = value; }
        }

        protected override void OnEnter(EventArgs e)
        {
            if (Enabled)
            {
                IHelpLabel frm = FindForm() as IHelpLabel;
                if (frm != null) frm.WriteHelp(HelpLabelText);
            }
            base.OnEnter(e);
        }
        protected override void OnLeave(EventArgs e)
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.WriteHelp("");
            base.OnLeave(e);
        }

        #endregion

        [Category("BOF")]
        [Bindable(true)]
        public object Value
        {
            get { return this.SelectedValue; }
            set
            {
                if (this.SelectedValue != value
                    && (
                        this.SelectedValue != null
                        && value != null
                        && this.SelectedValue.ToString() != value.ToString()
                        )
                    )
                {
                    this.SelectedValue = value;
                }
            }
        }
        public void FireValueChanged()
        {
            IHelpLabel frm = FindForm() as IHelpLabel;
            if (frm != null)
                frm.SetError(this, "");
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
        }
        bool event_disabled = false;
        protected override void OnSelectedValueChanged(EventArgs e)
        {
            if (!event_disabled)
            {
                //Console.WriteLine("Chooser={4} value={0}, sitem={1}, sindex={2}, stext={3}",this.Value, this.SelectedItem, this.SelectedIndex , this.SelectedText, this);
                base.OnSelectedValueChanged(e);
                FireValueChanged();
            }
        }
    }
}
