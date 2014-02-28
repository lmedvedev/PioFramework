using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// »нтерфейс дл€ фильтров Dat и Set классов
    /// </summary>
    public interface IDAOFilter
    {
        void Reset();

        void Merge(IDAOFilter flt);

        void AddParameters(int num, object val);
        void AddParameterID(int num, int id);
        void AddWhere(FilterUnitBase flt);
        void RemoveWhere(FilterUnitBase flt);
        void AddOrder(ColumnOrder order);
        void AddJoin(ColumnJoin join);
        void AddJoin(ColumnJoin[] joins);
        void AddApply(FunctionApply apply);
        void AddTopDistinct(int top, bool distinct);

        List<ColumnOrder> OrderList { get;}

        List<FilterUnitBase> WhereList { get;}

        SortedList<int, object> ParameterList { get;}

        List<ColumnJoin> JoinList { get;}
        List<FunctionApply> ApplyList { get;}

        int Top { get;}
        bool Distinct { get;}
        bool IsEmpty();
        string ParameterString();
        string WhereString();
        string OrderString();
        string JoinString();
        string ToString();
        string toSQL(object val);

        string From
        {
            get;
            set;
        }
    }

    #region Order by
    public enum OrderType { ASC, DESC };
    /// <summary>
    ///  ласс представл€ющий колонки дл€ сортировки
    /// </summary>
    public class ColumnOrder
    {
        private string _table;
        private string _column;
        private OrderType _ordertype;
        public ColumnOrder(string column) : this("", column, OrderType.ASC) { }
        public ColumnOrder(string column, OrderType ordertype) : this("", column, ordertype) { }
        public ColumnOrder(string table, string column, OrderType ordertype)
        {
            _table = table;
            _column = column;
            _ordertype = ordertype;
        }
        public override string ToString()
        {
            return string.Format("{0} {1}", FilterUnitBase.colName(_table, _column), _ordertype);
        }
    }
    #endregion

    #region Joins and Apply
    public enum ApplyType { OUTER, CROSS };
    public enum JoinType { INNER, LEFT, RIGHT };
    public enum XmlFunction { query, value, exist, nodes };

    public class FunctionApply
    {
        public FunctionApply(string applystring)
        {
            _applystring = applystring;
        }


        //public FunctionApply(string jointable, string joincolumn, string basetable, string basecolumn)
        //    : this(ApplyType.CROSS, jointable, joincolumn, basetable, basecolumn) { }

        //public FunctionApply(ApplyType applytype, string jointable, string joincolumn, string basetable, string basecolumn)
        //{
        //    _applystring = string.Format("{0} JOIN {1} ON [{1}].[{2}] = [{3}].[{4}]"
        //        , getApplyType(applytype)
        //        , jointable
        //        , joincolumn
        //        , basetable
        //        , basecolumn
        //        );
        //}
        //public FunctionApply(string joinname, string jointable, string joincolumn, string basetable, string basecolumn)
        //    : this(ApplyType.CROSS, joinname, jointable, joincolumn, basetable, basecolumn) { }
        public FunctionApply(ApplyType applyType, string applynameTable, string applynameColumn, XmlFunction xmlFunction, string xmlXPath, string xmlColumn)
        {
            _applystring = string.Format("{0} APPLY {1}.{2}('{3}') as {4}({5})"
                , getApplyType(applyType)
                , xmlColumn
                , xmlFunction
                , xmlXPath
                , applynameTable
                , applynameColumn
                );
        }

        public FunctionApply(ApplyType applytype, string applyname, string applyfunction, string basetable, string basecolumn)
        {
            _applystring = string.Format("{0} APPLY {1}([{2}].[{3}]) as {4}"
                , getApplyType(applytype)
                , applyfunction
                , basetable
                , basecolumn
                , applyname
                );
        }
        private string _applystring;

        string getApplyType(ApplyType applytype)
        {
            switch (applytype)
            {
                case ApplyType.CROSS: return "CROSS";
                case ApplyType.OUTER: return "OUTER";
            }
            return "";
        }

        public override string ToString()
        {
            return _applystring;
        }

    }
    public class ColumnJoin
    {
        public ColumnJoin(string joinstring)
        {
            _joinstring = joinstring;
        }


        public ColumnJoin(string jointable, string joincolumn, string basetable, string basecolumn)
            : this(JoinType.INNER, jointable, joincolumn, basetable, basecolumn) { }

        public ColumnJoin(JoinType jointype, string jointable, string joincolumn, string basetable, string basecolumn)
        {
            _joinstring = string.Format("{0} JOIN {1} ON [{1}].[{2}] = [{3}].[{4}]"
                , getJoinType(jointype)
                , jointable
                , joincolumn
                , basetable
                , basecolumn
                );
        }
        public ColumnJoin(string joinname, string jointable, string joincolumn, string basetable, string basecolumn)
            : this(JoinType.INNER, joinname, jointable, joincolumn, basetable, basecolumn) { }
        public ColumnJoin(JoinType jointype, string joinname, string jointable, string joincolumn, string basetable, string basecolumn)
        {
            _joinstring = string.Format("{0} JOIN {1} {2} ON [{2}].[{3}] = [{4}].[{5}]"
                , getJoinType(jointype)
                , jointable
                , joinname
                , joincolumn
                , basetable
                , basecolumn
                );
        }
        private string _joinstring;

        string getJoinType(JoinType jointype)
        {
            switch (jointype)
            {
                case JoinType.INNER: return "INNER";
                case JoinType.LEFT: return "LEFT OUTER";
                case JoinType.RIGHT: return "RIGHT OUTER";
            }
            return "";
        }

        public override string ToString()
        {
            return _joinstring;
        }

    }
    #endregion
}

