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
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.AffiliateHistory {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0", IsNullable=false)]
    public partial class campaignResourceURL {
        
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0", IsNullable=false)]
    public partial class resource {
        
        private affiliateResourceURL affiliateResourceURLField;
        
        private campaignResourceURL campaignResourceURLField;
        
        private string usernameField;
        
        private string actionTakenField;
        
        private string timestampField;
        
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
        public campaignResourceURL campaignResourceURL {
            get {
                return this.campaignResourceURLField;
            }
            set {
                this.campaignResourceURLField = value;
            }
        }
        
        /// <remarks/>
        public string username {
            get {
                return this.usernameField;
            }
            set {
                this.usernameField = value;
            }
        }
        
        /// <remarks/>
        public string actionTaken {
            get {
                return this.actionTakenField;
            }
            set {
                this.actionTakenField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string timestamp {
            get {
                return this.timestampField;
            }
            set {
                this.timestampField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/affiliateHistory/v1_0", IsNullable=false)]
    public partial class affiliateHistory {
        
        private resource[] resourceField;
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("resource")]
        public resource[] resource {
            get {
                return this.resourceField;
            }
            set {
                this.resourceField = value;
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
