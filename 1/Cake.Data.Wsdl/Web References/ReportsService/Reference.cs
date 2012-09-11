﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace Cake.Data.Wsdl.ReportsService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="reportsSoap", Namespace="http://cakemarketing.com/api/4/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(base_response))]
    public partial class reports : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ConversionsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public reports() {
            this.Url = global::Cake.Data.Wsdl.Properties.Settings.Default.Cake_Data_Wsdl_ReportsService_reports;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ConversionsCompletedEventHandler ConversionsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cakemarketing.com/api/4/Conversions", RequestNamespace="http://cakemarketing.com/api/4/", ResponseNamespace="http://cakemarketing.com/api/4/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public conversion_report_response Conversions(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending) {
            object[] results = this.Invoke("Conversions", new object[] {
                        api_key,
                        start_date,
                        end_date,
                        affiliate_id,
                        advertiser_id,
                        offer_id,
                        campaign_id,
                        creative_id,
                        include_tests,
                        start_at_row,
                        row_limit,
                        sort_field,
                        sort_descending});
            return ((conversion_report_response)(results[0]));
        }
        
        /// <remarks/>
        public void ConversionsAsync(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending) {
            this.ConversionsAsync(api_key, start_date, end_date, affiliate_id, advertiser_id, offer_id, campaign_id, creative_id, include_tests, start_at_row, row_limit, sort_field, sort_descending, null);
        }
        
        /// <remarks/>
        public void ConversionsAsync(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending, object userState) {
            if ((this.ConversionsOperationCompleted == null)) {
                this.ConversionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConversionsOperationCompleted);
            }
            this.InvokeAsync("Conversions", new object[] {
                        api_key,
                        start_date,
                        end_date,
                        affiliate_id,
                        advertiser_id,
                        offer_id,
                        campaign_id,
                        creative_id,
                        include_tests,
                        start_at_row,
                        row_limit,
                        sort_field,
                        sort_descending}, this.ConversionsOperationCompleted, userState);
        }
        
        private void OnConversionsOperationCompleted(object arg) {
            if ((this.ConversionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConversionsCompleted(this, new ConversionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public enum ConversionsSortFields {
        
        /// <remarks/>
        conversion_id,
        
        /// <remarks/>
        visitor_id,
        
        /// <remarks/>
        request_session_id,
        
        /// <remarks/>
        click_id,
        
        /// <remarks/>
        conversion_date,
        
        /// <remarks/>
        transaction_id,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class conversion_report_response : get_response {
        
        private conversion[] conversionsField;
        
        /// <remarks/>
        public conversion[] conversions {
            get {
                return this.conversionsField;
            }
            set {
                this.conversionsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class conversion {
        
        private string conversion_idField;
        
        private int visitor_idField;
        
        private int request_session_idField;
        
        private System.Nullable<int> click_idField;
        
        private System.DateTime conversion_dateField;
        
        private affiliate affiliateField;
        
        private advertiser advertiserField;
        
        private offer offerField;
        
        private int campaign_idField;
        
        private creative creativeField;
        
        private string sub_id_1Field;
        
        private string sub_id_2Field;
        
        private string sub_id_3Field;
        
        private string sub_id_4Field;
        
        private string sub_id_5Field;
        
        private string conversion_typeField;
        
        private payment paidField;
        
        private payment receivedField;
        
        private bool pixel_droppedField;
        
        private bool suppressedField;
        
        private bool returnedField;
        
        private bool testField;
        
        private string transaction_idField;
        
        private string ip_addressField;
        
        private string referrer_urlField;
        
        private disposition dispositionField;
        
        private string noteField;
        
        /// <remarks/>
        public string conversion_id {
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<int> click_id {
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
        public int campaign_id {
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
        public string sub_id_4 {
            get {
                return this.sub_id_4Field;
            }
            set {
                this.sub_id_4Field = value;
            }
        }
        
        /// <remarks/>
        public string sub_id_5 {
            get {
                return this.sub_id_5Field;
            }
            set {
                this.sub_id_5Field = value;
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
        public payment paid {
            get {
                return this.paidField;
            }
            set {
                this.paidField = value;
            }
        }
        
        /// <remarks/>
        public payment received {
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
        public disposition disposition {
            get {
                return this.dispositionField;
            }
            set {
                this.dispositionField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="API:id_name_store")]
    public partial class affiliate {
        
        private int affiliate_idField;
        
        private string affiliate_nameField;
        
        /// <remarks/>
        public int affiliate_id {
            get {
                return this.affiliate_idField;
            }
            set {
                this.affiliate_idField = value;
            }
        }
        
        /// <remarks/>
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class disposition {
        
        private bool approvedField;
        
        private string disposition_nameField;
        
        private string contactField;
        
        private System.DateTime disposition_dateField;
        
        /// <remarks/>
        public bool approved {
            get {
                return this.approvedField;
            }
            set {
                this.approvedField = value;
            }
        }
        
        /// <remarks/>
        public string disposition_name {
            get {
                return this.disposition_nameField;
            }
            set {
                this.disposition_nameField = value;
            }
        }
        
        /// <remarks/>
        public string contact {
            get {
                return this.contactField;
            }
            set {
                this.contactField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime disposition_date {
            get {
                return this.disposition_dateField;
            }
            set {
                this.disposition_dateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class payment {
        
        private byte currency_idField;
        
        private decimal amountField;
        
        private string formatted_amountField;
        
        /// <remarks/>
        public byte currency_id {
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="API:id_name_store")]
    public partial class creative {
        
        private int creative_idField;
        
        private string creative_nameField;
        
        /// <remarks/>
        public int creative_id {
            get {
                return this.creative_idField;
            }
            set {
                this.creative_idField = value;
            }
        }
        
        /// <remarks/>
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="API:id_name_store")]
    public partial class offer {
        
        private int offer_idField;
        
        private string offer_nameField;
        
        /// <remarks/>
        public int offer_id {
            get {
                return this.offer_idField;
            }
            set {
                this.offer_idField = value;
            }
        }
        
        /// <remarks/>
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="API:id_name_store")]
    public partial class advertiser {
        
        private int advertiser_idField;
        
        private string advertiser_nameField;
        
        /// <remarks/>
        public int advertiser_id {
            get {
                return this.advertiser_idField;
            }
            set {
                this.advertiser_idField = value;
            }
        }
        
        /// <remarks/>
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(get_response))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(conversion_report_response))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class base_response {
        
        private bool successField;
        
        private string messageField;
        
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
        public string message {
            get {
                return this.messageField;
            }
            set {
                this.messageField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(conversion_report_response))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/4/")]
    public partial class get_response : base_response {
        
        private int row_countField;
        
        /// <remarks/>
        public int row_count {
            get {
                return this.row_countField;
            }
            set {
                this.row_countField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void ConversionsCompletedEventHandler(object sender, ConversionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConversionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ConversionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public conversion_report_response Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((conversion_report_response)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591