using System;
using DA;
using BO;


namespace BOF
{
    public interface ILoadFilterForm
    {
        BaseLoadFilter GetFilter();
        void Init(BaseLoadFilter flt);
        event EventHandler CloseCancel;
        event EventHandler CloseApply;

        void Show();
        //void ResetToDefault();

    }
}
