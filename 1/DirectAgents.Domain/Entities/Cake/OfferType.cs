using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class OfferType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfferTypeId { get; set; }
        public string OfferTypeName { get; set; }
    }
}
