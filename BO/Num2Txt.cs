using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class Num2Txt
    {
        public static string Int2Text(int val)
        {
            //return Decimal2Text((decimal)val);
            return INT(val);
        }

        public static string Decimal2Text(decimal val)
        {
            //return CreateNumString(val, true, "", "", "", false, "", "", ""); 
            return UNIT(val);
        }

        public static string Unit2Text(decimal val)
        {
            //return CreateNumString(val, false, "", "", "", false, "�����", "�����", "����");
            return Str((decimal)val, CurrencyType.COUNT, true);
        }

        public static string INTEG5(decimal val)
        {
            return INTEG(val);
        }
        public static string Rub2Text(decimal val)
        {
            //return CreateNumString(Math.Round(val, 2), true, "�����", "�����", "������", false, "�������", "�������", "������");
            return RUR(val);
        }

        public static string USD2Text(decimal val)
        {
            //return CreateNumString(Math.Round(val, 2), true, "������", "�������", "��������", true, "����", "�����", "������");
            return USD(val);
        }

        public static string Share2Text(decimal val)
        {
            //return CreateNumString(val, true, "", "", "", false, "���", "���", "����");
            return UNIT(val);
        }

        #region Bugaev Commented
        //public static string CreateNumString(decimal val, bool seniorMale, string seniorOne, string seniorTwo, string seniorFive, bool juniorMale, string juniorOne, string juniorTwo, string juniorFive)
        //{
        //    if (val == 0) return "-";
        //    string ret = (val < 0) ? "����� " : "";
        //    string value = Math.Abs(val).ToString().Replace(",", ".");
        //    int i = value.IndexOf(".");
        //    ret += GetIntegral(value.Substring(0, ((i > 0) ? i : value.Length)).ToCharArray(), 0);
        //    ret += ((ret == "") ? "���� " : "") + "����� ";
        //    ret = ret.Replace("���� �����", "���� �����").Replace("��� �����", "��� �����");
        //    string fractal = value.Substring(i + 1);
        //    if (seniorOne != "" || seniorTwo != "" || seniorFive != "")
        //    {
        //        ret += ((i < 0) ? "" : GetIntegral(fractal.ToCharArray(), 0));
        //        ret = ret.Replace("���� ����� ", ((seniorMale) ? "���� " : "���� ") + seniorOne + " ");
        //        ret = ret.Replace("��� ����� ", ((seniorMale) ? "��� " : "��� ") + seniorTwo + " ");
        //        ret = ret.Replace("����� ", seniorTwo + " ");
        //        ret = ret.Replace("����� ", seniorFive + " ");
        //    }
        //    else
        //    {
        //        fractal = fractal.TrimEnd('0');
        //        ret += ((i < 0) ? "" : GetFractal(fractal.ToCharArray()));
        //    }
        //    if (juniorOne != "" || juniorTwo != "" || juniorFive != "")
        //    {
        //        ret = ret.Replace("���� " + seniorFive + " ", "");
        //        if (i >= 0)
        //        {
        //            if (seniorOne == "" && seniorTwo == "" && seniorFive == "")
        //                ret += juniorTwo;
        //            else if (fractal != "")
        //            {
        //                string end = GetIntegral(fractal.ToCharArray(), 0);
        //                ret = ret.Replace(GetFractal(fractal.ToCharArray()), end);
        //                if (fractal.EndsWith("1"))
        //                    ret += juniorOne;
        //                else if (fractal.EndsWith("2"))
        //                    ret += juniorTwo;
        //                else if (fractal.EndsWith("3") || fractal.EndsWith("4"))
        //                    ret += juniorTwo;
        //                else if (end == "")
        //                    ret += fractal + " " + juniorFive;
        //                else
        //                    ret += juniorFive;
        //                if (!juniorMale)
        //                {
        //                    ret = ret.Replace("���� " + juniorOne, "���� " + juniorOne);
        //                    ret = ret.Replace("��� " + juniorTwo, "��� " + juniorTwo);
        //                }
        //            }
        //        }
        //        else if (ret.EndsWith("���� ����� "))
        //            ret = ret.Replace("���� ����� ", ((seniorMale) ? "���� " : "���� ") + juniorOne);
        //        else if (ret.EndsWith("��� ����� "))
        //            ret = ret.Replace("��� ����� ", ((seniorMale) ? "��� " : "��� ") + juniorTwo);
        //        else if (ret.EndsWith("��� ����� ") || ret.EndsWith("������ ����� "))
        //            ret = ret.Replace("����� ", juniorTwo);
        //        else
        //            ret = ret.Replace("����� ", juniorFive);
        //    }
        //    if (ret.Length > 0) ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1);
        //    return ret;
        //}

        //public static string GetIntegral(char[] val, int deep)
        //{
        //    if (val.Length == 0) return "";
        //    string ret = "";
        //    string[,] numbers = {
        //     { "", "���� ", "��� ", "��� ", "������ ", "���� ", "����� ", "���� ", "������ ", "������ "},
        //     { "", "������ ", "�������� ", "�������� ", "����� ", "��������� ", "���������� ", "��������� ", "����������� ", "��������� " },
        //     { "", "��� ", "������ ", "������ ", "��������� ", "������� ", "�������� ", "������� ", "��������� ", "��������� " } };
        //    int num = int.Parse(val[val.Length - 1].ToString());
        //    Array.Resize<char>(ref val, val.Length - 1);
        //    ret = GetIntegral(val, deep + 1) + numbers[deep % 3, num] + ret;
        //    ret = ret.Replace("������ ����", "�����������").Replace("������ ���", "����������").Replace("������ ���", "����������").Replace("������ ������", "������������").Replace("������ ����", "����������").Replace("������ �����", "�����������").Replace("������ ����", "����������").Replace("������ ������", "������������").Replace("������ ������", "������������");
        //    ret = ret.Replace("���� ������", "���� ������").Replace("��� ������", "��� ������");
        //    ret = ret.Replace("���� �����", "���� ������").Replace("��� �����", "��� ������");
        //    string addition = (deep % 3 == 0) ? new string[] { "", "�����", "�������", "��������", "��������", "���������" }[deep / 3] : "";
        //    if (ret != "" && addition != "" && (deep % 3 != 0 || (num != 0 || (val.Length > 1 && val[val.Length - 2] != '0') || (val.Length > 0 && val[val.Length - 1] != '0')))) ret += addition + ((deep == 3) ? ((ret.EndsWith("����� ") || num >= 5 || num == 0) ? " " : ((num == 1) ? "� " : "� ")) : ((ret.EndsWith("����� ") || num >= 5 || num == 0) ? "�� " : ((num == 1) ? " " : "� ")));
        //    return ret;
        //}

        //public static string GetFractal(char[] val)
        //{
        //    if (val.Length == 0) return "";
        //    int num = int.Parse(((val.Length < 2) ? "" : val[val.Length - 2].ToString()) + val[val.Length - 1].ToString());
        //    string addition = new string[] { "", "�����", "���", "������", "������������", "���������", "��������", "��������������", "�����������", "���������" }[val.Length] + ((num % 10 == 1 && num != 11) ? "�� " : "�� ");
        //    return string.Concat(GetIntegral(val, 0), addition).Replace("���� " + addition, "���� " + addition).Replace("��� " + addition, "��� " + addition);
        //}

        #endregion

        #region OldNoReal
        public enum CurrencyType { RUR, USD, EURO, UNIT, KOP, COUNT, SHT, DAY }
        private class RusNumber
        {
            private static string[] hunds ={ "", "��� ", "������ ", "������ ", "��������� ", "������� ", "�������� ", "������� ", "��������� ", "��������� " };
            private static string[] tens ={ "", "������ ", "�������� ", "�������� ", "����� ", "��������� ", "���������� ", "��������� ", "����������� ", "��������� " };

            public static string Str(int val, bool male, string one, string two, string five)
            {
                string[] frac20 =
                {
                    "", "���� ", "��� ", "��� ", "������ ", "���� ", "����� ",
                    "���� ", "������ ", "������ ", "������ ", "����������� ",
                    "���������� ", "���������� ", "������������ ", "���������� ",
                    "����������� ", "���������� ", "������������ ", "������������ "
                };

                int num = val % 1000;
                if (0 == num) return "";
                if (num < 0) throw new ArgumentOutOfRangeException("val", "�������� �� ����� ���� �������������");
                if (!male)
                {
                    frac20[1] = "���� ";
                    frac20[2] = "��� ";
                }

                StringBuilder r = new StringBuilder(hunds[num / 100]);

                if (num % 100 < 20)
                {
                    r.Append(frac20[num % 100]);
                }
                else
                {
                    r.Append(tens[num % 100 / 10]);
                    r.Append(frac20[num % 10]);
                }

                r.Append(Case(num, one, two, five));

                if (r.Length != 0) r.Append(" ");
                return r.ToString();
            }

            public static string Case(int val, string one, string two, string five)
            {
                int t = (val % 100 > 20) ? val % 10 : val % 20;

                switch (t)
                {
                    case 1: return one;
                    case 2:
                    case 3:
                    case 4: return two;
                    default: return five;
                }
            }
        };

        private class CurrencyInfo
        {
            public bool male;
            public string seniorOne, seniorTwo, seniorFive;
            public string juniorOne, juniorTwo, juniorFive;
            public CurrencyInfo()
            {
                male = false; seniorOne = ""; seniorTwo = ""; seniorFive = ""; juniorOne = ""; juniorTwo = ""; juniorFive = "";
            }
            public CurrencyInfo(CurrencyType t)
            {
                switch (t)
                {
                    case CurrencyType.KOP:
                        Register(true, "�������", "�������", "������", "�������", "�������", "������");
                        break;
                    case CurrencyType.RUR:
                        Register(true, "�����", "�����", "������", "�������", "�������", "������");
                        break;
                    case CurrencyType.EURO:
                        Register(true, "����", "����", "����", "��������", "���������", "����������");
                        break;
                    case CurrencyType.USD:
                        Register(true, "������", "�������", "��������", "����", "�����", "������");
                        break;
                    case CurrencyType.UNIT:
                        Register(false, "�����", "�����", "�����", "", "", "");
                        break;
                    case CurrencyType.COUNT:
                        Register(false, "�����", "�����", "�����", "�����", "�����", "����");
                        break;
                    case CurrencyType.SHT:
                        Register(false, "��.", "��.", "��.", "", "", "");
                        break;
                    case CurrencyType.DAY:
                        Register(true, "����", "���", "����", "", "", "");
                        break;
                }
            }
            private void Register(bool mal, string sOne, string sTwo, string sFive, string jOne, string jTwo, string jFive)
            {
                male = mal;
                seniorOne = sOne; seniorTwo = sTwo; seniorFive = sFive;
                juniorOne = jOne; juniorTwo = jTwo; juniorFive = jFive;
            }

        };

        public static string RUR(decimal val)
        {
            return Str(val, CurrencyType.RUR, true);
        }
        public static string DAY(int val)
        {
            return Str(val, CurrencyType.DAY, false);
        }

        public static string RURSHORT(decimal val)
        {
            return Math.Floor((double)val).ToString("#,##0") + " ���. " + (((double)val - Math.Floor((double)val)) * 100).ToString("00") + " ���.";
        }
        public static string INT(int val)
        {
            return INT(val, true);
        }
        public static string INT(int val, bool male)
        {
            return Str((decimal)val, male, "", "", "", "", "", "", false);
        }

        public static string USD(decimal val)
        {
            return Str(val, CurrencyType.USD, true);
        }

        public static string UNIT(decimal val, int fractNum)
        {
            string s = Str(val, CurrencyType.UNIT, false);
            if (val < 0) val = -val;
            int n = (int)val;
            int remainder;
            switch (fractNum)
            {
                case 2:
                    remainder = (int)((val - n + 0.005M) * 100);
                    s += remainder.ToString(" #0 ") + "����� ���";
                    break;
                case 3:
                    remainder = (int)((val - n + 0.0005M) * 1000);
                    s += remainder.ToString(" ##0 ") + "�������� ���";
                    break;
                case 4:
                    remainder = (int)((val - n + 0.00005M) * 10000);
                    s += remainder.ToString(" ###0 ") + "�������������� ���";
                    break;
                case 5:
                    remainder = (int)((val - n + 0.000005M) * 100000);
                    s += remainder.ToString(" ####0 ") + "����������� ���";
                    break;
                case 6:
                    remainder = (int)((val - n + 0.0000005M) * 1000000);
                    s += remainder.ToString(" #####0 ") + "���������� ���";
                    break;
                case 7:
                    remainder = (int)((val - n + 0.00000005M) * 10000000);
                    s += remainder.ToString(" ######0 ") + "���������������� ���";
                    break;
            }
            return s;
        }

        public static string INTEG(decimal val)
        {
            string s = Str(val, CurrencyType.UNIT, false);
            if (val < 0) val = -val;
            int n = (int)val;

            s += " �";

            int fr = (int)((val - n) * 10000000);

            if (fr % 1000000 == 0)
            {
                fr /= 1000000;
                s += fr.ToString(" 0 ") + "�������";
            }
            else if (fr % 100000 == 0)
            {
                fr /= 100000;
                s += fr.ToString(" #0 ") + "�����";
            }
            else if (fr % 10000 == 0)
            {
                fr /= 10000;
                s += fr.ToString(" ##0 ") + "��������";
            }
            else if (fr % 1000 == 0)
            {
                fr /= 1000;
                s += fr.ToString(" ###0 ") + "��������������";
            }
            else if (fr % 100 == 0)
            {
                fr /= 100;
                s += fr.ToString(" ####0 ") + "�����������";
            }
            else if (fr % 10 == 0)
            {
                fr /= 10;
                s += fr.ToString(" #####0 ") + "����������";
            }
            else 
                s += fr.ToString(" ######0 ") + "����������������";

            return s;
        }
        private static string[] fracParts = 
            {
                "�����", "���", "������", "������������", "���������", "��������", "��������������", "�����������", "���������", "���������������", "������������"
            };

        public static string UNIT(decimal val)
        {
            string s = Str(val, CurrencyType.UNIT, false).Trim();
            string sval = val.ToString();
            int pos = sval.IndexOfAny(new char[] { ',', '.' });
            if (pos >= 0)
            {
                sval = sval.Substring(pos + 1);
                sval = sval.TrimEnd('0');
                int len = sval.Length;
                if (len > 0)
                {
                    int iFrac = 0;
                    int.TryParse(sval, out iFrac);
                    string nfr = fracParts[len - 1];
                    string sFrac = Str((decimal)iFrac, false
                        , nfr + "��", nfr + "��", nfr + "��"
                        , "", "", "", false);
                    s += " " + sFrac.ToLower();
                }
            }
            return s;
        }

        static string Str(decimal val, CurrencyType ctype, bool fractPart)
        {
            CurrencyInfo info = new CurrencyInfo(ctype);
            return Str(val, info.male,
                info.seniorOne, info.seniorTwo, info.seniorFive,
                info.juniorOne, info.juniorTwo, info.juniorFive,
                fractPart);
        }

        static string Str(decimal val, bool male, string seniorOne, string seniorTwo, string seniorFive, string juniorOne, string juniorTwo, string juniorFive, bool fractPart)
        {
            bool minus = false;
            if (val < 0) { val = -val; minus = true; }

            int n = (int)val;
            int remainder = (int)((val - n + 0.005M) * 100);

            StringBuilder r = new StringBuilder();

            if (0 == n) r.Append("���� ");
            if (n % 1000 != 0)
                r.Append(RusNumber.Str(n, male, seniorOne, seniorTwo, seniorFive));
            else
                r.Append(seniorFive);

            n /= 1000;

            r.Insert(0, RusNumber.Str(n, false, "������", "������", "�����"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "�������", "��������", "���������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "��������", "���������", "����������"));
            n /= 1000;

            r.Insert(0, RusNumber.Str(n, true, "���������", "����������", "�����������"));
            if (minus) r.Insert(0, "����� ");

            if (fractPart)
            {
                r.Append(remainder.ToString(" 00 "));
                r.Append(RusNumber.Case(remainder, juniorOne, juniorTwo, juniorFive));
            }
            //������ ������ ����� ���������
            r[0] = char.ToUpper(r[0]);

            return r.ToString();
        }
        #endregion
    }
}
