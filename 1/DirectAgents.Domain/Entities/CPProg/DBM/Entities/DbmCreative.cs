using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    public class DbmCreative : DbmEntity
    {
        public int? LineItemId { get; set; }

        [ForeignKey("LineItemId")]
        public virtual DbmLineItem LineItem { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public string Size { get; set; }

        public string Type { get; set; }
    }
}
