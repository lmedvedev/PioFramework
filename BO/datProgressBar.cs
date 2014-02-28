using System;

namespace BO
{
    public class datProgressBar
    {
        public static event ProgressEventHandler ProgressEvent = null;
        public static datProgressBar Current = null;
        datProgressBar Prev = null;
        public int StartPos = 0;
        public int EndPos = 100;
        public int Count = 100;
        public int Pos = 0;
        public string Comment = "";

        public static void InitCount(int count)
        {
            if (Current == null) return;
            Current.Count = count;
            Current.Pos = 0;
        }
        public static string GetComment() { return Current.Comment; }

        #region Push/Pop
        public static void Push()
        {
            Push("");
        }
        public static void Push(string comment)
        {
            if (Current == null) return;
            Push(Current.Pos, Current.Pos + 1, Current.Count, comment);
        }
        public static void Push(int start, int end)
        {
            Push(start, end, "");
        }
        public static void Push(int start, int end, string comment)
        {
            datProgressBar tmp = new datProgressBar();
            tmp.Prev = Current;
            tmp.StartPos = start;
            if (Current != null) tmp.Comment = Current.Comment + "\n";
            tmp.Comment += comment;
            tmp.EndPos = (start > end) ? start : end;
            Current = tmp;
        }
        public static void Push(int start, int end, int count, string comment)
        {
            if (Current == null) return;

            Push(Current.StartPos + start * (Current.EndPos - Current.StartPos) / count
                , Current.StartPos + end * (Current.EndPos - Current.StartPos) / count
                , comment);
        }
        public static void Push(int start, int end, int count)
        {
            Push(start, end, count, "");
        }
        public static void Pop()
        {
            if (Current != null)
            {
                datProgressBar tmp = Current;
                Current = Current.Prev;
                tmp = null;
            }
        }
        #endregion

        #region Show
        public static void MoveNext()
        {
            if (Current == null) return;
            Current.Pos++;
        }

        public static void Show()
        {
            Show("");
        }
        public static void Show(string caption)
        {
            if (Current == null) return;
            string format = Current.Comment.Length > 0 ? "{0}\n{1}" : "{1}";
            Show(Current.Pos, Current.Count, string.Format(format, Current.Comment, caption));
        }
        public static void Show(int position, int count, string caption)
        {
            Show(position * 100 / count, caption);
        }
        public static void Show(int position, string caption)
        {
            if (ProgressEvent != null) ProgressEvent(new ProgressEventArgs(position, caption));
        }
        #endregion
    }
    public delegate void ProgressEventHandler(ProgressEventArgs e);
    public class ProgressEventArgs : EventArgs
    {
        public int Position = 0;
        public string Caption = "";
        public ProgressEventArgs(int position, string caption)
        {
            Position = position;
            Caption = caption;
        }
    }
}
