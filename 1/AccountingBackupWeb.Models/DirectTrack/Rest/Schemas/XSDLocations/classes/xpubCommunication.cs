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
namespace AccountingBackupWeb.Models.DirectTrack.Schemas.Classes.XpubCommunication {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class xpubClassifiedResourceURL {
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class respondingCompanyResourceURL {
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class campaignResourceURL {
        
        private string locationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public enum approval {
        
        /// <remarks/>
        Pending,
        
        /// <remarks/>
        Sent,
        
        /// <remarks/>
        Accepted,
        
        /// <remarks/>
        Rejected,
        
        /// <remarks/>
        Counter,
        
        /// <remarks/>
        Closed,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute("allowChildEditCreatives", Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class comment {
        
        private string dateField;
        
        private string respondingCompanyResourceURLField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string date {
            get {
                return this.dateField;
            }
            set {
                this.dateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string respondingCompanyResourceURL {
            get {
                return this.respondingCompanyResourceURLField;
            }
            set {
                this.respondingCompanyResourceURLField = value;
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class commentHistory {
        
        private comment[] commentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("comment")]
        public comment[] comment {
            get {
                return this.commentField;
            }
            set {
                this.commentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.digitalriver.com/directtrack/api/xpubCommunication/v1_0", IsNullable=false)]
    public partial class xpubCommunication {
        
        private xpubClassifiedResourceURL xpubClassifiedResourceURLField;
        
        private respondingCompanyResourceURL respondingCompanyResourceURLField;
        
        private campaignResourceURL campaignResourceURLField;
        
        private string responseCommentField;
        
        private decimal impressionPayoutField;
        
        private bool impressionPayoutFieldSpecified;
        
        private decimal clickPayoutField;
        
        private bool clickPayoutFieldSpecified;
        
        private decimal leadPayoutField;
        
        private bool leadPayoutFieldSpecified;
        
        private decimal flatSalePayoutField;
        
        private bool flatSalePayoutFieldSpecified;
        
        private decimal flatSubSalePayoutField;
        
        private bool flatSubSalePayoutFieldSpecified;
        
        private decimal percentSalePayoutField;
        
        private bool percentSalePayoutFieldSpecified;
        
        private decimal percentSubSalePayoutField;
        
        private bool percentSubSalePayoutFieldSpecified;
        
        private string digitalSignatureField;
        
        private approval approvalField;
        
        private bool approvalFieldSpecified;
        
        private booleanInt allowChildEditCreativesField;
        
        private bool allowChildEditCreativesFieldSpecified;
        
        private comment[] commentHistoryField;
        
        private string locationField;
        
        /// <remarks/>
        public xpubClassifiedResourceURL xpubClassifiedResourceURL {
            get {
                return this.xpubClassifiedResourceURLField;
            }
            set {
                this.xpubClassifiedResourceURLField = value;
            }
        }
        
        /// <remarks/>
        public respondingCompanyResourceURL respondingCompanyResourceURL {
            get {
                return this.respondingCompanyResourceURLField;
            }
            set {
                this.respondingCompanyResourceURLField = value;
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
        public string responseComment {
            get {
                return this.responseCommentField;
            }
            set {
                this.responseCommentField = value;
            }
        }
        
        /// <remarks/>
        public decimal impressionPayout {
            get {
                return this.impressionPayoutField;
            }
            set {
                this.impressionPayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool impressionPayoutSpecified {
            get {
                return this.impressionPayoutFieldSpecified;
            }
            set {
                this.impressionPayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal clickPayout {
            get {
                return this.clickPayoutField;
            }
            set {
                this.clickPayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool clickPayoutSpecified {
            get {
                return this.clickPayoutFieldSpecified;
            }
            set {
                this.clickPayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal leadPayout {
            get {
                return this.leadPayoutField;
            }
            set {
                this.leadPayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leadPayoutSpecified {
            get {
                return this.leadPayoutFieldSpecified;
            }
            set {
                this.leadPayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal flatSalePayout {
            get {
                return this.flatSalePayoutField;
            }
            set {
                this.flatSalePayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool flatSalePayoutSpecified {
            get {
                return this.flatSalePayoutFieldSpecified;
            }
            set {
                this.flatSalePayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal flatSubSalePayout {
            get {
                return this.flatSubSalePayoutField;
            }
            set {
                this.flatSubSalePayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool flatSubSalePayoutSpecified {
            get {
                return this.flatSubSalePayoutFieldSpecified;
            }
            set {
                this.flatSubSalePayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal percentSalePayout {
            get {
                return this.percentSalePayoutField;
            }
            set {
                this.percentSalePayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentSalePayoutSpecified {
            get {
                return this.percentSalePayoutFieldSpecified;
            }
            set {
                this.percentSalePayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal percentSubSalePayout {
            get {
                return this.percentSubSalePayoutField;
            }
            set {
                this.percentSubSalePayoutField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentSubSalePayoutSpecified {
            get {
                return this.percentSubSalePayoutFieldSpecified;
            }
            set {
                this.percentSubSalePayoutFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string digitalSignature {
            get {
                return this.digitalSignatureField;
            }
            set {
                this.digitalSignatureField = value;
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
        public booleanInt allowChildEditCreatives {
            get {
                return this.allowChildEditCreativesField;
            }
            set {
                this.allowChildEditCreativesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool allowChildEditCreativesSpecified {
            get {
                return this.allowChildEditCreativesFieldSpecified;
            }
            set {
                this.allowChildEditCreativesFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("comment", IsNullable=false)]
        public comment[] commentHistory {
            get {
                return this.commentHistoryField;
            }
            set {
                this.commentHistoryField = value;
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
