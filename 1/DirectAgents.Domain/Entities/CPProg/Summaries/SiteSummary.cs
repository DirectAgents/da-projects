using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg
{
    /// DailySummary for a Site / ExtAccount
    public class SiteSummary : DatedStatsSummary
    {
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        [NotMapped]
        public string SiteName
        {
            get => siteName;
            set => siteName = value?.ToLower();
        }
        private string siteName;
    }
}