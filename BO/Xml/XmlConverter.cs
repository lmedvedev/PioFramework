using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Serialization;

namespace BO
{
    public class XmlConverter
    {
        private XmlSchemaSet Schemas = new XmlSchemaSet();

        public void AddSchema(string path)
        {
            using (TextReader stringReader = new StringReader(path))
            {
                XmlSchema sch = XmlSchema.Read(stringReader, null);
                Schemas.Add(sch);
            }
        }

        public void AddSchema(FileInfo file)
        {
            using (FileStream stream = new FileStream(file.FullName, FileMode.Open))
            {
                XmlSchema sch = XmlSchema.Read(stream, null);
                Schemas.Add(sch);
            }
        }

        public T Read<T>(string path)
        {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas = Schemas;
                settings.ValidationEventHandler += ValidationE;
                settings.ValidationType = ValidationType.Schema;
                //settings.IgnoreWhitespace = true;

                XmlSerializer ser = new XmlSerializer(typeof(T));
                ser.UnknownElement += new XmlElementEventHandler(ser_UnknownElement);
                ser.UnknownAttribute += new XmlAttributeEventHandler(ser_UnknownAttribute);
                ser.UnreferencedObject += new UnreferencedObjectEventHandler(ser_UnreferencedObject);

                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    try
                    {
                        object ags = ser.Deserialize(reader);
                        return (T)ags;
                    }
                    catch (Exception exp)
                    {
                        string str = exp.Message;
                        throw exp;
                    }
                }

        }

        public T ReadFromXML<T>(string xml)
        {
            return (T)Deserialize(typeof(T), xml);
        }

        public object Deserialize(Type type, string xml, XmlSchemaSet schemas)
        {
            XmlSerializer sr = new XmlSerializer(type);
            XmlReaderSettings settings = new XmlReaderSettings();
            
            settings.Schemas = schemas;
            settings.ValidationEventHandler += ValidationE;
            settings.ValidationType = ValidationType.Schema;

            //settings.ConformanceLevel = ConformanceLevel.Document;

            //XmlDocument x = new XmlDocument();
            //x.LoadXml(xml);

            //settings.ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.ProcessSchemaLocation;

            using (StringReader sreader = new StringReader(xml))
            {
                using (XmlReader reader = XmlReader.Create(sreader, settings))
                {
                    return sr.Deserialize(reader);
                }
            }
        }

        public object Deserialize(Type type, string xml, string schema)
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            using (TextReader stringReader = new StringReader(schema))
            {
                XmlSchema sch = XmlSchema.Read(stringReader, null);
                schemas.Add(sch);
            }
            return Deserialize(type, xml, schemas);
        }
        
        public object Deserialize(Type type, string xml)
        {
            return Deserialize(type, xml, Schemas);
        }

        public object Deserialize(Type type, XmlDocument xmldoc)
        {
            return (xmldoc == null) ? null : Deserialize(type, xmldoc.OuterXml);
        }

        public XmlDocument ReadToDoc(string xml, bool Validate)
        {
            return ReadToDoc(xml, Validate, Schemas);
        }

        public XmlDocument ReadToDoc(string xml)
        {
            return ReadToDoc(xml, true);
        }

        public XmlDocument ReadToDoc(string xml, bool Validate, XmlSchemaSet schemas)
        {
            xml = xml.Trim();
            XmlDocument ret = new XmlDocument();
            ret.Schemas = schemas;
            ret.PreserveWhitespace = true;
            ret.LoadXml(xml);
            if (Validate && Schemas.Count > 0)
                ret.Validate(ValidationE);

            return ret;
        }

        public XmlDocument ReadToDocFromFile(string path)
        {
            StreamReader reader = new StreamReader(path, true);
            XmlDocument ret = new XmlDocument();
            ret.Schemas = Schemas;
            ret.PreserveWhitespace = true;
            ret.LoadXml(reader.ReadToEnd());
            //if (ret.DocumentElement != null && ret.DocumentElement.Attributes["xml:space"] == null)
            //    ret.DocumentElement.Attributes.Append(ret.CreateAttribute("xml:space"));
            //ret.DocumentElement.Attributes["xml:space"].Value = "preserve";
            ret.Validate(ValidationE);
            return ret;
        }

        //UTF8
        public static Encoding utf8 = new UTF8Encoding();
        public static Encoding windows1251 = Encoding.GetEncoding(1251);

        public XmlDocument Serialize(Type type, object val, string schema)
        {
            return Serialize(type, val, true, utf8, schema);
        }
        public XmlDocument Serialize(Type type, object val)
        {
            return Serialize(type, val, true, utf8, "");
        }
        public XmlDocument Serialize(Type type, object val, bool validate)
        {
            return Serialize(type, val, validate, utf8);
        }

        public XmlDocument Serialize(Type type, object val, bool validate, Encoding enc)
        {
            return Serialize(type, val, validate, enc, "");
        }
        
        public XmlDocument Serialize(Type type, object val, bool validate, Encoding enc, string schema)
        {
            XmlSerializer sr = new XmlSerializer(type);
            MemoryStream stream = new MemoryStream();
            using (XmlTextWriter tw = new XmlTextWriter(stream, enc))
            {
                try
                {
                    sr.Serialize(tw, val);
                }
                catch (Exception exp)
                {
                    string str = exp.Message;
                    throw;
                }
            }
            XmlSchemaSet schemas;
            if (schema == "")
                schemas = Schemas;
            else
            {
                schemas = new XmlSchemaSet();
                using (TextReader stringReader = new StringReader(schema))
                {
                    XmlSchema sch = XmlSchema.Read(stringReader, null);
                    schemas.Add(sch);
                }
            }
            return ReadToDoc(enc.GetString(stream.ToArray()), validate, schemas);
        }

        public string Validate(string xml)
        {
            string ret = "";
            if (Schemas.Count == 0) return ret;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            doc.Schemas = Schemas;
            try
            {
                doc.Validate(ValidationE);
                return "";
            }
            catch (Exception exp)
            {
                ret = exp.Message;
            }
            return ret;
        }

        private void ValidationE(Object sender, ValidationEventArgs e)
        {
            throw e.Exception;
            //throw new Exception(string.Format("{0} \nג פאיכו {1} \nLine number: {2} \nLine position: {3}", e.Message,e.Exception.SourceUri,e.Exception.LineNumber,e.Exception.LinePosition));
        }
        void ser_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            throw new Exception(e.ToString());
        }

        void ser_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            throw new Exception(e.ToString());
        }

        void ser_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            throw new Exception(string.Format("Attribute: {0}={1}, expected: {2}",e.Attr.Name, e.Attr.Value, e.ExpectedAttributes));
        }

        void ser_UnknownElement(object sender, XmlElementEventArgs e)
        {
            throw new Exception(e.ToString());
        }
    }
}
