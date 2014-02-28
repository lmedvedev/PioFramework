using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DA
{
    /// <summary>
    /// Фильтр по ID или списку ID
    /// </summary>
    /// 
    public class FilterGUID : FilterColumnBase
    { 
        #region Constructors
        public FilterGUID(string column) : base(column) { }
        public FilterGUID(string table, string column) : base(table, column) { }

        //public FilterGUID() : this("", "id") { }

        //public FilterGUID(Guid id) : this("", "id", id) { }
        public FilterGUID(string column, Guid id) : this("", column, id) { }
        public FilterGUID(string table, string column, Guid id)
            : this(table, column)
        {
            AddID(id);
        }
        public FilterGUID(DataTable findtable, string findcolumn)
            : this("", "id", findtable, findcolumn) { }
        public FilterGUID(string fltcolumn, DataTable findtable, string findcolumn)
            : this("", fltcolumn, findtable, findcolumn) { }
        public FilterGUID(string flttable, string fltcolumn, DataTable findtable, string findcolumn)
            : base(flttable, fltcolumn)
        {
            AddIDRange(findtable, findcolumn);
        }
        #endregion

        #region Fields
        List<Guid> _IDList = new List<Guid>();

        public List<Guid> IDList
        {
            get { return _IDList; }
            set { _IDList = value; }
        }
        #endregion

        public int IdCount
        {
            get { return _IDList.Count; }
        }
        public void AddID(Guid id)
        {
                _IDList.Add(id);
        }
        public void AddIDRange(DataTable table, string column)
        {
            AddID(Guid.Empty);
            int index = table.Columns[column].Ordinal;
            foreach (DataRow row in table.Rows)
            {
                object val = row[index];
                if (val is Guid)
                    AddID((Guid)val);
            }
        }
        public void AddIDRange(List<Guid> idList)
        {
            foreach (Guid id in idList)
            {
                AddID(id);
            }
        }
        public void RemoveID(Guid id)
        {
            if (_IDList.Contains(id))
            {
                //_IDList.Remove(id);
                _IDList.RemoveAll(delegate(Guid id1) { return (id1 == id); });
            }
        }
        public bool Contain(Guid id)
        {
            return (_IDList.BinarySearch(id) >= 0);
            
        }

        public override string ToString()
        {
            if (_IDList.Count == 0)
                return "";

            if (_IDList.Count == 1)
                return string.Format("({0}={1})", Column, Filter.toSQL(_IDList[0]));
            else
            {
                _IDList.Sort();
                string prefix = "(" + Column + " in (";
                StringBuilder sb = new StringBuilder();
                Guid prev = Guid.Empty;
                foreach (Guid id in _IDList)
                {
                    if (id != prev)
                    {
                        sb.AppendFormat("{0}{1}", prefix, Filter.toSQL(id));
                        prefix = ",";
                        prev = id;
                    }
                }
                sb.Append("))");
                return sb.ToString();
            }
        }
    }
    
    public class FilterID : FilterColumnBase
    {
        #region Constructors
        public FilterID(string column) : base(column) { }
        public FilterID(string table, string column) : base(table, column) { }

        public FilterID() : this("", "id") { }
        
        public FilterID(int id) : this("", "id", id) { }
        public FilterID(string column, int id) : this("", column, id) { }
        public FilterID(string table, string column, int id)
            : this(table, column)
        {
            AddID(id);
        }
        public FilterID(DataTable findtable, string findcolumn)
            : this("", "id", findtable, findcolumn) { }
        public FilterID(string fltcolumn, DataTable findtable, string findcolumn)
            : this("", fltcolumn, findtable, findcolumn) { }
        public FilterID(string flttable, string fltcolumn, DataTable findtable, string findcolumn)
            : base(flttable, fltcolumn)
        {
            AddIDRange(findtable, findcolumn);
        }
        #endregion

        #region Fields
        List<int> _IDList = new List<int>();

        public List<int> IDList
        {
            get { return _IDList; }
            set { _IDList = value; }
        }
        #endregion

        public int IdCount
        {
            get { return _IDList.Count; }
        }
        public void AddID(int id)
        {
                _IDList.Add(id);
        }
        public void AddIDRange(DataTable table, string column)
        {
            AddID(0);
            int index = table.Columns[column].Ordinal;
            foreach (DataRow row in table.Rows)
            {
                object val = row[index];
                if (val is int)
                    AddID((int)val);
            }
        }
        public void AddIDRange(List<int> idList)
        {
            foreach (int id in idList)
            {
                AddID(id);
            }
        }
        public void RemoveID(int id)
        {
            if (_IDList.Contains(id))
            {
                //_IDList.Remove(id);
                _IDList.RemoveAll(delegate(int id1){return (id1 == id);});
            }
        }
        public bool Contain(int id)
        {
            return (_IDList.BinarySearch(id) >= 0);
            
        }

        public override string ToString()
        {
            if (_IDList.Count == 0)
                return "";
            if (_IDList.Count == 1)
                return string.Format("({0}={1})", Column, _IDList[0]);
            else
            {
                _IDList.Sort();
                string prefix = "(" + Column + " in (";
                StringBuilder sb = new StringBuilder();
                int prev = -1;
                foreach (int id in _IDList)
                {
                    if (id != prev)
                    {
                        sb.AppendFormat("{0}{1}", prefix, id);
                        prefix = ",";
                        prev = id;
                    }
                }
                sb.Append("))");
                return sb.ToString();
            }
        }
    }
}
