using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public interface IDropDownControl
    {
        DataGridView Grid { get;}
        event EventHandler<EventArgs<int>> RowSelected;
        event EventHandler<EventArgs<int>> RowEntered;
        event EventHandler DropDownClosed;
        void WriteText(string text);
    }
    public interface IChooserController
    {
    }
}
