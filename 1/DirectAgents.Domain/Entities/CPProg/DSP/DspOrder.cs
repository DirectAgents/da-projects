using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DSP
{
    public class DspOrder : DspBaseItem
    {
        public int? AdvertiserId { get; set; }

        [ForeignKey("AdvertiserId")]
        public virtual DspAdvertiser Advertiser { get; set; }
    }
}
