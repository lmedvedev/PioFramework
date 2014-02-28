using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;
using System.IO;
using System.Net.Mail;
using System.Xml.Schema;
using System.DirectoryServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Win32;
using System.Reflection;
using System.Diagnostics;
using System.Net;

namespace BO
{
    public static class Common
    {
        public enum propertyAD
        {
            displayName,
            mail

            //cn = Медведев Л.И.
            //givenName = Медведев Л.И.
            //displayName = Медведев Л.И.
            //name = Медведев Л.И.
            //description = progremmer
            //distinguishedName = CN=Медведев Л.И.,OU=IT_TEst(novikov),OU=ITR_USER,DC=universal,DC=ru
            //instanceType = 4
            //whenCreated = 21.02.2013 14:12:54
            //whenChanged = 17.06.2013 5:02:16
            //uSNCreated = System.__ComObject
            //memberOf[0] = CN=exnews,CN=Users,DC=universal,DC=ru
            //memberOf[1] = CN=ITR_ro,OU=Global Groups,DC=universal,DC=ru
            //memberOf[2] = CN=ITR_support,OU=Global Groups,DC=universal,DC=ru
            //memberOf[3] = CN=ITR_Development,OU=Global Groups,DC=universal,DC=ru
            //memberOf[4] = CN=Add_to_Domain_WS_v,OU=Global Groups,DC=universal,DC=ru
            //memberOf[5] = CN=Trastet Users for REGEDIT_v,OU=Global Groups,DC=universal,DC=ru
            //memberOf[6] = CN=Who member_v,OU=Global Groups,DC=universal,DC=ru
            //memberOf[7] = CN=Access to Internet_v,OU=Global Groups,DC=universal,DC=ru
            //memberOf = System.Object[]
            //uSNChanged = System.__ComObject
            //proxyAddresses = SMTP:medvedevli@sgc.ru
            //objectGUID = System.Byte[]
            //userAccountControl = 66048
            //badPwdCount = 0
            //codePage = 0
            //countryCode = 0
            //badPasswordTime = System.__ComObject
            //lastLogon = System.__ComObject
            //logonHours = System.Byte[]
            //pwdLastSet = System.__ComObject
            //primaryGroupID = 513
            //objectSid = System.Byte[]
            //accountExpires = System.__ComObject
            //logonCount = 430
            //sAMAccountName = medvedevli
            //sAMAccountType = 805306368
            //showInAddressBook[0] = CN=All Recipients(VLV),CN=All System Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook[1] = CN=Глоб. список адресов по умолчанию,CN=All Global Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook[2] = CN=Все пользователи,CN=All Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook[3] = CN=All Mailboxes(VLV),CN=All System Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook[4] = CN=Mailboxes(VLV),CN=All System Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook[5] = CN=ДКИТ,CN=All Address Lists,CN=Address Lists Container,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //showInAddressBook = System.Object[]
            //legacyExchangeDN = /o=SGC/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Recipients/cn=useref6a2091
            //userPrincipalName = medvedevli@universal.ru
            //objectCategory = CN=Person,CN=Schema,CN=Configuration,DC=universal,DC=ru
            //dSCorePropagationData[0] = 26.02.2013 11:57:22
            //dSCorePropagationData[1] = 26.02.2013 11:57:22
            //dSCorePropagationData[2] = 26.02.2013 11:57:22
            //dSCorePropagationData[3] = 08.01.1601 15:10:56
            //dSCorePropagationData = System.Object[]
            //lastLogonTimestamp = System.__ComObject
            //mail = medvedevli@sgc.ru
            //mDBUseDefaults = True
            //msExchMailboxGuid = System.Byte[]
            //msExchMailboxSecurityDescriptor = System.__ComObject
            //msExchRecipientDisplayType = 1073741824
            //msExchVersion = System.__ComObject
            //msExchUserAccountControl = 0
            //msExchRecipientTypeDetails = System.__ComObject
            //msExchUMDtmfMap[0] = emailAddress:6338333854
            //msExchUMDtmfMap[1] = lastNameFirstName:
            //msExchUMDtmfMap[2] = firstNameLastName:
            //msExchUMDtmfMap = System.Object[]
            //homeMTA = CN=Microsoft MTA,CN=MSS-2,CN=Servers,CN=Exchange Administrative Group (FYDIBOHF23SPDLT),CN=Administrative Groups,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //msExchHomeServerName = /o=SGC/ou=Exchange Administrative Group (FYDIBOHF23SPDLT)/cn=Configuration/cn=Servers/cn=MSS-2
            //msExchRBACPolicyLink = CN=Default Role Assignment Policy,CN=Policies,CN=RBAC,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //msExchPoliciesIncluded[0] = 8f56ced1-86f5-4b24-a111-2c50fae6adfb
            //msExchPoliciesIncluded[1] = {26491cfc-9e50-4857-861b-0cb8df22b5d7}
            //msExchPoliciesIncluded = System.Object[]
            //extensionAttribute1 = 18
            //homeMDB = CN=IT,CN=Databases,CN=Exchange Administrative Group (FYDIBOHF23SPDLT),CN=Administrative Groups,CN=SGC,CN=Microsoft Exchange,CN=Services,CN=Configuration,DC=universal,DC=ru
            //msExchTextMessagingState[0] = 302120705
            //msExchTextMessagingState[1] = 16842751
            //msExchTextMessagingState = System.Object[]
            //mailNickname = medvedevli
            //msExchWhenMailboxCreated = 21.02.2013 14:18:31
            //nTSecurityDescriptor = System.__ComObject

        }
        static Color InvertColor(Color color)
        {
            return Color.FromArgb(int.MaxValue - color.ToArgb());
        }

        public static DateTime Fix1SDate(DateTime bd)
        {
            //if (Math.Abs(bd.Year - DateTime.Today.Year) > 100 && bd != DateTime.MinValue)
            //{
            //    string s = "0000" + bd.Year.ToString();
            //    s = "19" + s.Substring(s.Length - 2, 2);

            //    return new DateTime(int.Parse(s), bd.Month, bd.Day);
            //}
            if (bd == new DateTime(2001, 1, 1))
                return DateTime.MinValue;
            else if (bd.Year > DateTime.Today.Year + 100)
                return new DateTime(bd.Year - 2000, bd.Month, bd.Day);
            else 
                return bd;
        }

        public static string GetADUserProperty(propertyAD prop, string domainLogin)
        {

            Exception ExOut;
            Dictionary<string, object> allPropValues = null;
            //Dictionary<string, object> allPropValues = new Dictionary<string,object>();
            string ret = GetADUserProperty(prop, domainLogin, out ExOut, ref allPropValues);

            if (allPropValues != null)
                foreach (KeyValuePair<string, object> item in allPropValues)
                {
                    Type t = item.Value.GetType();

                    if (t.IsArray)
                    {
                        int i = 0;
                        foreach (var item1 in (item.Value as Array))
                        {
                            Debug.Print("{0}[{1}] = {2}", item.Key, i++, item1.ToString());
                        }
                    }
                    Debug.Print("{0} = {1}", item.Key, item.Value);
                }


            return ret;
        }

