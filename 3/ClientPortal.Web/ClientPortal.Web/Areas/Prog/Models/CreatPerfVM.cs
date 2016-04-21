using System;
using System.Collections.Generic;
using ClientPortal.Web.Models;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class CreatPerfVM
    {
        public UserInfo UserInfo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<BasicStat> CreativeStats { get; set; }
    }
}