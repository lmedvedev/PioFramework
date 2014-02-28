using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DA
{
    /// <summary>
    /// Фильтр заточенный под SQL Server
    /// </summary>
    public class SQLFilter : IDAOFilter
    {
        #region Fields
        /// <summary>
        /// Список Orders
        /// </summary>
        List<ColumnOrder> _OrderList = null;
        
        /// <summary>
        /// Список фильтров
        /// </summary>
        List<FilterUnitBase> _WhereList = null;

        /// <summary>
        /// Список Параметров для функций
        /// </summary>
        SortedList<int, object> _ParameterList = null;

        List<ColumnJoin> _JoinList = null;
        List<FunctionApply> _ApplyList = null;

        int _Top = 0;
        bool _Distinct = false;

        private string _From;

        public string From
        {
            get { return _From; }
            set { _From = value; }
        }


        #endregion

        #region IDAOFilter Members

        /// <summary>
        /// Очистить
        /// </summary>
        public void Reset()
        {
            _WhereList = null;
            _JoinList = null;
            _OrderList = null;
        }

        public bool IsEmpty()
        {
            return (_JoinList == null || _JoinList.Count == 0)
                && (_WhereList == null || _WhereList.Count == 0)
                && (_ApplyList == null || _ApplyList.Count == 0)
                ;
        }

        public void Merge(IDAOFilter flt)
        {
            foreach (FilterUnitBase fl in flt.WhereList)
            {
                this.AddWhere(fl);
            }
            foreach (int key in flt.ParameterList.Keys)
            {
                this.AddParameters(key,flt.ParameterList[key]);
            }
            foreach (ColumnJoin join in flt.JoinList)
            {
                this.AddJoin(join);
            }
            foreach (FunctionApply apply in flt.ApplyList)
            {
                this.AddApply(apply);
            }
            foreach (ColumnOrder order in flt.OrderList)
            {
                this.AddOrder(order);
            }
        }

        /// <summary>
        /// Добавить фильтр
        /// </summary>
        /// <param name="where"> Фильтр </param>
        public void AddWhere(FilterUnitBase where)
        {
            if (_WhereList == null)
                _WhereList = new List<FilterUnitBase>();
            _WhereList.Add(where);
        }
        public void RemoveWhere(FilterUnitBase flt)
        {
            _WhereList.Remove(flt);
        }

        public void AddTopDistinct(int top, bool distinct)
        {
            _Top = top;
            _Distinct = distinct;
        }

        public void AddParameters(int num, object val)
        {
            if (_ParameterList == null)
                _ParameterList = new SortedList<int, object>();
            _ParameterList.Add(num, val);
        }
        public void AddParameterID(int num, int id)
        {
            if (id <= 0) 
                AddParameters(num, null);
            else
                AddParameters(num, id);
        }

        /// <summary>
        /// Добавить Order
        /// </summary>
        /// <param name="order">Order</param>
        public void AddOrder(ColumnOrder order)
        {
            if (_OrderList == null)
                _OrderList = new List<ColumnOrder>();
            _OrderList.Add(order);
        }

        public void AddJoin(ColumnJoin join)
        {
            if (_JoinList == null)
                _JoinList = new List<ColumnJoin>();
            _JoinList.Add(join);
        }

        public void AddApply(FunctionApply apply)
        {
            if (_ApplyList == null)
                _ApplyList = new List<FunctionApply>();
            _ApplyList.Add(apply);
        }

        public void AddJoin(ColumnJoin[] joins) 
        {
            foreach (ColumnJoin join in joins)
                AddJoin(join);
        }

        public List<ColumnOrder> OrderList { get { return _OrderList; } }

        public List<FilterUnitBase> WhereList { get { return _WhereList; } }

        public List<ColumnJoin> JoinList { get { return _JoinList; } }
        public List<FunctionApply> ApplyList { get { return _ApplyList; } }

        public SortedList<int, object> ParameterList { get { return _ParameterList; } }
        public int Top { get { return _Top; } }
        public bool Distinct { get { return _Distinct; } }


        /// <summary>
        /// Формирование Where части запроса
        /// </summary>
        /// <returns>строка Where</returns>
        public string WhereString()
        {
            if (_WhereList == null) return "";
            StringBuilder where = new StringBuilder();
            string prefix = "WHERE ";
            foreach (FilterUnitBase flt in _WhereList)
            {
                if (flt != null)
                {
                    string sflt = flt.ToString();
                    if (sflt.Length > 0)
                    {
                        where.AppendFormat("{0} {1}", prefix, sflt);
                        prefix = " AND ";
                    }
                }
            }
            return where.ToString();
        }

        public string ParameterString()
        {
            if (_ParameterList == null) return "";
            StringBuilder prm = new StringBuilder();
            string prefix = "(";
            foreach (object val in _ParameterList.Values)
            {
                prm.AppendFormat("{0} {1}", prefix, toSQL(val));
                prefix = ", ";
            }
            return prm.ToString() + " )";

        }
        public virtual string toSQL(object val) 
        {
            if (val == null) return "null";
            if (val is string) return "'" + val + "'";
            if (val is DateTime)
            {
                DateTime dt = (DateTime)val;
                if (dt == DateTime.MinValue) 
                    return "null";
                else
                    return "'" + dt.ToString("yyyyMMdd") + "'";
            }
            if (val is bool)
            {
                bool v = (bool)val;
                if (v)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            if (val is Guid)
            {
                return string.Format("'{0:D}'", val);
            }
            if (val is XmlDocument) return "'" + ((XmlDocument)val).InnerXml + "'";
            else return val.ToString();
        }

        /// <summary>
        /// Формирование Order части запроса
        /// </summary>
        /// <returns>строка Oreder</returns>
        public string OrderString()
        {
            if (_OrderList == null) return "";
            StringBuilder sb = new StringBuilder("");
            if (_OrderList.Count > 0)
            {
                string prefix = " ORDER BY ";
                foreach (ColumnOrder co in _OrderList)
                {
                    sb.AppendFormat("{0}{1}", prefix, co);
                    prefix = ",";
                }
            }
            return sb.ToString();
        }
        public string JoinString()
        {
            if (_JoinList == null) return "";
            StringBuilder sb = new StringBuilder("");
            if (_JoinList.Count > 0)
            {
                foreach (ColumnJoin co in _JoinList)
                {
                    sb.AppendFormat(" {0} ", co);
                }
            }
            return sb.ToString();
        }

        public string ApplyString()
        {
            if (_ApplyList == null) return "";
            StringBuilder sb = new StringBuilder("");
            if (_ApplyList.Count > 0)
            {
                foreach (FunctionApply co in _ApplyList)
                {
                    sb.AppendFormat(" {0} ", co);
                }
            }
            return sb.ToString();
        }
        
        /// <summary>
        /// To String
        /// </summary>
        /// <returns>строковое представление запроса для фильтров</returns>
        public override string ToString()
        {
            return JoinString() + ApplyString() + WhereString() + " " + OrderString();
        }
        #endregion






    }
}
