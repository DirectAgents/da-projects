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
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.AffiliateManualEmail {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0", IsNullable=false)]
    public partial class affiliateManualEmail {
        
        private affiliateResourceURL[] sendToField;
        
        private string sendDateField;
        
        private string fromAddressField;
        
        private affiliateEmailTemplateResourceURL affiliateEmailTemplateResourceURLField;
        
        private string subjectField;
        
        private string plainTextMessageField;
        
        private string htmlMessageField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("affiliateResourceURL", IsNullable=false)]
        public affiliateResourceURL[] sendTo {
            get {
                return this.sendToField;
            }
            set {
                this.sendToField = value;
            }
        }
        
        /// <remarks/>
        public string sendDate {
            get {
                return this.sendDateField;
            }
            set {
                this.sendDateField = value;
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
        public affiliateEmailTemplateResourceURL affiliateEmailTemplateResourceURL {
            get {
                return this.affiliateEmailTemplateResourceURLField;
            }
            set {
                this.affiliateEmailTemplateResourceURLField = value;
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
        public string plainTextMessage {
            get {
                return this.plainTextMessageField;
            }
            set {
                this.plainTextMessageField = value;
            }
        }
        
        /// <remarks/>
        public string htmlMessage {
            get {
                return this.htmlMessageField;
            }
            set {
                this.htmlMessageField = value;
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0", IsNullable=false)]
    public partial class affiliateEmailTemplateResourceURL {
        
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateManualEmail/v1_0", IsNullable=false)]
    public partial class sendTo {
        
        private affiliateResourceURL[] affiliateResourceURLField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("affiliateResourceURL")]
        public affiliateResourceURL[] affiliateResourceURL {
            get {
                return this.affiliateResourceURLField;
            }
            set {
                this.affiliateResourceURLField = value;
            }
        }
    }
}
