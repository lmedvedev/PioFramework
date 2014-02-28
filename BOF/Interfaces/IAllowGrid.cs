using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BOF
{
    public interface IAllowGrid
    {
        System.Windows.Forms.DataGridViewColumn[] GetColumns();
    }
}
