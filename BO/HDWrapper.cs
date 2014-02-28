using System;
using System.Collections.Generic;
using System.Text;
using DA;
using System.Xml.Serialization;

namespace BO
{
    public class HDWrapper<HD> : BaseDat<HD>, IHDWrapper
        where HD : BaseDat<HD>, new()
    {
        public HDWrapper(Dictionary<Type, IDetailsWrapper> details, HD header)
        {
            _details = details;
            _header = header;
        }
        public HDWrapper()
        {
        }

        private Dictionary<Type, IDetailsWrapper> _details = null;
        private HD _header = null;

        [XmlIgnore()]
        public Dictionary<Type, IDetailsWrapper> Details 
        {
            get { return _details; }
            set { _details = value; }
        }
        public HD Header
        {
            get { return _header; }
            set { _header = value; }
        }

        public override object Clone() 
        {
            Dictionary<Type, IDetailsWrapper> cloneDets = new Dictionary<Type, IDetailsWrapper>();

            foreach (Type key in _details.Keys)
            {
                cloneDets.Add(key, (IDetailsWrapper)_details[key].Clone());
            }

            return Activator.CreateInstance(this.GetType(), cloneDets, (HD)Header.Clone());
        }
        
        public override void CopyTo(BaseDat dat)
        {
            HDWrapper<HD> to = (HDWrapper<HD>)dat;

            this.Header.CopyTo(to.Header);
            
            to.Details = new Dictionary<Type, IDetailsWrapper>();
            foreach (Type key in _details.Keys)
            {
                to.Details.Add(key, (IDetailsWrapper)_details[key].Clone());
            }
        }
        public override void Load(params object[] args)
        {
            _header.Load(args);
            LoadDetails();
        }
        public virtual void LoadDetails()
        {
            foreach (IDetailsWrapper det in Details.Values)
            {
                det.Load();
            }
        }
        public override bool EqualDat(BaseDat dat)
        {
            if (dat != null)
            {
                HDWrapper<HD> x = (HDWrapper<HD>)dat;
                return CompareDat(Header, x.Header);
            }
            else
                return false;
        }

        public int Save(BaseDat olddat)
        {
            try
            {
                HDWrapper<HD> old = (HDWrapper<HD>)olddat;
                this.DataAccessor.TransactionBegin();
                int ret = 0;
                if (!old.Header.EqualDat(this.Header))
                    ret = Header.Save();

                foreach (Type key in _details.Keys)
                {
                    Details[key].SaveSet(old.Details[key].DetSet);
                }
                this.DataAccessor.TransactionCommit();
                return ret;
            }
            catch
            {
                this.DataAccessor.TransactionRollback();
                throw;
            }
        }
        public void Reload()
        {
            Load(((IDat)this.Header).ID);
        }
        public override string ToString()
        {
            return _header != null ? _header.ToString() : "";
        }
    }
}
