using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;

namespace DA
{
    public class MDBDataAccess : IDataAccess
    {
        public void LogRequest(bool writelogs)
        {
        }

        public MDBDataAccess(string filename)
            : this(filename, "")
        {
        }

        public MDBDataAccess(string filename, string pass)
            : base()
        {
            string ConnString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename;
            if (!string.IsNullOrEmpty(pass))
                ConnString += ";Password=" + pass + ";user=Admin";
            _dbConnection = new OleDbConnection(ConnString);
            //SetConnectionString(cstr);
            Global.AddDAO(this);
            ConnectionOpen();
        }
        #region IDataAccess Members
        #region Command
        public OleDbCommand getCommand(string name, ref bool isOpen)
        {
            OleDbCommand cmd = null;
            cmd = new OleDbCommand();
            //cmd.CommandText = name;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = TimeOut;
            isOpen = ConnectionOpen();
            cmd.Connection = _dbConnection;

            //OleDbParameter par = new OleDbParameter("@RETURN_VALUE", SqlDbType.Int);
            //par.Direction = ParameterDirection.ReturnValue;

            //cmd.Parameters.Add(par);
            return cmd;
        }
        #endregion
        public string SubStringFunction()
        {
            return "Mid";
        }
        public void Execute(string command)
        {
            bool wasOpen = false;
            try
            {
                OleDbCommand cmd = getCommand("", ref wasOpen);
                cmd.CommandText = command;
                cmd.ExecuteNonQuery();
            }
            finally { ConnectionClose(wasOpen); }
        }
        public int Insert(string tableName, Dictionary<string, object> parameters)
        {
            int ret = 0;
            bool wasOpen = false;
            try
            {

                OleDbCommand cmd = getCommand(tableName, ref wasOpen);
                //DataTable InTable = new DataTable();

                StringBuilder ctextFields = new StringBuilder(string.Format("insert into {0} (", tableName));
                StringBuilder ctextValues = new StringBuilder(" values (");

                OleDbParameter p;
                int PreservedID = 0;
                foreach (string key in parameters.Keys)
                {
                    if (key.ToLower() == "id")
                        continue;

                    ctextFields.Append(key + ",");

                    if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                        ctextValues.AppendFormat("p_{0},", key);
                    else
                        ctextValues.AppendFormat("[p_{0}],", key);

                    p = new OleDbParameter();
                    p.Direction = ParameterDirection.Input;
                    p.ParameterName = "p_" + key;

                    object par_value = parameters[key];

                    if (par_value is DateTime)
                        p.Value = ((DateTime)par_value).ToOADate();
                    else if (par_value is Guid)
                        p.Value = ((Guid)par_value).ToString("N");
                    else if (par_value is Decimal)
                    {
                        p.DbType = DbType.Double;
                        p.Value = (decimal)par_value;
                    }
                    else
                        p.Value = par_value;

                    if (p.Value == null)
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);

                    //Console.WriteLine("{0}\t\"{1}\"", p.ParameterName, p.Value);
                }

                ctextFields = ctextFields.Remove(ctextFields.Length - 1, 1).Append(")");
                ctextValues = ctextValues.Remove(ctextValues.Length - 1, 1).Append(")");

                cmd.CommandText = ctextFields.ToString() + ctextValues.ToString();
                cmd.Transaction = _dbTransaction;
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected != 1)
                    throw new System.Exception(string.Format("При вставке получилось {0} записей", rowsAffected));

                cmd.CommandText = "select @@identity;";
                OleDbDataReader idReader = cmd.ExecuteReader();
                if (idReader.Read())
                    ret = idReader.GetInt32(0);
                else
                    throw new System.Exception("Невозможно определить ID добавленной записи");

                if (ret == 0)
                    ret = PreservedID;

