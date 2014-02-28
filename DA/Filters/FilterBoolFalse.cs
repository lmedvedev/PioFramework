using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    public class FilterBoolFalse : FilterBool
    {
        #region Constructors
        public FilterBoolFalse(string column)
            : this("", column) { }
        public FilterBoolFalse(string table, string column)
            : base(table, column, false) { }
        #endregion
    }
}
