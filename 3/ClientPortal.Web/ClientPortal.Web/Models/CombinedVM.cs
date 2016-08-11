using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Models
{
    public class CombinedVM
    {
        public UserInfo UserInfo { get; set; }
        public BasicStat MTDStat { get; set; }
        public BasicStat WTDStat { get; set; }
    }
}