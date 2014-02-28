using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Collections;


namespace BOF
{
    public delegate void DatEventDelegate(object sender, DatEventArgs e);
    public delegate void DatEventDelegate<DAT>(object sender, DatEventArgs<DAT> e)
        where DAT : BaseDat<DAT>, new();

    public delegate void ObjEventDelegate(object sender, ValueEventArgs e);

    public interface ICtlGrid
    {
        event DatEventDelegate RowChanged;
        event DatEventDelegate AskNewRow;

        void Select(DataRow row);
        //ISetList SetList { get;}
    }
    public class DatEventArgs : EventArgs
    {
        public BaseDat DatEntity = null;
        public DatEventArgs(BaseDat dat)
        {
            DatEntity = dat;
        }
    }
    public class DatEventArgs<DAT> : EventArgs
            where DAT : BaseDat<DAT>, new()
    {
        public DAT DatEntity = null;
        public DatEventArgs(DAT dat)
        {
            DatEntity = dat;
        }
    }
}
