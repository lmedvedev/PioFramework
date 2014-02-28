using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    /// <summary>
    /// јтрибут дл€ свойств Dat-классов, соответствующих SQL - таблицам.
    /// ≈сли свойство имеет этот атрибут, то параметр Ordinal соответствует пор€дковому 
    /// номеру колонки в таблице, а пререметр Name - названию колонки.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class FieldInfoDat : Attribute
    {
        private int _Ordinal = -1;
        private string _Name = "";
        private bool _IsOffline = false;

        /// <summary>
        /// ѕор€дковый номер колонки в таблице (0-based)
        /// </summary>
        public int Ordinal
        {
            get { return _Ordinal; }
        }

        /// <summary>
        /// Ќазвание колонки в таблице
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// ѕри загрузке пол€ с типом дат класса грузитс€ только ID из базы, сам класс грузитс€ позже вручную
        /// </summary>
        public bool IsOffline
        {
            get { return _IsOffline; }
        }
        
        /// <summary>
        ///  онструктор атрибута
        /// </summary>
        /// <param name="ordinal">ѕор€дковый номер колонки в таблице (0-based)</param>
        /// <param name="name">Ќазвание колонки в таблице</param>
        public FieldInfoDat(int ordinal, string name) : this (ordinal, name, false) { }
        public FieldInfoDat(int ordinal, string name, bool offlineload)
        {
            _Ordinal = ordinal;
            _Name = name;
            _IsOffline = offlineload;
        }
    }
    /// <summary>
    /// јтрибут используетс€ в Set-классах дл€ свойств типа <c>BaseSet</c>.
    /// ≈го смысл - показать св€зь между <c>BaseSet</c>-свойствами и колонками таблицы. 
    /// Ќапример, в классе TransactionsSet
    /// атрибут [FieldInfoOrdinals(11, 10)] дл€ свойства public RegsSet Regs
    /// означает, что 10-€ и 11-€ колонки в таблице Transactions ссылаютс€ на таблицу Regs.
    /// Ёто приведет к тому, что при загрузке объекта типа TransactionsSet автоматически загрузитс€
    /// коллекци€ Regs, на элементы которой будут ссылатьс€ элементы основной коллекции.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class FieldInfoOrdinals : Attribute
    {
        private List<int> _Ordinals;

        /// <summary>
        /// —писок пор€дковых номеров колонок, которые имеют ссылки на другие таблицы.
        /// </summary>
        public List<int> Ordinals
        {
            get { return _Ordinals; }
        }

        /// <summary>
        ///  онструктор атрибута
        /// </summary>
        /// <param name="values">—писок пор€дковых номеров колонок, которые имеют ссылки на другие таблицы.</param>
        public FieldInfoOrdinals(params int[] values)
        {
            _Ordinals = new List<int>(values);
        }
    }
}
