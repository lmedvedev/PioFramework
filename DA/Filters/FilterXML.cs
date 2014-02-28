using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DA
{
    public class FilterXML : FilterUnitBase
    {
        #region Constructors
        //public FilterXML(){}
       
        #endregion
        #region Properties
        protected XmlDocument doc = new XmlDocument();

        public override string ToString()
        {
            return "('" + doc.InnerXml + "')";
        }
        public XmlDocument XmlDoc { get { return doc; } }
        #endregion
    }
}