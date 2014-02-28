using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BO
{
    [TypeConverter(typeof(PathTreeNConverter)),
    Description("Путь для TreeNDat классов.")]
    //    [Bindable(true)]
    [Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]

    public class PathTreeN : IComparable<PathTreeN>, IComparable, ICloneable, IEquatable<PathTreeN>, INotifyPropertyChanged
    {
        private List<string> _list;
        public PathTreeN()
        {
            this._list = new List<string>();
        }
        private PathTreeN(List<string> value)
        {
            if (value.Count == 0)
                throw new ArgumentException("Can't create PathTreeN from empty List");
            if (value.TrueForAll(delegate(string item) { return !item.Contains(":"); }))
                _list = value;
            else
                throw new ArgumentException("Can't create PathTreeN from List that contains semicolon.");
        }
        public PathTreeN(params string[] value)
            : this(new List<string>(value)) { }
        public PathTreeN(string value)
            : this(convertStr2List(value)) { }
        public PathTreeN(PathTreeN parent, string code)
        {
            if (code.Contains(":"))
                throw new ArgumentException("Can't create PathTreeN if Code parameter contains semicolon.");

            _list = new List<string>();
            if (parent != null)
                _list.AddRange(parent._list);
            _list.Add(code);
        }
        public static PathTreeN Top(PathTreeN source, int count)
        {
            PathTreeN ret = (PathTreeN)source.Clone();
            if (count <= ret.Count)
                ret._list.RemoveRange(count, ret.Count - count);
            else
                throw new ArgumentException(string.Format("Can't leave {0} nodes, because it have only {1}.", count, ret.Count));
            return ret;
        }
        
        //public void LeaveOnlyStart(int count)
        //{
        //    if (Count > count)
        //        _list.RemoveRange(count, Count - count);
        //    else
        //        throw new ArgumentException(string.Format("Can't leave {0} nodes, because it have only {1}.", count, Count));
        //}
        static private string[] convertStr2List(string value)
        {
            try
            {
                value = value.Trim();
                return Array.ConvertAll<string, string>(value.Trim().Split(':'), delegate(string val)
                    {
                        //int i = int.Parse(val);
                        //if (i < 0) throw new Exception();
                        return val;
                    });
            }
            catch
            {
                throw new ArgumentException("Can not convert '" + value + "' to type PathTreeN");
            }
        }

        public static bool IsPathTreeN(string value, bool allowZero)
        {
            string[] valArray = value.Trim().Split('.');
            for (int i = 0; i < valArray.Length; i++)
            {
                int val;
                if (!int.TryParse(valArray[i], out val)) return false;
                if (val < 0) return false;
                if (val == 0 && !allowZero) return false;
            }
            return true;
        }

        public static bool IsPathTreeN(string value)
        {
            return IsPathTreeN(value, false);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        [Browsable(true), /*ReadOnly(true),*/
        Description("Путь к родительскому узлу.")]
        public PathTreeN Parent
        {
            get
            {
                if (_list.Count == 1) return null;
                List<string> lst = new List<string>(_list);
                lst.RemoveAt(_list.Count - 1);
                return new PathTreeN(lst);
            }
            set
            {
                string code = CodeS;
                _list = value._list;
                _list.Add(code);
                NotifyPropertyChanged("Parent");
            }
        }


        [Browsable(true),/* ReadOnly(true),*/
        Description("Код конечного узла")]
        public string CodeS
        {
            get { return _list[_list.Count - 1]; }
            set
            {
                _list[_list.Count - 1] = value;
                NotifyPropertyChanged("Code");
            }
        }

        public string this[int index]
        {
            get { return _list[index]; }
        }

        [Browsable(true), Description("Код корневого узла.")]
        public string Root
        {
            get { return (_list.Count >= 1) ? _list[0] : ""; }
        }

        [Browsable(true), Description("Корневой узел.")]
        public PathTreeN RootNode
        {
            get
            {
                return new PathTreeN(Root);
            }
        }

        public override string ToString()
        {
            return string.Join(":", _list.ToArray());
        }

        public string ToString(List<string> formats)
        {
            List<string> ret = new List<string>();
            for (int i = 0; i < formats.Count; i++)
            {
                if (_list.Count > i)
                    ret.Add(string.Format(formats[i], _list[i]));
                else
                    ret.Add(string.Format(formats[i], 0));
            }
            for (int i = formats.Count; i < _list.Count; i++)
            {
                ret.Add(string.Format(formats[i], _list[i]));
            }

            return string.Join(":", ret.ToArray());
        }

        public int CompareTo(PathTreeN other)
        {
            if ((object)other == null) return 1;

            for (int i = 0; i < Math.Max(other._list.Count, _list.Count); i++)
            {
                if (i >= other._list.Count) return 1;
                if (i >= _list.Count) return -1;

                //if ()
                int cmp = 0;
                if (PathTree.IsPathTree(_list[i]) && PathTree.IsPathTree(other._list[i]))
                {
                    PathTree l1 = new PathTree(_list[i]);
                    PathTree l2 = new PathTree(other._list[i]);
                    cmp = l1.CompareTo(l2);
                }
                else
                    cmp = string.Compare(_list[i], other._list[i], true);
                if (cmp != 0) return cmp;
                //if (_list[i] > other._list[i]) return 1;
                //if (_list[i] < other._list[i]) return -1;
            }
            return 0;
        }

        public static bool IsChildOf(PathTreeN node, PathTreeN parent)
        {
            if (parent == null) return true;
            if (node == null) return false;
            if (node.Count < parent.Count) return false;
            for (int i = 0; i < parent.Count; i++)
            {
                if (node[i] != parent[i]) return false;
            }
            return true;
        }

        public bool IsChildOf(PathTreeN node)
        {
            return PathTreeN.IsChildOf(this, node);
        }

        #region IConvertible Members

        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ToString(IFormatProvider provider)
        {
            return ToString();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region Equal Members
        public static bool operator ==(PathTreeN a, PathTreeN b)
        {
            if ((a as object) == null && (b as object) == null) return true;
            if ((a as object) == null || (b as object) == null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(PathTreeN a, PathTreeN b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return _list.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;
            PathTreeN path = (PathTreeN)obj;
            return Equals(path);
            //if (path._list.Count != _list.Count) return false;
            //for (int i = 0; i < Math.Max(path._list.Count, _list.Count); i++)
            //{
            //    if (_list[i] != path._list[i]) return false;
            //}
            //return true;
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            PathTreeN pt = obj as PathTreeN;
            if ((object)pt == null) return 1;
            return CompareTo(pt);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            List<string> newlist = new List<string>(_list);
            return new PathTreeN(newlist);
        }
        #endregion

        #region IEquatable<PathTreeN> Members

        public bool Equals(PathTreeN path)
        {
            if (path._list.Count != _list.Count) return false;
            for (int i = 0; i < Math.Max(path._list.Count, _list.Count); i++)
            {
                if (_list[i] != path._list[i]) return false;
            }
            return true;
        }

        #endregion

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
    public class PathTreeNConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(PathTreeN))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is PathTreeN)
            {
                PathTreeN so = (PathTreeN)value;
                return so.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                string s = ((string)value).Trim();
                if (s.Length == 0)
                    return null;
                else
                    return new PathTreeN(s);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}

