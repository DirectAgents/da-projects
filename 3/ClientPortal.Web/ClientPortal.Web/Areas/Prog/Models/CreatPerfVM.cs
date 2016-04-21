using System;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Areas.Prog.Models
{
    public class CreatPerfVM
    {
        public UserInfo UserInfo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}