using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    public class FilterBoolTrue : FilterBool
    {
        #region Constructors
        public FilterBoolTrue(string column)
            : this("", column) { }
        public FilterBoolTrue(string table, string column)
            : base(table, column, true) { }
        #endregion
    }
}
