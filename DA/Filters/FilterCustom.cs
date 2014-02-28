using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Custom фильтр (пиши что хочу)
    /// </summary>
    public class FilterCustom : FilterUnitBase
    {
        string _filter;
        public FilterCustom(string custom)
        {
            _filter = custom;
        }
        public override string ToString()
        {
            if (_filter.Length == 0) return "";
            return _filter;
        }
    }
}
