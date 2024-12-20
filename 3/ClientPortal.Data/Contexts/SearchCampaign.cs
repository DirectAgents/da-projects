//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientPortal.Data.Contexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class SearchCampaign
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchCampaign()
        {
            this.SearchDailySummaries = new HashSet<SearchDailySummary>();
            this.GoogleAnalyticsSummaries = new HashSet<GoogleAnalyticsSummary>();
            this.SearchDailySummaries2 = new HashSet<SearchDailySummary2>();
            this.CallDailySummaries = new HashSet<CallDailySummary>();
            this.SearchConvSummaries = new HashSet<SearchConvSummary>();
        }
    
        public int SearchCampaignId { get; set; }
        public string SearchCampaignName { get; set; }
        public Nullable<int> AdvertiserId { get; set; }
        public string Channel { get; set; }
        public string ExternalId { get; set; }
        public Nullable<int> SearchAccountId { get; set; }
        public Nullable<int> AltSearchAccountId { get; set; }
        public string LCcmpid { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchDailySummary> SearchDailySummaries { get; set; }
        public virtual Advertiser Advertiser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GoogleAnalyticsSummary> GoogleAnalyticsSummaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchDailySummary2> SearchDailySummaries2 { get; set; }
        public virtual SearchAccount SearchAccount { get; set; }
        public virtual SearchAccount AltSearchAccount { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CallDailySummary> CallDailySummaries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchConvSummary> SearchConvSummaries { get; set; }
    }
}
