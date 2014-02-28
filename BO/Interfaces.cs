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
    /// ��������� ��� �������, � ������� ���� �������� ID.
    /// ������������ � �������, ����������� �� SQL-��������, � ������� ���� primary key - id.
    /// </summary>
    public interface IDat
    {
        int ID { get; set; }
    }

    /// <summary>
    /// ��������� ��� ������� - ������������,
    /// ��� ������� ������ �������� ��������� ���.
    /// </summary>
    public interface IDictDat : IDat
    {
        /// <summary>
        /// ��������� ��� - ���������� ���� � �������.
        /// </summary>
        string SCode { get; set; }

        /// <summary>
        /// ��������.
        /// </summary>
        string Name { get; set; }
    }

    public interface IHasName : IDat
    {
        /// <summary>
        /// ��������.
        /// </summary>
        string Name { get; set; }
    }
    public interface IHasVersion : IDat
    {
        /// <summary>
        /// ������.
        /// </summary>
        DateTime ValueDate { get; set; }
    }
    public interface IHasIsDeleted : IDat
    {
        /// <summary>
        /// �������?
        /// </summary>
        bool IsDeleted { get; set; }
    }



    /// <summary>
    /// ��������� ��� ������� - ��������. 
    /// ������ ������������ ��� ���� � ������ <c>ITreeDat</c>
    /// ������ ��� �������� �������� <c>ID</c> ��� ���������� <c>Parent_FP</c> � <c>Code</c>.
    /// </summary>
    public interface ICardDat : IDat
    {
        /// <summary>
        /// ���� � ���� ������.
        /// ����� ���� null, ����� �������� ��������� ������������� � ��������� ����.
        /// </summary>
        PathTree Parent_FP { get; set; }

        /// <summary>
        /// �������� ���, ���������� � �������� ������ ���������
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// �������� ��������
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// ����������� ����. �� ������������� ���� � �������.
        /// �������� ������������� <c>Parent_FP</c> � <c>Code</c>.
        /// ��� ���������� ������ �������� �������� <c>Parent_FP</c> � <c>Code</c>.
        /// </summary>
        PathCard FP { get; set; }
    }
    /// <summary>
    /// ��������� ��� ������� - ��������. 
    /// ������ ������������ ��� ���� � ������ <c>ITreeDat</c>
    /// ������ ��� �������� �������� <c>ID</c> ��� ���������� <c>Parent_FP</c> � <c>Code</c>.
    /// </summary>
    
    public interface ICardNDat : IDat
    {
        /// <summary>
        /// ���� � ���� ������.
        /// ����� ���� null, ����� �������� ��������� ������������� � ��������� ����.
        /// </summary>
        PathTreeN Parent_FPn { get; set; }

        /// <summary>
        /// ���, ���������� � �������� ������ ���������
        /// </summary>
        string CodeN { get; set; }

        /// <summary>
        /// �������� ��������
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// ����������� ����. �� ������������� ���� � �������.
        /// �������� ������������� <c>Parent_FPn</c> � <c>CodeN</c>.
        /// ��� ���������� ������ �������� �������� <c>Parent_FPn</c> � <c>CodeN</c>.
        /// </summary>
        PathCardN FPn { get; set; }
    }
    /// <summary>
    /// ��������� ��� ������� - ����� ��������.
    /// ������ ��� ���� ������ �������� <c>ID</c> ��� <c>FP</c>
    /// </summary>
    public interface ITreeDat : IDat
    {
        /// <summary>
        /// ������ ���� � ���� ������.
        /// </summary>
        PathTree FP { get; set; }

        /// <summary>
        /// ���� � �������� ����. Null, ���� ��� �������� ����.
        /// ��� ��������� ���������� � <c>FP</c>
        /// </summary>
        PathTree Parent_FP { get; set; }

        /// <summary>
        /// ��� ����
        /// ����� ��������� � .NET 4.0 � WinXP �������
        /// </summary>
        int Code { get; set; }

        /// <summary>
        /// �������� ����
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// ��������� ��� ������� - ����� ��������.
    /// ������ ��� ���� ������ �������� <c>ID</c> ��� <c>FP</c>
    /// </summary>
    public interface ITreeNDat : IDat
    {
        /// <summary>
        /// ������ ���� � ���� ������.
        /// </summary>
        PathTreeN FPn { get; set; }

        /// <summary>
        /// ���� � �������� ����. Null, ���� ��� �������� ����.
        /// ��� ��������� ���������� � <c>FP</c>
        /// </summary>
        PathTreeN Parent_FPn { get; set; }

        /// <summary>
        /// ��� ����
        /// ����� ��������� � .NET 4.0 � WinXP �������
        /// </summary>
        string CodeS { get; set; }

        /// <summary>
        /// �������� ����
        /// </summary>
        string Name { get; set; }
    }
    /// <summary>
    /// ��������� ��� ������� - ����������� ������������,
    /// ��� ������� ������ �������� ��������� ��� + ����.
    /// </summary>
    public interface ITreeDictDat : IDictDat, IXmlContent
    {
        /// <summary>
        /// ���� � ���� ������.
        /// ����� ���� null, ����� �������� ��������� ������������� � ��������� ����.
        /// </summary>
        PathTree Parent_FP { get; set; }
        string FP { get; }
        bool IsTree { get; set; }
    }

    /// <summary>
    /// ��������� ��� ������� ITree, � ������� � ���������� � CtlTree ����� ���������� ����������� ToString()
    /// </summary>
    public interface ITreeCustomDisplayName
    {
        string DisplayName { get;}
    }

    /// <summary>
    /// ��������� ��� Set-�������.
    /// ����� ��� ���������� ��������� �������,
    /// </summary>
    public interface ISet
    {
        /// <summary>
        /// ������� ��� �������������� Set-������ � ��������� �������� � ������ �����.
        /// </summary>
        /// <typeparam name="I">���, � �������� ����� ���� ������������ Dat-�����</typeparam>
        /// <returns>���������� generic-��������� � ���������� ���� I</returns>
        List<I> ConvertType<I>() where I : class;
        void Load();
    }

    /// <summary>
    /// ������� ��������� ��� Details-�������.
    /// ��������, ������ ���������� ������� ��� ������������ �����.
    /// � �������� ������������ � generic-��������, 
    /// ������ �� ��������� ����� ������ ��� ����� ���� <c>det is IDetailDat</c>.
    /// </summary>
    public interface IDetailDat : IDat
    {
        IDat Header { get; set; }
    }
    /// <summary>
    /// ���������, ����������� ������� Details-����� � Header-�������.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDetailDat<T> : IDat where T : BaseDat<T>, new()
    {
        /// <summary>
        /// ������ �� ������ - Header
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
    /// ����� ��� ���������� ����������� �������.
    /// ����������� ��� CRUD - �������� � Set-������
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

    /// ��������� ��� �������, � ������� ���� �������� XSLT ������.
    /// </summary>
    public interface IXSLTemplate
    {
        string GetTemplateName();
        string GetTemplatePath();
        XmlDocument Serialize();
    }

    /// ��������� ��� �������, � ������� ���� �������� HTML ������.
    /// </summary>
    public interface IHTMLTemplate
    {
        string GetHTMLName();
        string GetHTMLString();
    }


    /// <summary>
    /// ��������� ��� ��������� �������� �������� � ���������� ������.
    /// ����� ���� �����������, ��������, ��� ��������� �������� �������� 
    /// A.B.C, ��� �������� ������������� ������ � ������ GetValue. 
    /// ������������, � ��������, ��� ������� �� info-�������.
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
    /// ��������� ��� �������, ������� XmlContent.
    /// </summary>
    public interface IXmlContent
    {
        string XmlContent { get; set; }
    }

    /// <summary>
    /// ��������� ��� �������, ������� Info �, ��������������, XmlContent.
    /// </summary>
    public interface IInfo<T>
    {
        T Info { get; set; }
    }
}

