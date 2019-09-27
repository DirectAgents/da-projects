using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.Cake
{
    public class Affiliate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AffiliateId { get; set; }

        public string AffiliateName { get; set; }

        public int? AccountManagerId { get; set; }

        [ForeignKey("AccountManagerId")]
        public virtual Contact AccountManager { get; set; }
    }

    public class AffSub
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AffiliateId { get; set; }

        public virtual Affiliate Affiliate { get; set; }
    }
}
