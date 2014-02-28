using System;
using System.Collections.Generic;
using System.Text;
using DA;
using BO;

namespace BOF
{
    public class FilterEventsArgs2 : EventArgs
    {
        public FilterEventsArgs2(BaseLoadFilter filter)
        {
            Filter = filter;
        }
        private BaseLoadFilter _filter;
        public BaseLoadFilter Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

    }
}
