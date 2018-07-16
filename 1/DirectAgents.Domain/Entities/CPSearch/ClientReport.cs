using System.ComponentModel.DataAnnotations.Schema;
using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.Entities.CPSearch
{
    [Table("ClientReport")]
    public partial class ClientReport
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int StartDayOfWeek { get; set; }

        public int? SearchProfileId { get; set; }
        public int? ProgCampaignId { get; set; }

        [NotMapped]
        public SearchProfile SearchProfile { get; set; }
        [NotMapped]
        public Campaign ProgCampaign { get; set; }
    }
}
