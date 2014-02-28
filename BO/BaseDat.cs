using System;
using System.IO;
using System.Xml;
using System.Text;
//using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Permissions;
using DA;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Xml.Serialization;


//[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]
//[assembly: CLSCompliant(true)]
namespace BO
{
    #region Простой BaseDat
    [Serializable]
    public abstract class BaseDat : ICloneable, IFormattable, INotifyPropertyChanged, IPropValue, IValidate, IXSLTemplate, IHTMLTemplate//, IXmlSerializable
    {
        protected BaseDat()
        {
            Init();
        }
        protected BaseDat(params object[] args)
            : this()
        {
            if (args.Length != 0)
                Load(args);
        }
        protected virtual void Init()
        {
        }

        protected virtual void ExecArgs(ArrayList args)
        {
            //Формирование сообщения об ошибке
            string err = "";
            foreach (object arg in args)
            {
                if (err.Length == 0)
                    err = string.Format("Неверный конструктор класса {0}({1}", this.GetType(), arg.GetType());
                else
                    err += string.Format(", {0}", arg.GetType());
            }
            if (err.Length != 0) err += ")";
            throw new Exception(err);
        }

        public abstract string TableName();

        private IDataAccess _DataAccessor = null;

        [
        Bindable(BindableSupport.No)
        , SettingsBindable(false)
        , Browsable(false)
        , ReadOnly(true)
        , XmlIgnore()
        ]
        virtual public IDataAccess DataAccessor
        {
            get
            {
                if (_DataAccessor == null)
                    _DataAccessor = DA.Global.DefaultConnection;
                return _DataAccessor;
            }
            set { _DataAccessor = value; }
        }
        virtual protected bool DataAccessorIsNull()
        {
            return (_DataAccessor == null);
        }

        public void TransactionBegin()
        {
            this.DataAccessor.TransactionBegin();
        }
        public int TransactionCommit()
        {
            return this.DataAccessor.TransactionCommit();
        }
        public int TransactionRollback()
        {
            return this.DataAccessor.TransactionRollback();
        }


        public abstract int Save();

        public virtual void Delete()
        {
            if (this is IHasIsDeleted)
                this.DataAccessor.Execute(string.Format("update {0} set IsDeleted=1 where id = {1}", TableName(), ((IDat)this).ID));
            else
                this.DataAccessor.Delete(TableName(), ((IDat)this).ID);
        }

        public virtual DatErrorList ValidateErrors()
        {
            DatErrorList ret = new DatErrorList();
            return ret;
        }

        public virtual DatErrorList ValidateWarnings()
        {
            DatErrorList ret = new DatErrorList();
            return ret;
        }

        #region Load Functions
        public virtual void Load(IEnumerable reader)
        {
            LoadMembers(reader as object[]);
        }

        public virtual void Load(params object[] args)
        {
            ArrayList argList = null;
            foreach (object arg in args)
            {
                if (arg is IDataAccess)
                {
                    this.DataAccessor = (IDataAccess)arg;
                }
                else
                {
                    if (argList == null) argList = new ArrayList();
                    argList.Add(arg);
                }
            }
            if (argList != null)
                ExecArgs(argList);
        }

        /// <summary>
        /// Загрузка по id
        /// </summary>
        /// <param name="id">id</param>
        //public virtual void Load(int id)
        //{
        //    IDAOFilter filter = DataAccessor.NewFilter();
        //    filter.AddWhere(new FilterID(id));
        //    Load(filter);
        //}

        /// <summary>
        /// Загрузка по фильтрам... пока правда непонятно каким:))))
        /// </summary>
        /// <param name="filter">фильтр</param>
        public virtual void Load(IDAOFilter filter)
        {
            IDataAccess da = this.DataAccessor;
            bool wasOpen = da.ConnectionOpen();
            object[] vals = da.LoadDat(filter, TableName());
            LoadMembers(vals);
            da.ConnectionClose(wasOpen);
        }
        protected void Load(FilterUnitBase fUnit)
        {
            IDAOFilter filter = this.DataAccessor.NewFilter();
            filter.AddWhere(fUnit);
            Load(filter);
        }
        protected virtual void LoadMembers(object[] reader) { }

        protected virtual Dictionary<string, object> SaveMembers() { return null; }

        #endregion

        #region Type Converters & Functions
        public static decimal O2Decimal(object x)
        {
            if (Convert.IsDBNull(x))
                return 0;
            return Convert.ToDecimal(x);
        }
        public static int O2Int32(object x)
        {
            if (Convert.IsDBNull(x))
                return 0;
            return Convert.ToInt32(x);
        }
        public static bool O2Boolean(object x)
        {
            if (Convert.IsDBNull(x))
                return false;
            return Convert.ToBoolean(x);
        }
        public static string O2String(object x)
        {
            if (Convert.IsDBNull(x))
                return "";
            return Convert.ToString(x);
        }
        public static ET O2Enum<ET>(object x)
        {
            if (Convert.IsDBNull(x))
                return default(ET);
            return (ET)x;
        }

        public static XmlDocument O2XmlDocument(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;

            if (Common.IsNullOrEmpty(x.ToString()))
                return null;

            XmlDocument o = new XmlDocument();
            o.PreserveWhitespace = true;
            o.LoadXml((x is XmlDocument) ? ((XmlDocument)x).OuterXml : x.ToString());
            return o;
        }

        public static Guid O2Guid(object x)
        {
            if (Convert.IsDBNull(x) || x == null || Common.IsNullOrEmpty(x.ToString()))
                return new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            return new Guid(x.ToString());
        }

