using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace BO
{
    public static class ProxyInfo
    {
        public static object GetPropValue(object obj, string propname)
        {
            bool propIsfound = true;
            return GetPropValue(obj, propname, ref propIsfound);
        }
        /// <summary>
        /// Функция для получения значения свойства в любом объекте
        /// </summary>
        /// <param name="obj">объект со ложной структурой</param>
        /// <param name="propname">строка-свойство - пример: Client.Address.Post</param>
        /// <returns></returns>
        public static object GetPropValue(object obj, string propname, ref bool propIsfound)
        {
            propIsfound = true;
            propname = propname.Trim();
            if (propname.EndsWith(")"))
            {
                // Найдена агрегатная функция?
                int pos = propname.IndexOf('(');
                if (pos <= 1)
                {
                    propIsfound = false;
                    return null;
                }
                string sArrayProp = propname.Substring(0, pos);
                string[] arrayProp = sArrayProp.Split('.');
                if(arrayProp.Length<=1)
                {
                    propIsfound = false;
                    return null;
                }
                string aggrFunction = arrayProp[arrayProp.Length - 1];
                string aggrProp = String.Join(".", arrayProp, 0, arrayProp.Length - 1);
                string aggrParam = propname.Substring(pos+1, propname.Length-pos-2);
                object aggrValue = GetPropValue(obj, aggrProp, ref propIsfound);
                if (aggrValue == null)
                    return null;
                return CalcAggregate(aggrValue, aggrFunction, aggrParam, ref propIsfound);


            }
            string[] sprop = propname.Split('.');
            for (int i = 0; i < sprop.Length; i++)
            {
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);
                PropertyDescriptor p = pdc.Find(sprop[i], false);
                if (p == null)
                {
                    propIsfound = false;
                    return null;
                }
                else
                {
                    obj = p.GetValue(obj);
                    if (p.PropertyType.IsEnum)
                        obj = BO.Xml.XmlEnum.GetXmlEnumString(obj);
                    //obj = Enum.GetName(p.PropertyType, obj);
                }
            }
            return obj;
        }

        private static object CalcAggregate(object aggrValue, string aggrFunction, string aggrParam, ref bool propIsfound)
        {
            List<object> lst = new List<object>();
            if (aggrValue is object[])
                lst.AddRange(aggrValue as object[]);
            else
                lst.Add(aggrValue);
                switch (aggrFunction.ToUpper())
                {
                    case "SUM":
                        decimal retSum = 0;
                        foreach (object val in lst)
                        {
                            decimal dec = Convert.ToDecimal(GetPropValue(val, aggrParam));
                            retSum += dec;
                        }
                        return retSum;
                    case "COUNT":
                        int retCount = 0;
                        foreach (object val in lst)
                            retCount++;
                        return retCount;
                    case "FIRST":
                        object retFirst = null;
                        if (lst.Count > 0)
                        {
                            object first = lst[0];
                            retFirst = GetPropValue(first, aggrParam);
                        }
                        return retFirst;
                    case "LAST":
                        object retLast = null;
                        if (lst.Count > 0)
                        {
                            object last = lst[0];
                            retLast = GetPropValue(last, aggrParam);
                        }
                        return retLast;
                    case "MIN":
                        decimal retMin = decimal.MaxValue;
                        foreach (object val in lst)
                        {
                            decimal min = Convert.ToDecimal(GetPropValue(val, aggrParam));
                            if (retMin > min)
                                retMin = min;
                        }
                        if (retMin == decimal.MaxValue)
                            return null;
                        else
                            return retMin;
                    case "MAX":
                        decimal retMax = decimal.MinValue;
                        foreach (object val in lst)
                        {
                            decimal max = Convert.ToDecimal(GetPropValue(val, aggrParam));
                            if (retMax < max)
                                retMax = max;
                        }
                        if (retMax == decimal.MinValue)
                            return null;
                        else
                            return retMax;
                    case "AVG":
                        decimal retAvg = 0;
                        int cnt = 0;
                        foreach (object val in lst)
                        {
                            decimal dec = Convert.ToDecimal(GetPropValue(val, aggrParam));
                            retAvg += dec;
                            cnt++;
                        }
                        if (cnt == 0)
                            return null;
                        else
                            return retAvg / cnt;

                }
                propIsfound = false;
                return null;
        }
        /// <summary>
        /// Функция для задания значения свойству объекта.
        /// Если в объекте промежуточное свойство == null, то оно создается конструктором по умолчанию.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="propname">строка-свойство - пример: Client.Address.Post</param>
        /// <param name="value"></param>
        public static void SetPropValue(object root, string propname, object value)
        {
            object obj = root;
            object prop = null;
            string[] sprop = propname.Split('.');
            PropertyDescriptor p = null;
            for (int i = 0; i < sprop.Length; i++)
            {
                if (prop != null)
                    obj = prop;
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);
                p = pdc.Find(sprop[i], false);
                if (p == null)
                    throw new Exception(string.Format("Для типа {0} не найдено свойство {1}. Ошибка в элементе {2}", obj.GetType(), propname, sprop[i]));
                else
                {
                    prop = p.GetValue(obj);
                    if (prop == null && i < sprop.Length - 1)
                    {

                        prop = Activator.CreateInstance(p.PropertyType);
                        p.SetValue(obj, prop);
                    }
                }
            }
            if (p != null && obj != null)
                p.SetValue(obj, value);
        }
    }
}
