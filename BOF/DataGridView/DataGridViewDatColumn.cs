using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class DataGridViewDatColumn : DataGridViewColumn
    {
        public DataGridViewDatColumn()
            : base(new DataGridViewDatCell())
        {

        }
        public DataGridViewDatColumn(string format)
            : base(new DataGridViewDatCell(format))
        {
            _Format = format;
        }
        private BaseSet _SetClass;
        private string _Format = "";

        public BaseSet SetClass
        {
            get { return _SetClass; }
            set { _SetClass = value; }
        }

        public string Format
        {
            get { return _Format; }
            //set { _Format = value; }
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewDatCell)))
                {
                    throw new InvalidCastException("Must be a ChooserCell");
                }
                base.CellTemplate = value;
            }
        }

        //private Dictionary<string, string> _DropDownColumns = null;

        //public Dictionary<string, string> DropDownColumns
        //{
        //    get { return _DropDownColumns; }
        //    set { _DropDownColumns = value; }
        //}

        public event EventHandler<ChooserControlArgs> FormatDropDownEvent;
        public void FireFormatDropDownEvent(DataGridViewChooserControl ctl)
        {
            if (FormatDropDownEvent != null)
                FormatDropDownEvent(this, new ChooserControlArgs(ctl));
        }
        public class ChooserControlArgs : EventArgs
        {
            private DataGridViewChooserControl msg;

            public ChooserControlArgs(DataGridViewChooserControl chooser)
            {
                msg = chooser;
            }
            public DataGridViewChooserControl Message
            {
                get { return msg; }
                set { msg = value; }
            }
        }

    }
}
