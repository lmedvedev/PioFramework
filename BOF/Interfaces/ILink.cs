using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface ILink
    {
        event EventHandler LinkClicked;
        void FireLinkClicked();
    }
}