        public static string GetADUserProperty(propertyAD prop, string domainLogin, out Exception ExOut, ref Dictionary<string, object> allPropValues)
        {
            string ret = null;
            ExOut = null;
            try
            {

                string[] cred = domainLogin.Split('\\');

                string username = cred[1];
                string domain = cred[0];

                // ещё пример
                //DirectoryEntry entry = getDirectoryEntry();
                //DirectorySearcher searcher = new DirectorySearcher();
                //searcher.SearchRoot = entry;
                //searcher.Filter = "(sAMAccountName=" + UserName + ")";
                //SearchResult results = searcher.FindOne();
                //DirectoryEntry ent = results.GetDirectoryEntry();
                //if (ent.Properties[Field].Count > 0)
                //    return ent.Properties[Field][0].ToString();
                //else
                //    return "";

                // и ещё пример
                //var domainPath = "имя домена где производится поиск";
                //var directoryEntry = new DirectoryEntry(domainPath);
                //var dirSearcher = new DirectorySearcher(directoryEntry);
                //dirSearcher.SearchScope = SearchScope.Subtree;
                //dirSearcher.Filter = string.Format("(&(objectClass=user)(|(cn={0})(sn={0}*)(givenName={0})(sAMAccountName={0}*)))", searchString);
                //var searchResults = dirSearcher.FindAll();
                //foreach (SearchResult sr in searchResults)
                //{
                //var de = sr.GetDirectoryEntry();
                ////...do smth
                //}
                //Строка запроса (&(objectClass=user)(|(cn={0})(sn={0}*)(givenName={0})(sAMAccountName={0}*)))
                //соответственно здесь идет поиск пользователя по свойствам cn, sn, givenName, sAMAccountName 

                string filter = string.Format("(&(ObjectClass={0})(sAMAccountName={1}))", "person", username);
                string[] properties = new string[] { "fullname" };

                DirectoryEntry adRoot = new DirectoryEntry("LDAP://" + domain, null, null, AuthenticationTypes.Secure);
                DirectorySearcher searcher = new DirectorySearcher(adRoot);
                searcher.SearchScope = SearchScope.Subtree;
                searcher.ReferralChasing = ReferralChasingOption.All;
                searcher.PropertiesToLoad.AddRange(properties);
                searcher.Filter = filter;

                SearchResult result = searcher.FindOne();

                DirectoryEntry directoryEntry = result.GetDirectoryEntry();
                //

                ret = directoryEntry.Properties[prop.ToString()][0].ToString();

                if (allPropValues != null)
                {

                    foreach (PropertyValueCollection item in directoryEntry.Properties)
                    {
                        allPropValues[item.PropertyName] = item.Value;
                    }

                }
            }
            catch (Exception Ex)
            {
                ExOut = Ex;
            }

            return ret;
        }

        public const string HtmlDTD = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";
        public static TD[] ReDim<TD>(TD[] arr)
        {
            if (arr == null)
                arr = new TD[0];
            TD[] arr2 = new TD[arr.Length + 1];
            arr.CopyTo(arr2, 0);
            return arr2;
        }
        public static string RemoveSberTail(string xml, out string DocElement)
        {
            Regex reg = new Regex(@"<[\w]+");
            Match m = reg.Match(xml);
            DocElement = m.Value.Remove(0, 1);

            return xml.Substring(0, xml.IndexOf("</" + DocElement + ">") + DocElement.Length + 3);
        }
        public static string GlobalDecimal(string decstring)
        {
            if (!string.IsNullOrEmpty(decstring))
            {
                decstring = decstring.Replace(",", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                decstring = decstring.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            }
            //else
            //    decstring = "0";
            //decstring = decstring.Replace(" ", "");
            return decstring;
        }
        //public static void SendMail(string toAddress, string fromAddress, string subj, string messageText)
        //{
        //    SendMail(toAddress, "", fromAddress, "", subj, messageText, "mail1.piogmow.pioglobal.ru");
        //}
        //public static void SendMail(string toAddress, string toName, string fromAddress, string fromName, string subj, string messageText)
        //{
        //    SendMail(toAddress, toName, fromAddress, fromName, subj, messageText, "mail1.piogmow.pioglobal.ru");
        //}
        public static void SendMail(string toAddress, string toName, string fromAddress, string fromName, string subj, string messageText, string smtpHost)
        {
            SendMail(toAddress, toName, fromAddress, fromName, subj, messageText, smtpHost, null, null);
        }
        public static void SendMail(string toAddress, string toName, string fromAddress, string fromName, string subj, string messageText, string smtpHost, string smtpLogin, string smtpPassword)
        {
            //MailAddress mTo = new MailAddress(toAddress, toName, System.Text.Encoding.UTF8);
            MailAddress mFrom = new MailAddress(fromAddress, fromName, System.Text.Encoding.UTF8);

            MailMessage m = new MailMessage();

            m.SubjectEncoding = System.Text.Encoding.UTF8;
            m.Subject = subj;

            m.BodyEncoding = System.Text.Encoding.UTF8;
            m.Body = messageText;

            m.To.Add(toAddress);
            m.From = mFrom;

            SmtpClient client = new SmtpClient(smtpHost);
            if (!string.IsNullOrEmpty(smtpLogin))
                client.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
            client.Send(m);
        }
        public static FormatProvider Formatter = new FormatProvider();
        //public static string CreateHtmlPage(string dtd, string scripts, string styles, string title, string body)
        //{
        //    return string.Format("{0}\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<title>{3}</title>\r\n<script type=\"text/javascript\">\r\n{1}\r\n</script>\r\n<style>\r\n{2}\r\n</style>\r\n</head>\r\n<body>\r\n{4}\r\n</body>\r\n</html>\r\n", dtd, scripts, styles, title, body);
        //}
        //public static string CreateHtmlPage(string scripts, string styles, string title, string body)
        //{
        //    return CreateHtmlPage(HtmlDTD, scripts, styles, title, body);
        //}
        public static string GetNumberFromString(string s)
        {
            string ret = "";
            for (int i = 0; i < s.Length; i++)
                if (Char.IsDigit(s[i]))
                    ret += s[i];
                else
                    break;
            return ret;
        }

        public static string NullableString(string val)
        {
            return IsNullOrEmpty(val) ? null : val;
        }
        static XslCompiledTransform DefaultXSL()
        {
            XmlDocument x = new XmlDocument();
            XslCompiledTransform xslt = new XslCompiledTransform(true);
            x.LoadXml(Properties.Resources._default);
            xslt.Load(x);
            return xslt;
        }
        public static string TranslateXSL(XmlDocument xml)
        {
            return TranslateXSL(xml, DefaultXSL());
        }
        public static string TabulateXML(XmlDocument xml)
        {
            if (xml == null || string.IsNullOrEmpty(xml.OuterXml))
                return null;

            string ret = xml.OuterXml;
            ret = ret.Replace("><", ">\r\n<");
            int i = 0;
            foreach (XmlNode n in xml.ChildNodes)
            {
                ret = TabulateXmlNode(n, i, ret);
            }
            return ret;
        }

        private static string TabulateXmlNode(XmlNode n, int cnt, string text)
        {
            text = text.Replace("<" + n.Name, new string(' ', cnt) + "<" + n.Name);
            text = text.Replace("\r\n</" + n.Name, "\r\n" + new string(' ', cnt) + "</" + n.Name);
            if (n.HasChildNodes)
            {
                foreach (XmlNode nC in n.ChildNodes)
                {
                    text = TabulateXmlNode(nC, cnt + 1, text);
                }
            }
            else
            {
            }
            return text;
        }

        //public static string TranslateXSL(XmlDocument xml, string pathTo_XSLT_File)
        //{
        //}   

        public static string TranslateXSL(XmlDocument xml, XslCompiledTransform xsl)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriter str = XmlWriter.Create(sb);
            // open root node
            //str.WriteStartElement("html");
            
            xsl.Transform(xml, str);
            //try
            //{
            //    //if (xsl != null)
            //    //{
            //        xsl.Transform(xml, str);
            //    //}
            //    //else
            //    //{
            //    //    xml.WriteContentTo(str);
            //    //}
            //}
            //catch (System.Exception Ex)
            //{
            //    DefaultXSL().Transform(xml, str);
            //    sb.Append(Common.ExMessage(Ex));
            //}
            //str.WriteEndElement();
            
            return sb.ToString();
        }

        //public static string TranslateXSL(string xml, string xsl)
        //{
        //    XmlDocument xmldoc = new XmlDocument();
        //    xmldoc.LoadXml(xml);
        //    return TranslateXSL(xmldoc, xsl);
        //}

        //public static string TranslateXSL(XmlDocument xml, string xsl)
        //{
        //    if (xsl == "") return xml.OuterXml;

        //    XslCompiledTransform xslt = new XslCompiledTransform(true);
        //    //xslt.OutputSettings = new XmlWriterSettings();
        //    //xslt.OutputSettings.ConformanceLevel = ConformanceLevel.Auto;

        //    MemoryStream st_input = null;
        //    MemoryStream st_output = null;
        //    try
        //    {
        //        XmlDocument xsldoc = new XmlDocument();
        //        StringBuilder sb = new StringBuilder();
        //        XmlWriter str = XmlWriter.Create(sb);
        //        xsldoc.LoadXml(xsl);

        //        //на всяк случай (если внутри - не XML, а текст) обрамляем верхним тегом
        //        //XmlNodeList tmpl_nodes = xsldoc.DocumentElement.SelectNodes("//*[local-name(.)='template' and @match='/']");
        //        //XmlNodeList tmpl_nodes = xsldoc.DocumentElement.SelectNodes("//*[local-name(.)='template']");
        //        //if (tmpl_nodes.Count > 0)
        //        //    tmpl_nodes[0].InnerXml = "<a>" + tmpl_nodes[0].InnerXml + "</a>";

