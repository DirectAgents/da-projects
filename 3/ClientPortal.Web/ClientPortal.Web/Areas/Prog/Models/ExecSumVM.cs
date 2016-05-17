using System;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class ExecSumVM
    {
        public UserInfo UserInfo { get; set; }
        public DateTime StartDate { get; set; }
        public BasicStat MTDStat { get; set; }
        public BasicStat CTDStat { get; set; } // campaign-to-date
    }
}