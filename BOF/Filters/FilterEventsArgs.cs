using System;
using System.Collections.Generic;
using System.Text;
using DA;

namespace BOF
{
    public class FilterEventsArgs : EventArgs
    {
        public FilterEventsArgs(IDAOFilter filter)
        {
            Filter = filter;
        }
        private IDAOFilter _filter;
        public IDAOFilter Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }

    }
}
