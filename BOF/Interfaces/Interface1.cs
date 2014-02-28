using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface IDataMember
    {
        string DataMember { get;set;}
        void AddBinding(object datasource);
        void AddBinding();
        //void RemoveBinding(object datasource);
        void RemoveBinding();
        void WriteValue();
        event EventHandler ValueChanged;
    }
}
