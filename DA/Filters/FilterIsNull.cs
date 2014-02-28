using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    public class FilterIsNull : FilterColumnBase
    {
        #region Constructors
        public FilterIsNull(string column) : base(column) { }
        public FilterIsNull(string table, string column) : base(table, column) { }
        #endregion

        public override string ToString()
        {
            return string.Format("({0} IS NULL)", Column);
        }
    }
}