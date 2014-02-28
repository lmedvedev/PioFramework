using System;
using System.Collections.Generic;
using System.Text;
using DA;
using System.ComponentModel;

namespace BO
{
    public class DetailsWrapper<DD, DS, HD> : IDetailsWrapper<DD, DS, HD>
        where DD : BaseDat<DD>, IDetailDat<HD>, new()
        where DS : BaseSet<DD, DS>, new()
        where HD : BaseDat<HD>, IDat, new()
    {
        public DetailsWrapper(HD hdr)
        {
            _header = hdr;
            _detSet = new DS();
        }

        private DS _detSet = null;
        private HD _header = null;

        public HD Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public DS DetSet
        {
            get { return _detSet; }
            set { _detSet = value; }
        }

        public void Load()
        {
            //DetSet.LoadFilter.Reset();
            //DetSet.LoadFilter.AddWhere(new FilterID("Header_id", header.ID));
            /*
             * Хорошо бы сделать так, чтобы запроса к таблице c Header
             * вообще не было. Для этого надо что-то переписать в базовом load
             * или переопределить его здесь
             * 2007-02-14 Андрей
             * 
             * Сделал для set-классов. Для Dat - пока по-прежнему
             * 2007-2-15 Андрей
             */
            DetSet.LoadByHeaderDat(_header);

            foreach (DD dat in DetSet)
            {
                dat.Header = _header;
            }
        }
        /// <summary>
        /// Добавление details-класса.
        /// Надо использовать вместо DetSet.Add(dat), так как здесь еще прописывается 
        /// header.
        /// </summary>
        /// <param name="dat"></param>
        /// <example>
        /// Пример:
        /// <code>
        /// BaseAccount_CardsDat detBA = new BaseAccount_CardsDat();
        /// detBA.CardType = new CardTypesDat(9);
        /// acc.Details.Add(detBA);
        /// </code>
        /// </example>
        public void Add(DD dat)
        {
            dat.Header = _header;
            DetSet.Add(dat);
        }

        public void Remove(DD dat)
        {
            DetSet.Remove(dat);
        }

        public void Save()
        {
            if (_header == null)
                return;
            try
            {
                _header.DataAccessor.TransactionBegin();
                // Надо оптимизировать! Если просто загружать Set -класс, 
                // то он грузит и все возможные подклассы.
                // Здесь же достаточно лишь одной базовой таблицы - только 
                // для сравнения с сохраняемым set-классом 
                // 2007-02-14 Андрей
                DS old = new DS();
                //old.LoadFilter.AddWhere(new FilterID("Header_id", header.ID));
                old.LoadByHeaderDat(_header);
                SaveSet(old);
                _header.DataAccessor.TransactionCommit();
            }
            catch (Exception ex)
            {
                _header.DataAccessor.TransactionRollback();
                throw new Exception("Ошибка при сохранении Details для " + _header.ToString(), ex);
            }
        }

        public void SaveSet(DS setOld)
        {
            List<DD> added = GetAdded();
            foreach (DD addedDat in added)
                addedDat.Save();

            List<DD> changed = GetChanged(setOld);
            foreach (DD changeDat in changed)
                changeDat.Save();

            List<DD> deleted = GetDeleted(setOld);
            foreach (DD deletedDat in deleted)
                deletedDat.Delete();
        }

        private List<DD> GetAdded()
        {
            List<DD> added = new List<DD>();
            foreach (DD dat in DetSet)
            {
                if (dat.IsNew)
                    added.Add(dat);
            }
            return added;
        }
        private List<DD> GetDeleted(DS setOld)
        {
            List<DD> deleted = new List<DD>();
            foreach (DD dat in setOld)
            {
                if (Find(dat) == null)
                    deleted.Add(dat);
            }
            return deleted;
        }
        private List<DD> GetChanged(DS setOld)
        {
            List<DD> changed = new List<DD>();
            foreach (DD old in setOld)
            {
                DD dat = Find(old);
                if (dat != null && !old.EqualDat(dat))
                    changed.Add(dat);
            }
            return changed;
        }

        private DD Find(DD old)
        {
            foreach (DD dat in DetSet)
            {
                if (dat.ID == old.ID)
                    return dat;
            }
            return null;
        }

        public object Clone(HD cloneHeader)
        {
            DetailsWrapper<DD, DS, HD> clone = new DetailsWrapper<DD, DS, HD>(cloneHeader);
            clone.DetSet = this.DetSet.Clone() as DS;
            return clone;
        }


        public override bool Equals(object obj)
        {
            DetailsWrapper<DD, DS, HD> dat = obj as DetailsWrapper<DD, DS, HD>;
            if(dat != null)
            {
                int header_id1 = this.Header == null ? 0 : this.Header.ID;
                int header_id2 = dat.Header == null ? 0 : dat.Header.ID;
                if (header_id1 != header_id2) return false;
                if (!DetSet.Equals(dat.DetSet))
                    return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return Header.GetHashCode() + DetSet.GetHashCode();
        }
        public virtual string GetPropsString(params string[] exclusions)
        {
            StringBuilder sb = new StringBuilder();
            // собираем инфу по полям datailsDat-класса
            List<string> list = new List<string>(exclusions);
            List<DatDescriptor> dlst = new List<DatDescriptor>();

            foreach (DatDescriptor dd in BaseDat<DD>.GetRootProperties())
            {
                if (dd.Ordinal >= 0 && !list.Contains(dd.Name))
                    dlst.Add(dd);
            }
            // Сотрируем, чтобы порядок значений был одинаковый
            dlst.Sort(delegate(DatDescriptor dd1, DatDescriptor dd2)
                {
                    return dd1.Ordinal.CompareTo(dd2.Ordinal);
                });

            // Формируем упорядоченный набор datdetails
            List<DD> lstDat = DetSet.ConvertType<DD>();
            lstDat.Sort(delegate(DD dat1, DD dat2)
                {
                    return dat1.CompareTo(dat2);
                });
            foreach (DD dat in lstDat)
            {
                foreach (DatDescriptor dd in dlst)
                {
                    string s = Common.PropValue2String(dd.GetValue(dat), dd.PropertyType).Trim();
                    sb.Append(s);
                }
            }
            return sb.ToString();
        }

    }
}
