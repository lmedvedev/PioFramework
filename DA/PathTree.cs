using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace BO
{
    [TypeConverter(typeof(PathTreeConverter)),
    Description("Путь для TreeDat классов.")]
//    [Bindable(true)]
    [Bindable(BindableSupport.Yes, BindingDirection.TwoWay)]

    public class PathTree : IComparable<PathTree>, IComparable, ICloneable, IEquatable<PathTree>, INotifyPropertyChanged
    {
        private List<int> _list;
        public PathTree()
        {
            this._list = new List<int>();
        }
        private PathTree(List<int> value) 
        {
            if (value.Count == 0)
                throw new ArgumentException("Can't create PathTree from empty List");
            if (value.TrueForAll(delegate(int item) { return item > 0; }))
                _list = value; 
            else
                throw new ArgumentException("Can't create PathTree from List with negative or zero values.");
        }
        public PathTree(params int[] value) 
            : this(new List<int>(value)) { }
        public PathTree(string value)
            : this(convertStr2List(value)) { }
        public PathTree(PathTree parent, int code)
        {
            if(code<=0)
                throw new ArgumentException("Can't create PathTree with negative or zero values of Code parameter.");

            _list = new List<int>();
            if (parent != null)
                _list.AddRange(parent._list);
            _list.Add(code);
        }
        public void RemoveFromStart(int count)
        {
            if (Count > count)
                _list.RemoveRange(0, count);
            else
                throw new ArgumentException(string.Format("Can't remove {0} nodes, because it have only {1}.", count, Count));
        }
        static private int[] convertStr2List(string value)
        {
            try
            {
                value = value.Trim();
                return Array.ConvertAll<string, int>(value.Trim().Split('.'), delegate(string val)
                    {
                        int i = int.Parse(val);
                        if (i < 0) throw new Exception();
                        return i;
                    });
            }
            catch
            {
                throw new ArgumentException("Can not convert '" + value + "' to type PathTree");
            }
        }

        public static bool IsPathTree(string value, bool allowZero)
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

        public static bool IsPathTree(string value)
        {
            return IsPathTree(value, false);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        [Browsable(true), /*ReadOnly(true),*/
        Description("Путь к родительскому узлу.")]
        public PathTree Parent
        {
            get
            {
                if (_list.Count == 1) return null;
                List<int> lst = new List<int>(_list);
                lst.RemoveAt(_list.Count - 1);
                return new PathTree(lst);
            }
            set
            {
                int code = Code;
                _list = value._list;
                _list.Add(code);
                NotifyPropertyChanged("Parent");
            }
        }


        [Browsable(true),/* ReadOnly(true),*/
        Description("Код конечного узла")]
        public int Code
        {
            get { return _list[_list.Count - 1]; }
            set
            {
                _list[_list.Count - 1] = value;
                NotifyPropertyChanged("Code");
            }
        }

        public int this[int index]
        {
            get { return _list[index];}
        }
        
        [Browsable(true), Description("Код корневого узла.")]
        public int Root
        {
            get { return (_list.Count >= 1) ? _list[0] : 0; }
        }

        [Browsable(true), Description("Корневой узел.")]
        public PathTree RootNode
        {
            get
            {
                return new PathTree(Root);
            }
        }

        public override string ToString()
        {
            return string.Join(".", Array.ConvertAll<int, string>(_list.ToArray(), delegate(int val) { return val.ToString(); }));
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

            return string.Join(".", ret.ToArray());
        }

        public int CompareTo(PathTree other)
        {
            if ((object)other == null) return 1;
            for (int i = 0; i < Math.Max(other._list.Count, _list.Count); i++)
            {
                if (i >= other._list.Count) return 1;
                if (i >= _list.Count) return -1;
                if (_list[i] > other._list[i]) return 1;
                if (_list[i] < other._list[i]) return -1;
            }
            return 0;
        }

        public static bool IsChildOf(PathTree node, PathTree parent)
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

        public bool IsChildOf(PathTree node)
        {
            return PathTree.IsChildOf(this,node);
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
        public static bool operator ==(PathTree a, PathTree b)
        {
            if ((a as object) == null && (b as object) == null) return true;
            if ((a as object) == null || (b as object) == null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(PathTree a, PathTree b)
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
            PathTree path = (PathTree)obj;
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
            PathTree pt = obj as PathTree;
            if ((object)pt == null) return 1;
            return CompareTo(pt);
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            List<int> newlist = new List<int>(_list);
            return new PathTree(newlist);
        }
        #endregion

        #region IEquatable<PathTree> Members

        public bool Equals(PathTree path)
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
    public class PathTreeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(PathTree))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is PathTree)
            {
                PathTree so = (PathTree)value;
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
                if (s.Length==0)
                    return null;
                else
                    return new PathTree(s);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}