        public static System.Drawing.Bitmap O2Bitmap(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;

            //Bitmap o = new Bitmap(new MemoryStream(x));
            //return o;
            if (x is Bitmap)
                return new Bitmap((Bitmap)x);
            else
                return new Bitmap(new MemoryStream((byte[])x));
        }
        public static System.Drawing.Image O2Image(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;

            if (x is Image)
                return new Bitmap((Image)x);
            else
                return new Bitmap(new MemoryStream((byte[])x));
        }
        public static byte[] O2ByteArray(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;

            return (byte[])x;
        }
        public static PathTree O2PathTree(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;
            return new PathTree(Convert.ToString(x));
        }
        public static PathTreeN O2PathTreeN(object x)
        {
            if (Convert.IsDBNull(x) || x == null)
                return null;
            return new PathTreeN(Convert.ToString(x));
        }
        public static DateTime O2DateTime(object x)
        {
            if (Convert.IsDBNull(x))
                return DateTime.MinValue;
            return Convert.ToDateTime(x);
        }
        public static U To<U>(object obj_id) where U : BaseDat<U>, new()
        {
            int id = O2Int32(obj_id);
            if (id > 0)
            {
                U dat = new U();
                dat.Load(id);
                return dat;
            }
            else
                return null;
        }
        public static U obj2<U>(object dat) where U : BaseDat<U>, new()
        {
            return dat as U;
        }
        public static bool IsBaseDatType(Type type)
        {
            if (type == typeof(object))
                return false;
            if (type == typeof(BaseDat))
                return true;
            return IsBaseDatType(type.BaseType);
        }
        public object obj2Type(Type type, object val)
        {
            if (type == typeof(string)) return BaseDat.O2String(val);
            if (type == typeof(decimal)) return BaseDat.O2Decimal(val);
            if (type == typeof(int)) return BaseDat.O2Int32(val);
            if (type == typeof(bool)) return BaseDat.O2Boolean(val);
            if (type == typeof(DateTime)) return BaseDat.O2DateTime(val);
            if (val == DBNull.Value) return null;
            return val;
        }
        #endregion

        #region ICloneable Members

        public virtual object Clone()
        {
            return null;
        }
        public abstract void CopyTo(BaseDat dat);

        #endregion

        #region IFormattable Members
        public static string ToString(BaseDat dat)
        {
            return BaseDat.ToString(dat, "");
        }
        public static string ToString(BaseDat dat, string format)
        {
            if (dat == null)
                return "";
            else
            {
                if (string.IsNullOrEmpty(format))
                    return dat.ToString();
                else
                    return dat.ToString(format, null);
            }
        }
        public override string ToString()
        {
            return this.ToString(null as string, null as IFormatProvider);
        }

        /// <summary>
        /// Функция для обработки формата выдачи dat-класса
        /// Для каждого типа - своя
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            string ret = "";

            if (this is ICardDat)
                ret = ToString((ICardDat)this, format, formatProvider);
            else if (this is ITreeDat)
                ret = ToString((ITreeDat)this, format, formatProvider);
            else if (this is IDictDat)
                ret = ToString((IDictDat)this, format, formatProvider);
            else if (this is IHasName)
                ret = ToString((IHasName)this, format, formatProvider);
            else if (this is IDat)
                ret = ToString((IDat)this, format, formatProvider);
            else
                ret = string.Format("{0}", GetType());

            if (this is IHasIsDeleted)
                ret += (((IHasIsDeleted)this).IsDeleted) ? " [удалено]" : "";

            return ret;
        }

        private static string ToString(IHasName dat, string format, IFormatProvider formatProvider)
        {
            return string.Format("{0}", dat.Name.Trim());
        }

        private static string ToString(ICardDat dat, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return string.Format("{0}: {1}", dat.FP, dat.Name);
            else
            {
                string result = format;
                format = format.ToLower();
                if (format.Contains("c"))
                {
                    result = result.Replace("c", dat.Code.ToString());
                }
                if (format.Contains("n"))
                {
                    result = result.Replace("n", dat.Name);
                }
                if (format.Contains("p"))
                {
                    string p = "";
                    if (dat.Parent_FP != null)
                        p = dat.Parent_FP.ToString();
                    result = result.Replace("p", p);
                }
                if (format.Contains("f"))
                {
                    string f = "";
                    if (dat.FP != null)
                        f = dat.FP.ToString();
                    result = result.Replace("f", f);
                }
                //return ToString((IDat)dat, result, formatProvider);
                return result;
            }
        }
        private static string ToString(IDictDat dat, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return string.Format("{0}: {1}", dat.SCode, dat.Name);
            else
            {
                string result = format;
                format = format.ToLower();
                if (format.Contains("c"))
                {
                    result = result.Replace("c", dat.SCode);
                }
                if (format.Contains("n"))
                {
                    result = result.Replace("n", dat.Name);
                }
                //return ToString((IDat)dat, result, formatProvider);
                return result;
            }
        }
        private static string ToString(ITreeDat dat, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return string.Format("{0}: {1}", dat.FP, dat.Name);
            else
            {
                string result = format;
                format = format.ToLower();
                if (format.Contains("c"))
                {
                    string c = "";
                    if (dat.FP != null)
                        c = dat.FP.Code.ToString();
                    result = result.Replace("c", c);
                }
                if (format.Contains("n"))
                {
                    result = result.Replace("n", dat.Name);
                }
                if (format.Contains("p"))
                {
                    string p = "";
                    if (dat.Parent_FP != null)
                        p = dat.Parent_FP.ToString();
                    result = result.Replace("p", p);
                }
                if (format.Contains("f"))
                {
                    string f = "";
                    if (dat.FP != null)
                        f = dat.FP.ToString();
                    result = result.Replace("f", f);
                }
                //return ToString((IDat)dat, result, formatProvider);
                return result;
            }
        }
        private static string ToString(IDat dat, string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return string.Format("{0}(ID:{1})", dat.GetType(), dat.ID);
            else
            {
                string result = format;
                format = format.ToLower();
                if (format.Contains("i"))
                {
                    result = result.Replace("i", dat.ID.ToString());
                }
                if (format.Contains("t"))
                {
                    result = result.Replace("t", dat.GetType().ToString());
                }
                //return ToString((IDat)dat, result, formatProvider);
                return result;
            }
        }

