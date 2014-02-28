using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DA
{
    /// <summary>
    /// Класс формирующий фильтр для Card
    /// </summary>
    public class FilterTree : FilterColumnBase
    {
        #region Constructors
        public FilterTree(string parent, bool IsFull) : this("", "Parent_FP", parent, IsFull) { }
        public FilterTree(PathTree parent, bool IsFull) : this("", "Parent_FP", parent, IsFull) { }
        public FilterTree(string table, string parent, bool IsFull) : this(table, "Parent_FP", parent, IsFull) { }
        public FilterTree(string table, PathTree parent, bool IsFull) : this(table, "Parent_FP", parent, IsFull) { }
        public FilterTree(string table, string columnName, object value, bool IsFull)
            : base(table, columnName)
        {
            _value = value;
            _IsFull = IsFull;
        }
        #endregion

        #region Fields
        private bool _IsFull = false;
        private object _value;
        #endregion
        public override string ToString()
        {
            string fp = "";
            if (_value is string && PathTree.IsPathTree((string)_value))
                fp = (string)_value;
            else if (_value is PathTree)
                fp = ((PathTree)_value).ToString();
            string ret = null;

            if (_value == null)
                ret = string.Format("({0} is null)"
                    , Column
                    );
            else if (string.IsNullOrEmpty(_value.ToString()))
                return "";
            else

                ret = string.Format("({0} = '{1}')"
                    , Column
                    , fp
                    );

            if (_IsFull && !string.IsNullOrEmpty(fp))
            {
                ret += string.Format(" OR ({0} like '{1}.%')"
                , Column
                , fp
                );
                ret = string.Format("({0})", ret);
            }

            return ret;
        }
//        public void Reset()
//        {
//            _value = null; //String.Empty;
////            _isLike = false;
//        }
    }
}
