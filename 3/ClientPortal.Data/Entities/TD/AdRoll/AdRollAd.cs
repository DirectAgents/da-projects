
using System;
namespace ClientPortal.Data.Entities.TD.AdRoll
{
    public class AdRollAd
    {
        public int Id { get; set; }
        public int AdRollProfileId { get; set; }
        public virtual AdRollProfile AdRollProfile { get; set; }

        public string Name { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
