using System.Collections.Generic;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class StratSumVM
    {
        public UserInfo UserInfo { get; set; }
        public IEnumerable<BasicStat> MTDStrategyStats { get; set; }
    }
}