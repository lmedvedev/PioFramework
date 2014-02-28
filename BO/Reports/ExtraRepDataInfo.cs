using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace BO.Reports
{
    public partial class ExtraRepDataInfo
    {
        //static Vars()
        //{
        //    vars = new Vars();
        //    //Deserialize it!
        //}
        public static void Init(string path)
        {
            XmlConverter conv = new XmlConverter();
            conv.AddSchema(global::BO.Properties.Resources.RepDataInfo);
            vars = null;
            if (File.Exists(path))
                vars = conv.Read<Vars>(path);
        }

        public static Vars vars;
        public static object GetValue(object dat, string property)
        {
            bool propIsfound = true;
            return GetValue(dat, property, ref propIsfound);
        }
        public static object GetValue(object dat, string property, ref bool propIsfound)
        {
            if (dat == null)
            {
                propIsfound = false;
                return null;
            }
            else
            {
                propIsfound = true;
                Type datType = dat.GetType();
                varInfo var = GetVar(datType, property.ToUpper());
                if (var == null)
                {
                    propIsfound = false;
                    return ProxyInfo.GetPropValue(dat, property, ref propIsfound);
                }
                try
                {
                    Result result = doEval(dat, var.Item);
                    if (result.HasError)
                        return result.Error;
                    else
                        return result.Value;
                }
                catch
                {
                    return var.err;
                }
            }
        }

        private static Result doEval(object dat, evalBase eval)
        {
            if (eval is evalCALCType)
                return doCalc(dat, (evalCALCType)eval);
            else if (eval is evalArrayType)
                return doValue(dat, (evalArrayType)eval);
            else if (eval is evalDateType)
                return new Result(((evalDateType)eval).val);
            else if (eval is evalNumberType)
                return new Result(((evalNumberType)eval).val);
            else if (eval is evalStringType)
                return new Result(((evalStringType)eval).val);
            else if (eval is evalBoolType)
                return new Result(((evalBoolType)eval).val);
            else if (eval is evalIFType)
                return doIF(dat, (evalIFType)eval);
            else if (eval is evalValueType)
                return doValue(dat, (evalValueType)eval);
            else if (eval is evalSFormatType)
            {
                string sval = doFormat(dat, ((evalSFormatType)eval).val);
                return new Result(sval);
            }
            else
            {
                Result result = new Result(null);
                result.Error = "#Unknown Operation";
                return result;
            }
        }

        private static string doFormat(object dat, string p)
        {
            string format = p.Replace("{", @"{{").Replace("}", @"}}").Replace("[#", "{0:").Replace("#]", "}");
            string ret = string.Format(format, dat);
            return ret;
        }

        private static Result doValue(object dat, evalValueType eval)
        {
            Result result = new Result(null);
            try
            {
                //object val = ProxyInfo.GetPropValue(dat, eval.property);
                object val = ExtraRepDataInfo.GetValue(dat, eval.property);
                result.IsNull = (val == null);

                //object val = dat.GetDatValue(eval.property);
                if (val != null)
                    result.Value = val;
                else
                {
                    if (eval is evalNumberValueType)
                        result.Value = ((evalNumberValueType)eval).isnull;
                    else if (eval is evalDateValueType)
                        result.Value = ((evalDateValueType)eval).isnull;
                    else if (eval is evalStringValueType)
                        result.Value = ((evalStringValueType)eval).isnull;
                    else if (eval is evalBoolValueType)
                        result.Value = ((evalBoolValueType)eval).isnull;
                    else
                        result.Value = null;
                }
                return result;
            }
            catch
            {
                if (Common.IsNullOrEmpty(eval.err))
                    result.Error = "#Error";
                else
                    result.Error = eval.err;
            }
            return result;
        }

        private static Result doIF(object dat, evalIFType eval)
        {
            condBase cond = eval.cond.Item;
            if (doCond(dat, cond))
                return doEval(dat, eval.then.Item);
            else
                return doEval(dat, eval.@else.Item);
        }
        private static bool doCond(object dat, condBase cond)
        {
            if (cond is condANDType)
            {
                condANDType and = (condANDType)cond;
                foreach (condBase anditem in and.Items)
                {
                    if (!doCond(dat, anditem))
                        return false;
                }
                return true;
            }
            else if (cond is condORType)
            {
                condORType or = (condORType)cond;
                foreach (condBase oritem in or.Items)
                {
                    if (doCond(dat, oritem))
                        return true;
                }
                return false;
            }
            else if (cond is condNOTType)
            {
                return !doCond(dat, ((condNOTType)cond).Item);
            }
            else if (cond is condISNULLType)
            {
                condISNULLType isnull = (condISNULLType)cond;
                Result resultisnull = doEval(dat, isnull.Item);
                return resultisnull.IsNull;
            }
            else if (cond is condEQType)
            {
                condEQType eq = (condEQType)cond;
                string a = string.Format("{0}", doEval(dat, eq.Items[0]).Value);
                string b = string.Format("{0}", doEval(dat, eq.Items[1]).Value);
                return string.Equals(a, b, StringComparison.CurrentCultureIgnoreCase);
            }
            else if (cond is condGEType)
            {
                condGEType x = (condGEType)cond;
                return compareVals(dat, x.Items[0], x.Items[1]) >= 0;
            }
            else if (cond is condGTType)
            {
                condGTType x = (condGTType)cond;
                return compareVals(dat, x.Items[0], x.Items[1]) > 0;
            }
            else if (cond is condLEType)
            {
                condLEType x = (condLEType)cond;
                return compareVals(dat, x.Items[0], x.Items[1]) <= 0;
            }
            else if (cond is condLTType)
            {
                condLTType x = (condLTType)cond;
                return compareVals(dat, x.Items[0], x.Items[1]) < 0;
            }
            else if (cond is condLIKEType)
            {
                condLIKEType x = (condLIKEType)cond;
                return compareLIKE(dat, x.Items[0], x.Items[1]);
            }
            throw new Exception(string.Format("Ошибка - неизвестный тип {0} в функции doCond", cond.GetType()));
        }
        private static bool compareLIKE(object dat, evalBase val1, evalBase val2)
        {
            string a = "";
            string b = "";
            try
            {
                a = doEval(dat, val1).Value.ToString();
                b = doEval(dat, val2).Value.ToString();
                Regex rx = new Regex(b);

                return rx.IsMatch(a);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ошибка - при сравнении {0} и {1} в функции compareLIKE", a, b), ex);
            }
        }
        private static int compareVals(object dat, evalBase val1, evalBase val2)
        {
            object a = doEval(dat, val1).Value;
            object b = doEval(dat, val2).Value;

            if (a is DateTime && b is DateTime)
                return DateTime.Compare((DateTime)a, (DateTime)b);
            else if (
                   (a is int || a is decimal || a is double || a is Single || a is float)
                && (b is int || b is decimal || b is double || b is Single || b is float))
                return Decimal.Compare(Convert.ToDecimal(a), Convert.ToDecimal(b));
            else if (a is string && b is string)
                return string.Compare((string)a, (string)b, true);
            throw new Exception(string.Format("Ошибка - при сравнении {0} и {1} в функции comparevals", a, b));
        }

        private static Result doCalc(object dat, evalCALCType eval)
        {
            Result result = new Result(null);
            evalBase a = eval.Items[0];
            evalBase b = eval.Items[1];
            Result r1 = doEval(dat, a);
            if (r1.HasError)
                return r1;
            Result r2 = doEval(dat, b);
            if (r2.HasError)
                return r2;
            switch (eval.oper)
            {
                case arOperEnum.add:
                    if (r1.TypeValue == TypeEnum.String || r2.TypeValue == TypeEnum.String)
                        result.Value = Convert.ToString(r1.Value) + Convert.ToString(r2.Value);
                    else if (r1.TypeValue == TypeEnum.Number && r2.TypeValue == TypeEnum.Number)
                        result.Value = Convert.ToDecimal(r1.Value) + Convert.ToDecimal(r2.Value);
                    else if (r1.TypeValue == TypeEnum.Date && r2.TypeValue == TypeEnum.Number)
                    {
                        DateTime dt = Convert.ToDateTime(r1.Value);
                        double days = Convert.ToDouble(r2.Value);
                        result.Value = dt.AddDays(days);
                    }
                    else
                    {
                        result.Value = null;
                        result.Error = "#ADD:Bad Types";
                    }
                    break;
                case arOperEnum.sub:
                    if (r1.TypeValue == TypeEnum.Number && r2.TypeValue == TypeEnum.Number)
                        result.Value = Convert.ToDecimal(r1.Value) - Convert.ToDecimal(r2.Value);
                    else if (r1.TypeValue == TypeEnum.Date && r2.TypeValue == TypeEnum.Number)
                    {
                        DateTime dt = Convert.ToDateTime(r1.Value);
                        double days = Convert.ToDouble(r2.Value);
                        result.Value = dt.AddDays(-days);
                    }
                    else
                    {
                        result.Value = null;
                        result.Error = "#SUB:Bad Types";
                    }
                    break;
                case arOperEnum.mul:
                    if (r1.TypeValue == TypeEnum.Number && r2.TypeValue == TypeEnum.Number)
                        result.Value = Convert.ToDecimal(r1.Value) * Convert.ToDecimal(r2.Value);
                    else
                    {
                        result.Value = null;
                        result.Error = "#MUL:Bad Types";
                    }
                    break;
                case arOperEnum.div:
                    if (r1.TypeValue == TypeEnum.Number && r2.TypeValue == TypeEnum.Number
                        && Convert.ToDecimal(r2.Value) != 0)
                        result.Value = Convert.ToDecimal(r1.Value) * Convert.ToDecimal(r2.Value);
                    else
                    {
                        result.Value = null;
                        result.Error = "#MUL:Bad Types";
                    }
                    break;
            }
            return result;
        }

        private static varInfo GetVar(Type objType, string property)
        {
            if (Common.IsNullOrEmpty(property) || vars == null || objType == null)
                return null;

            string typename = objType.FullName;
            foreach (varTypeInfo ti in vars.varType)
            {
                if (ti.TypeName == typename)
                {
                    foreach (varInfo varItem in ti.var)
                    {
                        if (varItem.propname.ToUpper() == property)
                            return varItem;
                    }
                }
            }
            return GetVar(objType.BaseType, property);
        }

        //static private Boolean isBaseType(string typename, Type type)
        //{
        //    if (type == null)
        //        return false;
        //    if (type.FullName == typename)
        //        return true;
        //    return isBaseType(typename, type.BaseType);
        //}

    }
}
namespace BO
{
    public class Result
    {
        private object _Val;

        public object Value
        {
            get { return _Val; }
            set
            {
                _Val = value;
                if (value == null)
                    TypeValue = TypeEnum.NaN;
                else
                {
                    if (value is int || value is decimal || value is double || value is Single || value is float)
                        TypeValue = TypeEnum.Number;
                    else if (value is string)
                        TypeValue = TypeEnum.String;
                    else if (value is DateTime)
                        TypeValue = TypeEnum.Date;
                    else if (value is bool)
                        TypeValue = TypeEnum.Bool;
                    else if (value is Array)
                        TypeValue = TypeEnum.Array;
                    else
                    {
                        TypeValue = TypeEnum.NaN;
                        Error = "#Bad Type";
                    }
                }
            }
        }
        public string Error;
        public TypeEnum TypeValue;
        public bool HasError
        {
            get { return !Common.IsNullOrEmpty(Error); }
        }

        private bool _IsNull = true;

        public bool IsNull
        {
            get { return _IsNull; }
            set { _IsNull = value; }
        }

        public Result(object value)
        {
            Error = "";
            Value = value;
            IsNull = value == null;
        }
    }
    public enum TypeEnum { String, Number, Date, Bool, Array, NaN }
}
