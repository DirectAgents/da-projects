using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM
{
    public class DbmCreative : DbmBaseEntity
    {
        public int? LineItemId { get; set; }

        [ForeignKey("LineItemId")]
        public virtual DbmLineItem LineItem { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Size { get; set; }

        public string Type { get; set; }
    }
}
