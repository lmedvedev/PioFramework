using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace BO
{
    [TypeConverter(typeof(PathCardConverter))
    , Description("Путь для CardDat классов.")
    ]
    public class PathCard : IComparable, INotifyPropertyChanged
    {
        public PathCard()
        {
            _Parent = null;
        }
        public PathCard(PathTree parent, int code)
        {
            Parent = parent;
            Code = code;
        }
        public PathCard(string spath)
        {
            try
            {
                string[] parts = spath.Split('\\');
                if (string.IsNullOrEmpty(parts[0]))
                    Parent = null;
                else
                    Parent = new PathTree(parts[0]);
                Code = int.Parse(parts[1]);
            }
            catch
            {
                throw new ArgumentException("Строка '" + spath + "' не может быть преобразована в тип PathCard");
            }
        }

        private PathTree _Parent;
        private int _Code = 0;

        [
            Browsable(true)
            , Description("Путь к родительскому узлу.")
            , RefreshProperties(RefreshProperties.Repaint)
        ]
        public PathTree Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;
                NotifyPropertyChanged("Parent");
            }
        }

        [Browsable(true), Description("Код карточки")]
        public int Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                NotifyPropertyChanged("Code");
            }
        }

        [Browsable(true), Description("Код корневого узла.")]
        public int Root
        {
            get { return (Parent != null) ? Parent.Root : 0; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Parent != null) sb.Append(Parent.ToString());
            sb.Append("\\");
            //if (Code > 0) 
            sb.Append(Code.ToString());
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;
            PathCard cp = (PathCard)obj;
            return (PathTree.Equals(Parent, cp.Parent) && int.Equals(Code, cp.Code));
        }

        public static bool Equals(PathCard a, PathCard b)
        {
            if (a == null || b == null) return false;
            if (a.Code != b.Code) return false;
            return PathTree.Equals(a.Parent, b.Parent);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            //return 0;
            if (obj == null) return 1;
            PathCard fp = (PathCard)obj;
            if (fp.Parent == null && Parent != null) return -1;
            if (fp.Parent != null && Parent == null) return 1;
            int i = 0;
            if (Parent != null)
                i = Parent.CompareTo(fp.Parent);
            return (i == 0) ? Code.CompareTo(fp.Code) : i;
        }

        public static int Compare(PathCard a, PathCard b)
        {
            if (a == null && b == null) return 0;
            if (a != null)
                return a.CompareTo(b);
            else
                return -b.CompareTo(a);
        }
        public static bool IsPathCard(string value)
        {
            string[] valArray = value.Trim().Split('\\');
            if (valArray.Length != 2) return false;
            if (!PathTree.IsPathTree(valArray[0])) return false;

            int val;
            if (!int.TryParse(valArray[1], out val)) return false;
            if (val <= 0) return false;

            return true;
        }

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
    public class PathCardConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(PathCard))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is PathCard)
            {

                PathCard so = (PathCard)value;

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
                return new PathCard((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}
