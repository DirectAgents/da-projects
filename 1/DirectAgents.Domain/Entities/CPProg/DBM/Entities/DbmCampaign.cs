using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    public class DbmCampaign : DbmEntity
    {
        public int? AdvertiserId { get; set; }

        [ForeignKey("AdvertiserId")]
        public virtual DbmAdvertiser Advertiser { get; set; }
    }
}
