using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class Advertiser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AdvertiserId { get; set; }
        public string AdvertiserName { get; set; }

        public virtual List<Offer> Offers { get; set; }
    }
}
