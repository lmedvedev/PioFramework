using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// ������� ����� ��� ������������� �������
    /// </summary>
    abstract public class FilterUnitBase
    {
        abstract public override string ToString();
        /// <summary>
        /// ����� ����������� �������� ������� ��� ������������� � ��������
        /// </summary>
        /// <param name="table">��� �������</param>
        /// <param name="column">�������� �������</param>
        /// <returns> �������� ������� ����������������� ��� ������������� � �������� </returns>
        public static string colName(string table, string column)
        {
            if (string.IsNullOrEmpty(table))
                return string.Format("[{0}]", column);
            else
                return string.Format("[{0}].[{1}]", table, column);
        }
        
        public static IDAOFilter Filter = null;
    }
}
