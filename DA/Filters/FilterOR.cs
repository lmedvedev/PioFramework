using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Класс принимает список фильтров и вставляем между ними OR
    /// </summary>
    public class FilterOR : FilterUnitBase
    {
        public FilterOR(params FilterUnitBase[] andList)
        {
            AddOR(andList);
        }

        public void AddOR(params FilterUnitBase[] orList)
        {
            if (_OrList == null && orList.Length > 0)
                _OrList = new List<FilterUnitBase>();
            foreach(FilterUnitBase or in orList)
                _OrList.Add(or);
        }

        private List<FilterUnitBase> _OrList = null;

        public override string ToString()
        {
            if (_OrList == null) return "";
            string prefix = "(";
            StringBuilder sb = new StringBuilder();
            foreach (FilterUnitBase or in _OrList)
            {
                string sflt = or.ToString();
                if (sflt.Length > 0)
                {
                    sb.AppendFormat("{0}{1}", prefix, sflt);
                    prefix = " OR ";
                }
            }
            if (sb.ToString().Length > 0) sb.Append(")");
            return sb.ToString();
        }
    }
}
