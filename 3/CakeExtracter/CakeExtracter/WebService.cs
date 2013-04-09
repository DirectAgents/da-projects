namespace CakeMarketing
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "reportsSoap", Namespace = "http://cakemarketing.com/api/5/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(base_response))]
    public partial class reports : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        /// <remarks/>
        public reports()
        {
            this.Url = "https://login.directagents.com/api/5/reports.asmx";
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cakemarketing.com/api/5/Conversions", RequestNamespace = "http://cakemarketing.com/api/5/", ResponseNamespace = "http://cakemarketing.com/api/5/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public conversion_report_response Conversions(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending)
        {
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
        public System.IAsyncResult BeginConversions(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Conversions", new object[] {
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
                        sort_descending}, callback, asyncState);
        }

        /// <remarks/>
        public conversion_report_response EndConversions(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((conversion_report_response)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cakemarketing.com/api/5/ConversionChanges", RequestNamespace = "http://cakemarketing.com/api/5/", ResponseNamespace = "http://cakemarketing.com/api/5/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public conversion_report_response ConversionChanges(string api_key, System.DateTime changes_since, bool include_new_conversions, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending)
        {
            object[] results = this.Invoke("ConversionChanges", new object[] {
                        api_key,
                        changes_since,
                        include_new_conversions,
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
        public System.IAsyncResult BeginConversionChanges(string api_key, System.DateTime changes_since, bool include_new_conversions, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, ConversionsSortFields sort_field, bool sort_descending, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("ConversionChanges", new object[] {
                        api_key,
                        changes_since,
                        include_new_conversions,
                        affiliate_id,
                        advertiser_id,
                        offer_id,
                        campaign_id,
                        creative_id,
                        include_tests,
                        start_at_row,
                        row_limit,
                        sort_field,
                        sort_descending}, callback, asyncState);
        }

        /// <remarks/>
        public conversion_report_response EndConversionChanges(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((conversion_report_response)(results[0]));
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://cakemarketing.com/api/5/Clicks", RequestNamespace = "http://cakemarketing.com/api/5/", ResponseNamespace = "http://cakemarketing.com/api/5/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public click_report_response Clicks(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit)
        {
            object[] results = this.Invoke("Clicks", new object[] {
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
                        row_limit});
            return ((click_report_response)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginClicks(string api_key, System.DateTime start_date, System.DateTime end_date, int affiliate_id, int advertiser_id, int offer_id, int campaign_id, int creative_id, bool include_tests, int start_at_row, int row_limit, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Clicks", new object[] {
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
                        row_limit}, callback, asyncState);
        }

        /// <remarks/>
        public click_report_response EndClicks(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((click_report_response)(results[0]));
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public enum ConversionsSortFields
    {

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

        /// <remarks/>
        last_updated,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class conversion_report_response : get_response
    {

        /// <remarks/>
        public conversion[] conversions;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class conversion
    {

        /// <remarks/>
        public string conversion_id { get; set; }

        /// <remarks/>
        public int visitor_id { get; set; }

        /// <remarks/>
        public int request_session_id { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<int> click_id { get; set; }

        /// <remarks/>
        public System.DateTime conversion_date { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> last_updated { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<System.DateTime> click_date { get; set; }

        /// <remarks/>
        public affiliate affiliate { get; set; }

        /// <remarks/>
        public advertiser advertiser { get; set; }

        /// <remarks/>
        public offer offer { get; set; }

        /// <remarks/>
        public int campaign_id { get; set; }

        /// <remarks/>
        public creative creative { get; set; }

        /// <remarks/>
        public string sub_id_1;

        /// <remarks/>
        public string sub_id_2;

        /// <remarks/>
        public string sub_id_3;

        /// <remarks/>
        public string sub_id_4;

        /// <remarks/>
        public string sub_id_5;

        /// <remarks/>
        public string conversion_type { get; set; }

        /// <remarks/>
        public payment paid;

        /// <remarks/>
        public payment received;

        /// <remarks/>
        public byte step_reached;

        /// <remarks/>
        public bool pixel_dropped { get; set; }

        /// <remarks/>
        public bool suppressed { get; set; }

        /// <remarks/>
        public bool returned { get; set; }

        /// <remarks/>
        public bool test { get; set; }

        /// <remarks/>
        public string transaction_id { get; set; }

        /// <remarks/>
        public string conversion_ip_address { get; set; }

        /// <remarks/>
        public string click_ip_address { get; set; }

        /// <remarks/>
        public string country { get; set; }

        /// <remarks/>
        public string conversion_referrer_url { get; set; }

        /// <remarks/>
        public string click_referrer_url { get; set; }

        /// <remarks/>
        public string conversion_user_agent { get; set; }

        /// <remarks/>
        public string click_user_agent { get; set; }

        /// <remarks/>
        public disposition disposition;

        /// <remarks/>
        public string note { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class affiliate
    {

        /// <remarks/>
        public int affiliate_id { get; set; }

        /// <remarks/>
        public string affiliate_name;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class browser
    {

        /// <remarks/>
        public byte browser_id { get; set; }

        /// <remarks/>
        public string browser_name { get; set; }

        /// <remarks/>
        public version browser_version { get; set; }

        /// <remarks/>
        public version browser_version_minor { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class version
    {

        /// <remarks/>
        public short version_id { get; set; }

        /// <remarks/>
        public string version_name { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class operating_system
    {

        /// <remarks/>
        public byte operating_system_id { get; set; }

        /// <remarks/>
        public string operating_system_name { get; set; }

        /// <remarks/>
        public version operating_system_version { get; set; }

        /// <remarks/>
        public version operating_system_version_minor { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class device
    {

        /// <remarks/>
        public short device_id { get; set; }

        /// <remarks/>
        public string device_name { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class isp
    {

        /// <remarks/>
        public int isp_id { get; set; }

        /// <remarks/>
        public string isp_name { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class language
    {

        /// <remarks/>
        public byte language_id { get; set; }

        /// <remarks/>
        public string language_name { get; set; }

        /// <remarks/>
        public string language_abbr { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class region
    {

        /// <remarks/>
        public string region_code { get; set; }

        /// <remarks/>
        public string region_name { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class country
    {

        /// <remarks/>
        public string country_code { get; set; }
        /// <remarks/>
        public string country_name { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class click
    {

        /// <remarks/>
        public int click_id { get; set; }

        /// <remarks/>
        public int visitor_id { get; set; }

        /// <remarks/>
        public int request_session_id { get; set; }

        /// <remarks/>
        public System.DateTime click_date { get; set; }

        /// <remarks/>
        public affiliate affiliate { get; set; }

        /// <remarks/>
        public advertiser advertiser { get; set; }

        /// <remarks/>
        public offer offer { get; set; }

        /// <remarks/>
        public int campaign_id { get; set; }

        /// <remarks/>
        public creative creative { get; set; }

        /// <remarks/>
        public string sub_id_1;

        /// <remarks/>
        public string sub_id_2;

        /// <remarks/>
        public string sub_id_3;

        /// <remarks/>
        public string sub_id_4;

        /// <remarks/>
        public string sub_id_5;

        /// <remarks/>
        public string ip_address { get; set; }

        /// <remarks/>
        public string user_agent { get; set; }

        /// <remarks/>
        public string referrer_url { get; set; }

        /// <remarks/>
        public string request_url { get; set; }

        /// <remarks/>
        public string redirect_url { get; set; }

        /// <remarks/>
        public country country;

        /// <remarks/>
        public region region { get; set; }

        /// <remarks/>
        public language language;

        /// <remarks/>
        public isp isp;

        /// <remarks/>
        public device device;

        /// <remarks/>
        public operating_system operating_system;

        /// <remarks/>
        public browser browser;

        /// <remarks/>
        public string disposition { get; set; }

        /// <remarks/>
        public string paid_action { get; set; }

        /// <remarks/>
        public int total_clicks { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class advertiser
    {

        /// <remarks/>
        public int advertiser_id { get; set; }

        /// <remarks/>
        public string advertiser_name;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class offer
    {

        /// <remarks/>
        public int offer_id { get; set; }

        /// <remarks/>
        public string offer_name;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "API:id_name_store")]
    public partial class creative
    {

        /// <remarks/>
        public int creative_id { get; set; }

        /// <remarks/>
        public string creative_name;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class disposition
    {

        /// <remarks/>
        public bool approved { get; set; }

        /// <remarks/>
        public string disposition_name { get; set; }

        /// <remarks/>
        public string contact { get; set; }

        /// <remarks/>
        public System.DateTime disposition_date { get; set; }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class payment
    {

        /// <remarks/>
        public byte currency_id;

        /// <remarks/>
        public decimal amount;

        /// <remarks/>
        public string formatted_amount;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(get_response))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(click_report_response))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(conversion_report_response))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class base_response
    {

        /// <remarks/>
        public bool success;

        /// <remarks/>
        public string message;
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(click_report_response))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(conversion_report_response))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class get_response : base_response
    {

        /// <remarks/>
        public int row_count;
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("SharpDevelop", "4.3.1.9430-406354be")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://cakemarketing.com/api/5/")]
    public partial class click_report_response : get_response
    {

        /// <remarks/>
        public click[] clicks;
    }
}
