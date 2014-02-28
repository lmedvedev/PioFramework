using System;
using System.Collections.Generic;
using System.Text;
using BOF;
using DA;
using BO;

namespace BOF
{
    public class SysAuditFilter : BaseLoadFilter
    {
        public SysAuditFilter() { }

        public override IDAOFilter GetFilter()
        {
            IDataAccess da = SysAuditDat.ClassDataAccessor;
            IDAOFilter f = da.NewFilter();

            f.AddWhere(new FilterDateFromTo(SysAuditColumns.Dt, DateFrom, DateTo.AddDays(1)));

            if (!string.IsNullOrEmpty(Tbl))
            {
                Tbl = "%" + Tbl + "%";
                f.AddWhere(new FilterString(SysAuditColumns.Tbl, Tbl, true));
            }

            if (ID != 0)
            {
                //f.AddWhere(new FilterID(SysAuditColumns.TblID, ID));
                f.AddWhere(new FilterCustom(string.Format("OpContent.value('/*[1]/@id', 'int') = {0}", ID)));
            }

            if (!string.IsNullOrEmpty(User))
            {
                User = "%" + User + "%";
                f.AddWhere(new FilterString(SysAuditColumns.Usr, User, true));
            }

            if (!string.IsNullOrEmpty(Host))
            {
                Host = "%" + Host + "%";
                f.AddWhere(new FilterString(SysAuditColumns.ClientHost, Host, true));
            }

            if (!string.IsNullOrEmpty(Application))
            {
                Application = "%" + Application + "%";
                f.AddWhere(new FilterString(SysAuditColumns.ClientApplication, Application, true));
            }

            if (!string.IsNullOrEmpty(Server))
            {
                Server = "%" + Server + "%";
                f.AddWhere(new FilterString(SysAuditColumns.Srv, Server, true));
            }

            if (!string.IsNullOrEmpty(Base))
            {
                Base = "%" + Base + "%";
                f.AddWhere(new FilterString(SysAuditColumns.Db, Base, true));
            }

            return f;
        }

        private DateTime _DateFrom = DateTime.MinValue;
        private DateTime _DateTo = DateTime.MinValue;
        private string _Tbl;
        private int _ID;
        private string _User;
        private string _Host;
        private string _Application;
        private string _Server;
        private string _Base;

        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { _DateFrom = value; }
        }
        public DateTime DateTo
        {
            get { return _DateTo; }
            set { _DateTo = value; }
        }
        public string Tbl
        {
            get { return _Tbl; }
            set { _Tbl = value; }
        }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }
        public string Host
        {
            get { return _Host; }
            set { _Host = value; }
        }
        public string Application
        {
            get { return _Application; }
            set { _Application = value; }
        }
        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
        public string Base
        {
            get { return _Base; }
            set { _Base = value; }
        }
    }
}
