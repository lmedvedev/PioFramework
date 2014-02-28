using System;
using System.Collections.Generic;
using System.Text;

namespace BOF
{
    public interface ICaption
    {
        event EventHandler CaptionChanged;
        CaptionAlignment CaptionAlign { get;set;}
        string Caption { get;set;}
    }

    public enum CaptionAlignment { LeftTop = 0, MiddleLeft = 1, TopLeft = 2, TopCenter = 3, TopRight = 4, RightTop = 5, MiddleRight = 6 }
}
