using System;
using System.Collections.Generic;
using System.Text;
using DA;

namespace BO
{
    public abstract class BaseLoadFilter
    {
        public BaseLoadFilter()
        {
            //ResetToDefault();
        }

        public abstract IDAOFilter GetFilter();

        public override string ToString()
        {
            return GetFilter().ToString();
        }

        public virtual void ResetToDefault()
        {
        }
    }
}
