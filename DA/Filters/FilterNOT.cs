using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Класс принимает фильтпр и вставляем перед ним NOT
    /// </summary>
    public class FilterNOT : FilterUnitBase
    {
        FilterUnitBase _filter;
        public FilterNOT(FilterUnitBase flt)
        {
            _filter = flt;
        }
        public override string ToString()
        {
            if(_filter == null) return "";
            string sflt = _filter.ToString();
            if (sflt.Length == 0) return "";
            return "(NOT " + sflt + ")";
        }
    }
}
