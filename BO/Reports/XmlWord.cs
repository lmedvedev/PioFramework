using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Reflection;

namespace BO
{
    public class WordXmlDocument : XmlDocument
    {
        string FILE_NAME = "word.xml";
        private XmlNode _currentsection;
        private XmlNode _currentparagraph;
        public SortedList ReplaceList = new SortedList();
        //public string XML_Templates_Path = @"\\team\Masters\HR";

        private string GetNamespace(string prefix)
        {
            string ret = "";
            switch (prefix)
            {
                case "w": ret = "http://schemas.microsoft.com/office/word/2003/wordml"; break;
                case "v": ret = "urn:schemas-microsoft-com:vml"; break;
                case "w10": ret = "urn:schemas-microsoft-com:office:word"; break;
                case "sl": ret = "http://schemas.microsoft.com/schemaLibrary/2003/core"; break;
                case "aml": ret = "http://schemas.microsoft.com/aml/2001/core"; break;
                case "wx": ret = "http://schemas.microsoft.com/office/word/2003/auxHint"; break;
                case "o": ret = "urn:schemas-microsoft-com:office:office"; break;
                case "dt": ret = "uuid:C2F41010-65B3-11d1-A29F-00AA00C14882"; break;
            }
            return ret;
        }

        public WordXmlDocument() : base()
        {
            AppendChild(CreateElement("w", "wordDocument", GetNamespace("w")));
            AppendAttribute("xmlns:w", GetNamespace("w"));
            AppendAttribute("xmlns:v", GetNamespace("v"));
            AppendAttribute("xmlns:w10", GetNamespace("w10"));
            AppendAttribute("xmlns:sl", GetNamespace("sl"));
            AppendAttribute("xmlns:aml", GetNamespace("aml"));
            AppendAttribute("xmlns:wx", GetNamespace("wx"));
            AppendAttribute("xmlns:o", GetNamespace("o"));
            AppendAttribute("xmlns:dt", GetNamespace("dt")); 
            AppendAttribute("w:macrosPresent","no"); 
            AppendAttribute("w:embeddedObjPresent","no"); 
            AppendAttribute("w:ocxPresent","no");
            AppendAttribute("xml:space", "preserve");
            XmlNode properties = AppendChild("o", "DocumentProperties");
            AppendChild("w", "fonts");
            AppendChild("w", "styles");
            AppendChild("w", "docPr");
            XmlNode body = AppendChild(DocumentElement, "w", "body");
            AppendChild(body, "w", "sectPr");
            _currentsection = AppendChild(body, "wx", "sect");
        }

