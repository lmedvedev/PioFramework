using BO;
using System;
using System.Xml;
using System.Drawing;
using System.Data.Common;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BO
{
    public partial class SysAuditDat : BaseDat<SysAuditDat>, IDat
    {
        public SysAuditDat()
        {
        }

        public SysAuditDat(params object[] args)
            :
                base(args)
        {
        }

        static SysAuditDat()
        {
            converter = new XmlConverter();
            string schema = Properties.Resources.SysAudit;
            converter.AddSchema(schema);
        }
        static XmlConverter converter;

        #region Extra Fields
        private Int32 _ID;

        private Int32 _TblID;

        private String _Srv;

        private String _Db;

        private String _Tbl;

        private Int32 _Operation;

        private XmlDocument _OpContent;

        private String _ClientHost;

        private String _ClientApplication;

        private DateTime _Dt;

        private String _Usr;

        private SysAuditInfo _Info;
        private DetailsWrapper<SysAuditDetailsDat, SysAuditDetailsSet, SysAuditDat> _Details = null;
        #endregion
        
        #region Extra Members

        public Int32 TblID
        {
            get { return _TblID; }
            set { _TblID = value; }
        }

        public virtual DateTime DtDate
        {
            get
            {
                return _Dt.Date;
            }
        }
        public static object Get_DtDate(BaseDat dat)
        {
            return ((SysAuditDat)dat).DtDate;
        }

        public static object Get_TblID(BaseDat dat)
        {
            return ((SysAuditDat)dat).TblID;
        }


        public string OperationType
        {
            get
            {
                switch (Operation)
                {
                    case -1:
                        return "delete";
                    case 0:
                        return "update";
                    case 1:
                        return "insert";
                    default:
                        return "error";
                }
            }

        }
        public static object Get_OperationType(BaseDat dat)
        {
            return ((SysAuditDat)dat).OperationType;
        }

        /// <summary></summary>
        [FieldInfoDat(0, "ID")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual Int32 ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    NotifyPropertyChanged(SysAuditColumns.ID);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(1, "Srv")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String Srv
        {
            get
            {
                return _Srv;
            }
            set
            {
                if (_Srv != value)
                {
                    _Srv = value;
                    NotifyPropertyChanged(SysAuditColumns.Srv);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(2, "Db")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String Db
        {
            get
            {
                return _Db;
            }
            set
            {
                if (_Db != value)
                {
                    _Db = value;
                    NotifyPropertyChanged(SysAuditColumns.Db);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(3, "Tbl")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String Tbl
        {
            get
            {
                return _Tbl;
            }
            set
            {
                if (_Tbl != value)
                {
                    _Tbl = value;
                    NotifyPropertyChanged(SysAuditColumns.Tbl);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(4, "Operation")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual Int32 Operation
        {
            get
            {
                return _Operation;
            }
            set
            {
                if (_Operation != value)
                {
                    _Operation = value;
                    NotifyPropertyChanged(SysAuditColumns.Operation);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(5, "OpContent")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual XmlDocument OpContent
        {
            get
            {
                return _OpContent;
            }
            set
            {
                if (_OpContent != value)
                {
                    _OpContent = value;
                    NotifyPropertyChanged(SysAuditColumns.OpContent);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(6, "ClientHost")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String ClientHost
        {
            get
            {
                return _ClientHost;
            }
            set
            {
                if (_ClientHost != value)
                {
                    _ClientHost = value;
                    NotifyPropertyChanged(SysAuditColumns.ClientHost);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(7, "ClientApplication")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String ClientApplication
        {
            get
            {
                return _ClientApplication;
            }
            set
            {
                if (_ClientApplication != value)
                {
                    _ClientApplication = value;
                    NotifyPropertyChanged(SysAuditColumns.ClientApplication);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(8, "Dt")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual DateTime Dt
        {
            get
            {
                return _Dt;
            }
            set
            {
                if (_Dt != value)
                {
                    _Dt = value;
                    NotifyPropertyChanged(SysAuditColumns.Dt);
                }
            }
        }

        /// <summary></summary>
        [FieldInfoDat(9, "Usr")]
        [System.Diagnostics.DebuggerHiddenAttribute()]
        public virtual String Usr
        {
            get
            {
                return _Usr;
            }
            set
            {
                if (_Usr != value)
                {
                    _Usr = value;
                    NotifyPropertyChanged(SysAuditColumns.Usr);
                }
            }
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_ID(BaseDat dat)
        {
            return ((SysAuditDat)dat).ID;
        }

        public static void Set_ID(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).ID = O2Int32(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Srv(BaseDat dat)
        {
            return ((SysAuditDat)dat).Srv;
        }

        public static void Set_Srv(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Srv = O2String(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Db(BaseDat dat)
        {
            return ((SysAuditDat)dat).Db;
        }

        public static void Set_Db(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Db = O2String(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Tbl(BaseDat dat)
        {
            return ((SysAuditDat)dat).Tbl;
        }

        public static void Set_Tbl(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Tbl = O2String(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Operation(BaseDat dat)
        {
            return ((SysAuditDat)dat).Operation;
        }

        public static void Set_Operation(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Operation = O2Int32(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_OpContent(BaseDat dat)
        {
            return ((SysAuditDat)dat).OpContent;
        }

        public static void Set_OpContent(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).OpContent = O2XmlDocument(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_ClientHost(BaseDat dat)
        {
            return ((SysAuditDat)dat).ClientHost;
        }

        public static void Set_ClientHost(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).ClientHost = O2String(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_ClientApplication(BaseDat dat)
        {
            return ((SysAuditDat)dat).ClientApplication;
        }

        public static void Set_ClientApplication(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).ClientApplication = O2String(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Dt(BaseDat dat)
        {
            return ((SysAuditDat)dat).Dt;
        }

        public static void Set_Dt(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Dt = O2DateTime(value);
        }

        [System.Diagnostics.DebuggerHiddenAttribute()]
        public static object Get_Usr(BaseDat dat)
        {
            return ((SysAuditDat)dat).Usr;
        }

        public static void Set_Usr(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Usr = O2String(value);
        }

        public SysAuditInfo Info
        {
            get { return _Info; }
            set
            {
                if (_Info != value)
                {
                    _Info = value;
                    NotifyPropertyChanged(SysAuditColumns.Info);
                }
            }
        }
        [XmlIgnore()]
        public DetailsWrapper<SysAuditDetailsDat, SysAuditDetailsSet, SysAuditDat> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        public static object Get_Info(BaseDat dat)
        {
            return ((SysAuditDat)dat).Info;
        }
        public static void Set_Info(BaseDat dat, object value)
        {
            ((SysAuditDat)dat).Info = value as SysAuditInfo;
        }
        #endregion

        protected override void Init()
        {
            base.Init();
            _Details = new DetailsWrapper<SysAuditDetailsDat, SysAuditDetailsSet, SysAuditDat>(this);
        }
        protected override void LoadMembers(object[] reader)
        {
            base.LoadMembers(reader);
            FillExtraMembers();

            Details.Load();
        }
        public void FillExtraMembers()
        {
            Info = converter.ReadFromXML<SysAuditInfo>(this._OpContent.OuterXml);
            TblID = Info.id;
        }
    }
    public partial class SysAuditSet : BaseSet<SysAuditDat, SysAuditSet>, ISet, ISetReload
    {
        public SysAuditSet()
        {
        }

        public static List<string> GetTablesList()
        {

            string select = @"exec sp_tables @table_owner=""dbo"", @table_type=""'TABLE'""";

            List<string> list = DA.Global.DefaultConnection.LoadList<string>(select, delegate(DbDataReader reader)
                                {
                                    return reader.GetString(2);
                                }
            );
            return list;

        }

        public override void Load()
        {
            bool isOpen = DataAccessor.ConnectionOpen();
            try
            {
                base.Load();
                foreach (SysAuditDat dat in this)
                {
                    dat.FillExtraMembers();
                }
            }
            catch { throw; }
            finally
            {
                DataAccessor.ConnectionClose(isOpen);
            }

        }
        
        #region ISetReload Members
        public event EventHandler SetReloaded;
        public void FireSetReloaded()
        {
            if (SetReloaded != null)
                SetReloaded(this, EventArgs.Empty);
        }
        #endregion
    }

    public partial class SysAuditDetailsSet : BaseSet<SysAuditDetailsDat, SysAuditDetailsSet>, ISet
    {
        private SysAuditSet _SysAudit = new SysAuditSet();

        static SysAuditDetailsSet()
        {
            BaseSet<SysAuditDetailsDat, SysAuditDetailsSet>.HeaderOrdinal = 1;
        }

        public override void LoadByHeaderDat(IDat header)
        {
            SysAuditDat sd = (SysAuditDat)header;
            sd.Details.DetSet.Clear();

            if (sd.Info != null && sd.Info.field != null)
            {
                foreach (SysAuditFieldInfo fi in sd.Info.field)
                {
                    SysAuditDetailsDat sysDet = new SysAuditDetailsDat();
                    sysDet.Header = sd;
                    sysDet.FieldName = fi.column;
                    if (fi.o != null)
                    {
                        if(fi.o is XmlNode[])
                            sysDet.OldValue = ((XmlNode[])fi.o)[0].OuterXml;
                    }
                    if (fi.n != null)
                    {
                        if (fi.n is XmlNode[])
                            sysDet.NewValue = ((XmlNode[])fi.n)[0].OuterXml;
                    }

                    this.Add(sysDet);
                }
            }
        }

        [FieldInfoOrdinals(1)]
        public SysAuditSet SysAudit
        {
            get
            {
                return _SysAudit;
            }
            set
            {
                _SysAudit = value;
            }
        }

    }
    public partial class SysAuditDetailsDat : BaseDat<SysAuditDetailsDat>, IDetailDat<SysAuditDat>
    {
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return FieldName;// PrintInfo.ToString(Info, format, formatProvider);
        }

        #region Extra Fields

        public SysAuditDat _Header = null;

        private string _FieldName = "";

        private string _OldValue = "";
        private string _NewValue = "";

        #endregion

        #region Extra Members

        [FieldInfoDat(1, "Header_id")]
        public virtual SysAuditDat Header
        {
            get
            {
                return _Header;
            }
            set
            {
                if (_Header != value)
                {
                    _Header = value;
                    NotifyPropertyChanged(SysAuditDetailsColumns.Header);
                }
            }
        }

        [FieldInfoDat(2, "FieldName")]
        public string FieldName
        {
            get { return _FieldName; }
            set
            {
                if (_FieldName != value)
                {
                    _FieldName = value;
                    NotifyPropertyChanged(SysAuditDetailsColumns.FieldName);
                }
            }
        }

        [FieldInfoDat(3, "OldValue")]
        public string OldValue
        {
            get { return _OldValue; }
            set
            {
                if (_OldValue != value)
                {
                    _OldValue = value;
                    NotifyPropertyChanged(SysAuditDetailsColumns.OldValue);
                }
            }
        }

        [FieldInfoDat(4, "NewValue")]
        public string NewValue
        {
            get { return _NewValue; }
            set 
            {
                if (_NewValue != value)
                {
                    _NewValue = value;
                    NotifyPropertyChanged(SysAuditDetailsColumns.NewValue);
                }
            }
        }


        public static object Get_Header(BaseDat dat)
        {
            return ((SysAuditDetailsDat)dat).Header;
        }

        public static void Set_Header(BaseDat dat, object value)
        {
            ((SysAuditDetailsDat)dat).Header = value as SysAuditDat;
        }

        public static object Get_FieldName(BaseDat dat)
        {
            return ((SysAuditDetailsDat)dat).FieldName;
        }

        public static void Set_FieldName(BaseDat dat, object value)
        {
            ((SysAuditDetailsDat)dat).FieldName = O2String(value);
        }

        public static object Get_OldValue(BaseDat dat)
        {
            return ((SysAuditDetailsDat)dat).OldValue;
        }

        public static void Set_OldValue(BaseDat dat, object value)
        {
            ((SysAuditDetailsDat)dat).OldValue= O2String(value);
        }

        public static object Get_NewValue(BaseDat dat)
        {
            return ((SysAuditDetailsDat)dat).NewValue;
        }

        public static void Set_NewValue(BaseDat dat, object value)
        {
            ((SysAuditDetailsDat)dat).NewValue = O2String(value);
        }

        #endregion

        #region IDat Members

        private int _ID;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        #endregion
    }

    public partial class SysAuditColumns
    {
        /// <summary></summary>
        public const string ID = "ID";

        /// <summary></summary>
        public const string TblID = "TblID";

        /// <summary></summary>
        public const string Srv = "Srv";

        /// <summary></summary>
        public const string Db = "Db";

        /// <summary></summary>
        public const string Tbl = "Tbl";

        /// <summary></summary>
        public const string Operation = "Operation";

        /// <summary></summary>
        public const string OperationType = "OperationType";

        /// <summary></summary>
        public const string OpContent = "OpContent";

        /// <summary></summary>
        public const string ClientHost = "ClientHost";

        /// <summary></summary>
        public const string ClientApplication = "ClientApplication";

        /// <summary></summary>
        public const string Dt = "Dt";

        /// <summary></summary>
        public const string DtDate = "DtDate";

        /// <summary></summary>
        public const string Usr = "Usr";
        
        /// <summary></summary>
        public const string Info = "Info";
    }
    public partial class SysAuditDetailsColumns
    {
        /// <summary></summary>
        public const string Header = "Header";


        /// <summary></summary>
        public const string FieldName = "FieldName";

        /// <summary></summary>
        public const string OldValue = "OldValue";

        /// <summary></summary>
        public const string NewValue = "NewValue";
    }
}