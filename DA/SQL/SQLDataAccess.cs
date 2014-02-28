using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.Common;

namespace DA
{
    /// <summary>
    /// Класс выполняющий сохранение и загрузку данных на SQL2005
    /// </summary>
    public class SQLDataAccess : IDataAccess
    {
        private bool logRequest = false;
        public SQLDataAccess(string cstr)
            : base()
        {
            _dbConnection = new SqlConnection(cstr);
            LogRequest(false);
            //SetConnectionString(cstr);
            Global.AddDAO(this);
        }

        public void LogRequest(bool writelogs)
        {
            logRequest = writelogs;
            if (writelogs)
                _dbConnection.StateChange += new StateChangeEventHandler(_dbConnection_StateChange);
            else
                _dbConnection.StateChange -= new StateChangeEventHandler(_dbConnection_StateChange);
        }

        public string SubStringFunction()
        {
            return "substring";
        }

        public int Insert(string tableName, Dictionary<string, object> parameters)
        {
            return (int)ExecProcedure(string.Format("sp_{0}_Insert", tableName), parameters);
        }

        public object ExecProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            bool wasOpen = false;
            try
            {
                SqlCommand cmd = getCommand(procedureName, ref wasOpen);

                SqlParameter p;
                foreach (string key in parameters.Keys)
                {
                    p = new SqlParameter();
                    p.ParameterName = "@" + key;
                    p.Direction = ParameterDirection.Input;
                    p.Value = parameters[key];
                    
                    //if(p.Value is XmlElement)
                    //    p.SqlDbType = SqlDbType.Xml;
                    cmd.Parameters.Add(p);
                }
#if DEBUG
                if (logRequest) LogQuery(cmd);
#endif
                cmd.ExecuteNonQuery();
                return cmd.Parameters["@RETURN_VALUE"].Value;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally { ConnectionClose(wasOpen); }
        }
        public void Execute(string query)
        {
            bool wasOpen = false;
            try
            {
                SqlCommand cmd = getSqlCommand(query, ref wasOpen);
                cmd.ExecuteNonQuery();
            }
            finally { ConnectionClose(wasOpen); }
        }

        private void LogQuery(SqlCommand cmd)
        {
            if (cmd != null)
            {
                StringBuilder ds = new StringBuilder();
                switch (cmd.CommandType)
                {
                    case CommandType.StoredProcedure:
                        ds.AppendFormat("exec {0}\r\n", cmd.CommandText);
                        break;
                    case CommandType.TableDirect:
                        ds.AppendFormat("{0}\r\n", cmd.CommandText);
                        break;
                    case CommandType.Text:
                        ds.AppendFormat("{0}\r\n", cmd.CommandText);
                        break;
                    default:
                        break;
                }
                if (cmd.Parameters != null)
                    foreach (SqlParameter par in cmd.Parameters)
                    {
                        switch (par.Direction)
                        {
                            case ParameterDirection.Input:
                                ds.AppendFormat("{0} = {1} /* INPUT */", par.ParameterName, PrintParameterValue(par));
                                break;
                            case ParameterDirection.InputOutput:
                                ds.AppendFormat("{0} = {1} /* INPUT/OUTPUT */", par.ParameterName, PrintParameterValue(par));
                                break;
                            case ParameterDirection.Output:
                                ds.AppendFormat("-- OUTPUT {0} = {1}", par.ParameterName, PrintParameterValue(par));
                                break;
                            case ParameterDirection.ReturnValue:
                                ds.AppendFormat("-- RETURN_VALUE {0} = {1}", par.ParameterName, PrintParameterValue(par));
                                break;
                            default:
                                break;
                        }
                        if (par != cmd.Parameters[cmd.Parameters.Count - 1])
                            ds.Append(",\r\n");
                    }
                Console.WriteLine(ds.ToString());
            }
        }
        private string PrintParameterValue(SqlParameter par)
        {
            if (par.Value == null)
                return "null";

            switch (par.SqlDbType)
            {
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                case SqlDbType.SmallInt:
                case SqlDbType.Timestamp:
                case SqlDbType.TinyInt:
                    return string.Format("{0}", par.SqlValue);

                case SqlDbType.Bit:
                    return string.Format("{0}", par.SqlValue);

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                    return string.Format("'{0:yyyyMMdd HH:mm:ss}'", par.Value);

                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Money:
                case SqlDbType.Real:
                case SqlDbType.SmallMoney:
                    return string.Format("{0:0.#}", par.SqlValue);

                case SqlDbType.NChar:
                case SqlDbType.NText:
                case SqlDbType.NVarChar:
                    return string.Format("N'{0}'", par.SqlValue);

                case SqlDbType.Text:
                case SqlDbType.Char:
                case SqlDbType.VarChar:
                case SqlDbType.Variant:
                case SqlDbType.Xml:
                    return string.Format("'{0}'", par.SqlValue);

                //case SqlDbType.Udt:
                //case SqlDbType.UniqueIdentifier:
                //case SqlDbType.VarBinary:
                //case SqlDbType.Image:
                //case SqlDbType.Binary:

                default:
                    return string.Format("'{0}'", par.SqlValue);
            }
        }

