﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.832
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 
namespace BO {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://BO/SysAudit.xsd")]
    [System.Xml.Serialization.XmlRootAttribute("Audit", Namespace="http://BO/SysAudit.xsd", IsNullable=false)]
    public partial class SysAuditInfo {
        
        private SysAuditFieldInfo[] fieldField;
        
        private SysAuditOperTypeEnum operTypeField;
        
        private string tableField;
        
        private int idField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("field", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysAuditFieldInfo[] field {
            get {
                return this.fieldField;
            }
            set {
                this.fieldField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SysAuditOperTypeEnum OperType {
            get {
                return this.operTypeField;
            }
            set {
                this.operTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string table {
            get {
                return this.tableField;
            }
            set {
                this.tableField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://BO/SysAudit.xsd")]
    public partial class SysAuditFieldInfo {
        
        private object nField;
        
        private object oField;
        
        private string columnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object n {
            get {
                return this.nField;
            }
            set {
                this.nField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object o {
            get {
                return this.oField;
            }
            set {
                this.oField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string column {
            get {
                return this.columnField;
            }
            set {
                this.columnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://BO/SysAudit.xsd")]
    public enum SysAuditOperTypeEnum {
        
        /// <remarks/>
        UPDATE,
        
        /// <remarks/>
        INSERT,
        
        /// <remarks/>
        DELETE,
    }
}
