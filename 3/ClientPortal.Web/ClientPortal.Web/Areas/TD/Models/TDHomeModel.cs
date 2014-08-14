using ClientPortal.Web.Controllers;
using ClientPortal.Web.Models;
using System.Globalization;

namespace ClientPortal.Web.Areas.TD.Models
{
    public class TDHomeModel
    {
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }

        public DatesModel Dates { get; set; }

        public TDHomeModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;
            bool useYesterdayAsLatest = true; // for TradingDesk
            var dates = new Dates(useYesterdayAsLatest, userInfo.WeekStartDay);
            Dates = new DatesModel(dates, this.CultureInfo);
        }

    }
}