using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDAnalysisModel
    {
        public UserInfo UserInfo { get; set; }
        public IEnumerable<UserListStat> UserListStats { get; set; }

        public TDAnalysisModel(UserInfo userInfo)
        {
            UserInfo = userInfo;
        }
    }
}