using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Фильтр для временного промежутка
    /// </summary>
    public class FilterDateFromTo : FilterColumnBase
    {
        #region Constructors
        public FilterDateFromTo(string column, DateTime dateFrom, DateTime dateTo)
            : this("", column, dateFrom, dateTo) { }

        public FilterDateFromTo(string table, string column, DateTime dateFrom, DateTime dateTo) 
            : base(table, column) 
        {
            _dtFrom = dateFrom;
            _dtTo = dateTo;
        }
        #endregion

        #region Fields
        private DateTime _dtFrom;

        public DateTime DtFrom
        {
            get { return _dtFrom; }
            set { _dtFrom = value; }
        }
        private DateTime _dtTo;

        public DateTime DtTo
        {
            get { return _dtTo; }
            set { _dtTo = value; }
        }
        #endregion

        public override string ToString()
        {
            string sDateFrom = Filter.toSQL(_dtFrom);
            string sDateTo = Filter.toSQL(_dtTo);

            if (_dtFrom != DateTime.MinValue && _dtTo != DateTime.MinValue && _dtTo != DateTime.MinValue.AddDays(1))
                return string.Format("({0} BETWEEN {1} AND {2})", Column, sDateFrom, sDateTo);
            else if (_dtFrom != DateTime.MinValue)
                return string.Format("({0} >= {1})", Column, sDateFrom);
            else if (_dtTo != DateTime.MinValue && _dtTo != DateTime.MinValue.AddDays(1))
                return string.Format("({0} <= {1})", Column, sDateTo);
            else 
                return "";
        }

        public void Reset()
        {
            _dtFrom = DateTime.MinValue;
            _dtTo = DateTime.MinValue;
        }
    }
}