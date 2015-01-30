using ClientPortal.Web.Controllers;
using ClientPortal.Web.Models;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDHomeModel
    {
        public UserInfo UserInfo { get; set; }
        public DatesModel Dates { get; set; }

        public TDHomeModel(UserInfo userInfo)
        {
            UserInfo = userInfo;
            Dates = new DatesModel(UserInfo.TD_Dates, UserInfo.CultureInfo);
        }

    }
}