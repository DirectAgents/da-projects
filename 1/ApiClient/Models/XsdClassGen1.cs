﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiClient.Models {
    
}
namespace ApiClient.Models {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class affiliate {
        
        private int affiliate_idField;
        
        private string affiliate_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public int affiliate_id {
            get {
                return this.affiliate_idField;
            }
            set {
                this.affiliate_idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public string affiliate_name {
            get {
                return this.affiliate_nameField;
            }
            set {
                this.affiliate_nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class conversion {
        
        private int conversion_idField;
        
        private int visitor_idField;
        
        private int request_session_idField;
        
        private int click_idField;
        
        private System.DateTime conversion_dateField;
        
        private affiliate affiliateField;
        
        private advertiser advertiserField;
        
        private offer offerField;
        
        private ushort campaign_idField;
        
        private creative creativeField;
        
        private string sub_id_1Field;
        
        private string sub_id_2Field;
        
        private string sub_id_3Field;
        
        private string conversion_typeField;
        
        private paid paidField;
        
        private received receivedField;
        
        private bool pixel_droppedField;
        
        private bool suppressedField;
        
        private bool returnedField;
        
        private bool testField;
        
        private string transaction_idField;
        
        private string ip_addressField;
        
        private string referrer_urlField;
        
        private string noteField;
        
        /// <remarks/>
        public int conversion_id {
            get {
                return this.conversion_idField;
            }
            set {
                this.conversion_idField = value;
            }
        }
        
        /// <remarks/>
        public int visitor_id {
            get {
                return this.visitor_idField;
            }
            set {
                this.visitor_idField = value;
            }
        }
        
        /// <remarks/>
        public int request_session_id {
            get {
                return this.request_session_idField;
            }
            set {
                this.request_session_idField = value;
            }
        }
        
        /// <remarks/>
        public int click_id {
            get {
                return this.click_idField;
            }
            set {
                this.click_idField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime conversion_date {
            get {
                return this.conversion_dateField;
            }
            set {
                this.conversion_dateField = value;
            }
        }
        
        /// <remarks/>
        public affiliate affiliate {
            get {
                return this.affiliateField;
            }
            set {
                this.affiliateField = value;
            }
        }
        
        /// <remarks/>
        public advertiser advertiser {
            get {
                return this.advertiserField;
            }
            set {
                this.advertiserField = value;
            }
        }
        
        /// <remarks/>
        public offer offer {
            get {
                return this.offerField;
            }
            set {
                this.offerField = value;
            }
        }
        
        /// <remarks/>
        public ushort campaign_id {
            get {
                return this.campaign_idField;
            }
            set {
                this.campaign_idField = value;
            }
        }
        
        /// <remarks/>
        public creative creative {
            get {
                return this.creativeField;
            }
            set {
                this.creativeField = value;
            }
        }
        
        /// <remarks/>
        public string sub_id_1 {
            get {
                return this.sub_id_1Field;
            }
            set {
                this.sub_id_1Field = value;
            }
        }
        
        /// <remarks/>
        public string sub_id_2 {
            get {
                return this.sub_id_2Field;
            }
            set {
                this.sub_id_2Field = value;
            }
        }
        
        /// <remarks/>
        public string sub_id_3 {
            get {
                return this.sub_id_3Field;
            }
            set {
                this.sub_id_3Field = value;
            }
        }
        
        /// <remarks/>
        public string conversion_type {
            get {
                return this.conversion_typeField;
            }
            set {
                this.conversion_typeField = value;
            }
        }
        
        /// <remarks/>
        public paid paid {
            get {
                return this.paidField;
            }
            set {
                this.paidField = value;
            }
        }
        
        /// <remarks/>
        public received received {
            get {
                return this.receivedField;
            }
            set {
                this.receivedField = value;
            }
        }
        
        /// <remarks/>
        public bool pixel_dropped {
            get {
                return this.pixel_droppedField;
            }
            set {
                this.pixel_droppedField = value;
            }
        }
        
        /// <remarks/>
        public bool suppressed {
            get {
                return this.suppressedField;
            }
            set {
                this.suppressedField = value;
            }
        }
        
        /// <remarks/>
        public bool returned {
            get {
                return this.returnedField;
            }
            set {
                this.returnedField = value;
            }
        }
        
        /// <remarks/>
        public bool test {
            get {
                return this.testField;
            }
            set {
                this.testField = value;
            }
        }
        
        /// <remarks/>
        public string transaction_id {
            get {
                return this.transaction_idField;
            }
            set {
                this.transaction_idField = value;
            }
        }
        
        /// <remarks/>
        public string ip_address {
            get {
                return this.ip_addressField;
            }
            set {
                this.ip_addressField = value;
            }
        }
        
        /// <remarks/>
        public string referrer_url {
            get {
                return this.referrer_urlField;
            }
            set {
                this.referrer_urlField = value;
            }
        }
        
        /// <remarks/>
        public string note {
            get {
                return this.noteField;
            }
            set {
                this.noteField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class advertiser {
        
        private int advertiser_idField;
        
        private string advertiser_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public int advertiser_id {
            get {
                return this.advertiser_idField;
            }
            set {
                this.advertiser_idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public string advertiser_name {
            get {
                return this.advertiser_nameField;
            }
            set {
                this.advertiser_nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class offer {
        
        private int offer_idField;
        
        private string offer_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public int offer_id {
            get {
                return this.offer_idField;
            }
            set {
                this.offer_idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public string offer_name {
            get {
                return this.offer_nameField;
            }
            set {
                this.offer_nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class creative {
        
        private int creative_idField;
        
        private string creative_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public int creative_id {
            get {
                return this.creative_idField;
            }
            set {
                this.creative_idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace="API:id_name_store")]
        public string creative_name {
            get {
                return this.creative_nameField;
            }
            set {
                this.creative_nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class paid {
        
        private int currency_idField;
        
        private decimal amountField;
        
        private string formatted_amountField;
        
        /// <remarks/>
        public int currency_id {
            get {
                return this.currency_idField;
            }
            set {
                this.currency_idField = value;
            }
        }
        
        /// <remarks/>
        public decimal amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string formatted_amount {
            get {
                return this.formatted_amountField;
            }
            set {
                this.formatted_amountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class received {
        
        private int currency_idField;
        
        private decimal amountField;
        
        private string formatted_amountField;
        
        /// <remarks/>
        public int currency_id {
            get {
                return this.currency_idField;
            }
            set {
                this.currency_idField = value;
            }
        }
        
        /// <remarks/>
        public decimal amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        public string formatted_amount {
            get {
                return this.formatted_amountField;
            }
            set {
                this.formatted_amountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://cakemarketing.com/api/4/", IsNullable=false)]
    public partial class conversion_report_response {
        
        private bool successField;
        
        private int row_countField;
        
        private conversion[] conversionsField;
        
        /// <remarks/>
        public bool success {
            get {
                return this.successField;
            }
            set {
                this.successField = value;
            }
        }
        
        /// <remarks/>
        public int row_count {
            get {
                return this.row_countField;
            }
            set {
                this.row_countField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("conversion", IsNullable=false)]
        public conversion[] conversions {
            get {
                return this.conversionsField;
            }
            set {
                this.conversionsField = value;
            }
        }
    }
}
