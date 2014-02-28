using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Класс принимает список фильтров и вставляем между ними AND
    /// </summary>
    public class FilterAND : FilterUnitBase
    {
        public FilterAND(params FilterUnitBase[] andList)
        {
            AddAND(andList);
        }

        private List<FilterUnitBase> _AndList = null;

        public List<FilterUnitBase> AndList
        {
            get { return _AndList; }
            set { _AndList = value; }
        }
        public void AddAND(params FilterUnitBase[] andList)
        {
            if (_AndList == null && andList.Length > 0)
                _AndList = new List<FilterUnitBase>();
            foreach (FilterUnitBase and in andList)
            {
                //предлагается тут проверять каждый and на null. если null - не добавлять
                _AndList.Add(and);
            }
        }

        public override string ToString()
        {
            if (_AndList == null) return "";
            string prefix = "(";
            StringBuilder sb = new StringBuilder();
            foreach (FilterUnitBase and in _AndList)
            {
                string sflt = and.ToString();
                if (sflt.Length > 0)
                {
                    sb.AppendFormat("{0}{1}", prefix, sflt);
                    prefix = " AND ";
                }
            }
            if (sb.ToString().Length > 0) sb.Append(")");
            return sb.ToString();
        }
    }
}
