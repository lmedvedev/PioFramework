using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.Data.Common;

namespace DA
{
    /// <summary>
    /// ��������� ��� ������� ����������� ���������� � �������� ������
    /// </summary>
    public delegate void LoadMembersDelegate(object[] reader);
    public interface IDataAccess
    {
        ///// <summary>
        ///// �������� ������ � �������� ������
        ///// </summary>
        ///// <param name="row">������ � �������</param>
        ///// <returns>id ����������� ������</returns>
        int Insert(string tableName, Dictionary<string, object> parameters);
        
        ///// <summary>
        ///// �������� ������ � ��������� ������
        ///// </summary>
        ///// <param name="row">������ � �������</param>
        void Update(string tableName, Dictionary<string, object> parameters);
        object ExecProcedure(string procedureName, Dictionary<string, object> parameters);
        void Execute(string query);
        //void Update(string procedureName, string parameterNamePrefix, Dictionary<string, object> parameters);
        string SubStringFunction();

        /// <summary>
        /// ������� ������ �� ��������� ������
        /// </summary>
        /// <param name="row">������ � �������</param>
        void Delete(string tableName, int id);

        object[] LoadDat(IDAOFilter filter,string tableName);

        //LMedvedev ������� top � distinct
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
        /// ������� � ���������� �����-������ ��������������� ����������� connection
        /// </summary>
        /// <returns>IDAOFilter</returns>
        IDAOFilter NewFilter();

        bool ConnectionOpen();
        void ConnectionClose();
        void ConnectionClose(bool wasOpen);

        /// <summary>
        /// ���������� ���������� ��� ���������,����� ��� ����������� ����� ��� ����������
        /// </summary>
        /// <returns>���������� ��� ���������</returns>
        string GetKey();

        /// <summary>
        /// ����������� ������ ����� ��������
        /// </summary>
        void LogRequest(bool writelogs);

        int TimeOut { get; set;}
        DbDataReader ReaderOpen(out bool wasOpen, string sql);
        void ReaderClose(DbDataReader reader, bool wasOpen);
    }
}
