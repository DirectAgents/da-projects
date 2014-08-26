using ClientPortal.Web.Controllers;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class SearchIndexModel
    {
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }
        // TODO: make CultureInfo a singleton (on demand)

        public bool HasLogo { get; set; }
        public DatesModel Dates { get; set; }

        public SearchIndexModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;
            HasLogo = (userInfo.Logo != null);

            var dates = new Dates(userInfo.UseYesterdayAsLatest, userInfo.SearchStartDayOfWeek);
            Dates = new DatesModel(dates, this.CultureInfo);
        }
    }
}