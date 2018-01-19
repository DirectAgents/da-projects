namespace DirectAgents.Domain.Entities.CPSearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchCampaign")]
    public partial class SearchCampaign
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchCampaign()
        {
            SearchConvSummaries = new HashSet<SearchConvSummary>();
            SearchDailySummaries = new HashSet<SearchDailySummary>();
        }

        public int SearchCampaignId { get; set; }

        [Required]
        [StringLength(255)]
        public string SearchCampaignName { get; set; }

        public int? AdvertiserId { get; set; }

        [StringLength(255)]
        public string Channel { get; set; }

        public int? ExternalId { get; set; }

        public int? SearchAccountId { get; set; }

        public int? AltSearchAccountId { get; set; }

        [StringLength(100)]
        public string LCcmpid { get; set; }

        public virtual SearchAccount SearchAccount { get; set; }

        public virtual SearchAccount AltSearchAccount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchConvSummary> SearchConvSummaries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchDailySummary> SearchDailySummaries { get; set; }
    }
}
