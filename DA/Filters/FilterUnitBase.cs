using System;
using System.Collections.Generic;
using System.Text;

namespace DA
{
    /// <summary>
    /// Базовый класс для представления фильтра
    /// </summary>
    abstract public class FilterUnitBase
    {
        abstract public override string ToString();
        /// <summary>
        /// Метод формирующий название колонки для использования в фильтрах
        /// </summary>
        /// <param name="table">Имя таблицы</param>
        /// <param name="column">Название колонки</param>
        /// <returns> Название колонки отформатированное для использования в фильтрах </returns>
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
