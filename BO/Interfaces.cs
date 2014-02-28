using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace BO
{
    /// <summary>
    /// </summary>
    /// 
    public interface IHasBackColor
    {
        string BackColor { get; set; }
    }

    public interface IHasForeColor
    {
        string ForeColor { get; set; }
    }    
    public interface IDatGuid
    {
        //Guid ID { get; set; }
    }

    public interface IDatNoID
    {
        //Guid ID { get; set; }
    }

    /// <summary>
    /// Интерфейс для классов, у которых есть свойство ID.
    /// Используется в классах, сгенеренных по SQL-таблицам, в которых есть primary key - id.
    /// </summary>
    public interface IDat
    {
        int ID { get; set; }
    }

    /// <summary>
    /// Интерфейс для классов - справочников,
    /// для которых ключом является строковый код.
    /// </summary>
    public interface IDictDat : IDat
    {
        /// <summary>
        /// Строковый код - уникальное поле в таблице.
        /// </summary>
        string SCode { get; set; }

        /// <summary>
        /// Название.
        /// </summary>
        string Name { get; set; }
    }

    public interface IHasName : IDat
    {
        /// <summary>
        /// Название.
        /// </summary>
        string Name { get; set; }
    }
    public interface IHasVersion : IDat
    {
        /// <summary>
        /// Версия.
        /// </summary>
        DateTime ValueDate { get; set; }
    }
    public interface IHasIsDeleted : IDat
    {
        /// <summary>
        /// Удалено?
        /// </summary>
        bool IsDeleted { get; set; }
    }



    /// <summary>
    /// Интерфейс для классов - карточек. 
    /// Обычно используется как лист в дереве <c>ITreeDat</c>
    /// Ключом для карточки является <c>ID</c> или комбинация <c>Parent_FP</c> и <c>Code</c>.
    /// </summary>
    public interface ICardDat : IDat
    {
        /// <summary>
        /// Путь к узлу дерева.
        /// Может быть null, тогда карточка считается прикрепленной к корневому узлу.
        /// </summary>
        PathTree Parent_FP { get; set; }

        /// <summary>
        /// Числовой код, уникальный в пределах одного поддерева
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Название карточки
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Виртуальное поле. Не соответствует полю в таблице.
        /// Является конкатенацией <c>Parent_FP</c> и <c>Code</c>.
        /// При присвоении нового значения изменяет <c>Parent_FP</c> и <c>Code</c>.
        /// </summary>
        PathCard FP { get; set; }
    }
    /// <summary>
    /// Интерфейс для классов - карточек. 
    /// Обычно используется как лист в дереве <c>ITreeDat</c>
    /// Ключом для карточки является <c>ID</c> или комбинация <c>Parent_FP</c> и <c>Code</c>.
    /// </summary>
    
    public interface ICardNDat : IDat
    {
        /// <summary>
        /// Путь к узлу дерева.
        /// Может быть null, тогда карточка считается прикрепленной к корневому узлу.
        /// </summary>
        PathTreeN Parent_FPn { get; set; }

        /// <summary>
        /// Код, уникальный в пределах одного поддерева
        /// </summary>
        string CodeN { get; set; }

        /// <summary>
        /// Название карточки
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Виртуальное поле. Не соответствует полю в таблице.
        /// Является конкатенацией <c>Parent_FPn</c> и <c>CodeN</c>.
        /// При присвоении нового значения изменяет <c>Parent_FPn</c> и <c>CodeN</c>.
        /// </summary>
        PathCardN FPn { get; set; }
    }
    /// <summary>
    /// Интерфейс для классов - узлов деревьев.
    /// Ключом для узла дерева являются <c>ID</c> или <c>FP</c>
    /// </summary>
    public interface ITreeDat : IDat
    {
        /// <summary>
        /// Полный путь к узлу дерева.
        /// </summary>
        PathTree FP { get; set; }

        /// <summary>
        /// Путь к родителю узла. Null, если это корневой узел.
        /// При изменении изменяется и <c>FP</c>
        /// </summary>
        PathTree Parent_FP { get; set; }

        /// <summary>
        /// Код узла
        /// Чиним сломанный в .NET 4.0 и WinXP биндинг
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// Название узла
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// Интерфейс для классов - узлов деревьев.
    /// Ключом для узла дерева являются <c>ID</c> или <c>FP</c>
    /// </summary>
    public interface ITreeNDat : IDat
    {
        /// <summary>
        /// Полный путь к узлу дерева.
        /// </summary>
        PathTreeN FPn { get; set; }

        /// <summary>
        /// Путь к родителю узла. Null, если это корневой узел.
        /// При изменении изменяется и <c>FP</c>
        /// </summary>
        PathTreeN Parent_FPn { get; set; }

        /// <summary>
        /// Код узла
        /// Чиним сломанный в .NET 4.0 и WinXP биндинг
        /// </summary>
        string CodeS { get; set; }

        /// <summary>
        /// Название узла
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// Интерфейс для классов - древовидных справочников,
    /// для которых ключом является строковый код + узел.
    /// </summary>
    public interface ITreeDictDat : IDictDat, IXmlContent
    {
        /// <summary>
        /// Путь к узлу дерева.
        /// Может быть null, тогда карточка считается прикрепленной к корневому узлу.
        /// </summary>
        PathTree Parent_FP { get; set; }
        string FP { get; }
        bool IsTree { get; set; }
    }

    /// <summary>
    /// Интерфейс для классов ITree, у которых в дропдаунах и CtlTree нужно показывать собственный ToString()
    /// </summary>
    public interface ITreeCustomDisplayName
    {
        string DisplayName { get;}
    }

    /// <summary>
    /// Интерфейс для Set-классов.
    /// Нужен для добавления некоторых функций,
    /// </summary>
    public interface ISet
    {
        /// <summary>
        /// Функция для преобразования Set-класса в коллекцию объектов с другим типом.
        /// </summary>
        /// <typeparam name="I">Тип, к которому может быть преобразован Dat-класс</typeparam>
        /// <returns>Возвращает generic-коллекцию с элементами типа I</returns>
        List<I> ConvertType<I>() where I : class;
        void Load();
    }

    /// <summary>
    /// Базовый интерфейс для Details-классов.
    /// Например, строки банковской выписки или спецификации счета.
    /// В основном используется в generic-варианте, 
    /// данный же интерфейс нужен обычно для строк типа <c>det is IDetailDat</c>.
    /// </summary>
    public interface IDetailDat : IDat
    {
        IDat Header { get; set; }
    }
    /// <summary>
    /// Интерфейс, позволяющий связать Details-класс с Header-классом.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDetailDat<T> : IDat where T : BaseDat<T>, new()
    {
        /// <summary>
        /// Ссылка на объект - Header
        /// </summary>
        T Header { get; set; }
    }

    public interface IExpPropDat<T> : IDat where T : BaseDat<T>, new()
    {
        T Root
        {
            get;
            set;
        }
    }
    public interface IExpPropRoot<T> : IDat where T : BaseDat<T>, new()
    {
        Type GetExpPropDatType();
        Type GetExpPropSetType();
        IExpPropDat<T> ExpProp
        {
            get;
            set;
        }
    }
    public interface IHDWrapper
    {
        int Save(BaseDat dat);
    }

    /// <summary>
    /// Нужен для управления перегрузкой списков.
    /// Поднимается при CRUD - операции в Set-классе
    /// </summary>
    public interface ISetReload
    {
        event EventHandler SetReloaded;
        void FireSetReloaded();
    }

    public interface IRTFDoc
    {
        string getRTFDoc();
    }

    /// Интерфейс для классов, у которых есть свойство XSLT шаблон.
    /// </summary>
    public interface IXSLTemplate
    {
        string GetTemplateName();
        string GetTemplatePath();
        XmlDocument Serialize();
    }

    /// Интерфейс для классов, у которых есть свойство HTML шаблон.
    /// </summary>
    public interface IHTMLTemplate
    {
        string GetHTMLName();
        string GetHTMLString();
    }


    /// <summary>
    /// Интерфейс для получения значения свойства в экземпляре класса.
    /// Может быть использован, например, для получения значения свойства 
    /// A.B.C, или значения существующего только в методе GetValue. 
    /// Используется, в основном, для отчетов по info-классам.
    /// </summary>
    public interface IPropValue
    {
        object GetValue(string property);
    }
    public interface IValidate
    {
        DatErrorList ValidateErrors();
        DatErrorList ValidateWarnings();
    }

    /// <summary>
    /// Интерфейс для классов, имеющих XmlContent.
    /// </summary>
    public interface IXmlContent
    {
        string XmlContent { get; set; }
    }

    /// <summary>
    /// Интерфейс для классов, имеющих Info и, соответственно, XmlContent.
    /// </summary>
    public interface IInfo<T>
    {
        T Info { get; set; }
    }
}

