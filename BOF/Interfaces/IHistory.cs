using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface IHistory
    {
        string[] GetHistoryList();
        void HistorySelected(string value);
    }
}
