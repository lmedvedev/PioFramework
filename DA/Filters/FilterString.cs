using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.XPath;

namespace DA
{
    /// <summary>
    /// Класс формирующий фильтр для String
    /// </summary>
    public class FilterString : FilterColumnBase
    {
        #region Constructors
        public FilterString(string column, string value) : this("", column, value, false) { }
        public FilterString(string table, string column, string value) : this(table, column, value, false) { }
        public FilterString(string column, string value, bool isLike) : this("", column, value, isLike) { }
        public FilterString(string table, string column, string value, bool isLike)
            : base(table, column)
        {
            _value = value;
            _isLike = isLike;
        }
        #endregion

        #region Fields
        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private bool _isLike;

        public bool IsLike
        {
            get { return _isLike; }
            set { _isLike = value; }
        }
        #endregion
        public void Reset()
        {
          _value = String.Empty;
          _isLike = false;
        }
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_value))
            {
                //return string.Format("({0} is null)"
                //, Column
                //);
                return string.Empty;
            }
            else
            {
                string valueText = _value;
                if (_isLike && !_value.Contains("%"))
                    valueText = "%" + _value + "%";
                
                return string.Format("({0}{1}'{2}')"
                    , Column
                    , (_isLike) ? " like " : "="
                    , valueText
                    );
            }
        }
    }
    public class FilterStringXML : FilterColumnBase
    {
        #region Constructors
        public FilterStringXML(string xmlcolumn, string xmlfunction, string value) : this("", xmlcolumn, xmlfunction, value, false) { }
        public FilterStringXML(string table, string xmlcolumn, string xmlfunction, string value) : this(table, xmlcolumn, xmlfunction, value, false) { }
        public FilterStringXML(string xmlcolumn, string xmlfunction, string value, bool isLike) : this("", xmlcolumn, xmlfunction, value, isLike) { }
        public FilterStringXML(string table, string xmlcolumn, string xmlfunction, string value, bool isLike)
            : base(table, xmlcolumn)
        {
            _value = value;
            _xmlfunction = xmlfunction;
            _isLike = isLike;
        }
        #endregion

        #region Fields
        private string _xmlfunction;
        private string _value;
        private bool _isLike;
        #endregion
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_value))
            {
                return string.Format("({0}.{1} is null)"
                , Column
                , _xmlfunction
                );
            }
            else
            {
                return string.Format("({0}.{1}{2}'{3}')"
                    , Column
                    , _xmlfunction
                    , (_isLike) ? " like " : "="
                    , _value
                    );
            }
        }
    }

    //public class FilterStringXmlValue : FilterColumnBase
    //{
    //    #region Constructors
    //    public FilterStringXmlValue(string table, string column, string value) : this(table, column, value, false) { }
    //    public FilterStringXmlValue(string table, string column, string value, bool isLike)
    //        : base(table, column)
    //    {
    //        _value = value;
    //        _isLike = isLike;
    //    }
    //    #endregion

    //    #region Fields
    //    private string _value;
    //    private bool _isLike;
    //    #endregion
    //    public override string ToString()
    //    {
    //        //where f.p.value('.','varchar(max)') like '%кокин%'

    //        return string.Format("({0}.value('.','varchar(max)') {1} '{2}')"
    //            , Column
    //            , (_isLike) ? " like " : "="
    //            , _value
    //            );
    //    }
    //}

}
