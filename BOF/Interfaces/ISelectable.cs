using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface ISelectable : IValue
    {
        event ValueEventHandler ValueSelected;
        void FireValueSelected();
    }
}