        //        xslt.Load(xsldoc);
        //        if (xml.Schemas != null && xml.Schemas.Schemas() != null)
        //        {
        //            if (xsldoc.Schemas == null)
        //                xsldoc.Schemas = new XmlSchemaSet();

        //            foreach (XmlSchema sch in xml.Schemas.Schemas())
        //            {
        //                xsldoc.Schemas.Add(sch);
        //            }
        //            //xsldoc.Schemas.Compile();
        //        }

        //        xslt.Transform(xml, str);
        //        XmlDocument ret = new XmlDocument();
        //        ret.LoadXml(sb.ToString());
        //        //return ret.DocumentElement.InnerXml;
        //        return ret.DocumentElement.OuterXml;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw new Exception("Неудачная попытка открыть xsl-шаблон!\n" + exp.Message, exp);
        //    }
        //    finally
        //    {
        //        if (st_input != null) st_input.Close();
        //        if (st_output != null) st_output.Close();
        //    }
        //}

        public static string PurifyString(string input, string contains)
        {
            string ret = "";
            if (!Common.IsNullOrEmpty(input))
            {
                for (int i = 0; i < input.Length; i++)
                {
                    string f = input[i].ToString();
                    if (contains.Contains(f))
                        ret += f;
                }
            }
            return ret;
        }

        public static string DatProp2String(object value, Type type)
        {
            if (type == typeof(DateTime))
            {
                DateTime val = DateTime.MinValue;
                if (value != null)
                    val = BaseDat.O2DateTime(value);
                return (val == DateTime.MinValue) ? "" : val.ToString((val.TimeOfDay.Ticks == 0) ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm");
            }
            else if (type == typeof(decimal))
            {
                decimal val = 0;
                if (value != null)
                    val = BaseDat.O2Decimal(value);
                return (val == 0) ? "" : val.ToString("###0.#######").Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
            }
            else if (type == typeof(bool) && value != null)
            {
                bool val = BaseDat.O2Boolean(value);
                return val ? "1" : "0";
            }
            return (value == null) ? "" : value.ToString();
        }

        public static string DecimalToString(decimal x)
        {
            return PropValueDecimal2String(Round(x, 2));
        }

        public static decimal RoundMoney(decimal x)
        {
            return Round(x, 2);
        }

        public static decimal Round(decimal x, int precision)
        {
            //return decimal.Round(x + (decimal)0.000000001, precision);
            return decimal.Round(x, precision, MidpointRounding.AwayFromZero);
        }

        public static string RemoveSpaces(string s)
        {
            if (string.IsNullOrEmpty(s))
                return "";

            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }
            return s.Trim();
        }

        public static bool IsNullOrEmpty(string s)
        {
            return (s == null) ? true : string.IsNullOrEmpty(s);
        }
        public static bool IsNullOrEmptyOrSpaced(string s)
        {
            return (s == null) ? true : string.IsNullOrEmpty(s.Trim());
        }
        public static string GetReportsInfoSchema()
        {
            return global::BO.Properties.Resources.ReportsInfo;
        }
        public static string ExMessage(Exception Ex)
        {
            string ret = Ex.Message;
            if (Ex.InnerException != null)
                ret = ret + "\r\n" + ExMessage(Ex.InnerException);
            return ret;
        }

        public static DatErrorList ExMessage2(Exception Ex)
        {
            DatErrorList ret = new DatErrorList();
            //ret.Add(Ex.HResult.ToString(), Ex.Message);
            ret.Add("", Ex.Message);
            if (Ex.InnerException != null)
                ret.AddRange(ExMessage2(Ex.InnerException));
            return ret;
        }

        public static object String2PropValue(string value, Type type)
        {
            if (type == typeof(DateTime)) return (value == "") ? DateTime.MinValue : DateTime.Parse(value);
            else if (type == typeof(decimal)) return (value == "") ? 0 : decimal.Parse(value.Replace(".", System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
            else if (type == typeof(int)) return (value == "") ? 0 : int.Parse(value);
            else if (type == typeof(XmlDocument))
            {
                XmlDocument ret = new XmlDocument();
                try
                {
                    ret.LoadXml(value);
                }
                catch (Exception exp)
                {
                    ret.AppendChild(ret.CreateElement("Error")).InnerText = exp.Message;
                }
                return ret;
            }
            return value;
        }

        public static string PropValue2String(object value, Type type)
        {
            if (type == typeof(DateTime))
            {
                DateTime val = DateTime.MinValue;
                DateTime.TryParse(value.ToString(), out val);
                return (val == DateTime.MinValue) ? "" : val.ToString((val.TimeOfDay.Ticks == 0) ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm");
            }

            if (type == typeof(decimal))
                return PropValueDecimal2String(value);

            if (type == typeof(float))
                return PropValueDecimal2String(value);

            if (type == typeof(XmlDocument))
                return (value == null) ? "" : ((XmlDocument)value).OuterXml;

            return value.ToString();
        }

        public static string PropValueDecimal2String(object value)
        {
            decimal val = 0;
            decimal.TryParse(value.ToString(), out val);
            return val.ToString("###0.#######").Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
        }

        public const string RTFheader = @"{\rtf1\ansi\ansicpg1251\deff0\deflang1049{\fonttbl{\f0\fswiss\fcharset204 Verdana}}{\colortbl ;\red0\green100\blue128;\red255\green0\blue0;}";

        public static string ValidateBankData(string BIK, string CAccount, string RAccount, string PAccount)
        {
            if (!IsNullOrEmpty(BIK))
            {
                BIK = BIK.Trim();
                BIK = BIK.Replace(" ", "");
            }
            if (!IsNullOrEmpty(CAccount))
            {
                CAccount = CAccount.Trim();
                CAccount = CAccount.Replace(" ", "");
            }
            if (!IsNullOrEmpty(RAccount))
            {
                RAccount = RAccount.Trim();
                RAccount = RAccount.Replace(" ", "");
            }
            if (!IsNullOrEmpty(PAccount))
            {
                PAccount = PAccount.Trim();
                PAccount = PAccount.Replace(" ", "");
            }
            string zzBIK = getSubString(BIK, 4, 2);
            string yyyBIK = getSubString(BIK, 6, 3);
            string yyyCacc = getSubString(CAccount, 17, 3);
            string currCodeCacc = getSubString(CAccount, 5, 3);
            string currCodeRacc = getSubString(RAccount, 5, 3);
            string currCodePacc = IsNullOrEmpty(PAccount) || PAccount.Length < 20 ? "" : PAccount.Substring(5, 3);

            string currCode = "810";

            if (yyyBIK != yyyCacc)
                return "Корр. счет не соответствует БИКу.";

            if (currCodeCacc != currCode)
                return "Неправильный код валюты в Корр. счете.";
            if (currCodeRacc != currCode)
                return "Неправильный код валюты в Расчетном счете.";
            if (!IsNullOrEmpty(currCodePacc) && currCodePacc != currCode)
                return "Неправильный код валюты в Лицевом счете.";

            string ret = ValidateBankAccount("0" + zzBIK, CAccount);
            if (!IsNullOrEmpty(ret))
                return ret + " в Корр. счете.";

            //ret = ValidateBankAccount("0" + zzBIK, RAccount);
            ret = ValidateBankAccount(yyyBIK, RAccount);
            if (!IsNullOrEmpty(ret))
                return ret + " в Расчетном счете.";

            if (!IsNullOrEmpty(PAccount) && PAccount.Length == 20)
            {
                ret = ValidateBankAccount(yyyBIK, PAccount);
                if (!IsNullOrEmpty(ret))
                    return ret + " в Лицевом счете.";
            }

            return "";
        }
        private static string getSubString(string s, int pos, int len)
        {
            if (IsNullOrEmpty(s) || s.Length < pos + len)
                return "";
            else
                return s.Substring(pos, len);
        }
        public static string ValidateBankAccount(string pref, string Account)
        {

            int result = 0;
            if (!string.IsNullOrEmpty(Account) && Account.Length == 20)
            {
                string coefficient = "71371371371371371371371";

                Account = pref + Account;
                for (int i = 0; i < Account.Length; i++)
                {
                    int acc = int.Parse(Account.Substring(i, 1));
                    int coef = int.Parse(coefficient.Substring(i, 1));
                    result += acc * coef % 10;
                }
                result = result % 10;
            }
            else
                result = -1;
            if (result == 0)
                return "";
            else
            {
                return "Неправильная контрольная сумма";
            }
        }
        public static void rtfLine(StringBuilder sb, string name, params string[] lines)
        {
            string s = "";
            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (s == "")
                        s = string.Format(@"\cf0{0}: \cf1\b ", name);
                    else
                        s += @"\line ";
                    s += line;
                }
            }
            if (!string.IsNullOrEmpty(s))
                sb.Append(s + @"\b0\par");
        }

