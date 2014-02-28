using System;
using System.Collections.Generic;
using System.Text;
using BO.Xml;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
namespace BO
{
    public static class Hash
    {
        #region Hash Code from Info Class
        public static string GetHashFromObj(object obj)
        {
            if (obj == null) return "";

            List<string> hashList = new List<string>();

            string name = "";
            Type type = obj.GetType();
            object[] attrs = type.GetCustomAttributes(false);
            foreach (object a in attrs)
            {
                XmlRootAttribute ra = a as XmlRootAttribute;
                if (ra != null)
                {
                    name = "/" + ra.ElementName;
                    break;
                }
            }

            FillHashSourceList(hashList, obj, name);
            hashList.Sort();
            StringBuilder sb = new StringBuilder();
            hashList.ForEach(delegate(string s) { sb.AppendLine(s); });
            return sb.ToString();
        }

        private static void FillHashSourceList(List<string> dict, object obj, string parentproperty)
        {
            if (obj == null) return;

            //Console.WriteLine("Enter in " + obj.ToString());
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(obj);
            Type type = obj.GetType();

            foreach (PropertyDescriptor p in pdc)
            {
                string propname = "/" + p.Name;
                object prop = p.GetValue(obj);
                if (prop != null)
                {
                    PropertyInfo pi = type.GetProperty(p.Name);

                    foreach (Attribute a in Attribute.GetCustomAttributes(pi))
                    {
                        XmlElementAttribute ea = a as XmlElementAttribute;
                        if (ea != null)
                        {
                            if (prop.GetType() == ea.Type)
                            {
                                propname = "/" + ea.ElementName;
                                break;
                            }
                        }
                        XmlTextAttribute ta = a as XmlTextAttribute;
                        if (ta != null && p.Name == "Value")
                        {
                            propname = "";
                            break;
                        }
                        XmlAttributeAttribute aa = a as XmlAttributeAttribute;
                        if (aa != null)
                        {
                            propname = "@" + p.Name;
                        }

                    }

                    string name = parentproperty + propname;
                    if (strValue(prop) != "" && hasXmlAttr(p))
                    {
                        if (p.PropertyType.IsArray)
                        {
                            int i = 0;
                            foreach (object propitem in prop as Array)
                            {
                                i++;
                                FillHashSourceList(dict, propitem, name + "[" + i.ToString() + "]");
                            }
                        }
                        else
                        {
                            string value = strValue(prop);
                            if (isValue(prop))
                            {
                                //Console.WriteLine("---"+name + "\t" + value);
                                dict.Add(name + "\t" + value);
                            }
                            else
                            {
                                FillHashSourceList(dict, prop, name);
                            }
                        }
                    }
                }
            }
        }
        private static bool hasXmlAttr(PropertyDescriptor pd)
        {
            foreach (Attribute a in pd.Attributes)
            {
                if (a is XmlElementAttribute
                    || a is XmlAttributeAttribute
                    || a is XmlTextAttribute)
                    return true;
            }
            return false;
        }
        private static bool isValue(object prop)
        {
            if (prop == null)
                return false;
            else if (prop is string)
                return true;
            else if (prop.GetType().IsValueType)
                return true;
            else
                return false;
        }
        private static string strValue(object prop)
        {
            if (prop == null)
                return "";
            if (prop.GetType().IsEnum)
                return XmlEnum.GetXmlEnumString(prop);
            else if (prop is DateTime)
                return ((DateTime)prop).ToString("s");
            else
                return prop.ToString().Trim();
        }
        #endregion