                return ret;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally { ConnectionClose(wasOpen); }
        }
        public object ExecProcedure(string procedureName, Dictionary<string, object> parameters)
        {
            throw new Exception("Этот метод не реализован в MDB");
        }
        public void Update(string tableName, Dictionary<string, object> parameters)
        {
            bool wasOpen = false;
            try
            {

                int idValue = 0;
                int MaxParams = 127;
                int commandPiece = parameters.Count / MaxParams + 1;

                //StringBuilder commandStart = new StringBuilder(string.Format("update {0} set ", tableName));
                //StringBuilder commandEnd = new StringBuilder(" where id = {0}", idValue);

                OleDbCommand[] commandArray = new OleDbCommand[commandPiece];

                int paramCounter = 0;
                foreach (string key in parameters.Keys)
                {
                    paramCounter++;
                    int currentCommandPiece = paramCounter / MaxParams;

                    if (key.ToLower() == "id")
                    {
                        idValue = (int)parameters[key];
                        continue;
                    }

                    if (commandArray[currentCommandPiece] == null)
                        commandArray[currentCommandPiece] = getCommand(tableName, ref wasOpen);

                    OleDbCommand cmd = (OleDbCommand)commandArray[currentCommandPiece];
                    cmd.CommandTimeout = TimeOut;
                    if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                        cmd.CommandText += string.Format("[{0}] = [p_{0}], ", key);
                    else
                        cmd.CommandText += string.Format("[{0}] = [p_{0}], ", key);

                    OleDbParameter p = new OleDbParameter();
                    p.Direction = ParameterDirection.Input;
                    p.ParameterName = "p_" + key;

                    object par_value = parameters[key];

                    if (par_value is DateTime)
                        p.Value = ((DateTime)par_value).ToOADate();
                    else if (par_value is Guid)
                        p.Value = ((Guid)par_value).ToString("N");
                    else if (par_value is Decimal)
                    {
                        p.DbType = DbType.Double;
                        p.Value = (decimal)par_value;
                    }
                    else
                        p.Value = par_value;

                    if (p.Value == null)
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                    //Console.WriteLine("{0}\t\"{1}\"", p.ParameterName, p.Value);
                }

                //ctext1 = ctext1.Remove(ctext1.Length - 2, 1);

                foreach (OleDbCommand cmd in commandArray)
                {
                    cmd.Transaction = _dbTransaction;
                    cmd.CommandText = string.Format("update {0} set {1} where id = {2}", tableName, cmd.CommandText.Substring(0, cmd.CommandText.Length - 2), idValue);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected != 1)
                        throw new System.Exception(string.Format("Попытка заапдейтить {0} записей", rowsAffected));
                }

            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally { ConnectionClose(wasOpen); }
        }
        public void Delete(string tableName, int id)
        {
            bool wasOpen = false;
            try
            {
                OleDbCommand cmd = getCommand(tableName, ref wasOpen);
                cmd.CommandText = string.Format("delete from {0} where id = ?", tableName);
                OleDbParameter p = new OleDbParameter("@id", id);
                p.Direction = ParameterDirection.Input;
                cmd.Parameters.Add(p);
                cmd.Transaction = _dbTransaction;
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected != 1)
                    throw new System.Exception(string.Format("Удалено {0} записей", rowsAffected));
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally { ConnectionClose(wasOpen); }
        }
        public object[] LoadDat(IDAOFilter filter, string tableName)
        {
            //if (filter != null)
            //    filter.AddTopDistinct(1, false);

            string sql = createSQL(filter, tableName);
            return LoadDat(sql);
        }
        private object[] LoadDat(string sql)
        {
            bool wasOpen = false;
            DbDataReader reader = null;
            object[] vals = null;
            try
            {
                reader = ReaderOpen(out wasOpen, sql);
                if (!reader.HasRows)
                    throw new Exception("Ошибка при загрузке - нет строк с указанными параметрами");
                int n = 0;
                while (reader.Read())
                {
                    if (n++ > 0)
                        throw new Exception(string.Format("Ошибка при загрузке - в базе есть более одной строки с такими параметрами"));

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

        //LMedvedev добавил top и distinct
        //public void LoadSet(IDAOFilter filter, bool distinct, int top, LoadMembersDelegate method, string tableName)
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
                //string topdistinct = "";

                //if (distinct)
                //    topdistinct += "distinct";

                //if (top > 0)
                //    topdistinct += " top " + top.ToString();

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
            catch
            {
                throw;
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
            //bool wasOpen = ConnectionOpen();
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
            //finally { ConnectionClose(wasOpen); }
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
        public IDAOFilter NewFilter()
        {
            MDBFilter flt = new MDBFilter();
            FilterUnitBase.Filter = flt;
            return flt;
        }
        #region Connection
        private OleDbConnection _dbConnection = null;
        private OleDbTransaction _dbTransaction = null;

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
        public void ConnectionCloseReal()
        {
            _dbConnection.ResetState();
            if (_dbConnection.State == ConnectionState.Open)
                _dbConnection.Close();
        }

        public void ConnectionClose(bool wasOpen)
        {
            //if (wasOpen) return;
            //if (_dbConnection.State == ConnectionState.Open)
            //    _dbConnection.Close();
        }
        public void ClearConnection()
        {
            if (_dbConnection != null) ConnectionClose(false);
            _dbConnection = null;
        }
        #endregion

        public string GetKey()
        {
            OleDbConnection cn = _dbConnection;
            string key = cn.DataSource + ":" + cn.Database;
            return key;
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

        #endregion
        #region DataReader

        public DbDataReader ReaderOpen(out bool wasOpen, string sql)
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                wasOpen = ConnectionOpen();
                cmd.Connection = _dbConnection;
                cmd.Transaction = _dbTransaction;
                cmd.CommandTimeout = TimeOut;
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

        #endregion


        #region IDataAccess Members


        //public void LoadSet(IDAOFilter filter, bool distinct, int top, LoadMembersDelegate method, string tableName)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public void LoadSet(IDAOFilter filter, bool distinct, LoadMembersDelegate method, string tableName)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        public void LoadSet(IDAOFilter filter, int top, LoadMembersDelegate method, string tableName)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void LoadSet(IDAOFilter filter, int top, LoadMembersDelegate method, string tableName, List<string> fieldsList)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        List<TOut> IDataAccess.LoadList<TOut>(string select, Converter<DbDataReader, TOut> method)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
