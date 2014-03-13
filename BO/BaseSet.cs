using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Text;
using System.Globalization;
using DA;
using System.Linq;
using System.Xml.Serialization;

namespace BO
{
    #region Простой BaseSet
    public abstract class BaseSet : ICloneable
    {
        protected BaseSet() { }
        public BaseSet(BaseLoadFilter loadFilter)
        {
            if (loadFilter != null)
            {
                LoadFilter = loadFilter.GetFilter();
                Load();
            }
        }

        public BaseSet(IDAOFilter loadFilter)
        {
            if (loadFilter != null)
            {
                LoadFilter = loadFilter;
                Load();
            }
        }

        abstract public object this[int index]
        {
            get;
            set;
        }
        protected IDAOFilter _LoadFilter;
        public IDAOFilter LoadFilter
        {
            get
            {
                if (_LoadFilter == null)
                    _LoadFilter = DataAccessor.NewFilter();
                return _LoadFilter;
            }
            set { _LoadFilter = value; }
        }

        virtual public bool IsCashSet
        {
            get { return false; }
            set { }
        }

        abstract public void FilterApply(string substring);
        abstract public void FilterApply(string substring, PropertyDescriptor[] pds);

        private IDataAccess _DataAccessor = null;
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

        public abstract string TableName();
        public abstract int Add(object value);
        public abstract bool Contains(object value);

        public virtual void Load()
        {
            this.DataAccessor.LoadSet(this.LoadFilter, LoadMembersDelegate, TableName());
        }

        //public virtual void Load(PropertyDescriptorCollection coll)
        //{
        //    List<string> fields = new List<string>();
        //    foreach (DatDescriptor pd in coll)
        //    {
        //        if (!string.IsNullOrEmpty(pd.FieldName) && pd.ParentDescriptor == null)
        //            fields.Add(pd.FieldName);
        //    }
        //    this.DataAccessor.LoadSet(this.LoadFilter, LoadMembersDelegate, TableName(), fields);
        //}
        public virtual void Load(PropertyDescriptorCollection coll)
        {
            List<DatDescriptor> list = new List<DatDescriptor>();
            foreach (DatDescriptor pd in coll)
            {
                if (!string.IsNullOrEmpty(pd.FieldName) && pd.ParentDescriptor == null)
                    list.Add(pd);
            }
            list.Sort(delegate(DatDescriptor dd1, DatDescriptor dd2)
                        {
                            return dd1.Ordinal.CompareTo(dd2.Ordinal);
                        });
            List<string> fields = list.ConvertAll<string>(delegate(DatDescriptor dd)
                        {
                            return dd.FieldName;
                        });

            this.DataAccessor.LoadSet(this.LoadFilter, LoadMembersDelegate, TableName(), fields);
        }
        private void LoadMembersDelegate(object[] reader)
        {
            LoadMembers(reader);
        }
        protected virtual void LoadMembers(object[] reader) { }

        protected static string getIDList(params List<int>[] list)
        {
            //return "";
            List<int> sList = new List<int>(list[0]);
            for (int i = 1; i < list.Length; i++)
            {
                sList.AddRange(list[i]);
            }
            sList.Sort();
            StringBuilder sb = new StringBuilder();
            sb.Append(",0");
            int prev = 0;
            foreach (int i in sList)
            {
                if (i != prev)
                {
                    sb.Append("," + i.ToString());
                    prev = i;
                }
            }
            sb.Remove(0, 1);
            return "where id in(" + sb.ToString() + ")";
        }

        public abstract List<int> GetIDList();
        public virtual object FindByID(int id) { return null; }

        public virtual int Count { get { return 0; } }
        public virtual int IndexOf(object value) { return -1; }
        abstract public int FindIndex(string value, bool strict);
        abstract public PropertyDescriptor SortProperty { get;}
        abstract public DatDescriptor GetDescriptor(string propertyname);
        abstract public object[] ToArray();