        public static string ConvertRTF(string rtf, object dat)
        {
            string rtf_fmt = rtf.Replace("{", @"{{").Replace("}", @"}}").Replace("[#", "{0:").Replace("#]", "}");
            return String.Format(Common.Formatter, rtf_fmt, dat);
            //if (dat != null)
            //    return dat.ToString(rtf_fmt, Common.Formatter);
            //else
            //    return rtf_fmt;
        }

        public static int DaysInYear(int year)
        {
            return (DateTime.IsLeapYear(year) ? 366 : 365);
        }

        public static string CreateFormatString(string fmt)
        {
            fmt = fmt.Replace("{", @"{{");
            fmt = fmt.Replace("}", @"}}");
            fmt = fmt.Replace("[#", "{0:");
            fmt = fmt.Replace("#]", "}");
            return fmt;
        }

        public static string SplittedText(string s, int maxLen)
        {
            string ret = "";
            int counter = 0;
            string[] headerText = s.Split(' ');
            for (int i = 0; i < headerText.Length; i++)
            {
                if (counter < maxLen)
                {
                    ret += headerText[i] + " ";
                }
                else
                {
                    ret += "\n" + headerText[i] + " ";
                    counter = 0;
                }
                counter += headerText[i].Length;
            }
            return ret;
        }

        #region MIME_and_Thumbnails

        public static bool ThumbnailCallback()
        {
            //заглушка для GenerateThumb2, не стирать
            return false;
        }

        public static Image GenerateThumbImage(Image original, int newHeight, int newWidth, bool SaveAspectRatio)
        {
            Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            if (SaveAspectRatio)
            {
                float aspect = (float)original.Height / (float)original.Width;
                if (aspect < 1) // ландшафт
                    newHeight = (int)(newHeight * aspect);
                else // портрет
                    newWidth = (int)(newWidth / aspect);

            }
            Image myThumbnail = original.GetThumbnailImage(newWidth, newHeight, myCallback, IntPtr.Zero);
            return myThumbnail;
        }

        public static byte[] GenerateThumb(byte[] myBytes, int newHeight, int newWidth)
        {
            try
            {
                Stream myStream = StreamConverter.ToStream(myBytes);

                Image oImg = Image.FromStream(myStream, true, true);
                Image oThumbNail = GenerateThumbImage(oImg, newHeight, newWidth, true);

                Stream stream2 = new MemoryStream();
                oThumbNail.Save(stream2, ImageFormat.Jpeg);
                oImg.Dispose();
                return StreamConverter.ToBytes(stream2);
            }
            catch (Exception) { return new byte[0]; }

        }
        [System.Runtime.InteropServices.DllImport("urlmon.dll", EntryPoint = "FindMimeFromData", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Ansi, SetLastError = true)]
        public static extern int FindMimeFromData(IntPtr pBC, [MarshalAs(UnmanagedType.LPWStr)] string pwzUrl, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer, int cbSize, [MarshalAs(UnmanagedType.LPWStr)] string pwzMimeProposed, int dwMimeFlags, [MarshalAs(UnmanagedType.LPWStr)] ref string ppwzMimeOut, int dwReserved);

        public static string GetMIMETypeFromBinary(byte[] fi)
        {
            string ret = "";
            int result = FindMimeFromData(IntPtr.Zero, "", fi, (fi.Length > 4096) ? 4096 : fi.Length, null, 0, ref ret, 0);
            if (string.IsNullOrEmpty(ret))
                return null;
            else
                return ret;
        }

        public static string GetMIMETypeFromRegistry(string fileExtension)
        {
            RegistryPermission regPerm = new RegistryPermission(RegistryPermissionAccess.Read, "\\HKEY_CLASSES_ROOT");
            RegistryKey classesRoot = Registry.ClassesRoot;
            RegistryKey typeKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type");

            foreach (string keyname in typeKey.GetSubKeyNames())
            {
                RegistryKey curKey = classesRoot.OpenSubKey("MIME\\Database\\Content Type\\" + keyname);

                if (curKey.GetValue("Extension") != null)
                {
                    string ext = curKey.GetValue("Extension").ToString();
                    if (curKey.GetValue("Extension").ToString().ToLower() == fileExtension.ToLower())
                        return keyname;
                }
            }

            return null;
        }
        public static string GetMIMETypeFromRegistry(FileInfo fi)
        {
            return GetMIMETypeFromRegistry(fi.Extension);
        }

        public static byte[] GenerateThumb(byte[] myBytes, int newHeight, int newWidth, bool SaveAspectRatio)
        {
            try
            {
                Stream myStream = StreamConverter.ToStream(myBytes);

                Image oImg = Image.FromStream(myStream, true, true);
                if (SaveAspectRatio)
                {
                    float aspect = (float)oImg.Height / (float)oImg.Width;
                    newHeight = (int)(newWidth * aspect);
                }
                Image oThumbNail = new Bitmap(newWidth, newHeight, oImg.PixelFormat);
                Graphics oGraphic = Graphics.FromImage(oThumbNail);
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle oRectangle = new Rectangle(0, 0, newWidth, newHeight);
                oGraphic.DrawImage(oImg, oRectangle);

                Stream stream2 = new MemoryStream();
                oThumbNail.Save(stream2, ImageFormat.Jpeg);
                oImg.Dispose();
                return StreamConverter.ToBytes(stream2);
            }
            catch (Exception) { return new byte[0]; }

        }
        #endregion
    }

    public static class StreamConverter
    {
        #region Public methods

        public static byte[] ToBytes(Stream stream)
        {
            long initialPosition = stream.Position;
            stream.Position = 0;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Position = initialPosition;

            return bytes;
        }

        public static Stream ToStream(byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        #endregion Public methods
    }
    public class FormatProvider : ICustomFormatter, IFormatProvider
    {
        public object GetFormat(Type formatType)
        {
            return (formatType == typeof(ICustomFormatter)) ? this : null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string[] frmt = string.IsNullOrEmpty(format) ? new string[] { "" } : format.Replace("::", "<!>").Split(':');
            if (frmt.Length > 0) frmt[0] = frmt[0].Replace("<!>", ":");
            if (arg == null)
                //return "";
                return frmt.Length > 1 ? frmt[1] : ""; // перенесено из ПИФ-Агентского BO
            else
            {
                if (arg is Guid)
                {
                    string ret = ((Guid)arg).ToString("D");
                    return ret;
                }
                else if (arg is String)
                {
                    string ret = arg as string;
                    string val = (string)arg;
                    switch (frmt[0].ToUpper())
                    {
                        case "GUID":
                            ret = new Guid(val).ToString("D");
                            break;
                    }
                    if (val == "" && frmt.Length > 1)
                        ret = frmt[1];
                    return ret;
                }
                else if (arg is DateTime)
                {
                    DateTime val = (DateTime)arg;
                    string ret = val.ToString(frmt[0]);

                    if (val == DateTime.MinValue
                        || val == new DateTime(1900, 1, 1, 0, 0, 0))
                        ret = (frmt.Length > 1) ? frmt[1] : "";
                    return ret;
                }
                else if (arg is decimal)
                {
                    decimal val = (decimal)arg;
                    string ret = val.ToString();

                    switch (frmt[0].ToUpper())
                    {
                        case "NUM":
                            ret = Num2Txt.Decimal2Text(val);
                            break;
                        case "INTEG":
                            ret = Num2Txt.INTEG5(val);
                            break;
                        case "UNIT":
                            ret = Num2Txt.Unit2Text(val);
                            break;
                        case "SHARE":
                            ret = Num2Txt.Share2Text(val);
                            break;
                        case "RUR":
                        case "RUB":
                        case "RUBKOP":
                        case "RURKOP":
                            ret = Num2Txt.Rub2Text(val);
                            break;
                        case "USD":
                        case "USDCENT":
                            ret = Num2Txt.USD2Text(val);
                            break;
                        default:
                            ret = val.ToString(frmt[0]);
                            if (string.IsNullOrEmpty(frmt[0]) && val % 1 > 0) ret = ret.TrimEnd('0');
                            break;
                    }
                    if (val == 0 && frmt.Length > 1)
                        ret = frmt[1];
                    return ret;
                }
                else if (!Common.IsNullOrEmpty(format))
                {
                    string fmt = "";
                    string[] pars = (format == "") ? new string[] { "" } : format.Split(':');
                    for (int i = 1; i < pars.Length; i++)
                    {
                        fmt += ":" + pars[i];
                    }
                    object val = null;
                    if (arg is IPropValue)
                        val = (arg as IPropValue).GetValue(pars[0]);
                    else
                        val = BO.Reports.ExtraRepDataInfo.GetValue(arg, pars[0]);
                    return string.Format(Common.Formatter, @"{0" + fmt + @"}", val);
                }
                else
                {
                    string ret = arg.ToString();
                    return ret;
                }
            }
        }
    }

