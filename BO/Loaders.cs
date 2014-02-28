using System;
using System.Collections.Generic;
using System.Text;
using DA;
using System.Collections;

namespace BO
{
    public static class Loaders
    {
        public static IDAOFilter CreateIDatFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 1 && args[0] is int)
            {
                filter.AddWhere(new FilterID((int)args[0]));
                return filter;
            }
            throw new ArgumentException("Ошибка формирования IDatFilter по заданным аргументам", args2string(args));
        }

        public static IDAOFilter CreateIDatGuidFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 2 && args[0] is string && (args[1] is Guid || args[1] is string))
            {
                if (args[1] is string)
                    filter.AddWhere(new FilterGUID((string)args[0], new Guid(args[1].ToString())));
                else if (args[1] is Guid)
                    filter.AddWhere(new FilterGUID((string)args[0], (Guid)args[1]));
                return filter;
            }
            throw new ArgumentException("Ошибка формирования IDatGuidFilter по заданным аргументам", args2string(args));
        }

        public static IDAOFilter CreateIDatNoIdFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 2 && args[0] is string && args[1] is int)
            {
                filter.AddWhere(new FilterID((string)args[0], (int)args[1]));
                return filter;
            }
            throw new ArgumentException("Ошибка формирования IDatGuidFilter по заданным аргументам", args2string(args));
        }

        public static IDAOFilter CreateICardDatFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 1)
            {
                if (args[0] is int)
                {
                    return CreateIDatFilter<T>(accessor, args);
                }
                else if (args[0] is PathCard)
                {
                    filter.AddWhere(new FilterAND(new FilterString(BaseDat<T>.GetFieldName("Parent_FP"), ((PathCard)args[0]).Parent.ToString()), new FilterID(BaseDat<T>.GetFieldName("Code"), ((PathCard)args[0]).Code)));
                    return filter;
                }
                else if (args[0] is string)
                {
                    string argVal = args[0].ToString();
                    if (PathCard.IsPathCard(argVal))
                    {
                        filter.AddWhere(new FilterAND(new FilterString(BaseDat<T>.GetFieldName("Parent_FP"), new PathCard(argVal).Parent.ToString()), new FilterID(BaseDat<T>.GetFieldName("Code"), new PathCard(argVal).Code)));
                        return filter;
                    }
                }
            }
            else if (args.Count == 2)
            {
                //if (args[0] is PathTree && args[1] is int)
                if (PathTree.IsPathTree(args[0].ToString()) && args[1] is int)
                {
                    filter.AddWhere(new FilterAND(new FilterString(BaseDat<T>.GetFieldName("Parent_FP"), ((PathTree)args[0]).ToString()), new FilterID(BaseDat<T>.GetFieldName("Code"), (int)args[0])));
                    return filter;
                }
                else if (args[0] is string && args[1] is int)
                {
                    filter.AddWhere(new FilterAND(new FilterString(BaseDat<T>.GetFieldName("Parent_FP"), (string)args[0]), new FilterID(BaseDat<T>.GetFieldName("Code"), (int)args[0])));
                    return filter;
                }
            }
            throw new ArgumentException("Ошибка формирования ICardDatFilter по заданным аргументам", args2string(args));
        }
        public static IDAOFilter CreateITreeDatFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 1)
            {
                if (args[0] is int)
                {
                    return CreateIDatFilter<T>(accessor, args);
                }
                else if (args[0] is PathTree)
                {
                    filter.AddWhere(new FilterString(BaseDat<T>.GetFieldName("FP"), ((PathTree)args[0]).ToString()));
                    return filter;
                }
                else if (args[0] is string)
                {
                    filter.AddWhere(new FilterString(BaseDat<T>.GetFieldName("FP"), (string)args[0]));
                    return filter;
                }
            }
            throw new ArgumentException("Ошибка формирования ITreeDatFilter по заданным аргументам", args2string(args));
        }
        public static IDAOFilter CreateITreeNDatFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 1)
            {
                if (args[0] is int)
                {
                    return CreateIDatFilter<T>(accessor, args);
                }
                else if (args[0] is PathTreeN)
                {
                    filter.AddWhere(new FilterString(BaseDat<T>.GetFieldName("FPn"), ((PathTreeN)args[0]).ToString()));
                    return filter;
                }
                else if (args[0] is string)
                {
                    filter.AddWhere(new FilterString(BaseDat<T>.GetFieldName("FPn"), (string)args[0]));
                    return filter;
                }
            }
            throw new ArgumentException("Ошибка формирования ITreeDatFilter по заданным аргументам", args2string(args));
        }
        public static IDAOFilter CreateIDictDatFilter<T>(IDataAccess accessor, ArrayList args) where T : BaseDat<T>//, new()
        {
            IDAOFilter filter = accessor.NewFilter();
            if (args.Count == 1)
            {
                if (args[0] is string)
                {
                    filter.AddWhere(new FilterString(BaseDat<T>.GetFieldName("SCode"), (string)args[0]));
                    return filter;
                }
                else if (args[0] is int)
                {
                    return CreateIDatFilter<T>(accessor, args);
                }
            }
            throw new ArgumentException("Ошибка формирования IDictDatFilter по заданным аргументам", args2string(args));
        }
        private static string args2string(ArrayList args)
        {
            List<object> list = new List<object>(args.ToArray());
            string[] sList = list.ConvertAll<string>(delegate(object o) { return o.ToString(); }).ToArray();
            return string.Join(";", sList);
        }
    }

}
