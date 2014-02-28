using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    abstract public class FilterColumnBase : FilterUnitBase
    {
        #region Constructors
        public FilterColumnBase(string column) : this("", column) { }
        public FilterColumnBase(string table, string column)
        {
            _table = table;
            _column = column;
        }
        #endregion

        #region Fields
        private string _table;

        public string Table
        {
            get { return _table; }
            set { _table = value; }
        }
        private string _column;
        #endregion

        public string Column
        {
            get { return colName(_table, _column); }
            set { _column = value; }
        }
    }
}
