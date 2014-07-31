using ClientPortal.Web.Models;

// UNUSED?

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDHomeModel
    {
        public TDHomeModel(UserInfo userInfo)
        {
            TDReportModel = new TDReportModel(userInfo);
        }

        public TDReportModel TDReportModel { get; set; }
    }
}