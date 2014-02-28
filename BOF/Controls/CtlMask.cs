using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace BOF
{
    public partial class CtlMask : CtlString
    {
        public CtlMask() 
        {
        }

        public override string Value
        {
            get
            {
                return base.Value;
            }
            set
            {
                base.Value = value;
            }
        }

        private string _MaskedValue;
        public string MaskedValue
        {
            get { return _MaskedValue; }
            set
            {
                _MaskedValue = value;
                UpdateText();
            }
        }

        private string _Mask;
        public string Mask
        {
            get { return _Mask; }
            set 
            { 
                _Mask = value; 
                UpdateText();
            }
        }

        private void UpdateText()
        {
        }
    }
}
