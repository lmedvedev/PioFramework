using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public partial class CtlIntString : CtlString
    {
        public CtlIntString()
        {
        }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        new public int Value
        {
            get 
            {
                int ret = 0;
                try
                {
                    ret = int.Parse(Text);
                }
                catch { }
                return ret; 
            }
            set
            {
                Text = value.ToString();
            }
        }
    }
}