        abstract public void Sort();
        abstract public void Sort(string name);
        abstract public void Sort(string name, ListSortDirection sortdirection);
        abstract public void Sort(string name, int sortdirection);
        #region Comparisons
        public static int CardsCodeComparison(ICardDat card1, ICardDat card2)
        {
            if (card1 == null && card2 == null) return 0;
            if (card1 == null) return -1;
            if (card2 == null) return 1;

            if (card1.ID == 0 && card2.ID == 0) return 0;
            if (card1.ID == 0) return -1;
            if (card2.ID == 0) return 1;
            if (card1.ID < 0 && card2.ID > 0) return -1;
            if (card1.ID > 0 && card2.ID < 0) return 1;

            return card1.Code.CompareTo(card2.Code);
        }
        public static int FPComparison(ITreeDat treeDat1, ITreeDat treeDat2)
        {
            if (treeDat1 == null && treeDat2 == null) return 0;
            if (treeDat1 == null) return -1;
            if (treeDat2 == null) return 1;
            return treeDat1.FP.CompareTo(treeDat2.FP);
        }
        public static int FPComparison(ITreeNDat treeDat1, ITreeNDat treeDat2)
        {
            if (treeDat1 == null && treeDat2 == null) return 0;
            if (treeDat1 == null) return -1;
            if (treeDat2 == null) return 1;
            return treeDat1.FPn.CompareTo(treeDat2.FPn);
        }
        public static int FPComparison(ICardDat card1, ICardDat card2)
        {
            if (card1 == null && card2 == null) return 0;
            if (card1 == null) return -1;
            if (card2 == null) return 1;
            return card1.FP.CompareTo(card2.FP);
        }
        public static int FPComparison(ICardNDat card1, ICardNDat card2)
        {
            if (card1 == null && card2 == null) return 0;
            if (card1 == null) return -1;
            if (card2 == null) return 1;
            return card1.FPn.CompareTo(card2.FPn);
        }
        public static int FPComparison(ITreeDictDat dat1, ITreeDictDat dat2)
        {
            if (dat1 == null && dat2 == null) 
                return 0;
            else if (dat1 == null) 
                return -1;
            else if (dat2 == null) 
                return 1;
            else
                return dat1.FP.CompareTo(dat2.FP);
        }
        public static int SCodeComparison(IDictDat dictDat1, IDictDat dictDat2)
        {
            if (dictDat1 == null && dictDat2 == null) return 0;
            if (dictDat1 == null) return -1;
            if (dictDat2 == null) return 1;
            return dictDat1.SCode.CompareTo(dictDat2.SCode);
        }

        #endregion

        #region ICloneable Members

        public abstract object Clone();

        #endregion
        /// <summary>
        /// Метод для сброса Binding
        /// </summary>
        public virtual void FireListReset() { }
        abstract public bool IsDatDerivedFrom(Type type);
        public static string GetIDs(List<int> ids)
        {
            string[] arr = ids.ConvertAll<string>(delegate(int i) { return i.ToString(); }).ToArray();
            return (arr.Length == 0) ? "-1" : string.Join(",", arr);
        }
        public static string GetIDs(IEnumerable dats)
        {
            List<int> ids = new List<int>();
            foreach (object dat in dats)
            {
                PropertyInfo prop = dat.GetType().GetProperty("ID");
                if (prop != null)
                    ids.Add((int)prop.GetValue(dat, null));
            } 
            return GetIDs(ids);
        }
    }
    #endregion