        public virtual object GetValue(string property)
        {
            //object ret = ProxyInfo.GetPropValue(this, property);
            object ret = BO.Reports.ExtraRepDataInfo.GetValue(this, property);
            if (ret == null)
                ret = GetCustomValue(property);
            return ret;
        }

        protected virtual object GetCustomValue(string propertyName)
        {
            return null;
        }
        #endregion
        public abstract bool EqualDat(BaseDat dat);

        /// <summary>
        /// Функция для получения новых уникальных ID. 
        /// Используется для нумерации созданных Dat-классов.
        /// При сохранении такие ID преобразуются в null и вызывается Insert
        /// </summary>
        /// <returns>Новый уникальный ID</returns>
        public abstract int GetNewID();
        /// <summary>
        /// Проверка- реальный ID у объекта IDat или объект новый.
        /// </summary>
        public abstract bool IsNew { get; }

        public virtual object GetDatValue(string name)
        {

            try
            {
                PropertyInfo prop = this.GetType().GetProperty(name);
                if (prop == null)
                    throw new Exception();
                else
                    return prop.GetValue(this, null);
            }
            catch (Exception ex)
            {
                string error = "Error while getting Value for " + this.ToString() + "." + name;
                throw new Exception(error, ex);
            }
        }

        public int GetIntFromTable(string select)
        {
            List<object[]> list = DataAccessor.LoadList(select);
            int val = 0;
            if (list != null && list.Count != 0 && list[0] != null && list[0].Length != 0)
                val = (int)list[0][0];
            return val;
        }

        public virtual string GetRTFDoc()
        {
            return "";
        }

        //public virtual string GetHTMLDoc() 
        //{
        //    return string.Format("<html><body><center>{0}</center></body></html>", ToString());
        //}

        public virtual string GetHTMLName()
        {
            return GetType().Name + ".html";
        }

        public virtual string GetHTMLString()
        {
            return "";
        }

        public virtual string GetTemplateName()
        {
            return GetType().FullName + ".xslt";
        }

        public virtual string GetTemplatePath()
        {
            string ret = System.Configuration.ConfigurationManager.AppSettings["PathToXSLTemplates"];

            if (string.IsNullOrEmpty(ret))
                ret = "file://///pf/Programs/XSLT/";

            return ret;
        }

