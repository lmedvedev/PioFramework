using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using DA;
using BO;

namespace BOF
{
    public class FilterController
    {
        public FilterController()
        {
            ButtonFilter = new ToolStripButton("‘ильтр", Icons.FilterForm.ToBitmap(), ButtonFilter_Click, "FilterShow");
            ButtonReset = new ToolStripButton("—брос", Icons.Filter, ButtonReset_Click, "FilterReset");
            ButtonReset.ToolTipText = "—бросить фильтр в состо€ние по умолчанию";
        }

        public void Init(ToolStrip tlb)
        {
            tlb.Items.Add(ButtonFilter);
            tlb.Items.Add(ButtonReset);
            filter_CloseApply(this, EventArgs.Empty);
        }

        private BaseLoadFilter _Filter;
        public BaseLoadFilter Filter
        {
            get
            {
                if (_Filter == null)
                    _Filter = FilterForm.GetFilter();
                return _Filter;
            }
            set { _Filter = value; }
        }

        private ILoadFilterForm _FilterForm = null;
        public virtual ILoadFilterForm FilterForm
        {
            get { return _FilterForm; }
            set
            {
                _FilterForm = value as ILoadFilterForm;
                if (_FilterForm != null)
                {
                    _FilterForm.CloseApply += new EventHandler(filter_CloseApply);
                }
            }
        }

        public ToolStripButton ButtonFilter;
        private ToolStripButton ButtonReset;

        void filter_CloseApply(object sender, EventArgs e)
        {
            if (FilterForm == null)
                return;
            BaseLoadFilter flt = FilterForm.GetFilter();
            if (flt != Filter)
            {
                Filter = flt;
                ButtonFilter.ToolTipText = flt.ToString();
                FireFilterChanged(flt);
            }
        }

        public event EventHandler<FilterEventsArgs2> FilterChanged = null;
        void FireFilterChanged(BaseLoadFilter filter)
        {
            if (FilterChanged != null)
                FilterChanged(this, new FilterEventsArgs2(filter));
        }

        void ButtonFilter_Click(object sender, EventArgs e)
        {
            //if (FilterForm != null)
            //{
            //ToolStripItem tsi = sender as ToolStripItem;
            //ShowFilterForm(tsi);
            //}
            ShowFilterForm();
        }

        public virtual void ShowFilterForm()
        {
            if (ButtonFilter != null)
            {
                if (ButtonFilter.Owner != null)
                    ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.WaitCursor;
                try
                {
                    int left = ButtonFilter.Bounds.Left;
                    int top = ButtonFilter.Bounds.Bottom;
                    Form frm = FilterForm as Form;
                    if (frm != null)
                    {
                        frm.StartPosition = FormStartPosition.Manual;
                        Point p = ButtonFilter.Owner.PointToScreen(new Point(left, top));
                        frm.Location = p;
                        FilterForm.Show();
                    }
                }
                finally
                {
                    if (ButtonFilter.Owner != null)
                        ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.Default;
                }
            }
        }
        void ButtonReset_Click(object sender, EventArgs e)
        {
            if (_FilterForm != null)
            {
                if (ButtonFilter != null && ButtonFilter.Owner != null)
                    ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.WaitCursor;
                try
                {
                    BaseLoadFilter flt = _FilterForm.GetFilter();
                    flt.ResetToDefault();
                    _FilterForm.Init(flt);
                    FireFilterChanged(flt);
                }
                finally
                {
                    if (ButtonFilter != null && ButtonFilter.Owner != null)
                        ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.Default;
                }
            }
        }
    }
    public class FilterController<FF> : FilterController
        where FF : FilterFormBase
    {
        public override ILoadFilterForm FilterForm
        {
            get
            {
                if (base.FilterForm == null)
                {
                    if (ButtonFilter != null && ButtonFilter.Owner != null)
                        ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.WaitCursor;
                    try
                    {
                        base.FilterForm = Activator.CreateInstance<FF>();
                        FilterForm.Init(Filter);
                    }
                    finally
                    {
                        if (ButtonFilter != null && ButtonFilter.Owner != null)
                            ButtonFilter.Owner.TopLevelControl.Cursor = Cursors.Default;
                    }
                }
                return base.FilterForm;
            }
            set
            {
                base.FilterForm = value;
            }
        }
    }
}