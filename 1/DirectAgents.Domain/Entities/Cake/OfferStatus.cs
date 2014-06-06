using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class OfferStatus
    {
        public const int Private = 2;
        public const int Inactive = 4;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OfferStatusId { get; set; }
        public string OfferStatusName { get; set; }
    }
}