        #region Hash Code from Xml
        public static string PifAgentHashSource(XmlNode n, Version schemeVersion, params string[] ExcludeNames)
        {
            string ret = "";
            List<string> nlist = new List<string>();
            FillHashSourceList(nlist, n, schemeVersion, ExcludeNames);
            nlist.Sort();
            foreach (string skey in nlist)
            {
                ret += skey + "\r\n";
            }
            //Console.WriteLine(ret);
            return ret;
        }
        public static int PifAgentHash(XmlNode n, Version schemeVersion, params string[] ExcludeNames)
        {
            return PifAgentHashSource(n, schemeVersion, ExcludeNames).GetHashCode();
        }
        public static string PifAgentHashHex(XmlNode n, Version schemeVersion, params string[] ExcludeNames)
        {
            return PifAgentHashHex(PifAgentHash(n, schemeVersion, ExcludeNames));
        }
        public static string PifAgentHashHex(XmlNode n, params string[] ExcludeNames)
        {
            return PifAgentHashHex(PifAgentHash(n, new Version("4.0"), ExcludeNames));
        }
        public static string PifAgentHashHex(int Num)
        {
            string ret = string.Format("{0:X}", Num).PadLeft(8, '0');
            return ret;
        }
        private static void FillHashSourceList(List<string> dict, XmlNode n, Version schemeVersion, params string[] ExcludeNames)
        {
            string code = "";
            string value = "";
            string par = "|" + string.Join("|", ExcludeNames) + "|";

            if (!par.ToLower().Contains("|" + n.Name.ToLower() + "|"))
            {

                if (n.HasChildNodes)
                {
                    foreach (XmlNode cn in n.ChildNodes)
                    {
                        FillHashSourceList(dict, cn, schemeVersion, ExcludeNames);
                    }
                }
                else
                {
                    code = FullNodePath(n, schemeVersion);
                    if (!string.IsNullOrEmpty(n.InnerText))
                    {
                        value = RemoveBadCharacters(n.InnerText);
                        dict.Add(code + "\t" + value);
                    }
                    //else
                    //    value = "null";

                }


                if (n.Attributes != null)
                {
                    foreach (XmlAttribute att in n.Attributes)
                    {
                        if (!att.Name.Contains(":") && !par.ToLower().Contains("|" + att.Name.ToLower() + "|"))
                        {
                            code = FullNodePath(n, schemeVersion) + "@" + att.Name;
                            value = RemoveBadCharacters(att.Value);
                            dict.Add(code + "\t" + value);
                        }
                    }
                }
            }
        }
        private static string RemoveBadCharacters(string source)
        {
            string ret = source;
            ret = ret.Replace("\r", "");
            ret = ret.Replace("\n", "");
            ret = ret.Replace("\t", "");
            return ret;
        }

        //private static string FullNodePath(XmlNode n)
        //{
        //    string ret = "";
        //    if (n.ParentNode != null && n.ParentNode is XmlElement)
        //        ret += FullNodePath(n.ParentNode);

        //    if (n is XmlElement)
        //        ret += "/" + n.Name;

        //    return ret;
        //}
        //private static string FullNodePath(XmlNode n)
        //{
        //    string ret = "";

        //    if (n.ParentNode != null && n.ParentNode is XmlElement)
        //        ret += FullNodePath(n.ParentNode);

        //    if (n is XmlElement)
        //        ret += "/" + n.Name;

        //    int arrayIndex = 0;
        //    int arrayCount = 0;

        //    if (n.ParentNode != null)
        //        foreach (XmlNode x in n.ParentNode.ChildNodes)
        //        {
        //            if (x.Name == n.Name)
        //            {
        //                arrayCount++;

        //                if (x == n)
        //                    arrayIndex = arrayCount;
        //            }
        //        }

        //    if (arrayCount > 1 && arrayIndex != 0)
        //        ret += string.Format("[{0}]", arrayIndex);


        //    return ret;
        //}
        private static string FullNodePath(XmlNode n)
        {
            return FullNodePath(n, new Version("1.0"));
        }

        private static string FullNodePath(XmlNode n, Version schemeVersion)
        {
            string ret = "";

            if (n.ParentNode != null && n.ParentNode is XmlElement)
                ret += FullNodePath(n.ParentNode, schemeVersion);

            if (n is XmlElement)
                ret += "/" + n.Name;

            int arrayIndex = 0;
            int arrayCount = 0;

            if (schemeVersion != new Version("1.0"))
            {
                if (n.ParentNode != null)
                    foreach (XmlNode x in n.ParentNode.ChildNodes)
                    {
                        if (x.Name == n.Name)
                        {
                            arrayCount++;

                            if (x == n)
                                arrayIndex = arrayCount;
                        }
                    }

                if (arrayCount > 1 && arrayIndex != 0)
                    ret += string.Format("[{0}]", arrayIndex);
            }


            return ret;
        }
        #endregion
    }
}
