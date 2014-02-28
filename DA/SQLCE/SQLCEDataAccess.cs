using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Xml;


namespace DA
{
    public class SQLCEDataAccess : IDataAccess
    {
        private SqlCeConnection _dbConnection = null;
        private SqlCeTransaction _dbTransaction = null;

        public SQLCEDataAccess(string filename)
            : base()
        {
            string ConnString = @"Data Source='" + filename + "'";
            if (!File.Exists(filename))
                new SqlCeEngine(ConnString).CreateDatabase();
            _dbConnection = new SqlCeConnection(ConnString);
            Global.AddDAO(this);
            ConnectionOpen();
        }

        public string GetKey()
        {
            return _dbConnection.DataSource + ":" + _dbConnection.Database;
        }
        private int _TimeOut = 120;
        public int TimeOut
        {
            get
            {
                return _TimeOut;
            }
            set
            {
                _TimeOut = value;
            }
        }

        public bool ConnectionOpen()
        {
            bool isOpen = (_dbConnection.State == ConnectionState.Open);
            if (!isOpen)
                _dbConnection.Open();
            return isOpen;
        }
        public void ConnectionClose()
        {
            ConnectionClose(false);
        }
        public void ConnectionClose(bool wasOpen)
        {
            if (wasOpen) return;
            if (_dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }

        public void TransactionBegin()
        {
            try
            {
                if (_dbTransaction == null)
                    _dbTransaction = _dbConnection.BeginTransaction();
            }
            catch (System.Exception e)
            {
                _dbTransaction = null;
                throw new Exception("Ошибка при открытии транзакции: " + e.Message);
            }
        }
        public int TransactionCommit()
        {
            if (_dbTransaction != null)
                _dbTransaction.Commit();
            _dbTransaction = null;
            return 0;
        }
        public int TransactionRollback()
        {
            if (_dbTransaction != null)
                _dbTransaction.Rollback();
            _dbTransaction = null;
            return 0;
        }

        public DbDataReader ReaderOpen(out bool wasOpen, string sql)
        {
            try
            {
                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                wasOpen = ConnectionOpen();
                cmd.Connection = _dbConnection;
                cmd.Transaction = _dbTransaction;
                return cmd.ExecuteReader();
            }
            catch (System.Exception e) { throw e; }
        }
        public void ReaderClose(DbDataReader reader, bool wasOpen)
        {
            if (reader != null && !reader.IsClosed)
            {
                try { reader.Close(); }
                finally { ConnectionClose(wasOpen); }
            }
        }
        string createSQL(IDAOFilter filter, string tableName)
        {
            return createSQL(filter, tableName, null);
        }
        string createSQL(IDAOFilter filter, string tableName, List<string> fields)
        {
            string parameters = "";
            string fltstring = "";
            string topdistinct = "";
            if (filter != null)
            {
                fltstring = filter.ToString();
                parameters = filter.ParameterString();

                if (filter.Distinct)
                    topdistinct += "distinct";

                if (filter.Top > 0)
                    topdistinct += " top " + filter.Top.ToString();
            }

            if (fields == null)
                fields = new List<string>();

            //fields.Add("id");
            //fields.Add("orderdate");

            if (fields.Count == 0)
                fields.Add("*");

            string sql = string.Format("select {0} {1} from [{2}]{3} {4}"
                , topdistinct
                , string.Join(",", fields.ConvertAll<string>(delegate(string o) { return "[" + tableName + "]." + o; }).ToArray())
                , tableName
                , parameters
                , fltstring
                );
            return sql;
        }

        public List<object[]> LoadList(string select)
        {
            return LoadList<object[]>(select, delegate(DbDataReader rd)
            {
                object[] vals = new object[rd.FieldCount];
                rd.GetValues(vals);
                return vals;
            });
        }
        public List<TOut> LoadList<TOut>(string select, Converter<DbDataReader, TOut> method)
        {
            bool wasOpen = false;
            DbDataReader reader = null;
            List<TOut> result = new List<TOut>();
            try
            {
                reader = ReaderOpen(out wasOpen, select);
                while (reader.Read())
                {
                    result.Add(method(reader));
                }
                return result;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                ReaderClose(reader, wasOpen);
            }
        }

        public void LoadSet(IDAOFilter filter, LoadMembersDelegate method, string tableName)
        {
            LoadSet(filter, method, tableName, null);
        }
        public void LoadSet(IDAOFilter filter, LoadMembersDelegate method, string tableName, List<string> fieldsList)
        {
            bool wasOpen = false;
            DbDataReader reader = null;
            try
            {
                reader = ReaderOpen(out wasOpen, createSQL(filter, tableName, fieldsList));
                while (reader.Read())
                {
                    object[] vals = new object[reader.FieldCount];
                    reader.GetValues(vals);
                    method(vals);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                ReaderClose(reader, wasOpen);
            }
        }

        public int Insert(string tableName, Dictionary<string, object> parameters)
        {
            KeyValuePair<string, object> id = parameters.FirstOrDefault(k => k.Key.ToLower() == "id");
            if (id.Key != null)
                parameters.Remove(id.Key);
            string query = "INSERT INTO " + tableName + " (" + string.Join(",", parameters.Keys.ToArray()) + ") VALUES (@" + string.Join(",@", parameters.Keys.ToArray()) + ")";
            if (Execute(query, parameters) > 1)
                throw new System.Exception("Попытка множественного добавления записей");
            bool wasOpen = false;
            DbDataReader reader = null;
            try
            {
                reader = ReaderOpen(out wasOpen, "SELECT @@IDENTITY;");
                reader.Read();
                return (int)reader.GetDecimal(0);
            }
            catch (Exception exp)
            {
                throw new System.Exception("Невозможно определить ID добавленной записи:\n" + exp.Message);
            }
            finally
            {
                ReaderClose(reader, wasOpen);
            }
        }

        public void Update(string tableName, Dictionary<string, object> parameters)
        {
            KeyValuePair<string, object> id = parameters.FirstOrDefault(k => k.Key.ToLower() == "id");
            if (id.Key != null)
                parameters.Remove(id.Key);
            string query = "UPDATE " + tableName + " SET " + parameters.Keys.Aggregate<string, string>("", (res, elem) => res + (string.IsNullOrEmpty(res) ? "" : ",") + elem + " = @" + elem) + " WHERE ID=" + id.Value.ToString();
            if (Execute(query, parameters) > 1)
                throw new System.Exception("Попытка множественного редактирования записей");
        }

        public void Delete(string tableName, int id)
        {
            if (Execute(string.Format("DELETE FROM {0} WHERE ID = {1}", tableName, id), null) > 1)
                throw new System.Exception("Попытка множественного удаления записей");
        }

        public void Execute(string query)
        {
            Execute(query, null);
        }

        public int Execute(string command, Dictionary<string, object> parameters)
        {
            bool wasOpen = false;
            try
            {
                SqlCeCommand cmd = new SqlCeCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = command;
                if (parameters != null && parameters.Count > 0)
                    cmd.Parameters.AddRange(parameters.Select(k => new SqlCeParameter
                    {
                        ParameterName = "@" + k.Key,
                        IsNullable = true,
                        Value = (k.Value == null) ? DBNull.Value : k.Value,
                        DbType = GetDBType(k.Value),
                        Direction = ParameterDirection.Input
                    }).ToArray());
                wasOpen = ConnectionOpen();
                cmd.Connection = _dbConnection;
                cmd.Transaction = _dbTransaction;
                return cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                if (exp.Message.Contains("unique index"))
                    throw new Exception("Произошло нарушение уникальности ключевого поля. Элемент с таким значением уже присутствует.");
                else if (exp.Message.Contains("cannot contain null"))
                    throw new Exception("Не заполнено обязательное поле.");
                else if (exp.Message.Contains("primary key value cannot be deleted"))
                    throw new Exception("Необходимо сначала удалить элементы, ссылающиеся на этот элемент.");
                else
                    throw exp;
            }
            finally { ConnectionClose(wasOpen); }
        }

        public DbType GetDBType(object val)
        {
            if (val is string)
                return DbType.String;
            else if (val is int)
                return DbType.Int32;
            else if (val is decimal)
                return DbType.Double;
            else
                return DbType.Binary;
        }

        public IDAOFilter NewFilter()
        {
            SQLCEFilter flt = new SQLCEFilter();
            FilterUnitBase.Filter = flt;
            return flt;
        }

        #region IDataAccess Members

        public object ExecProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public string SubStringFunction()
        {
            throw new NotImplementedException();
        }

        public object[] LoadDat(IDAOFilter filter, string tableName)
        {
            throw new NotImplementedException();
        }

        public void LogRequest(bool writelogs)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
