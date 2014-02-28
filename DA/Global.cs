using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DA
{
    /// <summary>
    /// Пул connections к источникам данных
    /// </summary>
    public class Global
    {

        private static Dictionary<string, IDataAccess> DAList = new Dictionary<string, IDataAccess>();

        public static Dictionary<string, IDataAccess> Connections
        {
            get { return Global.DAList; }
            set { Global.DAList = value; }
        }
        /// <summary>
        /// Метод возвращающий connection по ключю
        /// </summary>
        /// <param name="key">Ключ connection</param>
        /// <returns>Connection</returns>
        public static IDataAccess GetDA(string key)
        {
            return DAList[key];
        }
        /// <summary>
        /// Добавления connection в пул
        /// </summary>
        /// <param name="da">Connection</param>
        /// <returns>Ключ добавленного connection</returns>
        public static string AddDAO(IDataAccess da)
        {
            string key = da.GetKey();
            if (!DAList.ContainsKey(key))
                DAList.Add(key, da);
            return key;
        }
        private static string defaultConnKey = "";
        /// <summary>
        /// Connection по умолчанию
        /// </summary>
        public static IDataAccess DefaultConnection
        {
            get 
            {
                if (defaultConnKey != "")
                    return GetDA(defaultConnKey);
                else
                    return null;
            }
            set
            {
                if (value != null)
                    defaultConnKey = AddDAO(value);
                else
                    defaultConnKey = "";
            }
        }
        public static decimal toDec(object x)
        {
            if (x is SqlDecimal)
                return (((SqlDecimal)x).IsNull) ? 0 : ((SqlDecimal)x).Value;
            else
                return (x == null || x == DBNull.Value) ? 0 : Convert.ToDecimal(x);
        }

        public static decimal GetDecValue(System.Data.Common.DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return 0;
            else
                return reader.GetDecimal(ordinal);
        }
        public static int GetRefValue(System.Data.Common.DbDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return 0;
            else
                return reader.GetInt32(ordinal);
        }

    }
    public class TimerClass : List<TimerClass.Now>
    {
        #region Timer Functions
        private static TimerClass _timer;

        public static TimerClass Timer
        {
            get
            {
                if (_timer == null)
                    _timer = new TimerClass();
                return _timer;
            }
        }
        public static void TimerAdd(string eventName)
        {
            Timer.Add(eventName);
        }
        public static void TimerClear()
        {
            _timer = null;
        }
        #endregion
        public TimerClass() : base() { }

        public class Now
        {
            public DateTime time;
            public string timeName;
            public Now(string evName)
            {
                time = DateTime.Now;
                timeName = evName;
            }
        }

        public void Add(string evName)
        {
            Add(new Now(evName));
        }
        public void Add(TimerClass timer)
        {
            foreach (Now now in timer)
            {
                Add(now);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count - 1; i++)
            {
                sb.AppendFormat("{0}={1:##0.00};", this[i].timeName, (this[i + 1].time - this[i].time).TotalSeconds);
            }
            double total = GetTotalTime();
            if (total > 0)
                sb.AppendFormat("Total={0:##0.00}", total);
            return sb.ToString();
        }
        public double GetTotalTime()
        {
            if (Count > 1)
                return (this[Count - 1].time - this[0].time).TotalSeconds;
            else
                return -1;
        }
    }
} 
