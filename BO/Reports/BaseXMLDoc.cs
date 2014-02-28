using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Globalization;

namespace BO
{
    public class BaseXmlDoc : XmlDocument
    {
        public BaseXmlDoc() : base() { }
        public XmlNode AppendChild(string nameElement)
        {
            return (this.DocumentElement == null) ? this.AppendChild(this.CreateElement(nameElement)) : this.DocumentElement.AppendChild(this.CreateElement(nameElement));
        }
        public XmlNode AppendChild(XmlNode node, string attrName, string attrValue)
        {
            if (attrName != "")
            {
                XmlAttribute attr = this.CreateAttribute(attrName);
                attr.Value = attrValue;
                node.Attributes.Append(attr);
            }
            return node;
        }
        public XmlNode AppendChild(string nameElement, string attrName, string attrValue)
        {
            return this.AppendChild(this, nameElement, attrName, attrValue);
        }

        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, string attrValue, DateTime Text)
        {
            return AppendChild(node, nameElement, attrName, attrValue, toStr(Text));
        }

        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, string attrValue, string Text)
        {
            XmlNode n = AppendChild(node, nameElement, attrName, attrValue);
            if (Text != "") n.InnerText = Text;
            return n;
        }

        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, string attrValue)
        {
            //			XmlNode newnode = node.AppendChild(this.CreateElement(nameElement));
            //			XmlAttribute attr = this.CreateAttribute(attrName);
            //			attr.Value = attrValue;
            //			newnode.Attributes.Append(attr);
            return AppendChild(node, nameElement, attrName, attrValue, "", "");
            //			return newnode ;
        }
        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, string attrValue, string Prefix, string Namespace)
        {
            XmlNode newnode = this.CreateElement(nameElement);
            newnode = (node == null) ? this.AppendChild(newnode) : node.AppendChild(newnode);
            if (attrName != "")
            {
                XmlAttribute attr = (Prefix != "" && Namespace != "") ? this.CreateAttribute(Prefix, attrName, Namespace) : this.CreateAttribute(attrName);
                attr.Value = attrValue;
                newnode.Attributes.Append(attr);
            }
            return newnode;
        }
        public XmlNode AppendChild(XmlNode node, string nameElement)
        {
            XmlNode newnode = node.AppendChild(this.CreateElement(nameElement));
            return newnode;
        }
        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, DateTime attrValue)
        {
            return this.AppendChild(node, nameElement, attrName, toStr(attrValue));
        }

        public XmlNode AppendChild(XmlNode node, string nameElement, string attrName, decimal attrValue)
        {
            return this.AppendChild(node, nameElement, attrName, toStr(attrValue));
        }

        public XmlAttribute AppendAttribute(XmlNode node, string attrName, string attrValue)
        {
            return AppendAttribute(node, attrName, attrValue, "", "");
        }
        public XmlAttribute AppendAttribute(XmlNode node, string attrName, string attrValue, string Prefix, string Namespace)
        {
            XmlAttribute attr = (Prefix != "" && Namespace != "") ? this.CreateAttribute(Prefix, attrName, Namespace) : this.CreateAttribute(attrName);
            attr.Value = attrValue;
            node.Attributes.Append(attr);
            return attr;
        }
        public XmlAttribute AppendAttribute(XmlNode node, string attrName, DateTime attrValue)
        {
            return this.AppendAttribute(node, attrName, toStr(attrValue));
        }

        private string toStr(DateTime val)
        {
            return (val == DateTime.MinValue) ? "" : val.ToString(Formats.SQLDATE);
        }
        private string toStr(decimal val)
        {
            return val.ToString(new CultureInfo("en-US"));
        }
        public class Formats
        {
            public static string DATE = "dd.MM.yyyy";
            public static string DATETIME = "dd.MM.yyyy HH:mm";
            public static string DATETIMEFULL = "dd.MM.yyyy HH:mm:ss.fff";
            public static string SQLDATE = "yyyy-MM-dd";
            public static string SQLDATETIME = "yyyy-MM-dd HH:mm";
            public static string SQLDATETIMEFULL = "yyyy-MM-dd HH:mm:ss";
            public static string INTEGER = "#,##0";
            public static string NUMBER = "#";
            public static string MONEY = "#,##0.00;-#,##0.00;-";
            public static string MONEY4 = "#,##0.0000;-#,##0.0000;-";
            public static string MONEY5 = "#,##0.00000;-#,##0.00000;-";
            public static string MONEYDELIM = "###0.00";
            public static string AMOUNT = "#,##0.########;-#,##0.########;-";
        }
    }
}

