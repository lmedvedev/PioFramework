using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;

namespace BOF
{
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class LineDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                return SelectionRules.Moveable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable | SelectionRules.Visible;// | SelectionRules.Moveable | SelectionRules.Visible;
            }
        }
    }
}