    public abstract class BaseSet<TD, TS> : BaseSet, IBindingList, ITypedList, ISet, ICollection<TD>, ICancelAddNew, IEquatable<TS>
        where TS : BaseSet<TD, TS>//, new()
        where TD : BaseDat<TD>//, new()
    {
        public BaseSet() : base()
        {

        }
        public BaseSet(BaseLoadFilter loadFilter)
            : base(loadFilter)
        {
            //if (loadFilter != null)
            //{
            //    LoadFilter = loadFilter.GetFilter();
            //    Load();
            //}
        }

        public BaseSet(IDAOFilter loadFilter)
            : base(loadFilter)
        {
        }
        #region Internal Lists
        private List<TD> main_list = new List<TD>();

        protected List<TD> Main_list
        {
            get { return main_list; }
            set { main_list = value; }
        }
        private List<TD> flt_list = null;
        protected List<TD> ListDat
        {
            get
            {
                if (flt_list == null)
                    return main_list;
                else
                    return flt_list;
            }
            set
            {
                main_list = value;
                FilterReset();
            }
        }

        public override int Count
        {
            get { return ListDat.Count; }
        }
        public int CountAll
        {
            get { return main_list.Count; }
        }
        #endregion

        #region Filters
        private Predicate<TD> _fltMatch = null;
        public void FilterApply()
        {
            if (_fltMatch != null)
                FilterApply(_fltMatch);
            else
                FilterReset();
        }
        public void FilterApply(Predicate<TD> match)
        {

            _fltMatch = match;
            flt_list = main_list.FindAll(match);
            FireListChanged(ListChangedType.Reset, -1);
        }
        public override void FilterApply(string substring)
        {
            DatFilter<TD>.InitFilter(substring);
            FilterApply(DatFilter<TD>.FilterSubString);
        }
        public override void FilterApply(string substring, PropertyDescriptor[] pds)
        {
            DatFilter<TD>.InitFilter(substring, pds);
            FilterApply(DatFilter<TD>.FilterSubString);
        }
        public void FilterReset()
        {
            flt_list = null;
            FireListChanged(ListChangedType.Reset, -1);
        }

        public override DatDescriptor GetDescriptor(string propertyname)
        {
            return BaseDat<TD>.GetDescriptor(propertyname);
        }
        #endregion

        public long MaxFieldValue(string field)
        {
            long ret = 0;
            foreach (TD d in this.ListDat)
            {
                DatDescriptor dd = BaseDat<TD>.GetDescriptor(field);
                try
                {
                    long r = long.Parse(Common.GetNumberFromString(dd.GetValue(d).ToString()));
                    ret = Math.Max(ret, r);
                }
                catch { }
            }
            return ret;
        }

        #region Sort functions
        public override void Sort()
        {
            ListDat.Sort();
        }
        public void Sort(Comparison<TD> comparison)
        {
            isSorted = false;
            listSortDirection = ListSortDirection.Ascending;
            sortProperty = null;
            ListDat.Sort(comparison);
        }
        public void Sort(IComparer<TD> comparer)
        {
            isSorted = false;
            listSortDirection = ListSortDirection.Ascending;
            sortProperty = null;
            ListDat.Sort(comparer);
        }
        public void Sort(int index, int count, IComparer<TD> comparer)
        {
            isSorted = false;
            listSortDirection = ListSortDirection.Ascending;
            sortProperty = null;
            ListDat.Sort(index, count, comparer);
        }
        public override void Sort(string name)
        {
            Sort(name, ListSortDirection.Ascending);
        }
        public override void Sort(string name, ListSortDirection sortdirection)
        {
            PropertyDescriptor pd = BaseDat<TD>.GetDescriptor(name);
            if (pd != null) ApplySort(pd, sortdirection);
        }

        public override void Sort(string name, int sortdirection)
        {
            PropertyDescriptor pd = BaseDat<TD>.GetDescriptor(name);
            if (pd != null) ApplySort(pd, (ListSortDirection)sortdirection);
        }
        #endregion

        #region Load Functions

        public override string TableName()
        {
            return _searchDat.TableName();
        }
        public override void Load()
        {
            //Если 
            if (IsCashSet && (_LoadFilter == null || _LoadFilter.IsEmpty()))
            {
                if (_SingleSet == null)
                {
                    InnerLoad(null, "[id]");
                    SingleSet = Activator.CreateInstance<TS>();
                    if (Main_list != null)
                        SingleSet.Main_list.AddRange(ListDat);
                }
                else
                {
                    main_list = new List<TD>();
                    if (SingleSet.Main_list != null)
                        main_list.AddRange(SingleSet.Main_list);
                    LoadLinks();
                    AppendLinks();
                    ListDat.Sort();
                }
            }
            else
            {
                InnerLoad(null, "[id]");
            }
            ApplySort();
        }

        internal void InnerLoad(LinkSet ls, string columnname)
        {
            bool isOpen = DataAccessor.ConnectionOpen();
            try
            {
                FilterReset();
                main_list = new List<TD>();
                PrepareLinks();

                FilterID needsIds = ValidateFilterID(columnname);
                
                //base.Load();
                base.Load(BaseDat<TD>.GetProperties());
                
                RemoveGarbage(ls, needsIds);
                LoadLinks();
                AppendLinks();
                ListDat.Sort();
            }
            catch (Exception ex)
            {
                string sEx = string.Format("Error in {0}.Load", typeof(TS));
                throw new Exception(sEx, ex);
            }
            finally
            {
                DataAccessor.ConnectionClose(isOpen);
            }
            FireListChanged(ListChangedType.Reset, -1);
        }

        public virtual void LoadByHeaderDat(IDat header)
        {
            if (HeaderOrdinal == -1)
                throw new Exception(string.Format("Класс {0} не может быть загружен методом LoadByHeaderDat, так как не задана переменная HeaderOrdinal", typeof(TS)));

            LinkSet ls = BaseDat<TD>.RefProps[HeaderOrdinal];
            PropertyDescriptor pd = SubSetList[ls.PropertyIndex];
            BaseSet headerSet = pd.GetValue(this) as BaseSet;
            if (!headerSet.Contains(header))
                headerSet.Add(header);

            FilterID fid = new FilterID("header_id", header.ID);
            LoadFilter.Reset();
            LoadFilter.AddWhere(fid);

            InnerLoad(ls, "[header_id]");
        }
        //public virtual void LoadByHeaderSet2(BaseSet headerSet)
        //{
        //    if (HeaderOrdinal == -1)
        //        throw new Exception(string.Format("Класс {0} не может быть загружен методом LoadByHeaderSet, так как не задана переменная HeaderOrdinal", typeof(TS)));

        //    LinkSet ls = BaseDat<TD>.RefProps[HeaderOrdinal];
        //    PropertyDescriptor pd = SubSetList[ls.PropertyIndex];
        //    pd.SetValue(this, headerSet);

        //    FilterID fid = new FilterID("header_id");
        //    fid.AddIDRange(headerSet.GetIDList());
        //    LoadFilter.Reset();
        //    LoadFilter.AddWhere(fid);

        //    InnerLoad(ls, "[header_id]");
        //}
        public virtual void LoadByHeaderSet(BaseSet headerSet)
        {
            if (HeaderOrdinal == -1)
                throw new Exception(string.Format("Класс {0} не может быть загружен методом LoadByHeaderSet, так как не задана переменная HeaderOrdinal", typeof(TS)));

            if (headerSet == null || headerSet.Count == 0)
                return;

            LinkSet ls = BaseDat<TD>.RefProps[HeaderOrdinal];
            PropertyDescriptor pd = SubSetList[ls.PropertyIndex];
            pd.SetValue(this, headerSet);

            FilterID fid = new FilterID("header_id");
            fid.AddIDRange(headerSet.GetIDList());
            LoadFilter.Reset();
            LoadFilter.AddWhere(fid);

            InnerLoad(ls, "[header_id]");
        }
        protected void RemoveGarbage(LinkSet ls, FilterID flt)
        {
            if (flt != null)
            {
                List<TD> newList = new List<TD>(flt.IdCount);
                Dictionary<int, List<int>> refs = new Dictionary<int, List<int>>(BaseDat<TD>.RefProps.Count);
                foreach (int key in BaseDat<TD>.RefProps.Keys)
                {
                    refs[key] = new List<int>(flt.IdCount);
                }
                for (int i = 0; i < main_list.Count; i++)
                {
                    //TD dat = main_list[i];
                    int id;
                    if (ls == null)
                        id = ((IDat)main_list[i]).ID;
                    else
                        id = ls[i];
                    if (flt.Contain(id))
                    {
                        newList.Add(main_list[i]);
                        foreach (KeyValuePair<int, LinkSet> link in BaseDat<TD>.RefProps)
                        {
                            refs[link.Key].Add(link.Value[i]);
                        }
                    }
                }
                main_list = newList;
                foreach (int key in BaseDat<TD>.RefProps.Keys)
                {
                    BaseDat<TD>.RefProps[key].Clear();
                    BaseDat<TD>.RefProps[key].AddRange(refs[key]);
                }
            }

        }

        protected FilterID ValidateFilterID(string columnname)
        {
            FilterID needsIds = null;
            if (LoadFilter.WhereList != null)
            {
                for (int i = 0; i < LoadFilter.WhereList.Count; i++)
                {
                    FilterUnitBase flt = LoadFilter.WhereList[i];
                    if (flt is FilterID
                        && ((FilterID)flt).Column.ToLower() == columnname //"[id]"
                        && ((FilterID)flt).IdCount > 5000)
                    {
                        needsIds = (FilterID)flt;
                        LoadFilter.RemoveWhere(flt);
                    }
                }
            }
            return needsIds;
        }
        //protected void RemoveGarbage(FilterID flt)
        //{
        //    if (flt != null)
        //    {
        //        List<TD> newList = new List<TD>(flt.IdCount);
        //        Dictionary<int, List<int>> refs = new Dictionary<int, List<int>>(BaseDat<TD>.RefProps.Count);
        //        foreach (int key in BaseDat<TD>.RefProps.Keys)
        //        {
        //            refs[key] = new List<int>(flt.IdCount);
        //        }
        //        for (int i = 0; i < main_list.Count; i++)
        //        {
        //            TD dat = main_list[i];
        //            if (flt.Contain(((IDat)dat).ID))
        //            {
        //                newList.Add(dat);
        //                foreach (KeyValuePair<int, LinkSet> link in BaseDat<TD>.RefProps)
        //                {
        //                    refs[link.Key].Add(link.Value[i]);
        //                }
        //            }
        //        }
        //        main_list = newList;
        //        foreach (int key in BaseDat<TD>.RefProps.Keys)
        //        {
        //            BaseDat<TD>.RefProps[key].Clear();
        //            BaseDat<TD>.RefProps[key].AddRange(refs[key]);
        //        }
        //    }
        //}
        protected void PrepareLinks()
        {
            foreach (int key in BaseDat<TD>.RefProps.Keys)
            {
                BaseDat<TD>.RefProps[key].Clear();
            }
        }

        protected void LoadLinks()
        {
            int loadedSubSet = -1;
            if (HeaderOrdinal >= 0)
            {
                LinkSet ls = BaseDat<TD>.RefProps[HeaderOrdinal];
                loadedSubSet = ls.PropertyIndex;
            }

            for (int i = 0; i < SubSetList.Count; i++)
            {
                if (i != loadedSubSet)
                {
                    PropertyDescriptor pd = SubSetList[i];
                    BaseSet subClass = pd.GetValue(this) as BaseSet;

                    List<int> idList = new List<int>();
                    foreach (LinkSet link in BaseDat<TD>.RefProps.Values)
                    {
                        if (link.PropertyIndex == i)
                            idList.AddRange(link);
                    }
                    idList.Sort();
                    List<int> idListTrimmed = new List<int>();
                    int prev_id = 0;
                    foreach (int id in idList)
                    {
                        if (prev_id != id)
                        {
                            prev_id = id;
                            idListTrimmed.Add(id);
                        }
                    }
                    if (idListTrimmed.Count > 0)
                    {
                        if (!subClass.IsCashSet)
                        {
                            FilterID idfilter = new FilterID("id");
                            idfilter.AddIDRange(idListTrimmed);
                            subClass.LoadFilter.Reset();
                            subClass.LoadFilter.AddWhere(idfilter);
                        }
                        subClass.Load();
                    }
                }
            }
        }
        protected void AppendLinks()
        {
            foreach (KeyValuePair<int, LinkSet> link in BaseDat<TD>.RefProps)
            {
                LinkSet listRefs = link.Value;

                if (listRefs.Count > 0 && listRefs.PropertyIndex >= 0)
                {
                    int index = listRefs.PropertyIndex;
                    BaseSet subClass = SubSetList[index].GetValue(this) as BaseSet;

                    SetValueDelegate setValue = listRefs.SetValue;

                    for (int i = 0; i < listRefs.Count; i++)
                    {
                        int key = listRefs[i];
                        if (key == 0)
                            setValue((TD)this[i], null);
                        else
                            setValue((TD)this[i], subClass.FindByID(key));
                    }
                    listRefs.Clear();
                }
            }
        }
        protected virtual TD LoadDat(object[] reader)
        {
            //TD dat = new TD();
            TD dat = Activator.CreateInstance(typeof(TD)) as TD;

            //foreach (DatDescriptor dd in DatProps)
            //{
            //    if (!links.ContainsKey(dd.Ordinal))
            //        dd.SetValue(dat, reader[dd.Ordinal]);
            //}

            foreach (KeyValuePair<int, SetValueDelegate> item in BaseDat<TD>.ValProps)
            {
                try
                {
                    item.Value(dat, reader[item.Key]);
                }
                catch (Exception Ex)
                {
                    string s = Ex.ToString();
                    throw;
                }
            }
            return dat as TD;
        }
        
        public virtual void Delete()
        {
            foreach (TD dat in this)
                dat.Delete();
        }

        public void LoadDat(IDat dat)
        {
            if (dat != null)
            {
                TD find = FindByID(dat.ID) as TD;
                if (find != null)
                    find.CopyTo((TD)dat);
            }
        }

        protected override void LoadMembers(object[] reader)
        {
            TD dat = LoadDat(reader);
            foreach (KeyValuePair<int, LinkSet> link in BaseDat<TD>.RefProps)
                link.Value.Add(BaseDat.O2Int32(reader[link.Key]));
            main_list.Add(dat);
        }
        //protected override void LoadMembers(object[] reader)
        //{
        //    TD dat = LoadDat(reader);
        //    foreach (KeyValuePair<int, LinkSet> link in BaseDat<TD>.RefProps)
        //    {
        //        if (reader[link.Key] is int)
        //            link.Value.Add(BaseDat.O2Int32(reader[link.Key]));
        //        //else if (reader[link.Key] is Guid)
        //        //    link.Value.Add(BaseDat.O2Int32(reader[link.Key]));
        //    }   
        //    main_list.Add(dat);
        //}

        public override object FindByID(int id)
        {
            PropertyDescriptor pd = SortProperty;
            ListSortDirection dir = listSortDirection;
            if (pd == null || pd.Name.ToLower() != "id")
                Sort("id");

            ((IDat)_searchDat).ID = id;
            int index = ListDat.BinarySearch(_searchDat);
            object ret = null;
            if (index >= 0)
                ret = ListDat[index];
            if (pd != null && pd.Name.ToLower() != "id")
                ApplySort(pd, dir);
            
            return ret;
        }
        public override int FindIndex(string value, bool strict)
        {
            if (string.IsNullOrEmpty(value)) return -1;
            if (SortProperty == null) return -1;
            PropertyDescriptor pd = SortProperty;
            return ListDat.FindIndex(delegate(TD dat)
                    {
                        //string current = (string)pd.GetValue(dat);
                        string current = pd.GetValue(dat).ToString();
                        return current != null && ((strict) ? (current == value) : (current.ToLower().StartsWith(value.ToLower())));
                    });
        }

        /// <summary>
        /// Показывает, можно ли считать dat-класс типа type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsDatDerivedFrom(Type type)
        {
            return type.IsInstanceOfType(_searchDat);
        }
        #endregion

        #region Static Links
        /// <summary>
        /// Экземпляр класса, один на set-класс. Нужен для реализации поиска. 
        /// Или просто для обращения к методам или свойствам Dat-класса.
        /// </summary>
        private static TD _searchDat = Activator.CreateInstance<TD>();//new TD();
        private static List<PropertyDescriptor> SubSetList = new List<PropertyDescriptor>();

        protected static bool _IsCashSet = false;
        /// <summary>
        /// Если true, то при Load сначала проверяется SingleSet, 
        /// и если он !=null, то Set-класс берется оттуда. 
        /// Копируются только линки, сами Dat-классы не копируются.
        /// </summary>
        override public bool IsCashSet
        {
            get { return _IsCashSet; }
            set { _IsCashSet = value; }
        }

        private static TS _SingleSet = null;
        public static TS SingleSet
        {
            get
            {
                //if (_SingleSet == null)
                //{
                //    //_SingleSet = new TS();
                //    _SingleSet = Activator.CreateInstance<TS>();
                //    _SingleSet.Load();
                //}
                return _SingleSet;
            }
            set { _SingleSet = value; }
        }
        //public static void SingleSetReload()
        //{
        //    if (_SingleSet == null)
        //    {
        //        _SingleSet = Activator.CreateInstance<TS>();
        //        //_SingleSet = new TS();
        //    }
        //    SingleSet.Load();
        //}
        static BaseSet()
        {
            CreateLinks();
        }
        public static void CreateLinks()
        {
            try
            {
                PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(TS));
                foreach (PropertyDescriptor pd in pdc)
                {
                    Attribute ac = pd.Attributes[typeof(FieldInfoOrdinals)];
                    if (ac is FieldInfoOrdinals)
                    {
                        SubSetList.Add(pd);
                        FieldInfoOrdinals fid = (FieldInfoOrdinals)ac;
                        foreach (int ordinal in fid.Ordinals)
                        {
                            LinkSet ls = BaseDat<TD>.RefProps[ordinal];
                            ls.PropertyIndex = SubSetList.Count - 1;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string sEx = string.Format("Error in {0}.CreateLinks", typeof(TS));
                throw new Exception(sEx, ex);
            }
        }

        protected static int HeaderOrdinal = -1;


        public static IDataAccess ClassDataAccessor
        {
            get { return BaseDat<TD>.ClassDataAccessor; }
            set { BaseDat<TD>.ClassDataAccessor = value;}
        }

        [
        Bindable(BindableSupport.No)
        , SettingsBindable(false)
        , Browsable(false)
        , ReadOnly(true)
        ]
        [XmlIgnore]
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

        #region Interfaces Implementations
        #region IBindingList Members

        #region Events
        public override void FireListReset()
        {
            FireListChanged(ListChangedType.Reset, -1);
        }
        public void FireListChanged(ListChangedType type, int index)
        {
            this.OnListChanged(new ListChangedEventArgs(type, index));
        }
        private ListChangedEventArgs resetEvent = new ListChangedEventArgs(ListChangedType.Reset, -1);
        protected ListChangedEventHandler onListChanged;

        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }
        protected virtual void OnListChanged(ListChangedEventArgs ev)
        {
            if (onListChanged != null)
            {
                onListChanged(this, ev);
            }
        }
        #endregion

        #region Public bool Properties

        private bool _AllowEdit = true;
        private bool _AllowNew = true;
        private bool _AllowRemove = true;
        private bool _SupportsChangeNotification = true;
        private bool _SupportsSearching = true;
        private bool _SupportsSorting = true;

        public bool AllowEdit
        {
            get { return _AllowEdit; }
            set
            {
                _AllowEdit = value;
                OnListChanged(resetEvent);
            }
        }
        public bool AllowNew
        {
            get { return _AllowNew; }
            set
            {
                _AllowNew = value;
                OnListChanged(resetEvent);
            }
        }
        public bool AllowRemove
        {
            get { return _AllowRemove; }
            set
            {
                _AllowRemove = value;
                OnListChanged(resetEvent);
            }
        }
        public bool SupportsChangeNotification
        {
            get { return _SupportsChangeNotification; }
            set
            {
                _SupportsChangeNotification = value;
                OnListChanged(resetEvent);
            }
        }
        public bool SupportsSearching
        {
            get { return _SupportsSearching; }
            set
            {
                _SupportsSearching = value;
                OnListChanged(resetEvent);
            }
        }

        public bool SupportsSorting
        {
            get { return _SupportsSorting; }
            set
            {
                _SupportsSorting = value;
                OnListChanged(resetEvent);
            }
        }

        #endregion

        #region Sort
        protected bool isSorted = false;
        protected ListSortDirection listSortDirection = ListSortDirection.Ascending;
        protected PropertyDescriptor sortProperty = null;

        public bool IsSorted
        {
            get { return isSorted; }
        }
        public ListSortDirection SortDirection
        {
            get { return listSortDirection; }
        }
        public override PropertyDescriptor SortProperty
        {
            get { return sortProperty; }
        }

        public void AddIndex(PropertyDescriptor property)
        {
            isSorted = true;
            sortProperty = property;
        }
        public void RemoveIndex(PropertyDescriptor property)
        {
            sortProperty = null;
        }

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction)
        {
            if (isSorted && sortProperty == property)
            {
                if (listSortDirection != direction)
                {
                    listSortDirection = direction;
                    ListDat.Reverse();
                }
            }
            else
            {
                isSorted = true;
                sortProperty = property;
                listSortDirection = direction;
                ListDat.Sort(new PropertyComparer(property, direction));
            }
            OnListChanged(resetEvent);
        }
        public void ApplySort()
        {
            if (isSorted)
            {
                ListDat.Sort(new PropertyComparer(sortProperty, listSortDirection));
                OnListChanged(resetEvent);
            }
        }

        public void RemoveSort()
        {
            isSorted = false;
            sortProperty = null;
            OnListChanged(resetEvent);
        }
        #endregion

        public virtual object AddNew()
        {
            TD dat = Activator.CreateInstance<TD>(); //new TD();
            Add(dat);
            this.addNewPos = (dat != null) ? this.IndexOf(dat) : -1;
            return dat;
        }

        public int Find(PropertyDescriptor property, object key)
        {
            PropertyInfo pi = typeof(TD).GetProperty(property.Name);

            foreach (object o in this)
            {
                if (Match(typeof(TD).GetProperty(property.Name).GetValue(o, null), key))
                    return this.IndexOf(o);
            }
            return -1;
        }
        public TD Find(Predicate<TD> match)
        {
            return ListDat.Find(match);
        }
        public List<TD> FindAll(Predicate<TD> match)
        {
            return ListDat.FindAll(match);
        }

        #region Comparer
        public class PropertyComparer : IComparer<TD>
        {
            private static PropertyDescriptor _pd;
            ListSortDirection _direction;
            public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
            {
                _pd = property;
                _direction = direction;
            }
            #region IComparer<T> Members
            private object getValue(TD x)
            {
                return _pd.GetValue(x);
            }
            public int Compare(TD x, TD y)
            {
                object a = getValue(x);
                object b = getValue(y);
                int ret;
                if (a != null && b == null)
                    ret = 1;
                else if (a == null && b != null)
                    ret = -1;
                else if (a == null && b == null)
                    ret = 0;
                else
                {
                    if (a is IComparable)
                        ret = ((IComparable)a).CompareTo(b);
                    else
                        ret = a.ToString().CompareTo(b.ToString());
                }
                if (_direction == ListSortDirection.Descending)
                    ret = -ret;
                return ret;
            }

            #endregion
        }

        #endregion
        protected bool Match(object data, object searchValue)
        {
            // handle nulls
            if (data == null || searchValue == null)
            {
                return (bool)(data == searchValue);
            }

            // if its a string, our comparisons should be 
            // case insensitive.
            bool IsString = (bool)(data is string);


            // bit of validation b4 we start...
            if (data.GetType() != searchValue.GetType())
                throw new ArgumentException("Objects must be of the same type");

            if (!(data.GetType().IsValueType || data is string))
                throw new ArgumentException("Objects must be a value type");



            /*
             * Less than zero a is less than b. 
             * Zero a equals b. 
             * Greater than zero a is greater than b. 
             */

            if (IsString)
            {
                string stringData = ((string)data).ToLower(CultureInfo.CurrentCulture);
                string stringMatch = ((string)searchValue).ToLower(CultureInfo.CurrentCulture);

                return (bool)(stringData == stringMatch);
            }
            else
            {
                return (bool)(Comparer.Default.Compare(data, searchValue) == 0);
            }
        }

        #endregion

        #region IList Members
        public static TS Create(IEnumerable<TD> vals)
        {
            if (vals == null || vals.Count() == 0)
                return null;
            TS ret = Activator.CreateInstance<TS>();
            ret.AddRange(vals.Where(i => i != null));
            return ret;
        }

        public void AddRange(IEnumerable<TD> vals)
        {
            foreach (TD val in vals)
            {
                Add(val);
            }
        }
        public void AddRangeDistinct(IEnumerable<TD> vals)
        {
            foreach (TD val in vals)
            {
                if (!Contains(val))
                    Add(val);
            }
        }
        
        public void Add(TD dat)
        {
            Dictionary<int, LinkSet> refs = BaseDat<TD>.RefProps;
            foreach (KeyValuePair<int, LinkSet> kv in refs)
            {
                LinkSet ls = kv.Value;

                object subDat = ls.GetValue(dat);
                if (subDat != null)
                {
                    int index = ls.PropertyIndex;
                    BaseSet subSet = (BaseSet)SubSetList[index].GetValue(this);
                    if (subDat is IDat)
                    {
                        object findDat = subSet.FindByID(((IDat)subDat).ID);
                        if (findDat != null)
                            ls.SetValue(dat, findDat);
                        else
                            subSet.Add(subDat);
                    }
                }
            }
            main_list.Add(dat);
            FireListChanged(ListChangedType.ItemAdded, ListDat.Count - 1);
            dat.PropertyChanged -= delegate(object snd, PropertyChangedEventArgs ev) { this.FireListChanged(ListChangedType.ItemChanged, this.IndexOf(dat)); };
            dat.PropertyChanged += delegate(object snd, PropertyChangedEventArgs ev) { this.FireListChanged(ListChangedType.ItemChanged, this.IndexOf(dat)); };
        }

        public override int Add(object value)
        {
            // Здесь надо прописать не только добавление Dat-класса,
            // но и добавление подклассов в соответствующие под-Set-классы.
            // Оставим на потом. Андрей

            // Вроде сделал. Потестируем...
            Add((TD)value);
            return main_list.Count - 1;
        }

        public void Clear()
        {
            flt_list = null;
            main_list.Clear();
            OnListChanged(resetEvent);
        }

        public override bool Contains(object value)
        {
            if (value is TD)
                return ListDat.Contains((TD)value);
            else
                return false;
        }

        public override int IndexOf(object value)
        {
            if (value is TD)
                return ListDat.IndexOf((TD)value);
            else
                return -1;
        }

        public void Insert(int index, object value)
        {
            // Здесь надо прописать не только добавление Dat-класса,
            // но и добавление подклассов в соответствующие под-Set-классы.
            // Оставим на потом. Андрей
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        private bool _IsReadOnly = true;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set
            {
                _IsReadOnly = value;
                OnListChanged(resetEvent);
            }
        }

        public void Remove(object value)
        {
            int index = IndexOf(value);
            RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            main_list.RemoveAt(index);
            FireListChanged(ListChangedType.ItemDeleted, index);
        }

        public override object this[int index]
        {
            get 
            {
                if (index < 0)
                    return null;
                return ListDat[index]; 
            }
            set
            {
                ListDat[index] = (TD)value;
                FireListChanged(ListChangedType.ItemChanged, index);
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < ListDat.Count; i++)
                array.SetValue(ListDat[i], i);
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return ListDat.GetEnumerator();
        }

        #endregion

        #region ITypedList Members

        public PropertyDescriptorCollection GetItemProperties(PropertyDescriptor[] listAccessors)
        {
            return BaseDat<TD>.GetProperties();
        }

        public string GetListName(PropertyDescriptor[] listAccessors)
        {
            return typeof(TD).FullName;
        }

        #endregion

        #region IRow
        public List<I> ConvertType<I>() where I : class
        {
            return ListDat.ConvertAll<I>(delegate(TD dat) { return dat as I; });
        }
        public List<I> ConvertType<I>(Converter<TD, I> cnvrt) //where I : class
        {
            return ListDat.ConvertAll<I>(delegate(TD dat) { return cnvrt(dat); });
        }
        public List<I> ConvertAll<I>(Converter<TD, I> cnvrt) 
        {
            return ListDat.ConvertAll<I>(cnvrt);
        }

        public List<KeyValuePair<int, string>> Convert2KeyValuePairs()
        {

            return ListDat.ConvertAll<KeyValuePair<int, string>>(delegate(TD dat) 
            { 
                return new KeyValuePair<int, string>(((IDat)dat).ID, dat.ToString());
            });
                    
        }

        public override List<int> GetIDList()
        {
            return ListDat.ConvertAll<int>(delegate(TD dat) { return ((IDat)dat).ID; });
        }
        public override object[] ToArray()
        {
            if (ListDat != null)
                return ListDat.ToArray();
            else
                return null;
        }
        public string GetIDs()
        {
            return GetIDs(GetIDList());
        }
        #endregion

        #region IClonable Members
        public override object Clone()
        {
            TS newSet = Activator.CreateInstance<TS>(); //new TS();
            newSet.DataAccessor = this.DataAccessor;
            newSet.AddRange(ListDat.Select(i => i.Clone() as TD));
            return newSet;
        }
        #endregion

        #region ICollection<TD> Members

        public bool Contains(TD item)
        {
            return ListDat.Contains(item);
        }

        public void CopyTo(TD[] array, int arrayIndex)
        {
            ListDat.CopyTo(array, arrayIndex);
        }

        public bool Remove(TD item)
        {
            int index = IndexOf(item);
            bool ret = ListDat.Remove(item);

            FireListChanged(ListChangedType.ItemDeleted, index);
            return ret;
        }

        #endregion

        #region IEnumerable<TD> Members

        IEnumerator<TD> IEnumerable<TD>.GetEnumerator()
        {
            return ListDat.GetEnumerator();
        }

        #endregion

        #region ICancelAddNew Members
        private int addNewPos = -1;

        public void CancelNew(int itemIndex)
        {
            if ((this.addNewPos >= 0) && (this.addNewPos == itemIndex))
            {
                this.RemoveAt(this.addNewPos);
                this.addNewPos = -1;
            }
        }

        public void EndNew(int itemIndex)
        {
            if ((this.addNewPos >= 0) && (this.addNewPos == itemIndex))
            {
                this.addNewPos = -1;
            }
        }

        #endregion
        #endregion

        #region IEquatable<TS> Members

        public bool Equals(TS other)
        {
            if (other == null) return false;
            if (other.CountAll != this.CountAll) return false;
            List<TD> list = other.ConvertType<TD>();
            for (int i = 0; i < CountAll; i++)
            {
                if (!BaseDat<TD>.CompareDat(main_list[i], list[i]))
                    return false;
            }
            return true;
        }

        #endregion
    }

    #region Вспомогательные классы
    public class LinkSet : List<int>
    {
        public LinkSet() : base() { }
        public LinkSet(GetValueDelegate getValue, SetValueDelegate setValue)
            : base()
        {
            SetValue = setValue;
            GetValue = getValue;
        }
        public int PropertyIndex = -1;
        public SetValueDelegate SetValue = null;
        public GetValueDelegate GetValue = null;
    }
    public class SortState<TD> where TD : BaseDat<TD>
    {
        public SortState()
        {

        }

        public SortState(string prname, ListSortDirection sd, IComparer<TD> comp)
        {
            sortdirection = sd;
            sortProperty = prname;
            comparison = comp;
        }
        private ListSortDirection sortdirection = ListSortDirection.Ascending;
        private string sortProperty = "";
        private IComparer<TD> comparison = null;

        public ListSortDirection Sortdirection
        {
            get { return sortdirection; }
            set { sortdirection = value; }
        }
        public string SortProperty
        {
            get { return sortProperty; }
            set { sortProperty = value; }
        }
        public IComparer<TD> Comparison
        {
            get { return comparison; }
            set { comparison = value; }
        }
    }
    #endregion
}
