using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public interface IHelpLabel
    {
        void WriteHelp(string text);
        void SetError(Control ctl, string description);
    }
}
