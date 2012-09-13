﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cake.Data.Wsdl.SignupService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://cakemarketing.com/api/1/", ConfigurationName="SignupService.signupSoap")]
    public interface signupSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/Affiliate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string Affiliate(
                    string api_key, 
                    string company_name, 
                    string address_street, 
                    string address_street2, 
                    string address_city, 
                    string address_state, 
                    string address_zip_code, 
                    string address_country, 
                    string referred_by, 
                    string first_name, 
                    string last_name, 
                    string email_address, 
                    string password, 
                    string tax_class, 
                    string ssn_taxid, 
                    string website, 
                    string notes, 
                    string contact_title, 
                    string contact_phone_work, 
                    string contact_phone_cell, 
                    string contact_phone_fax, 
                    string contact_im_name, 
                    int contact_im_service, 
                    int media_type, 
                    int price_format, 
                    int primary_vertical_category, 
                    int secondary_vertical_category, 
                    int payment_to, 
                    string ip_address);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/Advertiser", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string Advertiser(
                    string api_key, 
                    string company_name, 
                    string address_street, 
                    string address_street2, 
                    string address_city, 
                    string address_state, 
                    string address_zip_code, 
                    string address_country, 
                    string first_name, 
                    string last_name, 
                    string email_address, 
                    string password, 
                    string website, 
                    string notes, 
                    string contact_title, 
                    string contact_phone_work, 
                    string contact_phone_cell, 
                    string contact_phone_fax, 
                    string contact_im_name, 
                    int contact_im_service, 
                    string ip_address);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/GetTrafficTypes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        Cake.Data.Wsdl.SignupService.TrafficTypes GetTrafficTypes(string api_key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/GetMediaTypes", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MediaType[] GetMediaTypes(string api_key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/GetPriceFormats", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        PriceFormat[] GetPriceFormats(string api_key);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://cakemarketing.com/api/1/GetVerticalCategories", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        VerticalCategory[] GetVerticalCategories(string api_key);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/1/")]
    public partial class TrafficTypes : object, System.ComponentModel.INotifyPropertyChanged {
        
        private MediaType[] mediaTypesField;
        
        private PriceFormat[] priceFormatsField;
        
        private VerticalCategory[] verticalCategoriesField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=0)]
        public MediaType[] MediaTypes {
            get {
                return this.mediaTypesField;
            }
            set {
                this.mediaTypesField = value;
                this.RaisePropertyChanged("MediaTypes");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=1)]
        public PriceFormat[] PriceFormats {
            get {
                return this.priceFormatsField;
            }
            set {
                this.priceFormatsField = value;
                this.RaisePropertyChanged("PriceFormats");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Order=2)]
        public VerticalCategory[] VerticalCategories {
            get {
                return this.verticalCategoriesField;
            }
            set {
                this.verticalCategoriesField = value;
                this.RaisePropertyChanged("VerticalCategories");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/1/")]
    public partial class MediaType : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int media_type_idField;
        
        private string type_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int media_type_id {
            get {
                return this.media_type_idField;
            }
            set {
                this.media_type_idField = value;
                this.RaisePropertyChanged("media_type_id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string type_name {
            get {
                return this.type_nameField;
            }
            set {
                this.type_nameField = value;
                this.RaisePropertyChanged("type_name");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/1/")]
    public partial class VerticalCategory : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int vertical_category_idField;
        
        private string category_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int vertical_category_id {
            get {
                return this.vertical_category_idField;
            }
            set {
                this.vertical_category_idField = value;
                this.RaisePropertyChanged("vertical_category_id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string category_name {
            get {
                return this.category_nameField;
            }
            set {
                this.category_nameField = value;
                this.RaisePropertyChanged("category_name");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://cakemarketing.com/api/1/")]
    public partial class PriceFormat : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int price_format_idField;
        
        private string format_nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int price_format_id {
            get {
                return this.price_format_idField;
            }
            set {
                this.price_format_idField = value;
                this.RaisePropertyChanged("price_format_id");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public string format_name {
            get {
                return this.format_nameField;
            }
            set {
                this.format_nameField = value;
                this.RaisePropertyChanged("format_name");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface signupSoapChannel : Cake.Data.Wsdl.SignupService.signupSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class signupSoapClient : System.ServiceModel.ClientBase<Cake.Data.Wsdl.SignupService.signupSoap>, Cake.Data.Wsdl.SignupService.signupSoap {
        
        public signupSoapClient() {
        }
        
        public signupSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public signupSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public signupSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public signupSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string Affiliate(
                    string api_key, 
                    string company_name, 
                    string address_street, 
                    string address_street2, 
                    string address_city, 
                    string address_state, 
                    string address_zip_code, 
                    string address_country, 
                    string referred_by, 
                    string first_name, 
                    string last_name, 
                    string email_address, 
                    string password, 
                    string tax_class, 
                    string ssn_taxid, 
                    string website, 
                    string notes, 
                    string contact_title, 
                    string contact_phone_work, 
                    string contact_phone_cell, 
                    string contact_phone_fax, 
                    string contact_im_name, 
                    int contact_im_service, 
                    int media_type, 
                    int price_format, 
                    int primary_vertical_category, 
                    int secondary_vertical_category, 
                    int payment_to, 
                    string ip_address) {
            return base.Channel.Affiliate(api_key, company_name, address_street, address_street2, address_city, address_state, address_zip_code, address_country, referred_by, first_name, last_name, email_address, password, tax_class, ssn_taxid, website, notes, contact_title, contact_phone_work, contact_phone_cell, contact_phone_fax, contact_im_name, contact_im_service, media_type, price_format, primary_vertical_category, secondary_vertical_category, payment_to, ip_address);
        }
        
        public string Advertiser(
                    string api_key, 
                    string company_name, 
                    string address_street, 
                    string address_street2, 
                    string address_city, 
                    string address_state, 
                    string address_zip_code, 
                    string address_country, 
                    string first_name, 
                    string last_name, 
                    string email_address, 
                    string password, 
                    string website, 
                    string notes, 
                    string contact_title, 
                    string contact_phone_work, 
                    string contact_phone_cell, 
                    string contact_phone_fax, 
                    string contact_im_name, 
                    int contact_im_service, 
                    string ip_address) {
            return base.Channel.Advertiser(api_key, company_name, address_street, address_street2, address_city, address_state, address_zip_code, address_country, first_name, last_name, email_address, password, website, notes, contact_title, contact_phone_work, contact_phone_cell, contact_phone_fax, contact_im_name, contact_im_service, ip_address);
        }
        
        public Cake.Data.Wsdl.SignupService.TrafficTypes GetTrafficTypes(string api_key) {
            return base.Channel.GetTrafficTypes(api_key);
        }
        
        public MediaType[] GetMediaTypes(string api_key) {
            return base.Channel.GetMediaTypes(api_key);
        }
        
        public PriceFormat[] GetPriceFormats(string api_key) {
            return base.Channel.GetPriceFormats(api_key);
        }
        
        public VerticalCategory[] GetVerticalCategories(string api_key) {
            return base.Channel.GetVerticalCategories(api_key);
        }
    }
}
