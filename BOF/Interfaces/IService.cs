using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface IService 
    {
        void Initialize(object sender, EventArgs e);
        void Dispose(object sender, EventArgs e);
    }
}
