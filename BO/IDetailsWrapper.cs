using System;
using System.Collections.Generic;
namespace BO
{
    public interface IDetailsWrapper
    {
        object Clone();
        BaseSet DetSet { get; set; }
        BaseDat Header { get; set; }
        void Load();
        void SaveSet(BaseSet setOld);
    }

    public interface IDetailsWrapper<DD, DS, HD> : IDetailsWrapper<HD>
        where DD : BaseDat<DD>, new()
        where DS : BaseSet<DD, DS>, new()
        where HD : BaseDat<HD>, new()
    {
        void Load();
        void SaveSet(DS setOld);
    }
    public interface IDetailsWrapper<HD>
        where HD : BaseDat<HD>//, new()
    {
        object Clone(HD header);
        HD Header { get;set;}
    }
}
