using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;

namespace BO.Xml
{
    /// <summary>
    /// Работа с enumeration, сгенеренными из Xml.
    /// </summary>
    public class XmlEnum
    {
        /// <summary>
        /// Преобразование enum-элемента в строку. Если для этого элемента определен атрибут XmlEnumAttribute,
        /// то берется значение из этого атрибута. Если такого атрибута нет, то берется значение.ToString()
        /// </summary>
        /// <typeparam name="E">enumeration</typeparam>
        /// <param name="item">enum item</param>
        /// <returns></returns>
        public static string GetXmlEnumString<E>(E item)
        {
            FieldInfo fi = typeof(E).GetField(item.ToString());
            if (fi != null)
            {
                object[] attrs = fi.GetCustomAttributes(false);
                foreach (object attr in attrs)
                {
                    XmlEnumAttribute xa = attr as XmlEnumAttribute;
                    if (xa != null)
                        return xa.Name;
                }
            }
            return item.ToString();
        }

        public static string GetXmlEnumString(object item)
        {
            if (item == null || !item.GetType().IsEnum) return "";
            FieldInfo fi = item.GetType().GetField(item.ToString());
            if (fi != null)
            {
                object[] attrs = fi.GetCustomAttributes(false);
                foreach (object attr in attrs)
                {
                    XmlEnumAttribute xa = attr as XmlEnumAttribute;
                    if (xa != null)
                        return xa.Name;
                }
            }
            return item.ToString();
        }

        public static E[] GetXmlEnumString<E>(string values, string[] separator)
        {
            string[] val = values.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            E[] ret = Array.ConvertAll<string, E>(val, delegate(string s)
            {
                return StringToXmlEnum<E>(s);
            });
            return ret;
        }

        /// <summary>
        /// Преобразование строки в enum-тип. Плевать на размер букв и Trim().
        /// Если подобрать значение не получается - ArgumentException
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static E StringToXmlEnum<E>(string s)
        {
            if (string.IsNullOrEmpty(s) || s.Trim().Length == 0)
                throw new ArgumentNullException("s");
            s = s.Trim().ToUpper();
            FieldInfo[] fis = typeof(E).GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (fi.IsLiteral)
                {
                    if (fi.Name.ToUpper() == s)
                        return (E)fi.GetRawConstantValue();
                    object[] attrs = fi.GetCustomAttributes(false);
                    foreach (object attr in attrs)
                    {
                        XmlEnumAttribute xa = attr as XmlEnumAttribute;
                        if (xa != null && xa.Name.ToUpper() == s)
                            return (E)fi.GetRawConstantValue();
                    }
                }
            }
            throw new ArgumentException(string.Format("Для перечисления {0} нет элемента, соответствующего значению '{1}'", typeof(E), s));
        }

        /// <summary>
        /// Строится массив строковых значений указанного enum-типа.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static string[] GetXmlEnumList<E>()
        {
            List<string> list = new List<string>();

            FieldInfo[] fis = typeof(E).GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (fi.IsLiteral)
                {
                    object[] attrs = fi.GetCustomAttributes(false);
                    if (attrs.Length == 0)
                        list.Add(fi.Name);
                    else
                    {
                        foreach (object attr in attrs)
                        {
                            XmlEnumAttribute xa = attr as XmlEnumAttribute;
                            if (xa != null)
                            {
                                list.Add(xa.Name);
                                break;
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Строится массив строковых значений по массиву указанного enum-типа.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static string[] GetXmlEnumList<E>(E[] arr)
        {
            List<string> list = new List<string>();
            foreach (E item in arr)
            {
                list.Add(item.ToString());
            }
            return list.ToArray();
        }

        /// <summary>
        /// Функция строит Dictionary по enum-типу. 
        /// Key=enum-элемент, Value=строковое значение.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <returns></returns>
        public static Dictionary<E, string> GetXmlEnumDictionary<E>()
        {
            Dictionary<E, string> dict = new Dictionary<E, string>();

            FieldInfo[] fis = typeof(E).GetFields();
            foreach (FieldInfo fi in fis)
            {
                if (fi.IsLiteral)
                {
                    E key = (E)fi.GetRawConstantValue();
                    object[] attrs = fi.GetCustomAttributes(false);
                    if (attrs.Length == 0)
                        dict.Add(key, fi.Name);
                    else
                    {
                        foreach (object attr in attrs)
                        {
                            XmlEnumAttribute xa = attr as XmlEnumAttribute;
                            if (xa != null)
                            {
                                dict.Add(key, xa.Name);
                                break;
                            }
                        }
                    }
                }
            }
            return dict;
        }

    }
}
