using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using BO;
using System.Collections;
using DA;

namespace BOF
{
    // Устарел, использовать только для "старых" классов типа HDWrapper
    public class DetailsListController<DD,DS,HD>: ListController
       where DD : BaseDat<DD>, new()
       where DS : BaseSet<DD, DS>, new()
       where HD: BaseDat<HD>, new()
    {
        public DetailsListController(HDWrapper<HD> header)
            : base()
        {
            _header = header;
        }
        private HDWrapper<HD> _header = null;
        public void Reload()
        {
            //DetailsWrapper<DD, DS, HD> det = (DetailsWrapper<DD, DS, HD>)_header.Details[typeof(DS)];
            IDetailsWrapper det = _header.Details[typeof(DS)];
            //det.Load();
            DS detSet = (DS)det.DetSet;
            Grid.DataSource = detSet;
        }
        protected override void Add()
        {
            try
            {
                base.Add();
                //EntityForm.CanSave = false;
                //DD dat = new DD();
                //((IDetailDat<HD>)dat).Header = _header.Header;
                //EntityForm.OldValue = dat;
                //EntityForm.BindControls(EntityForm);

                //if (EntityForm.ShowDialog() == DialogResult.OK)
                //{
                //    DS set = ((DS)Grid.DataSource);
                //    set.Add((DD)EntityForm.NewValue);
                //    Grid.SetDataSource(set);
                //}
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void Edit(BaseDat dat)
        {
            try
            {
                base.Edit(dat);
                //EntityForm.CanSave = false;
                //EntityForm.OldValue = dat;
                //EntityForm.BindControls(EntityForm);
                //if (EntityForm.ShowDialog() == DialogResult.OK)
                //{
                //    DS set = ((DS)Grid.DataSource);
                //    int index = set.IndexOf(dat);
                //    set[index] = EntityForm.OldValue;
                //    Grid.SetDataSource(set);
                //}
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        protected override void Delete(BaseDat dat)
        {
            try
            {
                //BaseDat dat = (BaseDat)Grid.Value;
                DS set = ((DS)Grid.DataSource);
                set.Remove(dat);
                Grid.SetDataSource(set);

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Common.ExMessage(Ex), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}