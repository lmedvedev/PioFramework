using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Базовый фильтр для BOOL
    /// </summary>
    public class FilterBool : FilterColumnBase
    {
       
        #region Constructors
        public FilterBool(string column, bool? istrue)
            : this("", column, istrue) { }

        public FilterBool(string table, string column, bool? istrue)
            : base(table, column)
        {
            _isTrue = istrue;
        }

        public FilterBool(string table, string column)
            : base(table, column)
        { }

        public void Reset()
        {
            _isTrue = null;
        }

        #endregion

        #region Fields
        private bool? _isTrue;

        public bool? IsTrue
        {
            get { return _isTrue; }
            set { _isTrue = value; }
        }
        #endregion

        public override string ToString()
        {
            if (_isTrue.HasValue)
                return string.Format("({0}={1})", Column, Filter.toSQL(_isTrue));
            
            return string.Empty;
        }
    }
}