    /// <summary>
    /// Универсальный класс для передачи данных через параметры.
    /// Для объявления обработчика события EventName (для примера - c типом int) надо написать: 
    /// <code>
    /// public event EventHandler<EventArgs<int>> EventName;
    /// </code>
    /// Потом вызывается это так: 
    /// EventName(this, new EventArgs<int>(145));
    /// </summary>
    /// <typeparam name="T">Любой тип</typeparam>
    public class EventArgs<T> : EventArgs
    {
        public T Data;
        public EventArgs(T value)
        {
            Data = value;
        }
    }
    public delegate void TEventHandler<T>(object sender, EventArgs<T> e);
    public enum DocStatus
    {
        UNKNOWN = 0,
        PARTIAL = 1,
        ALL = 2,
        NONE = 3
    }
    #region EXIF
    public class EXIFextractor : IEnumerable
    {
        /// <summary>
        /// Get the individual property value by supplying property name
        /// These are the valid property names :
        /// 
        /// "Exif IFD"
        /// "Gps IFD"
        /// "New Subfile Type"
        /// "Subfile Type"
        /// "Image Width"
        /// "Image Height"
        /// "Bits Per Sample"
        /// "Compression"
        /// "Photometric Interp"
        /// "Thresh Holding"
        /// "Cell Width"
        /// "Cell Height"
        /// "Fill Order"
        /// "Document Name"
        /// "Image Description"
        /// "Equip Make"
        /// "Equip Model"
        /// "Strip Offsets"
        /// "Orientation"
        /// "Samples PerPixel"
        /// "Rows Per Strip"
        /// "Strip Bytes Count"
        /// "Min Sample Value"
        /// "Max Sample Value"
        /// "X Resolution"
        /// "Y Resolution"
        /// "Planar Config"
        /// "Page Name"
        /// "X Position"
        /// "Y Position"
        /// "Free Offset"
        /// "Free Byte Counts"
        /// "Gray Response Unit"
        /// "Gray Response Curve"
        /// "T4 Option"
        /// "T6 Option"
        /// "Resolution Unit"
        /// "Page Number"
        /// "Transfer Funcition"
        /// "Software Used"
        /// "Date Time"
        /// "Artist"
        /// "Host Computer"
        /// "Predictor"
        /// "White Point"
        /// "Primary Chromaticities"
        /// "ColorMap"
        /// "Halftone Hints"
        /// "Tile Width"
        /// "Tile Length"
        /// "Tile Offset"
        /// "Tile ByteCounts"
        /// "InkSet"
        /// "Ink Names"
        /// "Number Of Inks"
        /// "Dot Range"
        /// "Target Printer"
        /// "Extra Samples"
        /// "Sample Format"
        /// "S Min Sample Value"
        /// "S Max Sample Value"
        /// "Transfer Range"
        /// "JPEG Proc"
        /// "JPEG InterFormat"
        /// "JPEG InterLength"
        /// "JPEG RestartInterval"
        /// "JPEG LosslessPredictors"
        /// "JPEG PointTransforms"
        /// "JPEG QTables"
        /// "JPEG DCTables"
        /// "JPEG ACTables"
        /// "YCbCr Coefficients"
        /// "YCbCr Subsampling"
        /// "YCbCr Positioning"
        /// "REF Black White"
        /// "ICC Profile"
        /// "Gamma"
        /// "ICC Profile Descriptor"
        /// "SRGB RenderingIntent"
        /// "Image Title"
        /// "Copyright"
        /// "Resolution X Unit"
        /// "Resolution Y Unit"
        /// "Resolution X LengthUnit"
        /// "Resolution Y LengthUnit"
        /// "Print Flags"
        /// "Print Flags Version"
        /// "Print Flags Crop"
        /// "Print Flags Bleed Width"
        /// "Print Flags Bleed Width Scale"
        /// "Halftone LPI"
        /// "Halftone LPIUnit"
        /// "Halftone Degree"
        /// "Halftone Shape"
        /// "Halftone Misc"
        /// "Halftone Screen"
        /// "JPEG Quality"
        /// "Grid Size"
        /// "Thumbnail Format"
        /// "Thumbnail Width"
        /// "Thumbnail Height"
        /// "Thumbnail ColorDepth"
        /// "Thumbnail Planes"
        /// "Thumbnail RawBytes"
        /// "Thumbnail Size"
        /// "Thumbnail CompressedSize"
        /// "Color Transfer Function"
        /// "Thumbnail Data"
        /// "Thumbnail ImageWidth"
        /// "Thumbnail ImageHeight"
        /// "Thumbnail BitsPerSample"
        /// "Thumbnail Compression"
        /// "Thumbnail PhotometricInterp"
        /// "Thumbnail ImageDescription"
        /// "Thumbnail EquipMake"
        /// "Thumbnail EquipModel"
        /// "Thumbnail StripOffsets"
        /// "Thumbnail Orientation"
        /// "Thumbnail SamplesPerPixel"
        /// "Thumbnail RowsPerStrip"
        /// "Thumbnail StripBytesCount"
        /// "Thumbnail ResolutionX"
        /// "Thumbnail ResolutionY"
        /// "Thumbnail PlanarConfig"
        /// "Thumbnail ResolutionUnit"
        /// "Thumbnail TransferFunction"
        /// "Thumbnail SoftwareUsed"
        /// "Thumbnail DateTime"
        /// "Thumbnail Artist"
        /// "Thumbnail WhitePoint"
        /// "Thumbnail PrimaryChromaticities"
        /// "Thumbnail YCbCrCoefficients"
        /// "Thumbnail YCbCrSubsampling"
        /// "Thumbnail YCbCrPositioning"
        /// "Thumbnail RefBlackWhite"
        /// "Thumbnail CopyRight"
        /// "Luminance Table"
        /// "Chrominance Table"
        /// "Frame Delay"
        /// "Loop Count"
        /// "Pixel Unit"
        /// "Pixel PerUnit X"
        /// "Pixel PerUnit Y"
        /// "Palette Histogram"
        /// "Exposure Time"
        /// "F-Number"
        /// "Exposure Prog"
        /// "Spectral Sense"
        /// "ISO Speed"
        /// "OECF"
        /// "Ver"
        /// "DTOrig"
        /// "DTDigitized"
        /// "CompConfig"
        /// "CompBPP"
        /// "Shutter Speed"
        /// "Aperture"
        /// "Brightness"
        /// "Exposure Bias"
        /// "MaxAperture"
        /// "SubjectDist"
        /// "Metering Mode"
        /// "LightSource"
        /// "Flash"
        /// "FocalLength"
        /// "Maker Note"
        /// "User Comment"
        /// "DTSubsec"
        /// "DTOrigSS"
        /// "DTDigSS"
        /// "FPXVer"
        /// "ColorSpace"
        /// "PixXDim"
        /// "PixYDim"
        /// "RelatedWav"
        /// "Interop"
        /// "FlashEnergy"
        /// "SpatialFR"
        /// "FocalXRes"
        /// "FocalYRes"
        /// "FocalResUnit"
        /// "Subject Loc"
        /// "Exposure Index"
        /// "Sensing Method"
        /// "FileSource"
        /// "SceneType"
        /// "CfaPattern"
        /// "Gps Ver"
        /// "Gps LatitudeRef"
        /// "Gps Latitude"
        /// "Gps LongitudeRef"
        /// "Gps Longitude"
        /// "Gps AltitudeRef"
        /// "Gps Altitude"
        /// "Gps GpsTime"
        /// "Gps GpsSatellites"
        /// "Gps GpsStatus"
        /// "Gps GpsMeasureMode"
        /// "Gps GpsDop"
        /// "Gps SpeedRef"
        /// "Gps Speed"
        /// "Gps TrackRef"
        /// "Gps Track"
        /// "Gps ImgDirRef"
        /// "Gps ImgDir"
        /// "Gps MapDatum"
        /// "Gps DestLatRef"
        /// "Gps DestLat"
        /// "Gps DestLongRef"
        /// "Gps DestLong"
        /// "Gps DestBearRef"
        /// "Gps DestBear"
        /// "Gps DestDistRef"
        /// "Gps DestDist"
        /// </summary>
        public object this[string index]
        {
            get
            {
                return properties[index];
            }
        }
        //
        private System.Drawing.Bitmap bmp;
        //
        private string data;
        //
        private translation myHash;
        //
        public Hashtable properties;
        //
        internal int Count
        {
            get
            {
                return this.properties.Count;
            }
        }
        //
        string sp;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="len"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void setTag(int id, string data)
        {
            Encoding ascii = Encoding.ASCII;
            this.setTag(id, data.Length, 0x2, ascii.GetBytes(data));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="len"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void setTag(int id, int len, short type, byte[] data)
        {
            PropertyItem p = CreatePropertyItem(type, id, len, data);
            this.bmp.SetPropertyItem(p);
            buildDB(this.bmp.PropertyItems);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tag"></param>
        /// <param name="len"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static PropertyItem CreatePropertyItem(short type, int tag, int len, byte[] value)
        {
            PropertyItem item;

            // Loads a PropertyItem from a Jpeg image stored in the assembly as a resource.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream emptyBitmapStream = assembly.GetManifestResourceStream("EXIFextractor.decoy.jpg");
            System.Drawing.Image empty = System.Drawing.Image.FromStream(emptyBitmapStream);

            item = empty.PropertyItems[0];

            // Copies the data to the property item.
            item.Type = type;
            item.Len = len;
            item.Id = tag;
            item.Value = new byte[value.Length];
            value.CopyTo(item.Value, 0);

            return item;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="sp"></param>
        public EXIFextractor(ref System.Drawing.Bitmap bmp, string sp)
        {
            properties = new Hashtable();
            //
            this.bmp = bmp;
            this.sp = sp;
            //
            myHash = new translation();
            buildDB(this.bmp.PropertyItems);
        }
        string msp = "";
        public EXIFextractor(ref System.Drawing.Bitmap bmp, string sp, string msp)
        {
            properties = new Hashtable();
            this.sp = sp;
            this.msp = msp;
            this.bmp = bmp;
            //				
            myHash = new translation();
            this.buildDB(bmp.PropertyItems);

        }
        public static PropertyItem[] GetExifProperties(string fileName)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream,
                /* useEmbeddedColorManagement = */ true,
                /* validateImageData = */ false);
            return image.PropertyItems;
        }
        public EXIFextractor(string file, string sp, string msp)
        {
            properties = new Hashtable();
            this.sp = sp;
            this.msp = msp;

            myHash = new translation();
            //				
            this.buildDB(GetExifProperties(file));

        }

        /// <summary>
        /// 
        /// </summary>
        private void buildDB(System.Drawing.Imaging.PropertyItem[] parr)
        {
            properties.Clear();
            //
            data = "";
            //
            Encoding ascii = Encoding.ASCII;
            //
            foreach (System.Drawing.Imaging.PropertyItem p in parr)
            {
                string v = "";
                string name = (string)myHash[p.Id];
                // tag not found. skip it
                if (name == null) continue;
                //
                data += name + ": ";
                //
                //1 = BYTE An 8-bit unsigned integer.,
                if (p.Type == 0x1)
                {
                    v = p.Value[0].ToString();
                }
                //2 = ASCII An 8-bit byte containing one 7-bit ASCII code. The final byte is terminated with NULL.,
                else if (p.Type == 0x2)
                {
                    // string					
                    v = ascii.GetString(p.Value);
                }
                //3 = SHORT A 16-bit (2 -byte) unsigned integer,
                else if (p.Type == 0x3)
                {
                    // orientation // lookup table					
                    switch (p.Id)
                    {
                        case 0x8827: // ISO
                            v = "ISO-" + convertToInt16U(p.Value).ToString();
                            break;
                        case 0xA217: // sensing method
                            {
                                switch (convertToInt16U(p.Value))
                                {
                                    case 1: v = "Not defined"; break;
                                    case 2: v = "One-chip color area sensor"; break;
                                    case 3: v = "Two-chip color area sensor"; break;
                                    case 4: v = "Three-chip color area sensor"; break;
                                    case 5: v = "Color sequential area sensor"; break;
                                    case 7: v = "Trilinear sensor"; break;
                                    case 8: v = "Color sequential linear sensor"; break;
                                    default: v = " reserved"; break;
                                }
                            }
                            break;
                        case 0x8822: // aperture 
                            switch (convertToInt16U(p.Value))
                            {
                                case 0: v = "Not defined"; break;
                                case 1: v = "Manual"; break;
                                case 2: v = "Normal program"; break;
                                case 3: v = "Aperture priority"; break;
                                case 4: v = "Shutter priority"; break;
                                case 5: v = "Creative program (biased toward depth of field)"; break;
                                case 6: v = "Action program (biased toward fast shutter speed)"; break;
                                case 7: v = "Portrait mode (for closeup photos with the background out of focus)"; break;
                                case 8: v = "Landscape mode (for landscape photos with the background in focus)"; break;
                                default: v = "reserved"; break;
                            }
                            break;
                        case 0x9207: // metering mode
                            switch (convertToInt16U(p.Value))
                            {
                                case 0: v = "unknown"; break;
                                case 1: v = "Average"; break;
                                case 2: v = "CenterWeightedAverage"; break;
                                case 3: v = "Spot"; break;
                                case 4: v = "MultiSpot"; break;
                                case 5: v = "Pattern"; break;
                                case 6: v = "Partial"; break;
                                case 255: v = "Other"; break;
                                default: v = "reserved"; break;
                            }
                            break;
                        case 0x9208: // light source
                            {
                                switch (convertToInt16U(p.Value))
                                {
                                    case 0: v = "unknown"; break;
                                    case 1: v = "Daylight"; break;
                                    case 2: v = "Fluorescent"; break;
                                    case 3: v = "Tungsten"; break;
                                    case 17: v = "Standard light A"; break;
                                    case 18: v = "Standard light B"; break;
                                    case 19: v = "Standard light C"; break;
                                    case 20: v = "D55"; break;
                                    case 21: v = "D65"; break;
                                    case 22: v = "D75"; break;
                                    case 255: v = "other"; break;
                                    default: v = "reserved"; break;
                                }
                            }
                            break;
                        case 0x9209:
                            {
                                switch (convertToInt16U(p.Value))
                                {
                                    case 0: v = "Flash did not fire"; break;
                                    case 1: v = "Flash fired"; break;
                                    case 5: v = "Strobe return light not detected"; break;
                                    case 7: v = "Strobe return light detected"; break;
                                    default: v = "reserved"; break;
                                }
                            }
                            break;
                        default:
                            v = convertToInt16U(p.Value).ToString();
                            break;
                    }
                }
                //4 = LONG A 32-bit (4 -byte) unsigned integer,
                else if (p.Type == 0x4)
                {
                    // orientation // lookup table					
                    v = convertToInt32U(p.Value).ToString();
                }
                //5 = RATIONAL Two LONGs. The first LONG is the numerator and the second LONG expresses the//denominator.,
                else if (p.Type == 0x5)
                {
                    // rational
                    byte[] n = new byte[p.Len / 2];
                    byte[] d = new byte[p.Len / 2];
                    Array.Copy(p.Value, 0, n, 0, p.Len / 2);
                    Array.Copy(p.Value, p.Len / 2, d, 0, p.Len / 2);
                    uint a = convertToInt32U(n);
                    uint b = convertToInt32U(d);
                    Rational r = new Rational(a, b);
                    //
                    //convert here
                    //
                    switch (p.Id)
                    {
                        case 0x9202: // aperture
                            v = "F/" + Math.Round(Math.Pow(Math.Sqrt(2), r.ToDouble()), 2).ToString();
                            break;
                        case 0x920A:
                            v = r.ToDouble().ToString();
                            break;
                        case 0x829A:
                            v = r.ToDouble().ToString();
                            break;
                        case 0x829D: // F-number
                            v = "F/" + r.ToDouble().ToString();
                            break;
                        default:
                            v = r.ToString("/");
                            break;
                    }

                }
                //7 = UNDEFINED An 8-bit byte that can take any value depending on the field definition,
                else if (p.Type == 0x7)
                {
                    switch (p.Id)
                    {
                        case 0xA300:
                            {
                                if (p.Value[0] == 3)
                                {
                                    v = "DSC";
                                }
                                else
                                {
                                    v = "reserved";
                                }
                                break;
                            }
                        case 0xA301:
                            if (p.Value[0] == 1)
                                v = "A directly photographed image";
                            else
                                v = "Not a directly photographed image";
                            break;
                        default:
                            v = "-";
                            break;
                    }
                }
                //9 = SLONG A 32-bit (4 -byte) signed integer (2's complement notation),
                else if (p.Type == 0x9)
                {
                    v = convertToInt32(p.Value).ToString();
                }
                //10 = SRATIONAL Two SLONGs. The first SLONG is the numerator and the second SLONG is the
                //denominator.
                else if (p.Type == 0xA)
                {

                    // rational
                    byte[] n = new byte[p.Len / 2];
                    byte[] d = new byte[p.Len / 2];
                    Array.Copy(p.Value, 0, n, 0, p.Len / 2);
                    Array.Copy(p.Value, p.Len / 2, d, 0, p.Len / 2);
                    int a = convertToInt32(n);
                    int b = convertToInt32(d);
                    Rational r = new Rational(a, b);
                    //
                    // convert here
                    //
                    switch (p.Id)
                    {
                        case 0x9201: // shutter speed
                            v = "1/" + Math.Round(Math.Pow(2, r.ToDouble()), 2).ToString();
                            break;
                        case 0x9203:
                            v = Math.Round(r.ToDouble(), 4).ToString();
                            break;
                        default:
                            v = r.ToString("/");
                            break;
                    }
                }
                // add it to the list
                if (properties[name] == null)
                    properties.Add(name, v);
                // cat it too
                data += v;
                data += this.sp;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return data;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        int convertToInt32(byte[] arr)
        {
            if (arr.Length != 4)
                return 0;
            else
                return arr[3] << 24 | arr[2] << 16 | arr[1] << 8 | arr[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        int convertToInt16(byte[] arr)
        {
            if (arr.Length != 2)
                return 0;
            else
                return arr[1] << 8 | arr[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        uint convertToInt32U(byte[] arr)
        {
            if (arr.Length != 4)
                return 0;
            else
                return Convert.ToUInt32(arr[3] << 24 | arr[2] << 16 | arr[1] << 8 | arr[0]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        uint convertToInt16U(byte[] arr)
        {
            if (arr.Length != 2)
                return 0;
            else
                return Convert.ToUInt16(arr[1] << 8 | arr[0]);
        }
        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            // TODO:  Add EXIFextractor.GetEnumerator implementation
            return (new EXIFextractorEnumerator(this.properties));
        }

        #endregion
    }

    //
    // dont touch this class. its for IEnumerator
    // 
    //
    class EXIFextractorEnumerator : IEnumerator
    {
        Hashtable exifTable;
        IDictionaryEnumerator index;

        internal EXIFextractorEnumerator(Hashtable exif)
        {
            this.exifTable = exif;
            this.Reset();
            index = exif.GetEnumerator();
        }

        #region IEnumerator Members

        public void Reset()
        {
            this.index = null;
        }

        public object Current
        {
            get
            {
                return (new KeyValuePair<object, object>(this.index.Key, this.index.Value));
            }
        }

        public bool MoveNext()
        {
            if (index != null && index.MoveNext())
                return true;
            else
                return false;
        }

        #endregion

    }
    /// <summary>
    /// Summary description for translation.
    /// </summary>
    public class translation : Hashtable
    {
        /// <summary>
        /// 
        /// </summary>
        public translation()
        {
            this.Add(0x8769, "Exif IFD");
            this.Add(0x8825, "Gps IFD");
            this.Add(0xFE, "New Subfile Type");
            this.Add(0xFF, "Subfile Type");
            this.Add(0x100, "Image Width");
            this.Add(0x101, "Image Height");
            this.Add(0x102, "Bits Per Sample");
            this.Add(0x103, "Compression");
            this.Add(0x106, "Photometric Interp");
            this.Add(0x107, "Thresh Holding");
            this.Add(0x108, "Cell Width");
            this.Add(0x109, "Cell Height");
            this.Add(0x10A, "Fill Order");
            this.Add(0x10D, "Document Name");
            this.Add(0x10E, "Image Description");
            this.Add(0x10F, "Equip Make");
            this.Add(0x110, "Equip Model");
            this.Add(0x111, "Strip Offsets");
            this.Add(0x112, "Orientation");
            this.Add(0x115, "Samples PerPixel");
            this.Add(0x116, "Rows Per Strip");
            this.Add(0x117, "Strip Bytes Count");
            this.Add(0x118, "Min Sample Value");
            this.Add(0x119, "Max Sample Value");
            this.Add(0x11A, "X Resolution");
            this.Add(0x11B, "Y Resolution");
            this.Add(0x11C, "Planar Config");
            this.Add(0x11D, "Page Name");
            this.Add(0x11E, "X Position");
            this.Add(0x11F, "Y Position");
            this.Add(0x120, "Free Offset");
            this.Add(0x121, "Free Byte Counts");
            this.Add(0x122, "Gray Response Unit");
            this.Add(0x123, "Gray Response Curve");
            this.Add(0x124, "T4 Option");
            this.Add(0x125, "T6 Option");
            this.Add(0x128, "Resolution Unit");
            this.Add(0x129, "Page Number");
            this.Add(0x12D, "Transfer Funcition");
            this.Add(0x131, "Software Used");
            this.Add(0x132, "Date Time");
            this.Add(0x13B, "Artist");
            this.Add(0x13C, "Host Computer");
            this.Add(0x13D, "Predictor");
            this.Add(0x13E, "White Point");
            this.Add(0x13F, "Primary Chromaticities");
            this.Add(0x140, "ColorMap");
            this.Add(0x141, "Halftone Hints");
            this.Add(0x142, "Tile Width");
            this.Add(0x143, "Tile Length");
            this.Add(0x144, "Tile Offset");
            this.Add(0x145, "Tile ByteCounts");
            this.Add(0x14C, "InkSet");
            this.Add(0x14D, "Ink Names");
            this.Add(0x14E, "Number Of Inks");
            this.Add(0x150, "Dot Range");
            this.Add(0x151, "Target Printer");
            this.Add(0x152, "Extra Samples");
            this.Add(0x153, "Sample Format");
            this.Add(0x154, "S Min Sample Value");
            this.Add(0x155, "S Max Sample Value");
            this.Add(0x156, "Transfer Range");
            this.Add(0x200, "JPEG Proc");
            this.Add(0x201, "JPEG InterFormat");
            this.Add(0x202, "JPEG InterLength");
            this.Add(0x203, "JPEG RestartInterval");
            this.Add(0x205, "JPEG LosslessPredictors");
            this.Add(0x206, "JPEG PointTransforms");
            this.Add(0x207, "JPEG QTables");
            this.Add(0x208, "JPEG DCTables");
            this.Add(0x209, "JPEG ACTables");
            this.Add(0x211, "YCbCr Coefficients");
            this.Add(0x212, "YCbCr Subsampling");
            this.Add(0x213, "YCbCr Positioning");
            this.Add(0x214, "REF Black White");
            this.Add(0x8773, "ICC Profile");
            this.Add(0x301, "Gamma");
            this.Add(0x302, "ICC Profile Descriptor");
            this.Add(0x303, "SRGB RenderingIntent");
            this.Add(0x320, "Image Title");
            this.Add(0x8298, "Copyright");
            this.Add(0x5001, "Resolution X Unit");
            this.Add(0x5002, "Resolution Y Unit");
            this.Add(0x5003, "Resolution X LengthUnit");
            this.Add(0x5004, "Resolution Y LengthUnit");
            this.Add(0x5005, "Print Flags");
            this.Add(0x5006, "Print Flags Version");
            this.Add(0x5007, "Print Flags Crop");
            this.Add(0x5008, "Print Flags Bleed Width");
            this.Add(0x5009, "Print Flags Bleed Width Scale");
            this.Add(0x500A, "Halftone LPI");
            this.Add(0x500B, "Halftone LPIUnit");
            this.Add(0x500C, "Halftone Degree");
            this.Add(0x500D, "Halftone Shape");
            this.Add(0x500E, "Halftone Misc");
            this.Add(0x500F, "Halftone Screen");
            this.Add(0x5010, "JPEG Quality");
            this.Add(0x5011, "Grid Size");
            this.Add(0x5012, "Thumbnail Format");
            this.Add(0x5013, "Thumbnail Width");
            this.Add(0x5014, "Thumbnail Height");
            this.Add(0x5015, "Thumbnail ColorDepth");
            this.Add(0x5016, "Thumbnail Planes");
            this.Add(0x5017, "Thumbnail RawBytes");
            this.Add(0x5018, "Thumbnail Size");
            this.Add(0x5019, "Thumbnail CompressedSize");
            this.Add(0x501A, "Color Transfer Function");
            this.Add(0x501B, "Thumbnail Data");
            this.Add(0x5020, "Thumbnail ImageWidth");
            this.Add(0x502, "Thumbnail ImageHeight");
            this.Add(0x5022, "Thumbnail BitsPerSample");
            this.Add(0x5023, "Thumbnail Compression");
            this.Add(0x5024, "Thumbnail PhotometricInterp");
            this.Add(0x5025, "Thumbnail ImageDescription");
            this.Add(0x5026, "Thumbnail EquipMake");
            this.Add(0x5027, "Thumbnail EquipModel");
            this.Add(0x5028, "Thumbnail StripOffsets");
            this.Add(0x5029, "Thumbnail Orientation");
            this.Add(0x502A, "Thumbnail SamplesPerPixel");
            this.Add(0x502B, "Thumbnail RowsPerStrip");
            this.Add(0x502C, "Thumbnail StripBytesCount");
            this.Add(0x502D, "Thumbnail ResolutionX");
            this.Add(0x502E, "Thumbnail ResolutionY");
            this.Add(0x502F, "Thumbnail PlanarConfig");
            this.Add(0x5030, "Thumbnail ResolutionUnit");
            this.Add(0x5031, "Thumbnail TransferFunction");
            this.Add(0x5032, "Thumbnail SoftwareUsed");
            this.Add(0x5033, "Thumbnail DateTime");
            this.Add(0x5034, "Thumbnail Artist");
            this.Add(0x5035, "Thumbnail WhitePoint");
            this.Add(0x5036, "Thumbnail PrimaryChromaticities");
            this.Add(0x5037, "Thumbnail YCbCrCoefficients");
            this.Add(0x5038, "Thumbnail YCbCrSubsampling");
            this.Add(0x5039, "Thumbnail YCbCrPositioning");
            this.Add(0x503A, "Thumbnail RefBlackWhite");
            this.Add(0x503B, "Thumbnail CopyRight");
            this.Add(0x5090, "Luminance Table");
            this.Add(0x5091, "Chrominance Table");
            this.Add(0x5100, "Frame Delay");
            this.Add(0x5101, "Loop Count");
            this.Add(0x5110, "Pixel Unit");
            this.Add(0x5111, "Pixel PerUnit X");
            this.Add(0x5112, "Pixel PerUnit Y");
            this.Add(0x5113, "Palette Histogram");
            this.Add(0x829A, "Exposure Time");
            this.Add(0x829D, "F-Number");
            this.Add(0x8822, "Exposure Prog");
            this.Add(0x8824, "Spectral Sense");
            this.Add(0x8827, "ISO Speed");
            this.Add(0x8828, "OECF");
            this.Add(0x9000, "Ver");
            this.Add(0x9003, "DTOrig");
            this.Add(0x9004, "DTDigitized");
            this.Add(0x9101, "CompConfig");
            this.Add(0x9102, "CompBPP");
            this.Add(0x9201, "Shutter Speed");
            this.Add(0x9202, "Aperture");
            this.Add(0x9203, "Brightness");
            this.Add(0x9204, "Exposure Bias");
            this.Add(0x9205, "MaxAperture");
            this.Add(0x9206, "SubjectDist");
            this.Add(0x9207, "Metering Mode");
            this.Add(0x9208, "LightSource");
            this.Add(0x9209, "Flash");
            this.Add(0x920A, "FocalLength");
            this.Add(0x927C, "Maker Note");
            this.Add(0x9286, "User Comment");
            this.Add(0x9290, "DTSubsec");
            this.Add(0x9291, "DTOrigSS");
            this.Add(0x9292, "DTDigSS");
            this.Add(0xA000, "FPXVer");
            this.Add(0xA001, "ColorSpace");
            this.Add(0xA002, "PixXDim");
            this.Add(0xA003, "PixYDim");
            this.Add(0xA004, "RelatedWav");
            this.Add(0xA005, "Interop");
            this.Add(0xA20B, "FlashEnergy");
            this.Add(0xA20C, "SpatialFR");
            this.Add(0xA20E, "FocalXRes");
            this.Add(0xA20F, "FocalYRes");
            this.Add(0xA210, "FocalResUnit");
            this.Add(0xA214, "Subject Loc");
            this.Add(0xA215, "Exposure Index");
            this.Add(0xA217, "Sensing Method");
            this.Add(0xA300, "FileSource");
            this.Add(0xA301, "SceneType");
            this.Add(0xA302, "CfaPattern");
            this.Add(0x0, "Gps Ver");
            this.Add(0x1, "Gps LatitudeRef");
            this.Add(0x2, "Gps Latitude");
            this.Add(0x3, "Gps LongitudeRef");
            this.Add(0x4, "Gps Longitude");
            this.Add(0x5, "Gps AltitudeRef");
            this.Add(0x6, "Gps Altitude");
            this.Add(0x7, "Gps GpsTime");
            this.Add(0x8, "Gps GpsSatellites");
            this.Add(0x9, "Gps GpsStatus");
            this.Add(0xA, "Gps GpsMeasureMode");
            this.Add(0xB, "Gps GpsDop");
            this.Add(0xC, "Gps SpeedRef");
            this.Add(0xD, "Gps Speed");
            this.Add(0xE, "Gps TrackRef");
            this.Add(0xF, "Gps Track");
            this.Add(0x10, "Gps ImgDirRef");
            this.Add(0x11, "Gps ImgDir");
            this.Add(0x12, "Gps MapDatum");
            this.Add(0x13, "Gps DestLatRef");
            this.Add(0x14, "Gps DestLat");
            this.Add(0x15, "Gps DestLongRef");
            this.Add(0x16, "Gps DestLong");
            this.Add(0x17, "Gps DestBearRef");
            this.Add(0x18, "Gps DestBear");
            this.Add(0x19, "Gps DestDistRef");
            this.Add(0x1A, "Gps DestDist");
        }
    }
    /// <summary>
    /// private class
    /// </summary>
    internal class Rational
    {
        private int n;
        private int d;
        public Rational(int n, int d)
        {
            this.n = n;
            this.d = d;
            simplify(ref this.n, ref this.d);
        }
        public Rational(uint n, uint d)
        {
            this.n = Convert.ToInt32(n);
            this.d = Convert.ToInt32(d);

            simplify(ref this.n, ref this.d);
        }
        public Rational()
        {
            this.n = this.d = 0;
        }
        public string ToString(string sp)
        {
            if (sp == null) sp = "/";
            return n.ToString() + sp + d.ToString();
        }
        public double ToDouble()
        {
            if (d == 0)
                return 0.0;

            return Math.Round(Convert.ToDouble(n) / Convert.ToDouble(d), 2);
        }
        private void simplify(ref int a, ref int b)
        {
            if (a == 0 || b == 0)
                return;

            int gcd = euclid(a, b);
            a /= gcd;
            b /= gcd;
        }
        private int euclid(int a, int b)
        {
            if (b == 0)
                return a;
            else
                return euclid(b, a % b);
        }
    }
    #endregion
}
