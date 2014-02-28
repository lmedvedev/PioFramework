using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BO;

namespace BOF
{
    public interface IViewTree : IViewSet
    {
        void Load();
        string Sort
        {
            get;
            set;
        }
        IEnumerable<BaseDat> DatInts { get; }
        BaseSet GetTree();
        //FPath Root { get; set; }
    }
}
