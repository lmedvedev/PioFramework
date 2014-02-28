using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DA
{
    /// <summary>
    /// Фильтр заточенный под MS Access
    /// </summary>
    public class MDBFilter : SQLFilter
    {
        public override string toSQL(object val) 
        {
            if (val == null) return "null";
            if (val is string) return "'" + val + "'";
            if (val is DateTime)
            {
                DateTime dt = (DateTime)val;
                if (dt == DateTime.MinValue) 
                    return "null";
                else
                    return "#" + dt.ToString("yyyy-MM-dd") + "#";
            }
            if (val is bool)
            {
                bool v = (bool)val;
                if (v)
                {
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            if (val is Guid)
            { 
                return string.Format("'{0:N}'", val);
            }

            if (val is XmlDocument) return "'" + ((XmlDocument)val).InnerXml + "'";
            else return val.ToString();
        }

     }
}
