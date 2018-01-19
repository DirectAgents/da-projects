namespace DirectAgents.Domain.Entities.CPSearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SearchAccount")]
    public partial class SearchAccount
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SearchAccount()
        {
            SearchCampaigns = new HashSet<SearchCampaign>();
            AltSearchCampaigns = new HashSet<SearchCampaign>();
        }

        public int SearchAccountId { get; set; }

        public int? AdvertiserId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Channel { get; set; }

        [StringLength(50)]
        public string AccountCode { get; set; }

        [StringLength(50)]
        public string ExternalId { get; set; }

        public int? SearchProfileId { get; set; }

        public decimal? RevPerOrder { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MinSynchDate { get; set; }

        public virtual SearchProfile SearchProfile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchCampaign> SearchCampaigns { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SearchCampaign> AltSearchCampaigns { get; set; }
    }
}
