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
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.Advertiser {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0", IsNullable=false)]
    public partial class advertiserGroup {
        
        private string locationField;
        
        private string nameField;
        
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0", IsNullable=false)]
    public enum status {
        
        /// <remarks/>
        active,
        
        /// <remarks/>
        inactive,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0", IsNullable=false)]
    public enum approval {
        
        /// <remarks/>
        approved,
        
        /// <remarks/>
        denied,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("agreedTerms", Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/advertiser/v1_0", IsNullable=false)]
    public partial class advertiser {
        
        private string companyField;
        
        private advertiserGroup advertiserGroupField;
        
        private string emailField;
        
        private status statusField;
        
        private bool statusFieldSpecified;
        
        private approval approvalField;
        
        private bool approvalFieldSpecified;
        
        private string passwordField;
        
        private string salutationField;
        
        private string firstNameField;
        
        private string middleNameField;
        
        private string lastNameField;
        
        private string websiteField;
        
        private string addressField;
        
        private string address2Field;
        
        private string cityField;
        
        private string stateField;
        
        private string zipField;
        
        private string countryField;
        
        private string phoneField;
        
        private string faxField;
        
        private string createdDateField;
        
        private booleanInt agreedTermsField;
        
        private bool agreedTermsFieldSpecified;
        
        private string notesField;
        
        private string currencyField;
        
        private booleanInt calledField;
        
        private bool calledFieldSpecified;
        
        private uint accessIDField;
        
        private bool accessIDFieldSpecified;
        
        private string externalIDField;
        
        private string locationField;
        
        /// <remarks/>
        public string company {
            get {
                return this.companyField;
            }
            set {
                this.companyField = value;
            }
        }
        
        /// <remarks/>
        public advertiserGroup advertiserGroup {
            get {
                return this.advertiserGroupField;
            }
            set {
                this.advertiserGroupField = value;
            }
        }
        
        /// <remarks/>
        public string email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
        
        /// <remarks/>
        public status status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public approval approval {
            get {
                return this.approvalField;
            }
            set {
                this.approvalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool approvalSpecified {
            get {
                return this.approvalFieldSpecified;
            }
            set {
                this.approvalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        public string salutation {
            get {
                return this.salutationField;
            }
            set {
                this.salutationField = value;
            }
        }
        
        /// <remarks/>
        public string firstName {
            get {
                return this.firstNameField;
            }
            set {
                this.firstNameField = value;
            }
        }
        
        /// <remarks/>
        public string middleName {
            get {
                return this.middleNameField;
            }
            set {
                this.middleNameField = value;
            }
        }
        
        /// <remarks/>
        public string lastName {
            get {
                return this.lastNameField;
            }
            set {
                this.lastNameField = value;
            }
        }
        
        /// <remarks/>
        public string website {
            get {
                return this.websiteField;
            }
            set {
                this.websiteField = value;
            }
        }
        
        /// <remarks/>
        public string address {
            get {
                return this.addressField;
            }
            set {
                this.addressField = value;
            }
        }
        
        /// <remarks/>
        public string address2 {
            get {
                return this.address2Field;
            }
            set {
                this.address2Field = value;
            }
        }
        
        /// <remarks/>
        public string city {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        public string state {
            get {
                return this.stateField;
            }
            set {
                this.stateField = value;
            }
        }
        
        /// <remarks/>
        public string zip {
            get {
                return this.zipField;
            }
            set {
                this.zipField = value;
            }
        }
        
        /// <remarks/>
        public string country {
            get {
                return this.countryField;
            }
            set {
                this.countryField = value;
            }
        }
        
        /// <remarks/>
        public string phone {
            get {
                return this.phoneField;
            }
            set {
                this.phoneField = value;
            }
        }
        
        /// <remarks/>
        public string fax {
            get {
                return this.faxField;
            }
            set {
                this.faxField = value;
            }
        }
        
        /// <remarks/>
        public string createdDate {
            get {
                return this.createdDateField;
            }
            set {
                this.createdDateField = value;
            }
        }
        
        /// <remarks/>
        public booleanInt agreedTerms {
            get {
                return this.agreedTermsField;
            }
            set {
                this.agreedTermsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool agreedTermsSpecified {
            get {
                return this.agreedTermsFieldSpecified;
            }
            set {
                this.agreedTermsFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string notes {
            get {
                return this.notesField;
            }
            set {
                this.notesField = value;
            }
        }
        
        /// <remarks/>
        public string currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <remarks/>
        public booleanInt called {
            get {
                return this.calledField;
            }
            set {
                this.calledField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool calledSpecified {
            get {
                return this.calledFieldSpecified;
            }
            set {
                this.calledFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public uint accessID {
            get {
                return this.accessIDField;
            }
            set {
                this.accessIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool accessIDSpecified {
            get {
                return this.accessIDFieldSpecified;
            }
            set {
                this.accessIDFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string externalID {
            get {
                return this.externalIDField;
            }
            set {
                this.externalIDField = value;
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
