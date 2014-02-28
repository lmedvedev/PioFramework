using System;
using System.Collections.Generic;
using System.Text;
using BO;

namespace DA
{
    /// <summary>
    /// Класс формирующий фильтр для Card
    /// </summary>
    public class FilterCard : FilterColumnBase
    {
        #region Constructors
        public FilterCard(string parent) : this("", "Parent_FP", "Code", parent) { }
        public FilterCard(PathCard card) : this("", "Parent_FP", "Code", card) { }
        public FilterCard(PathTree parent) : this("", "Parent_FP", "Code", parent) { }
        public FilterCard(string table, string parent) : this(table, "Parent_FP", "Code", parent) { }
        public FilterCard(string table, PathCard card) : this(table, "Parent_FP", "Code", card) { }
        public FilterCard(string table, PathTree parent) : this(table, "Parent_FP", "Code", parent) { }
        protected FilterCard(string table, string columnTree, string columnCard, object value)
            : base("")
        {
            _value = value;
            _columnTree = columnTree;
            _columnCard = columnCard;
            Table = table;
        }
        #endregion

        #region Fields
        private object _value;
        private string _columnTree;
        private string _columnCard;
        #endregion
        public override string ToString()
        {
            if (_value is PathCard)
            {
                return fmtCard((PathCard)_value);
            }
            else if (_value is PathTree)
            {
                FilterTree fltTree = new FilterTree(Table, _columnTree, (PathTree)_value,false);
                return fltTree.ToString();
            }
            else if (_value is string)
            {
                string sv = (string)_value;
                if (PathCard.IsPathCard(sv))
                {
                    return fmtCard(new PathCard(sv));
                }
                else if (PathTree.IsPathTree(sv))
                {
                    FilterTree fltTree = new FilterTree(Table, _columnTree, sv,false);
                    return fltTree.ToString();
                }
            }
            throw new ArgumentException("FilterCard - ошибка в значении " + _value.ToString());
        }
        private string fmtCard(PathCard card)
        {
            return string.Format("({0}{1} = '{3}' and {0}{2} = {4}) "
                , string.IsNullOrEmpty(Table) ? "" : Table + "."
                , _columnTree
                , _columnCard
                , card.Parent.ToString()
                , card.Code
                );
        }
    }
}
