using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public interface IValue 
    {
        bool ValueEventDisabled { get; set;}
        event ValueEventHandler ValueChanged;
        object Value { get; set;}
        void FireValueChanged();
    }

    public delegate void ValueEventHandler(object sender, ValueEventArgs e);

    public class ValueEventArgs : EventArgs
    {
        public bool Cancel = false;
        public object Value = null;
        public ValueEventArgs(object value)
        {
            Value = value;
        }
    }
}
