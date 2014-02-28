using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using System.Windows.Forms;
using System.Drawing;
using BO;

namespace BOF
{
    /// <summary>
    ///  онтроллер дл€ встройки в ChooserEditor выбора из списка.
    /// ≈сли в Editor.AutoCompleteMode != AutoCompleteMode.None, 
    /// то используетс€ встроенный в Windows механизм выбора по первым буквам.
    /// ¬ противном случае выбор производитс€ клавишами Up / Down
    /// </summary>
    /// <typeparam name="CS">“ип Set-класса</typeparam>
    /// <typeparam name="CD">“ип Dat-класса</typeparam>
    public class ChooserController<CS, CD> : IChooserController
        where CS : BaseSet<CD, CS>, new()
        where CD : BaseDat<CD>, new()
    {
        protected CS set = null;

        protected ChooserController() { }
        /// <summary>
        /// —оздаетс€ новый контроллер и прив€зываетс€ к заданному ChooserEditor.
        /// Set-класс загружаетс€ полностью.
        /// </summary>
        /// <param name="editor"></param>
        public ChooserController(ChooserEditor editor)
            : this(editor, null) { }
        public ChooserController(ChooserEditor editor, CS setclass)
            : this(editor, setclass, true)
        {
        }
        /// <summary>
        /// —оздаетс€ новый контроллер и прив€зываетс€ к заданному ChooserEditor.
        /// Set-класс используетс€ без загрузки! Ќадо загрузить заранее!
        /// </summary>
        /// <param name="editor"></param>
        /// <param name="setclass"></param>
        public ChooserController(ChooserEditor editor, CS setclass, bool reload)
        {
            editor.ReadOnly = false;
            editor.ValueChanged += new EventHandler(editor_ValueChanged);
            editor.TextChanged += new EventHandler(Editor_TextChanged);

            if (setclass == null)
            {
                set = new CS();
                if (reload)
                    set.Load();
            }
            else
                set = setclass;

            if (set.Count > 0)
            {
                CD d = set[0] as CD;
                if (d != null && d is IDat)
                {

                    Dictionary<int, string> dict = new Dictionary<int, string>(set.Count);
                    foreach (CD dat in set)
                    {
                        dict.Add(((IDat)dat).ID, BaseDat.ToString(dat, editor.Format));
                    }

                    set.Sort(delegate(CD dat1, CD dat2)
                        {
                            return string.Compare(dict[((IDat)dat1).ID], dict[((IDat)dat2).ID]);
                        });
                }
                else
                {
                    set.Sort(delegate(CD dat1, CD dat2)
                        {
                            return string.Compare(BaseDat.ToString(dat1, editor.Format), BaseDat.ToString(dat2, editor.Format));
                        });
                }
            }
            if (editor.AutoCompleteMode != AutoCompleteMode.None)
            {
                editor.AutoCompleteSource = AutoCompleteSource.CustomSource;
                editor.AutoCompleteCustomSource = new AutoCompleteStringCollection();

                List<string> ls = set.ConvertType<string>(delegate(CD dat)
                    {
                        return BaseDat.ToString(dat, editor.Format);
                    });
                editor.AutoCompleteCustomSource.AddRange(ls.ToArray());
            }
            else
            {
                editor.PrevValue += new EventHandler(Editor_PrevValue);
                editor.NextValue += new EventHandler(Editor_NextValue);
            }
            editor.Value = null;
        }
        void editor_ValueChanged(object sender, EventArgs e)
        {
            OnValueChanged(sender, e);
        }
        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            ChooserEditor editor = sender as ChooserEditor;
            if (editor != null && editor.Value != null)
                if (!(editor.Value is CD))
                {
                    string ex = string.Format("ѕопытка записи в поле {0} с типом {1} значени€ '{2}' c типом {3}"
                                , editor.Name
                                , typeof(CD)
                                , editor.Value
                                , editor.Value.GetType()
                                );

                    throw new Exception(ex);
                }
        }
        void Editor_NextValue(object sender, EventArgs e)
        {
            ChooserEditor editor = sender as ChooserEditor;
            if (editor != null && set != null)
            {
                int index = set.IndexOf(editor.Value) + 1;
                if (index < set.Count)
                    editor.Value = set[index] as CD;
            }
        }

        void Editor_PrevValue(object sender, EventArgs e)
        {
            ChooserEditor editor = sender as ChooserEditor;
            if (editor != null && set != null)
            {
                int index = set.IndexOf(editor.Value) - 1;
                if (index >= 0 && index < set.Count)
                    editor.Value = set[index] as CD;
            }
        }

        void Editor_TextChanged(object sender, EventArgs e)
        {
            ChooserEditor editor = sender as ChooserEditor;
            if (editor != null)
            {
                if (BaseDat.ToString(editor.Value, editor.Format) == editor.Text) return;

                if (editor.AutoCompleteMode != AutoCompleteMode.None)
                {
                    int index = editor.AutoCompleteCustomSource.IndexOf(editor.Text);
                    if (index >= 0)
                    {
                        CD dat = set[index] as CD;
                        editor.Value = dat;
                    }
                }
                else
                {
                    string text = editor.Text.ToLower();

                    for (int i = 0; i < set.Count; i++)
                    {
                        CD dat = set[i] as CD;
                        string sdat = dat.ToString(editor.Format, null);
                        if (dat != null && sdat.ToLower().StartsWith(text))
                        {
                            editor.Value = dat;
                            editor.SelectionStart = text.Length;
                            editor.SelectionLength = sdat.Length - text.Length;
                            return;
                        }
                    }
                }
            }
        }
    }
}
