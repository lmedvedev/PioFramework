using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    /// <summary>
    /// ������� ��� ������� Dat-�������, ��������������� SQL - ��������.
    /// ���� �������� ����� ���� �������, �� �������� Ordinal ������������� ����������� 
    /// ������ ������� � �������, � ��������� Name - �������� �������.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class FieldInfoDat : Attribute
    {
        private int _Ordinal = -1;
        private string _Name = "";
        private bool _IsOffline = false;

        /// <summary>
        /// ���������� ����� ������� � ������� (0-based)
        /// </summary>
        public int Ordinal
        {
            get { return _Ordinal; }
        }

        /// <summary>
        /// �������� ������� � �������
        /// </summary>
        public string Name
        {
            get { return _Name; }
        }

        /// <summary>
        /// ��� �������� ���� � ����� ��� ������ �������� ������ ID �� ����, ��� ����� �������� ����� �������
        /// </summary>
        public bool IsOffline
        {
            get { return _IsOffline; }
        }
        
        /// <summary>
        /// ����������� ��������
        /// </summary>
        /// <param name="ordinal">���������� ����� ������� � ������� (0-based)</param>
        /// <param name="name">�������� ������� � �������</param>
        public FieldInfoDat(int ordinal, string name) : this (ordinal, name, false) { }
        public FieldInfoDat(int ordinal, string name, bool offlineload)
        {
            _Ordinal = ordinal;
            _Name = name;
            _IsOffline = offlineload;
        }
    }
    /// <summary>
    /// ������� ������������ � Set-������� ��� ������� ���� <c>BaseSet</c>.
    /// ��� ����� - �������� ����� ����� <c>BaseSet</c>-���������� � ��������� �������. 
    /// ��������, � ������ TransactionsSet
    /// ������� [FieldInfoOrdinals(11, 10)] ��� �������� public RegsSet Regs
    /// ��������, ��� 10-� � 11-� ������� � ������� Transactions ��������� �� ������� Regs.
    /// ��� �������� � ����, ��� ��� �������� ������� ���� TransactionsSet ������������� ����������
    /// ��������� Regs, �� �������� ������� ����� ��������� �������� �������� ���������.
    /// </summary>
    [global::System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class FieldInfoOrdinals : Attribute
    {
        private List<int> _Ordinals;

        /// <summary>
        /// ������ ���������� ������� �������, ������� ����� ������ �� ������ �������.
        /// </summary>
        public List<int> Ordinals
        {
            get { return _Ordinals; }
        }

        /// <summary>
        /// ����������� ��������
        /// </summary>
        /// <param name="values">������ ���������� ������� �������, ������� ����� ������ �� ������ �������.</param>
        public FieldInfoOrdinals(params int[] values)
        {
            _Ordinals = new List<int>(values);
        }
    }
}
