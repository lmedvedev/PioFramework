using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace BO
{
    [TypeConverter(typeof(PathCardConverter))
    , Description("Путь для CardDatN классов.")
    ]
    public class PathCardN : IComparable, INotifyPropertyChanged
    {
        public PathCardN()
        {
            _Parent = null;
        }
        public PathCardN(PathTreeN parent, string codeN)
        {
            Parent = parent;
            CodeN = codeN;
        }
        public PathCardN(string spath)
        {
            try
            {
                string[] parts = spath.Split('\\');
                if (string.IsNullOrEmpty(parts[0]))
                    Parent = null;
                else
                    Parent = new PathTreeN(parts[0]);
                CodeN = parts[1];
            }
            catch
            {
                throw new ArgumentException("Строка '" + spath + "' не может быть преобразована в тип PathCardN");
            }
        }

        private PathTreeN _Parent;
        private string _CodeN = "";

        [
            Browsable(true)
            , Description("Путь к родительскому узлу.")
            , RefreshProperties(RefreshProperties.Repaint)
        ]
        public PathTreeN Parent
        {
            get { return _Parent; }
            set
            {
                _Parent = value;
                NotifyPropertyChanged("Parent");
            }
        }

        [Browsable(true), Description("Код карточки N")]
        public string CodeN
        {
            get { return _CodeN; }
            set
            {
                _CodeN = value;
                NotifyPropertyChanged("CodeN");
            }
        }

        [Browsable(true), Description("Код корневого узла.")]
        public string Root
        {
            get { return (Parent != null) ? Parent.Root : null; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Parent != null) sb.Append(Parent.ToString());
            sb.Append("\\");
            //if (Code > 0) 
            sb.Append(CodeN.ToString());
            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;
            PathCardN cp = (PathCardN)obj;
            return (PathTreeN.Equals(Parent, cp.Parent) && int.Equals(CodeN, cp.CodeN));
        }

        public static bool Equals(PathCardN a, PathCardN b)
        {
            if (a == null || b == null) return false;
            if (a.CodeN != b.CodeN) return false;
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
            PathCardN fp = (PathCardN)obj;
            if (fp.Parent == null && Parent != null) return -1;
            if (fp.Parent != null && Parent == null) return 1;
            int i = 0;
            if (Parent != null)
                i = Parent.CompareTo(fp.Parent);
            return (i == 0) ? CodeN.CompareTo(fp.CodeN) : i;
        }

        public static int Compare(PathCardN a, PathCardN b)
        {
            if (a == null && b == null) return 0;
            if (a != null)
                return a.CompareTo(b);
            else
                return -b.CompareTo(a);
        }
        public static bool IsPathCardN(string value)
        {
            string[] valArray = value.Trim().Split('\\');
            if (valArray.Length != 2) return false;
            if (!PathTreeN.IsPathTreeN(valArray[0])) return false;

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
    public class PathCardNConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(PathCardN))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(System.String) && value is PathCardN)
            {

                PathCardN so = (PathCardN)value;

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
                return new PathCardN((string)value);
            return base.ConvertFrom(context, culture, value);
        }
    }
}
