﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.1.
// 
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.SaleLineItem {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0", IsNullable=false)]
    public partial class saleStatus {
        
        private booleanInt deletedField;
        
        private bool deletedFieldSpecified;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public booleanInt deleted {
            get {
                return this.deletedField;
            }
            set {
                this.deletedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deletedSpecified {
            get {
                return this.deletedFieldSpecified;
            }
            set {
                this.deletedFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("owned", Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0", IsNullable=false)]
    public enum booleanInt {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0", IsNullable=false)]
    public partial class affiliateResourceURL {
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
        public string location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0", IsNullable=false)]
    public partial class lineItem {
        
        private decimal saleAmountField;
        
        private saleStatus saleStatusField;
        
        private string statusDateField;
        
        private affiliateResourceURL affiliateResourceURLField;
        
        private string productIDField;
        
        private booleanInt ownedField;
        
        private bool ownedFieldSpecified;
        
        private string theyGetField;
        
        private string weGetField;
        
        private uint idField;
        
        private bool idFieldSpecified;
        
        /// <remarks/>
        public decimal saleAmount {
            get {
                return this.saleAmountField;
            }
            set {
                this.saleAmountField = value;
            }
        }
        
        /// <remarks/>
        public saleStatus saleStatus {
            get {
                return this.saleStatusField;
            }
            set {
                this.saleStatusField = value;
            }
        }
        
        /// <remarks/>
        public string statusDate {
            get {
                return this.statusDateField;
            }
            set {
                this.statusDateField = value;
            }
        }
        
        /// <remarks/>
        public affiliateResourceURL affiliateResourceURL {
            get {
                return this.affiliateResourceURLField;
            }
            set {
                this.affiliateResourceURLField = value;
            }
        }
        
        /// <remarks/>
        public string productID {
            get {
                return this.productIDField;
            }
            set {
                this.productIDField = value;
            }
        }
        
        /// <remarks/>
        public booleanInt owned {
            get {
                return this.ownedField;
            }
            set {
                this.ownedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ownedSpecified {
            get {
                return this.ownedFieldSpecified;
            }
            set {
                this.ownedFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string theyGet {
            get {
                return this.theyGetField;
            }
            set {
                this.theyGetField = value;
            }
        }
        
        /// <remarks/>
        public string weGet {
            get {
                return this.weGetField;
            }
            set {
                this.weGetField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified {
            get {
                return this.idFieldSpecified;
            }
            set {
                this.idFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/saleLineItem/v1_0", IsNullable=false)]
    public partial class saleLineItem {
        
        private lineItem[] lineItemField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("lineItem")]
        public lineItem[] lineItem {
            get {
                return this.lineItemField;
            }
            set {
                this.lineItemField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType="anyURI")]
        public string location {
            get {
                return this.locationField;
            }
            set {
                this.locationField = value;
            }
        }
    }
}
