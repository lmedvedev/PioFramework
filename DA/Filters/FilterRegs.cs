using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DA
{
    public class FilterRegs : FilterXML
    {
        #region Constructors
        public FilterRegs(object Account, params object[] cards)
        {
            int BA_id = 0;
            string BA_Path = "";

            if (Account is int)
                BA_id = (int)Account;
            else if (Account is string)
                BA_Path = (string)Account;
            else
                throw new Exception(string.Format("Ошибка: неправильно задан BaseAccount!"));

            XmlNode regNode = doc.CreateElement("RegFilter");
            doc.AppendChild(regNode);
            if (BA_id != 0)
            {
                XmlAttribute baAcc_Id = doc.CreateAttribute("BaseAccount_id");
                baAcc_Id.Value = BA_id.ToString();
                regNode.Attributes.Append(baAcc_Id);
            }
            else if (BA_Path != "")
            {
                XmlAttribute baAcc_FP = doc.CreateAttribute("BaseAccount_FP");
                baAcc_FP.Value = BA_Path;
                regNode.Attributes.Append(baAcc_FP);
            }

            foreach (object card in cards)
            {
                createFilter(card, regNode);
            }
        }
        #endregion
        #region Properties
        #endregion
        private void createFilter(object obj, XmlNode regNode)
        {

            if (obj == null) return;
             
            int id = 0;
            string path = "";
            if (obj is int)
                id = (int)obj;
            else if (obj is string)
                path = (string)obj;
            else
                throw new Exception(string.Format("Ошибка: неправильно задан параметр!"));

            XmlNode cardNode = doc.CreateElement("Card");
            regNode.AppendChild(cardNode);
            if (id != 0)
            {
                XmlAttribute card_id = doc.CreateAttribute("id");
                card_id.Value = id.ToString();
                cardNode.Attributes.Append(card_id);
            }
            else if (path != "")
            {
                XmlAttribute card_fp = doc.CreateAttribute("FP");
                card_fp.Value = path;
                cardNode.Attributes.Append(card_fp);
            }
        }
    }
}