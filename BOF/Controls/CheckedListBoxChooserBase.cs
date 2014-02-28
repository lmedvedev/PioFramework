using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using BO;

namespace BOF
{
    //public delegate string Item2Text<T>(T item);
    /// <summary>
    /// Класс для показа в комбобоксе набора из классов.
    /// Полезно для enum-типов, например.
    /// Должна быть функция преобразования 
    /// </summary>
    public class CheckedListBoxChooserBase : CheckedListBox, IDataMember
    {
        public List<int> SelectedIDs
        {
            get
            {
                List<int> ret = new List<int>();
                foreach (object item in CheckedItems)
                {
                    if (item is IDat)
                        ret.Add(((IDat)item).ID);
                }
                return ret;
            }
            set
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    object item = Items[i];
                    if (item is IDat)
                        SetItemChecked(i, value.Contains(((IDat)item).ID));
                }
            }
        }
        public CheckedListBoxChooserBase()
            : base()
        {
            //DropDownStyle = ComboBoxStyle.DropDownList;
            //FlatStyle = FlatStyle.Flat;
            Cursor = Cursors.Hand;
        }

        protected const string BIND_PROPERTY = "SelectedValue";
        //        protected const string BIND_PROPERTY = "Value";

        /// <summary>
        /// Функция для начальной заливки пар в комбик. 
        /// </summary>
        /// <typeparam name="T">Enum-тип классов, которые будут показываться в комбо</typeparam>
        /// <param name="func">Делегат функции, возвращающий string из enum-класса</param>
        /// <param name="items">Список строк в комбо</param>
        public virtual void FillValues<T>(Item2Text<T> func, params T[] items)
        {
            List<BOF.ComboChooserBase.ComboItem> ds = new List<BOF.ComboChooserBase.ComboItem>();

            foreach (T item in items)
            {
                ds.Add(new BOF.ComboChooserBase.ComboItem(func(item), item));
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
                DataSource = new List<BOF.ComboChooserBase.ComboItem>();
                ValueMember = "Value";
                DisplayMember = "Key";
            }
            ((List<BOF.ComboChooserBase.ComboItem>)base.DataSource).Add(new BOF.ComboChooserBase.ComboItem(text, item));
        }

        public virtual void FillValues(params BOF.ComboChooserBase.ComboItem[] items)
        {
            FillValues(new List<BOF.ComboChooserBase.ComboItem>(items));
        }

        public virtual void FillValues(List<BOF.ComboChooserBase.ComboItem> items, object value)
        {
            event_disabled = true;
            List<BOF.ComboChooserBase.ComboItem> lst = (List<BOF.ComboChooserBase.ComboItem>)base.DataSource;
            bool equal = (lst != null && items != null && lst.Count == items.Count);
            if (equal)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    BOF.ComboChooserBase.ComboItem item_old = lst[i];
                    BOF.ComboChooserBase.ComboItem item_new = items[i];
                    equal &= (item_old.Key == item_new.Key);
                }
            }
            if (!equal)
            {
                base.DataSource = items;
                base.ValueMember = "Value";
                base.DisplayMember = "Key";
            }
            event_disabled = false;
            Value = value;
        }

        public virtual void FillValues(List<BOF.ComboChooserBase.ComboItem> items)
        {
            List<BOF.ComboChooserBase.ComboItem> lst = (List<BOF.ComboChooserBase.ComboItem>)base.DataSource;
            bool equal = (lst != null && items != null && lst.Count == items.Count);
            if (equal)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    BOF.ComboChooserBase.ComboItem item_old = lst[i];
                    BOF.ComboChooserBase.ComboItem item_new = items[i];
                    equal &= (item_old.Key == item_new.Key);
                }
            }
            if (!equal)
            {
                BOF.ComboChooserBase.ComboItem oldValue = this.SelectedItem as BOF.ComboChooserBase.ComboItem;
                base.DataSource = items;
                base.ValueMember = "Value";
                base.DisplayMember = "Key";

                if (oldValue != null)
                {
                    int ind = items.FindIndex(delegate(BOF.ComboChooserBase.ComboItem item)
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

        //[Serializable]
        //public class ComboItem
        //{
        //    string _Key;

        //    public string Key
        //    {
        //        get { return _Key; }
        //        set { _Key = value; }
        //    }
        //    object _Value;

        //    public object Value
        //    {
        //        get { return _Value; }
        //        set
        //        {
        //            if (_Value != value)
        //                _Value = value;
        //        }
        //    }
        //    public ComboItem(string key, object value)
        //    {
        //        _Key = key;
        //        _Value = value;
        //    }
        //}
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
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BIND_PROPERTY] == null)
                this.DataBindings.Add(BIND_PROPERTY, datasource, DataMember, true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void AddBinding()
        {
            OKCancelDatForm frm = this.FindForm() as OKCancelDatForm;
            if (frm != null)
                AddBinding(frm.NewValue);
        }

        public void RemoveBinding()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BIND_PROPERTY] != null)
                this.DataBindings.Remove(this.DataBindings[BIND_PROPERTY]);
        }

        public void WriteValue()
        {
            if (!string.IsNullOrEmpty(DataMember) && this.DataBindings[BIND_PROPERTY] != null)
                this.DataBindings[BIND_PROPERTY].WriteValue();
        }
        #endregion
        //protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        //{
        //    if (!this.DroppedDown)
        //    {
        //        if (keyData == Keys.Return || (keyData == Keys.Right))
        //        {
        //            Form frm = this.FindForm();
        //            if (frm != null)
        //                frm.SelectNextControl(this, true, true, true, true);
        //            if (keyData == Keys.Return) return true;
        //        }
        //        else if (keyData == Keys.Left)
        //        {
        //            Form frm = this.FindForm();
        //            if (frm != null)
        //                frm.SelectNextControl(this, false, true, true, false);
        //        }
        //    }
        //    return base.ProcessCmdKey(ref msg, keyData);
        //}

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
    }
}
