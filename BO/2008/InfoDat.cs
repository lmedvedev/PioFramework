using System;
using System.Collections.Generic;
using System.Text;
using BO;
//using PifBase;
using System.Xml;
using System.IO;
using BO.Xml;
using System.Data.Common;

namespace BO
{
    public partial class InfoDat<T> where T : class, new()
    {
        static InfoDat()
        {
            converter = new XmlConverter();
        }
        static void Init(string schema)
        {
            converter.AddSchema(schema);
        }
        static XmlConverter converter;

        #region Extra Fields
        private T _Info;
        #endregion

        #region Extra Members
        public T Info
        {
            get { return _Info; }
            set 
            {
                if (_Info != value)
                {
                    _Info = value;
                }
            }
        }
        #endregion

        public object Clone()
        {
            InfoDat<T> clone = (InfoDat<T>)base.MemberwiseClone();
            clone.Info = this.Info;
            return clone;
        }
    }

    public interface IDatInfo<T> where T : InfoDat<T>, new()
    {
        InfoDat<T> Info { get;set;}
    }

    public interface IDatInfo
    {
        object GetInfo();
    }
    
}
