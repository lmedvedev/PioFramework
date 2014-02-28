using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public interface IViewSet
    {
        BaseDat GetDat(int index);
        DataGridViewColumn[] GetColumns();
        int IndexOf(object value);
        void Fill(IList list);
    }
}
