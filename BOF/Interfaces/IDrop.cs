using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface IDrop : IValue
    {
        DropAlignment DropAlign { get;set;}
        Control DropControl();
    }

    public enum DropAlignment { Left = 0, Right = 1, Fit = 2 }
}
