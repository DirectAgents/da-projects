using ClientPortal.Web.Models;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class ExecSumVM
    {
        public UserInfo UserInfo { get; set; }
        public BasicStat MTDStat { get; set; }
        public BasicStat LastMonthStat { get; set; }
    }
}