        public virtual XmlDocument Serialize()
        {
            XmlConverter xml_ser = new XmlConverter();
            XmlDocument ret = xml_ser.Serialize(this.GetType(), this);
            string tmpl_name = GetTemplatePath() + GetTemplateName();
            ret.InsertBefore(ret.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"" + tmpl_name + "\""), ret.DocumentElement);
            return ret;
        }
        //public virtual XmlDocument Serialize()
        //{
        //    XmlConverter xml_ser = new XmlConverter();
        //    XmlDocument ret = xml_ser.Serialize(this.GetType(), this);
        //    //string tmpl_name = GetTemplatePath() + GetTemplateName();
        //    //ret.InsertBefore(ret.CreateProcessingInstruction("xml-stylesheet", "type=\"text/xsl\" href=\"" + tmpl_name + "\""), ret.DocumentElement);
        //    //return ret;
        //}
        #region INotifyPropertyChanged Members

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
    #endregion

    public abstract class BaseDat<T> : BaseDat, IComparable<T>, IEquatable<T> where T : BaseDat<T>//, new()
    {

        protected BaseDat()
            : base()
        {
            //Добавил Андрей. После обсуждения.
            //Код обеспечивает уникальность dat-класса в пределах сессии
            if (this is IDat)
                ((IDat)this).ID = GetNewID();
        }
        protected BaseDat(object[] args)
            : base(args)
        {

        }

        #region Static Methods
        static BaseDat()
        {
            try
            {
                CreateTableName();
                CreateProps(_Dlist, typeof(T), null, 0);
                CreateRefValLinks();

            }
            catch (Exception ex)
            {
                string sEx = string.Format("Error in {0} Static Constructor", typeof(T));
                throw new Exception(sEx, ex);
            }
        }
        static private string _tablename = "";
        public static string STableName
        {
            get { return _tablename; }
        }
        static private List<DatDescriptor> _Dlist = new List<DatDescriptor>();

        public static List<DatDescriptor> Dlist
        {
            get { return BaseDat<T>._Dlist; }
        }
        public static DatDescriptor GetDescriptor(string name)
        {
            int position = _Dlist.FindIndex(delegate(DatDescriptor dd)
                            {
                                return dd.Name.ToLower() == name.ToLower();
                            });
            return (position < 0) ? null : _Dlist[position];
        }

        public override object GetDatValue(string name)
        {
            try
            {
                DatDescriptor descr = GetDescriptor(name);
                if (descr == null)
                    return base.GetDatValue(name);
                else
                    return descr.GetValue(this);
            }
            catch (Exception ex)
            {
                string error = string.Format("Error in Value for {0}.{1}"
                    , this.GetType()
                    , name);
                throw new Exception(error, ex);
            }
        }

        /// <summary>
        /// Коллекция ссылок на простые свойства Dat-класса
        /// </summary>
        public static Dictionary<int, SetValueDelegate> ValProps;
        /// <summary>
        /// Коллекция ссылок на ссылки Dat-класса на другие Dat-классы
        /// </summary>
        public static Dictionary<int, LinkSet> RefProps;


        static private void CreateProps(List<DatDescriptor> dlist, Type type, DatDescriptor parent, int countRecurcy)
        {
            if (countRecurcy > 5) return;
            try
            {
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(type);
                foreach (PropertyDescriptor pd in pdc)
                {
                    if (!pd.PropertyType.IsAbstract && pd.ComponentType == type)
                    {
                        int ordinal = -1;
                        string fieldname = "";
                        bool offline = false;
                        Attribute ac = pd.Attributes[typeof(FieldInfoDat)];
                        if (ac is FieldInfoDat)
                        {
                            FieldInfoDat fid = (FieldInfoDat)ac;
                            ordinal = fid.Ordinal;
                            fieldname = fid.Name;
                            offline = fid.IsOffline;
                        }
                        string pdName = pd.Name;
                        MethodInfo miGet = type.GetMethod("Get_" + pdName);
                        MethodInfo miSet = type.GetMethod("Set_" + pdName);

                        DatDescriptor dd = new DatDescriptor(pdName, pd, ordinal, fieldname, parent);
                        dd.PD = pd;
                        dd.IsOffline = offline;
                        if (miGet != null)
                            dd.GetValDelegate = (GetValueDelegate)Delegate.CreateDelegate(typeof(GetValueDelegate), miGet);
                        if (miSet != null)
                            dd.SetValDelegate = (SetValueDelegate)Delegate.CreateDelegate(typeof(SetValueDelegate), miSet);
                        dlist.Add(dd);
                        if (IsBaseDatType(pd.PropertyType))
                            CreateProps(dlist, pd.PropertyType, dd, countRecurcy + 1);
                    }
                }

            }
            catch (Exception ex)
            {
                string sEx = string.Format("Error in {0}.CreateProps", typeof(T));
                throw new Exception(sEx, ex);
            }
        }

        public static bool OfflineLoad = false;
        static private void CreateRefValLinks()
        {
            try
            {
                ValProps = new Dictionary<int, SetValueDelegate>();
                RefProps = new Dictionary<int, LinkSet>();
                List<DatDescriptor> dlist = GetRootProperties();
                foreach (DatDescriptor dd in dlist)
                {
                    if (dd.Ordinal >= 0)
                    {
                        if (dd.SetValDelegate != null)
                        {
                            //Type thisType = typeof(T);
                            //bool isExpProp = (thisType.GetInterface("IExpPropDat`1") != null);
                            if (dd.ParentDescriptor == null)
                            {
                                if (IsBaseDatType(dd.PropertyType) && !dd.IsOffline)
                                {
                                    RefProps.Add(dd.Ordinal, new LinkSet(dd.GetValDelegate, dd.SetValDelegate));
                                }
                                else
                                    ValProps.Add(dd.Ordinal, dd.SetValDelegate);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sEx = string.Format("Error in {0}.CreateRefValLinks", typeof(T));
                throw new Exception(sEx, ex);
            }
        }
        static public string GetFieldName(string propertyName)
        {
            foreach (DatDescriptor dd in _Dlist)
            {
                if (dd.Name == propertyName)
                    return dd.FieldName;
            }
            throw new Exception(string.Format("В классе {0} не найдено поле {1}", typeof(T), propertyName));
        }

        static private void CreateTableName()
        {
            string nameDat = typeof(T).Name;
            if (nameDat.EndsWith("Dat"))
                _tablename = nameDat.Substring(0, nameDat.Length - 3);
        }
        static public PropertyDescriptorCollection GetProperties()
        {
            return new PropertyDescriptorCollection(_Dlist.ToArray());
        }
        static public List<DatDescriptor> GetRootProperties()
        {
            Type thisType = typeof(T);
            bool isExpProp = (thisType.GetInterface("IExpPropDat`1") != null);
            List<DatDescriptor> list = _Dlist.FindAll(delegate(DatDescriptor dd)
            {
                return (dd.ParentDescriptor == null);
                //|| (isExpProp && dd.ParentDescriptor.Name == "Root"));
            });
            return list;
        }

        //public override bool Equals(object obj)
        //{
        //    T other = obj as T;
        //    if (other == null) return false;
        //    return Equals(other);
        //}
        //public override int GetHashCode()
        //{
        //    return GetHashDat().GetHashCode();
        //}

        public static List<DatDescriptor> DiffDat(T dat1, T dat2)
        {
            List<DatDescriptor> ret = new List<DatDescriptor>();
            foreach (DatDescriptor dd in GetRootProperties())
            {
                if (dd.Name.ToLower() != "sysuser" && dd.Name.ToLower() != "sysdate")
                {
                    if (dd.Ordinal >= 0)
                    {
                        object val1 = dd.GetValue(dat1);
                        object val2 = dd.GetValue(dat2);

                        if (val1 is IDat) val1 = ((IDat)val1).ID;
                        if (val2 is IDat) val2 = ((IDat)val2).ID;

                        if (val1 != null && !val1.Equals(val2))
                            ret.Add(dd);

                        if (val1 == null && val2 != null)
                            ret.Add(dd);

                        //if (val1 != val2) return false;
                    }
                    else if (typeof(IDetailsWrapper<T>).IsAssignableFrom(dd.PropertyType))
                    {
                        // Реализация сравнения DetailsWrapper<,,> подклассов.
                        // Вроде все должно копироваться.
                        // 2007-2-19 Андрей
                        IDetailsWrapper<T> det1 = dd.GetValue(dat1) as IDetailsWrapper<T>;
                        IDetailsWrapper<T> det2 = dd.GetValue(dat2) as IDetailsWrapper<T>;
                        if (det1 != null)
                        {
                            if (!det1.Equals(det2))
                                ret.Add(dd);
                        }
                        else
                        {
                            if (!det2.Equals(det1))
                                ret.Add(dd);
                        }
                    }
                }

            }
            return ret;
        }

        static public bool CompareDat(T dat1, T dat2)
        {
            if (dat1 == null && dat2 == null) return true;
            if (dat1 == null || dat2 == null) return false;

            foreach (DatDescriptor dd in GetRootProperties())
            {
                if (dd.Ordinal >= 0)
                {
                    object val1 = dd.GetValue(dat1);
                    object val2 = dd.GetValue(dat2);

                    if (val1 is IDat) val1 = ((IDat)val1).ID;
                    if (val2 is IDat) val2 = ((IDat)val2).ID;

                    if (val1 != null && !val1.Equals(val2)) return false;
                    if (val1 == null && val2 != null) return false;
                    //if (val1 != val2) return false;
                }
                else if (typeof(IDetailsWrapper<T>).IsAssignableFrom(dd.PropertyType))
                {
                    // Реализация сравнения DetailsWrapper<,,> подклассов.
                    // Вроде все должно копироваться.
                    // 2007-2-19 Андрей
                    IDetailsWrapper<T> det1 = dd.GetValue(dat1) as IDetailsWrapper<T>;
                    IDetailsWrapper<T> det2 = dd.GetValue(dat2) as IDetailsWrapper<T>;
                    if (det1 != null)
                    {
                        if (!det1.Equals(det2)) return false;
                    }
                    else
                    {
                        if (!det2.Equals(det1)) return false;
                    }
                }
            }
            return true;
        }
        static public T CloneDat(T dat)
        {
            if (dat == null) return null;
            //T newDat = new T();
            T newDat = Activator.CreateInstance<T>();
            CopyDat(dat, newDat);

            //newDat.DataAccessor = dat.DataAccessor;
            //foreach (DatDescriptor dd in GetRootProperties())
            //{
            //    if (dd.Ordinal >= 0)
            //    {
            //        object val = dd.GetValue(dat);
            //        // Надо ли клонировать подклассы? 
            //        // После этого могут быть проблемы в Set - классах.
            //        //if (val is ICloneable) val = ((ICloneable)val).Clone();
            //        dd.SetValue(newDat, val);
            //    }
            //}
            return newDat;
        }
        static public void CopyDat(T from, T to)
        {
            if (from == null)
                throw new ArgumentNullException("from");
            if (to == null)
                throw new ArgumentNullException("to");
            to.DataAccessor = from.DataAccessor;
            List<DatDescriptor> list = GetRootProperties();
            foreach (DatDescriptor dd in list)
            {
                if (dd.Ordinal >= 0)
                {
                    object val = dd.GetValue(from);
                    // Надо ли клонировать подклассы? 
                    // После этого могут быть проблемы в Set - классах.
                    //if (val is ICloneable) val = ((ICloneable)val).Clone();
                    dd.SetValue(to, val);
                }
                else if (typeof(IDetailsWrapper<T>).IsAssignableFrom(dd.PropertyType))
                {
                    // Реализация копирования DetailsWrapper<,,> подкласса.
                    // Вроде все должно копироваться.
                    // 2007-2-19 Андрей
                    IDetailsWrapper<T> detFrom = dd.GetValue(from) as IDetailsWrapper<T>;
                    if (detFrom == null)
                        dd.SetValue(to, null);
                    else
                        dd.SetValue(to, detFrom.Clone(to));
                }
            }
        }

        static private IDataAccess _ClassDataAccessor = null;
        public static IDataAccess ClassDataAccessor
        {
            get { return BaseDat<T>._ClassDataAccessor; }
            set { BaseDat<T>._ClassDataAccessor = value; }
        }

        [
        Bindable(BindableSupport.No)
        , SettingsBindable(false)
        , Browsable(false)
        , ReadOnly(true)
        , System.Xml.Serialization.XmlIgnore()
        ]
        override public IDataAccess DataAccessor
        {
            get
            {
                if (DataAccessorIsNull())
                {
                    if (ClassDataAccessor == null)
                        return DA.Global.DefaultConnection;
                    else
                        return ClassDataAccessor;
                }
                return base.DataAccessor;
            }
            set { base.DataAccessor = value; }
        }
        protected override bool DataAccessorIsNull()
        {
            return base.DataAccessorIsNull() && ClassDataAccessor == null;
        }
        #endregion

        public override string TableName()
        {
            if (_tablename.Length > 0)
                return _tablename;
            else
                throw new Exception(string.Format("Для класса {0} не определена функция TableName!", typeof(T)));
        }

        protected override void LoadMembers(object[] reader)
        {
            foreach (DatDescriptor dd in GetRootProperties())
            {
                if (dd.Ordinal >= 0)
                {
                    object val = reader[dd.Ordinal];
                    if (val == DBNull.Value) val = null;

                    if (val != null && BaseDat.IsBaseDatType(dd.PropertyType))
                    {
                        BaseDat subDat = (BaseDat)Activator.CreateInstance(dd.PropertyType);
                        if (subDat is IDat)
                            subDat.Load(O2Int32(val));
                        else if (subDat is IDatGuid)
                            subDat.Load(O2Guid(val));

                        val = subDat;
                    }
                    if (dd.SetValDelegate != null)
                        dd.SetValDelegate(this, val);
                    else
                        dd.PD.SetValue(this, val);
                }
            }
        }

        protected override Dictionary<string, object> SaveMembers()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (DatDescriptor dd in GetRootProperties())
            {
                AddMemberValue(dict, dd);
            }
            return dict;
        }

        protected virtual void AddMemberValue(Dictionary<string, object> dict, DatDescriptor dd)
        {
            if (dd.FieldName.Length > 0 && dd.ParentDescriptor == null)
            {
                object val;
                if (dd.GetValDelegate != null)
                    val = dd.GetValDelegate(this);
                else
                    val = dd.PD.GetValue(this);

                if (val != null)
                {
                    if (val is IDat)
                        val = ((IDat)val).ID;
                    else if (val.GetType().IsClass)
                    {
                        if (val is XmlDocument)
                        {
                            XmlDocument x = (XmlDocument)val;
                            if (x.DocumentElement == null)
                                val = null;
                            else
                            {
                                if (x.DocumentElement != null && x.DocumentElement.Attributes["xml:space"] == null)
                                    x.DocumentElement.Attributes.Append(x.CreateAttribute("xml:space"));
                                x.DocumentElement.Attributes["xml:space"].Value = "preserve";
                                val = x.DocumentElement.OuterXml;
                            }
                        }
                        //else if (val is Guid)
                        //{
                        //    Guid x = (Guid)val;
                        //    val = x.ToString();
                        //}
                        //else if (val is System.Drawing.Image)
                        //{
                        //    System.Drawing.Image x = (System.Drawing.Image)val;
                        //    MemoryStream s = new MemoryStream();
                        //    x.Save(s, x.RawFormat);
                        //    val = s.ToArray();
                        //}
                        else if (val is byte[])
                        {
                            //System.Drawing.Image x = (System.Drawing.Image)val;
                            //MemoryStream s = new MemoryStream();
                            //x.Save(s, x.RawFormat);
                            //val = s.ToArray();
                        }
                        else if (val is System.Drawing.Bitmap || val is System.Drawing.Image)
                        {
                            System.Drawing.Bitmap x = (System.Drawing.Bitmap)val;
                            MemoryStream s = new MemoryStream();
                            //ImageFormat iformat = new ImageFormat(x.RawFormat.Guid);
                            //x.Save(s, x.RawFormat);
                            //x.Save(s, ImageFormat.Png);
                            x.Save(s, ImageFormat.Jpeg);
                            //x.Save(s, iformat);
                            val = s.ToArray();
                            //val = x.Save
                        }
                        else if (val is DateTime && (DateTime)val == DateTime.MinValue)
                            val = null;
                        else if (val is Guid)
                        {
                            Guid g = new Guid(val.ToString());
                            if (g == Guid.Empty)
                                val = null;
                            else
                                val = g.ToString("N");
                        }
                        else
                            val = val.ToString();
                    }
                    else if (val is DateTime && (DateTime)val == DateTime.MinValue)
                        val = null;
                    else if (val is Guid)
                    {
                        if ((Guid)val == Guid.Empty)
                            val = null;
                    }
                    else if (val is int && (int)val == 0 && (dd.FieldName.ToLower().EndsWith("_ref") || dd.FieldName.ToLower().EndsWith("_id")))
                        val = null;
                }
                dict.Add(dd.FieldName, val);
            }
        }

        public override int Save()
        {
            try
            {
                IDataAccess da = this.DataAccessor;

                if (this is IDat)
                {
                    if (this.IsNew)
                        ((IDat)this).ID = da.Insert(TableName(), SaveMembers());
                    else
                        da.Update(TableName(), SaveMembers());
                    return ((IDat)this).ID;
                }
                else if (this is IDatGuid)
                {
                    da.Insert(TableName(), SaveMembers());
                    return 0;
                }
                else
                {
                    throw new Exception("Для вызова Save() нужно, чтобы класс был IDat или IDatGuid");
                }
            }
            catch (Exception exp)
            {
                string str = exp.Message;
                throw exp;
            }
        }

        protected override void ExecArgs(ArrayList args)
        {

            try
            {
                if (args.Count == 1 && args[0] is FilterUnitBase)
                    Load(args[0] as FilterUnitBase);
                else if (this is ICardDat)
                    Load(Loaders.CreateICardDatFilter<T>(this.DataAccessor, args));
                else if (this is ITreeDat)
                    Load(Loaders.CreateITreeDatFilter<T>(this.DataAccessor, args));
                else if (this is ITreeNDat)
                    Load(Loaders.CreateITreeNDatFilter<T>(this.DataAccessor, args));
                else if (this is IDictDat)
                    Load(Loaders.CreateIDictDatFilter<T>(this.DataAccessor, args));
                else if (this is IDat)
                    Load(Loaders.CreateIDatFilter<T>(this.DataAccessor, args));
                else if (this is IDatNoID)
                {
                    args.Insert(0, BaseDat<T>.Dlist[0].FieldName);
                    Load(Loaders.CreateIDatNoIdFilter<T>(this.DataAccessor, args));
                }
                else if (this is IDatGuid)
                {
                    args.Insert(0, BaseDat<T>.Dlist[0].FieldName);
                    Load(Loaders.CreateIDatGuidFilter<T>(this.DataAccessor, args));
                }
                else
                    base.ExecArgs(args);

            }
            catch (System.Exception Ex)
            {
                string err = Ex.Message;
                //base.ExecArgs(args);
                throw Ex;
            }
        }

        public override int GetNewID()
        {
            _lastID++;
            return _lastID;
        }
        private static int _lastID = cnstMaxSavedID;
        private const int cnstMaxSavedID = 1000000;
        //private const int cnstMaxSavedID2 = System.Data.SqlTypes.SqlInt32.MaxValue;

        public override bool IsNew
        {
            get
            {
                if (this is IDat)
                {
                    int id = ((IDat)this).ID;
                    return id <= 0 || id >= cnstMaxSavedID;
                }
                return false;
            }
        }

        #region IComparable<T> Members
        public virtual int CompareTo(T other)
        {
            if (this is IDat)
                return ((IDat)this).ID.CompareTo(((IDat)other).ID);
            else
                return 0;
        }
        #endregion

        #region IEquatable<T> Members

        public virtual bool Equals(T other)
        {
            return ((IDat)this).ID.Equals(((IDat)other).ID);
        }
        #endregion

        #region ICloneable Members

        public override object Clone()
        {
            return BaseDat<T>.CloneDat((T)this);
        }
        public virtual void CopyTo(T dat)
        {
            BaseDat<T>.CopyDat((T)this, dat);
        }
        public override void CopyTo(BaseDat dat)
        {
            CopyTo((T)dat);
        }
        #endregion
        public override bool EqualDat(BaseDat dat)
        {
            return CompareDat(this as T, dat as T);
        }
        
        public List<DatDescriptor> DiffDat(BaseDat dat)
        {
            return DiffDat(this as T, dat as T);
        }

        #region IEditableObject Members (Надо и интерфейс добавить, если запускать.)
        //private T backupData = null;
        //private bool inTxn = false;
        //public void BeginEdit()
        //{
        //    if (!inTxn)
        //    {
        //        inTxn = true;
        //        backupData = (T)this.Clone();
        //    }
        //}

        //public void CancelEdit()
        //{
        //    if (inTxn)
        //    {
        //        backupData.CopyTo((T)this);
        //        backupData = null;
        //        inTxn = false;
        //    }
        //}

        //public void EndEdit()
        //{
        //    if (inTxn)
        //    {
        //        backupData = null;
        //        inTxn = false;
        //    }
        //}
        #endregion


        public bool AddErrorIfEmpty(DatErrorList list, string propertyName, string description)
        {
            return list.AddIfEmpty<T>(this, propertyName, description);
        }

#if DEBUG
        private static bool printonce = true;
#endif

        public virtual string GetPropsString(params string[] exclusions)
        {
            //StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>(exclusions);
            List<DatDescriptor> dlst = new List<DatDescriptor>();
            foreach (DatDescriptor dd in GetRootProperties())
            {
                if (dd.Ordinal >= 0 && !list.Contains(dd.Name))
                    dlst.Add(dd);
            }
            // Сотрируем, чтобы порядок значений был одинаковый
            dlst.Sort(delegate(DatDescriptor dd1, DatDescriptor dd2)
                {
                    return dd1.Ordinal.CompareTo(dd2.Ordinal);
                });
            string s = GetHashDatString(dlst);
            int hash = s.GetHashCode();

            //foreach (DatDescriptor dd in dlst)
            //{
            //    string s = Common.PropValue2String(dd.GetValue(this), dd.PropertyType).Trim();
            //    sb.Append(s);
            //}
#if DEBUG
            if (printonce)
            {
                printonce = false;
                Console.WriteLine("GetHashDatByFields(");
                string separator = "";
                foreach (DatDescriptor dd in dlst)
                {
                    Console.WriteLine(separator + "DatColumns." + dd.Name);
                    separator = ",";
                }
                Console.WriteLine(");");
                Console.WriteLine(s);
            }
#endif
            return s;
        }
        public string GetHashDat(params string[] exclusions)
        {
            //StringBuilder sb = new StringBuilder();
            List<string> list = new List<string>(exclusions);
            List<DatDescriptor> dlst = new List<DatDescriptor>();
            foreach (DatDescriptor dd in GetRootProperties())
            {
                if (dd.Ordinal >= 0 && !list.Contains(dd.Name))
                    dlst.Add(dd);
            }
            // Сотрируем, чтобы порядок значений был одинаковый
            dlst.Sort(delegate(DatDescriptor dd1, DatDescriptor dd2)
                {
                    return dd1.Ordinal.CompareTo(dd2.Ordinal);
                });
            string s = GetHashDatString(dlst);
            int hash = s.GetHashCode();

            //foreach (DatDescriptor dd in dlst)
            //{
            //    string s = Common.PropValue2String(dd.GetValue(this), dd.PropertyType).Trim();
            //    sb.Append(s);
            //}
#if DEBUG
            if (printonce)
            {
                printonce = false;
                Console.WriteLine("GetHashDatByFields(");
                string separator = "";
                foreach (DatDescriptor dd in dlst)
                {
                    Console.WriteLine(separator + "DatColumns." + dd.Name);
                    separator = ",";
                }
                Console.WriteLine(");");
                Console.WriteLine(s);
            }
#endif
            return hash.ToString("X");
        }
        public virtual string GetHashDatString(List<DatDescriptor> dlist)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DatDescriptor dd in dlist)
            {
                string s = Common.PropValue2String(dd.GetValue(this), dd.PropertyType).Trim();
                sb.Append("&" + s);
            }
            return sb.ToString();
        }
        public string GetHashDatByFields(params string[] fields)
        {
            StringBuilder sb = new StringBuilder();

            List<DatDescriptor> dlst = new List<DatDescriptor>();

            foreach (string sname in fields)
            {
                DatDescriptor dd = GetDescriptor(sname);
                dlst.Add(dd);
            }
            string s = GetHashDatString(dlst);
            int hash = s.GetHashCode();
#if DEBUG
            if (printonce)
                Console.WriteLine(s);
#endif
            return hash.ToString("X");
        }

    }

    #region DatDescriptor - Properties & Delegates
    public delegate object GetValueDelegate(BaseDat val);
    public delegate void SetValueDelegate(BaseDat dat, object value);

    public class DatDescriptor : PropertyDescriptor
    {
        public DatDescriptor(string name, PropertyDescriptor pd, int ordinal, string fieldname, DatDescriptor parentDescriptor)
            : base(GetFullName(name, parentDescriptor), null)
        {
            _Ordinal = ordinal;
            _FieldName = fieldname;
            _ParentDescriptor = parentDescriptor;
            _PD = pd;
        }

        #region Members
        private int _Ordinal;
        public int Ordinal
        {
            get { return _Ordinal; }
        }

        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
        }

        private bool _IsOffline;
        public bool IsOffline
        {
            get { return _IsOffline; }
            set { _IsOffline = value; }
        }

        private DatDescriptor _ParentDescriptor;
        public DatDescriptor ParentDescriptor
        {
            get { return _ParentDescriptor; }
        }

        private GetValueDelegate _GetValDelegate;
        public GetValueDelegate GetValDelegate
        {
            get { return _GetValDelegate; }
            set { _GetValDelegate = value; }
        }

        private SetValueDelegate _SetValDelegate;
        public SetValueDelegate SetValDelegate
        {
            get { return _SetValDelegate; }
            set { _SetValDelegate = value; }
        }

        private PropertyDescriptor _PD;
        public PropertyDescriptor PD
        {
            get { return _PD; }
            set { _PD = value; }
        }
        #endregion

        private static string GetFullName(string name, DatDescriptor parent)
        {
            if (parent != null && !Common.IsNullOrEmpty(parent.Name))
                return parent.Name + "__" + name;
            return name;
        }
        public override string ToString()
        {
            return string.Format("Name={0}, PropertyType={1}, Ordinal={2}, FieldName={3}", Name, PropertyType, Ordinal, FieldName);
        }

        #region PropertyDescriptor overrides
        public override object GetValue(object component)
        {
            if (ParentDescriptor != null)
                component = ParentDescriptor.GetValue(component);
            if (component is BaseDat)
            {
                if (_GetValDelegate != null)
                    return GetValDelegate(component as BaseDat);
                else
                    return PD.GetValue(component);
            }
            else
                return null;
        }
        public override void SetValue(object component, object value)
        {
            if (ParentDescriptor != null)
                component = ParentDescriptor.GetValue(component);
            if (component is BaseDat)
            {
                if (SetValDelegate != null)
                    SetValDelegate(component as BaseDat, value);
                else
                    PD.SetValue(component, value);
            }
        }

        public override Type PropertyType
        {
            get { return PD.PropertyType; }
        }
        public override bool IsReadOnly { get { return PD.IsReadOnly; } }
        public override Type ComponentType { get { return PD.ComponentType; } }
        public override bool CanResetValue(object component) { return PD.CanResetValue(component); }
        public override void ResetValue(object component)
        {
            PD.ResetValue(component);
        }
        public override bool ShouldSerializeValue(object component)
        {
            return PD.ShouldSerializeValue(component);
        }
        #endregion
    }

    #endregion

    #region DatConverter
    public class DatConverter<TD> : ExpandableObjectConverter where TD : BaseDat<TD>, new()
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(System.String))
                return true;
            if (destinationType == typeof(TD))
                return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String))
            {
                if (value is string)
                    return (string)value;
                if (value is TD)
                    return ((TD)value).ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return false;
            if (sourceType == typeof(TD))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is TD)
            {
                return (TD)value;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    public class DatFilter<TD> where TD : BaseDat<TD>//, new()
    {
        static public void InitFilter(string fltsubstring)
        {
            _FltSubString = fltsubstring;
            _pds = BaseDat<TD>.Dlist.ToArray();
        }

        static public void InitFilter(string fltsubstring, PropertyDescriptor[] pds)
        {
            _FltSubString = fltsubstring;
            _pds = pds;
        }

        static private string _FltSubString;
        static private PropertyDescriptor[] _pds;

        static public bool FilterSubString(TD dat)
        {
            if (Common.IsNullOrEmpty(_FltSubString))
                return true;

            string s = _FltSubString.ToLower();
            foreach (PropertyDescriptor dd in _pds)
            {
                object obj = dd.GetValue(dat);
                if (obj != null && obj.ToString().ToLower().Contains(s))
                    //if (dd.GetValue(dat).ToString().ToLower().Contains(s))
                    return true;
            }
            return false;
        }
    }
    #endregion


    #region DatErrorList - коллекция ошибок
    public class DatErrorList : List<DatErrorList.DatError>
    {
        public void Add(string propertyName, string description)
        {
            DatError de = new DatError(propertyName, description);
            this.Add(de);
        }
        public bool AddIfEmpty(string value, string propertyName)
        {
            bool ret = Common.IsNullOrEmpty(value);
            if (ret) Add(propertyName, "Не задано значение!");
            return ret;
        }
        public bool AddIfEmpty(DateTime value, string propertyName)
        {
            bool ret = (value == DateTime.MinValue);
            if (ret) Add(propertyName, "Не задана дата!");
            return ret;
        }
        public bool AddIfEmpty(Decimal value, string propertyName)
        {
            bool ret = (value == 0);
            if (ret) Add(propertyName, "Должно быть задано число!");
            return ret;
        }
        public bool AddIfEmpty(string value, string propertyName, string description)
        {
            bool ret = (value == null || Common.IsNullOrEmpty(value));
            if (ret) Add(propertyName, description);
            return ret;
        }
        public bool AddIfEmpty(DateTime value, string propertyName, string description)
        {
            bool ret = (value == DateTime.MinValue);
            if (ret) Add(propertyName, description);
            return ret;
        }
        public bool AddIfEmpty(Decimal value, string propertyName, string description)
        {
            bool ret = (value <= 0);
            if (ret) Add(propertyName, description);
            return ret;
        }
        public bool AddIfEmpty<T>(BaseDat<T> dat, string propertyName, string description) where T : BaseDat<T>//, new()
        {
            bool ret = false;
            DatDescriptor dd = BaseDat<T>.GetDescriptor(propertyName);
            if (dd.PropertyType == typeof(string))
                ret = AddIfEmpty((string)dd.GetValue(dat), propertyName, description);
            else if (dd.PropertyType == typeof(DateTime))
                ret = AddIfEmpty((DateTime)dd.GetValue(dat), propertyName, description);
            else if (dd.PropertyType == typeof(decimal))
                ret = AddIfEmpty((decimal)dd.GetValue(dat), propertyName, description);

            return ret;
        }

        public List<string> GetErrorList(string GUID)
        {
            List<string> ret = new List<string>();
            foreach (DatError err in this)
            {
                ret.Add(err.ToString() + string.Format(" в операции '{0}'", GUID));
            }
            return ret;
        }

        public override string ToString()
        {
            string ret = string.Empty;
            foreach (DatError err in this)
            {
                ret += string.Format("{0}: {1}\r\n", err.PropertyName, err.Description);
            }
            return ret;
        }

        public class DatError
        {
            public DatError(string propertyName, string description)
            {
                _PropertyName = propertyName;
                _Description = description;
            }
            string _PropertyName = "";
            public string PropertyName
            {
                get { return _PropertyName; }
                set { _PropertyName = value; }
            }

            string _Description = "";
            public string Description
            {
                get { return _Description; }
                set { _Description = value; }
            }

            public override string ToString()
            {
                return string.Format("ошибка '{0}' в поле '{1}'", Description, PropertyName);
            }
        }
    }
    #endregion
}
