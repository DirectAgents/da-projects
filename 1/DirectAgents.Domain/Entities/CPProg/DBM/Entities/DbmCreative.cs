using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    public class DbmCreative : DbmEntity
    {
        public int? AdvertiserId { get; set; }

        [ForeignKey("AdvertiserId")]
        public virtual DbmAdvertiser Advertiser { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public string Size { get; set; }

        public string Type { get; set; }
    }
}