        public WordXmlDocument(string templateFilePath, string templateFileName)
            : this()
        {
            string ValidPath = templateFilePath;
            if (ValidPath.Length > 0 && !ValidPath.EndsWith(@"\"))
            {
                ValidPath += @"\";
            }
            Load(ValidPath + templateFileName);
        }

        public WordXmlDocument(string templateResource)
            : this()
        {
            //Load(this.GetType().Assembly.GetManifestResourceStream(templateResource));
            Load(Assembly.GetCallingAssembly().GetManifestResourceStream(templateResource));
        }

        private void SaveFile()
        {
            StreamWriter writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME);
            string xml = OuterXml.Replace(@"\n", @"&#10;");
            if (!xml.Contains(@"<?mso-application progid=""Word.Document""?>")) xml = @"<?mso-application progid=""Word.Document""?>" + xml;
            if (!xml.Contains(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>")) xml = @"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>" + xml;
            writer.Write(xml);
            writer.Close();
        }

        public void Show()
        {
            SaveFile();
            ProcessStartInfo si = new ProcessStartInfo();
            
            si.FileName = "winword.exe";
            si.Arguments = "\"" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME + "\" /mPioXML_Open";

            //si.FileName = "cscript.exe";
            //si.UseShellExecute = false;
            //si.CreateNoWindow = true;
            //si.RedirectStandardError = true;
            //si.RedirectStandardOutput = true;
            //si.Arguments = "//Nologo OfficeDocument.vbs word \"" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME + "\"";

            Process.Start(si);
        }
        
        public string ShowByScript()
        {
            SaveFile();
            Process p = new Process();

            p.StartInfo.FileName = "cscript.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.ErrorDialog = true;
            //si.Arguments = "//Nologo OfficeDocument.vbs word \"" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME + "\"";
            p.StartInfo.Arguments = "OfficeDocument.vbs word \"" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME + "\"";

            p.Start();
            return p.StandardOutput.ReadToEnd();
        }

        public void Print()
        {
            SaveFile();
            ProcessStartInfo si = new ProcessStartInfo();

            si.FileName = "winword.exe";
            si.Arguments = "\"" + Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\" + FILE_NAME + "\" /mPioXML_Print";
            Process.Start(si);
        }

        private void doProcess(string verbExec, string fileName, string arguments, ProcessWindowStyle windowStyle)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            foreach (string verb in startInfo.Verbs)
                if (verb.Equals(verbExec))
                {
                    startInfo.Verb = verbExec;
                    startInfo.Arguments = arguments;
                    startInfo.UseShellExecute = true;
                    startInfo.CreateNoWindow = true;
                    startInfo.WindowStyle = windowStyle;
                    Process newProcess = new Process();
                    newProcess.StartInfo = startInfo;


                    newProcess.Start();
                    //newProcess.WaitForExit();
                    return;
                }
            throw new Exception("Для xml-файлов не прописана команда " + verbExec);
        }

        public XmlNode AppendParagraph(string text)
        {
            _currentparagraph = AppendChild(_currentsection, "w", "p");
            AppendText(text);
            return _currentparagraph;
        }

        public void AppendText(string text)
        {
            if (_currentparagraph == null)
                AppendParagraph(text);
            else
                AppendChild(AppendChild(_currentparagraph, "w", "r"), "w", "t").InnerText = text;
        }

        public void ReplaceText(string find, string replace)
        {
            find = find.Replace("<", "&lt;");
            find = find.Replace(">", "&gt;");
            string xml = OuterXml.Replace(find, replace);
            if (xml != "") LoadXml(xml);
        }
        public void ReplaceDict()
        {
            foreach (string DictVal in ReplaceList.Keys) 
            {
                ReplaceText(DictVal, ReplaceList[DictVal].ToString());
            }
        }

        public void FillTable(string name, List<Dictionary<string, string>> table)
        {
            XmlNode template = this.SelectNodes("//*[local-name() = 'tr' and contains(.,'[#" + name + ".')]")[0];
            if (template == null) return;
            foreach (Dictionary<string, string> row in table)
            {
                XmlNode newchild = template.Clone();
                foreach (KeyValuePair<string, string> col in row)
                {
                    foreach (XmlNode cell in newchild.SelectNodes("/*[contains(.,'[#" + name + "." + col.Key + "#]')]"))
                    {
                        cell.InnerXml = cell.InnerXml.Replace("[#" + name + "." + col.Key + "#]", col.Value);
                    }
                }
                template.ParentNode.AppendChild(newchild);
            }
            template.ParentNode.RemoveChild(template);
        }

        #region BaseMethods
        public XmlAttribute AppendAttribute(string name, string value)
        {
            return AppendAttribute(DocumentElement, "", name, value);
        }

        public XmlAttribute AppendAttribute(string prefix, string name, string value)
        {
            return AppendAttribute(DocumentElement, prefix, name, value);
        }

        public XmlAttribute AppendAttribute(XmlNode node, string prefix, string name, string value)
        {
            XmlAttribute ret = (prefix == "") ? CreateAttribute(name) : CreateAttribute(prefix, name, GetNamespace(prefix));
            ret.Value = value;
            node.Attributes.Append(ret);
            return ret;
        }

        public XmlNode AppendChild(string prefix, string name)
        {
            return AppendChild(DocumentElement, prefix, name, "");
        }

        public XmlNode AppendChild(XmlNode node, string prefix, string name)
        {
            return AppendChild(node, prefix, name, "");
        }

        public XmlNode AppendChild(XmlNode node, string prefix, string name, string value)
        {
            XmlNode ret = CreateElement(prefix, name, GetNamespace(prefix));
            ret.InnerText = value;
            node.AppendChild(ret);
            return ret;
        }
        #endregion
    }
}