        public void Update(string tableName, Dictionary<string, object> parameters)
        {
            ExecProcedure(string.Format("sp_{0}_Update", tableName), parameters);
        }

        public object[] LoadDat(IDAOFilter filter, string tableName)
        {
            if (filter != null)
                filter.AddTopDistinct(1, false);
    
            string sql = createSQL(filter, tableName);
            return LoadDat(sql);
        }
        public object[] LoadDat(string sql)
        {
            bool wasOpen = false;
            DbDataReader reader = null;
            object[] vals = null;
            try
            {
                reader = ReaderOpen(out wasOpen, sql);
                if (!reader.HasRows)
                    throw new Exception("Ошибка при загрузке - нет строк с указанными параметрами", new Exception(sql));
                int n = 0;
                while (reader.Read())
                {
                    if (n++ > 0)
                        throw new Exception(string.Format("Ошибка при загрузке - в базе есть более одной строки с такими параметрами"), new Exception(sql));

                    vals = new object[reader.FieldCount];
                    reader.GetValues(vals);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ReaderClose(reader, wasOpen);
            }
            return vals;
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
        //public void LoadSet(IDAOFilter filter, bool distinct, LoadMembersDelegate method, string tableName)
        //{
        //    LoadSet(filter, distinct, 0, method, tableName);
        //}

        //public void LoadSet(IDAOFilter filter, int top, LoadMembersDelegate method, string tableName)
        //{
        //    LoadSet(filter, false, top, method, tableName);
        //}        

        //public void LoadSet(IDAOFilter filter, LoadMembersDelegate method, string tableName)
        //{
        //    LoadSet(filter, false, 0, method, tableName);
        ////    bool wasOpen = false;
        ////    SqlDataReader reader = null;
        ////    try
        ////    {
        ////        reader = ReaderOpen(out wasOpen, createSQL("", filter, tableName));
        ////        while (reader.Read())
        ////        {
        ////            object[] vals = new object[reader.FieldCount];
        ////            reader.GetValues(vals);
        ////            method(vals);
        ////        }
        ////    }
        ////    catch (Exception exp)
        ////    {
        ////        throw exp;
        ////    }
        ////    finally
        ////    {
        ////        ReaderClose(reader, wasOpen);
        ////    }
        //}
        public List<object[]> LoadList(string select)
        {
            bool wasOpen = false;
            DbDataReader reader = null;
            List<object[]> result = new List<object[]>();
            try
            {
                reader = ReaderOpen(out wasOpen, select);
                while (reader.Read())
                {
                    object[] vals = new object[reader.FieldCount];
                    reader.GetValues(vals);
                    result.Add(vals);
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
        public void TransactionBegin()
        {
            //bool wasOpen = false;
            ConnectionOpen();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "BEGIN TRANSACTION";
                cmd.Connection = _dbConnection;
                cmd.ExecuteScalar();
            }
            catch (System.Exception e)
            {
                throw new Exception("Ошибка при открытии транзакции: " + e.Message);
            }
            //finally { ConnectionClose(wasOpen); }
        }
        public int TransactionCommit()
        {
            bool wasOpen = false;
            int trCount = 0;
            if (_dbConnection.State != ConnectionState.Open) return -1;
            try
            {
                SqlCommand cmd = getCommand("sp_Trancount", ref wasOpen);
                cmd.ExecuteNonQuery();
                trCount = (int)cmd.Parameters["@RETURN_VALUE"].Value;
                if (trCount > 0)
                {
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "COMMIT TRANSACTION";
                    cmd.Connection = _dbConnection;
                    cmd.ExecuteScalar();
                }
                return 0;
            }
            catch (System.Exception e)
            {
                throw new Exception("Ошибка при коммите транзакции: " + e.Message);
            }
            finally
            {
                if (trCount <= 1)
                    ConnectionClose(wasOpen);
            }
        }
        public int TransactionRollback()
        {
            bool wasOpen = false;
            if (_dbConnection.State != ConnectionState.Open) return -1;
            try
            {
                SqlCommand cmd = getCommand("sp_Trancount", ref wasOpen);
                cmd.ExecuteNonQuery();
                if ((int)cmd.Parameters["@RETURN_VALUE"].Value > 0)
                {
                    cmd = new SqlCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "ROLLBACK TRANSACTION";
                    cmd.Connection = _dbConnection;
                    cmd.ExecuteScalar();
                }

                return 0;
            }
            catch (System.Exception e)
            {
                throw new Exception("Ошибка при откате транзакции: " + e.Message);
            }
            finally { ConnectionClose(); }
        }

        /// <summary>
        /// Конструирование SQL запроса для загрузки данных из SQL Server
        /// </summary>
        /// <param name="topdistinct">Distinct</param>
        /// <param name="table">Название таблицы</param>
        /// <param name="filter">Фильтер</param>
        /// <returns></returns>
        string createSQL(IDAOFilter filter, string tableName)
        {
            return createSQL(filter, tableName, new List<string>());
        }
        
        public static string createSQL(IDAOFilter filter, string tableName, string[] fields)
        {
            return createSQL(filter, tableName, new List<string>(fields));
        }

        public static string createSQL(IDAOFilter filter, string tableName, string fields)
        {
            return createSQL(filter, tableName, new List<string>(fields.Split(',')));
        }

        public static string createSQL(IDAOFilter filter, string tableName, string fields, char fieldSplitter)
        {
            return createSQL(filter, tableName, new List<string>(fields.Split(fieldSplitter)));
        }
        public static string createSQL(IDAOFilter filter, string tableName, List<string> fields)
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
                , string.Join(",", fields.ConvertAll<string>(delegate(string o) { return "[" + tableName + "]." + ((o == "*") ? o : "[" + o + "]"); }).ToArray())
                , tableName
                , parameters
                , fltstring
                );
            return sql;
        }

        public IDAOFilter NewFilter()
        {
            SQLFilter flt = new SQLFilter();
            FilterUnitBase.Filter = flt;
            return flt;
        }

        public void Delete(string tableName, int id)
        {
            bool wasOpen = false;
            try
            {
                string sp_Name = string.Format("sp_{0}_Delete", tableName);
                SqlCommand cmd = getCommand(sp_Name, ref wasOpen);

                SqlParameter p = new SqlParameter("@id", id);
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);
                cmd.ExecuteNonQuery();

                //row["id"] = (int)cmd.Parameters["@RETURN_VALUE"].Value;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally { ConnectionClose(wasOpen); }
        }

        /// <summary>
        /// Возврашает уникальных хеш конекшена на основе connectionString 
        /// </summary>
        /// <returns></returns>
        public string GetKey()
        {
            SqlConnection cn = _dbConnection;
            string key = cn.DataSource + ":" + cn.Database;
            return key;
        }
        #region Events
        void _dbConnection_StateChange(object sender, StateChangeEventArgs e)
        {
#if DEBUG
            SqlConnection c = (SqlConnection)sender;
            Console.WriteLine(string.Format("-- {0:dd.MM.yyyy HH:mm:ss.ffff} -- Connection {1}:{2} changed state from {3} to {4} ", DateTime.Now, c.DataSource, c.Database, e.OriginalState, e.CurrentState));
#endif

        }
        void cmd_StatementCompleted(object sender, StatementCompletedEventArgs e)
        {
#if DEBUG
            SqlCommand statement = (SqlCommand)sender;
            Console.WriteLine(statement.CommandText);
            Console.WriteLine(string.Format("-- {0:dd.MM.yyyy HH:mm:ss.ffff} -- {1} row(s) returned.", DateTime.Now, e.RecordCount));
#endif
        }

        #endregion
        #region Sql
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

        #region SqlConnection
        private SqlConnection _dbConnection = null;

        public bool ConnectionOpen()
        {
            bool isOpen = (_dbConnection.State == ConnectionState.Open);
            if (!isOpen)
            {
                _dbConnection.Open();
            }
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
        public void ClearConnection()
        {
            if (_dbConnection != null) ConnectionClose(false);
            _dbConnection = null;
        }
        public void sql_ExecSet(DataTable table, string sql)
        {
            bool wasOpen = false;
            try
            {
                table.Clear();
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandText = sql;
                wasOpen = ConnectionOpen();
                da.SelectCommand.Connection = _dbConnection;
                da.SelectCommand.CommandTimeout = TimeOut;
                da.Fill(table);
            }
            finally { ConnectionClose(wasOpen); }
        }
        #endregion

        #region SqlCommand
        public SqlCommand getCommand(string name, ref bool isOpen)
        {
            SqlCommand cmd = null;
            cmd = new SqlCommand();
            if (logRequest)
                cmd.StatementCompleted += new StatementCompletedEventHandler(cmd_StatementCompleted);
            cmd.CommandText = name;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = TimeOut;
            isOpen = ConnectionOpen();
            cmd.Connection = _dbConnection;

            SqlParameter par = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            par.Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(par);
            return cmd;
        }
        public SqlCommand getSqlCommand(string sql, ref bool isOpen)
        {
            SqlCommand cmd = null;
            cmd = new SqlCommand();
            if (logRequest)
                cmd.StatementCompleted += new StatementCompletedEventHandler(cmd_StatementCompleted);
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = TimeOut;
            isOpen = ConnectionOpen();
            cmd.Connection = _dbConnection;
            return cmd;
        }
        public static Exception spError(System.Exception e, string procName)
        {
            return new Exception("Ошибка при вызове процедуры <" + procName + ">: " + e.Message);
        }
        #endregion

        #region SqlDataReader
        public DbDataReader ReaderOpen(out bool wasOpen, string sql)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = TimeOut;
                wasOpen = ConnectionOpen();
                cmd.Connection = _dbConnection;
                if (logRequest)
                    cmd.StatementCompleted += new StatementCompletedEventHandler(cmd_StatementCompleted);
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
        #endregion

        #endregion

    }
}
