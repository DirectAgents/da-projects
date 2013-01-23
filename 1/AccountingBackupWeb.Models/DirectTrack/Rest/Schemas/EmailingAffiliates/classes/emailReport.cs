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
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.EmailReport {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0", IsNullable=false)]
    public partial class emailReport {
        
        private string toAddressField;
        
        private string fromAddressField;
        
        private string subjectField;
        
        private status statusField;
        
        private string errorsField;
        
        private string smtpServerField;
        
        private string smtpUsernameField;
        
        private string smtpPasswordField;
        
        private emailType emailTypeField;
        
        private string locationField;
        
        /// <remarks/>
        public string toAddress {
            get {
                return this.toAddressField;
            }
            set {
                this.toAddressField = value;
            }
        }
        
        /// <remarks/>
        public string fromAddress {
            get {
                return this.fromAddressField;
            }
            set {
                this.fromAddressField = value;
            }
        }
        
        /// <remarks/>
        public string subject {
            get {
                return this.subjectField;
            }
            set {
                this.subjectField = value;
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
        public string errors {
            get {
                return this.errorsField;
            }
            set {
                this.errorsField = value;
            }
        }
        
        /// <remarks/>
        public string smtpServer {
            get {
                return this.smtpServerField;
            }
            set {
                this.smtpServerField = value;
            }
        }
        
        /// <remarks/>
        public string smtpUsername {
            get {
                return this.smtpUsernameField;
            }
            set {
                this.smtpUsernameField = value;
            }
        }
        
        /// <remarks/>
        public string smtpPassword {
            get {
                return this.smtpPasswordField;
            }
            set {
                this.smtpPasswordField = value;
            }
        }
        
        /// <remarks/>
        public emailType emailType {
            get {
                return this.emailTypeField;
            }
            set {
                this.emailTypeField = value;
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
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0", IsNullable=false)]
    public enum status {
        
        /// <remarks/>
        QUEUED,
        
        /// <remarks/>
        PROCESSING,
        
        /// <remarks/>
        SENT,
        
        /// <remarks/>
        ERROR,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/emailReport/v1_0", IsNullable=false)]
    public enum emailType {
        
        /// <remarks/>
        NORMAL,
        
        /// <remarks/>
        AUTOMATIC,
    }
}
