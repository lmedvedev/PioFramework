using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;

namespace BOF
{
    public class DatBinding : Binding
    {
        public DatBinding(string propertyName, Object dataSource, string dataMember)
            : base(propertyName, dataSource, dataMember) { }

        public DatBinding(string propertyName, Object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode)
            : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode) { }

        protected override void OnParse(ConvertEventArgs cevent)
        {
            if (cevent.DesiredType == typeof(PathTree))
            {
                if (cevent.Value is string && PathTree.IsPathTree((string)cevent.Value))
                    cevent.Value = new PathTree((string)cevent.Value);
            }
            else if (cevent.DesiredType == typeof(PathCard))
            {
                if (cevent.Value is string && PathCard.IsPathCard((string)cevent.Value))
                    cevent.Value = new PathCard((string)cevent.Value);
            }
            else if (cevent.DesiredType == typeof(Guid))
            {
                if (cevent.Value is string)
                {
                    try
                    {
                        cevent.Value = new Guid((string)cevent.Value);
                    }
                    catch 
                    {
                        cevent.Value = null;
                    }
                }
            }
            else
                base.OnParse(cevent);
        }
    }
}