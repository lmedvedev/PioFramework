using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.Common;

namespace DA
{
    /// <summary>
    /// Интерфейс для классов выполняющих сохранение и загрузку данных
    /// </summary>
    public delegate void LoadMembersDelegate(object[] reader);
    public interface IDataAccess
    {
        ///// <summary>
        ///// Вставить строку в источник данных
        ///// </summary>
        ///// <param name="row">Строка с данными</param>
        ///// <returns>id вставленной записи</returns>
        int Insert(string tableName, Dictionary<string, object> parameters);
        
        ///// <summary>
        ///// Обновить строку в источнике данных
        ///// </summary>
        ///// <param name="row">Строка с данными</param>
        void Update(string tableName, Dictionary<string, object> parameters);
        object ExecProcedure(string procedureName, Dictionary<string, object> parameters);
        void Execute(string query);
        //void Update(string procedureName, string parameterNamePrefix, Dictionary<string, object> parameters);
        string SubStringFunction();

        /// <summary>
        /// Удалить строку из источника данных
        /// </summary>
        /// <param name="row">Строка с данными</param>
        void Delete(string tableName, int id);

        object[] LoadDat(IDAOFilter filter,string tableName);

        //LMedvedev добавил top и distinct
        //void LoadSet(IDAOFilter filter, bool distinct, int top, LoadMembersDelegate method, string tableName);
        //void LoadSet(IDAOFilter filter, bool distinct, LoadMembersDelegate method, string tableName);
        //void LoadSet(IDAOFilter filter, int top, LoadMembersDelegate method, string tableName);
        void LoadSet(IDAOFilter filter, LoadMembersDelegate method, string tableName);
        void LoadSet(IDAOFilter filter, LoadMembersDelegate method, string tableName, List<string> fieldsList);

        List<object[]> LoadList(string select);
        List<TOut> LoadList<TOut>(string select, Converter<DbDataReader, TOut> method);
        void TransactionBegin();
        int TransactionCommit();
        int TransactionRollback();

        /// <summary>
        /// Создает и возвращает класс-фильтр соответствующий конкретному connection
        /// </summary>
        /// <returns>IDAOFilter</returns>
        IDAOFilter NewFilter();

        bool ConnectionOpen();
        void ConnectionClose();
        void ConnectionClose(bool wasOpen);

        /// <summary>
        /// Возвращает уникальный хеш конекшена,нужно для возможности вести пул конекшонов
        /// </summary>
        /// <returns>уникальный хеш конекшена</returns>
        string GetKey();

        /// <summary>
        /// Переключает запись логов запросов
        /// </summary>
        void LogRequest(bool writelogs);

        int TimeOut { get; set;}
        DbDataReader ReaderOpen(out bool wasOpen, string sql);
        void ReaderClose(DbDataReader reader, bool wasOpen);
    }
}
