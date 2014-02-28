using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LogHelper
    {
        private static object sync = new object();
        public static void Write(string fullText)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно
                string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log", AppDomain.CurrentDomain.FriendlyName, DateTime.Now));

                lock (sync)
                {
                    File.AppendAllText(filename, string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] {1}\r\n", DateTime.Now, fullText), Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }
        public static void Write(Exception ex)
        {
            try
            {
                string fullText = string.Format("[{0}.{1}()] {2}", ex.TargetSite.DeclaringType, ex.TargetSite.Name, Common.ExMessage(ex));
                Write(fullText);
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }
    }
}
