using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    public class SqlLoadException : Exception
    {
        public SqlLoadException(string sql,string message) : base(message)
        {
            _sql = sql;
        }
        
        private string _sql;
        public string Sql
        {
            get { return _sql; }
            set { _sql = value; }
        }


    }
}
