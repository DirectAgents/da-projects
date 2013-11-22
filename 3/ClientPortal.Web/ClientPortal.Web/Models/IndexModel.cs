using ClientPortal.Web.Controllers;
using System;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class IndexModel
    {
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }
        // TODO: make CultureInfo a singleton (on demand)

        public bool HasLogo { get; set; }
        public bool ShowCPMRep { get; set; }

        public DatesModel Dates { get; set; }

        public IndexModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;
            HasLogo = (userInfo.Logo != null);
            ShowCPMRep = userInfo.ShowCPMRep;

            var dates = new Dates(userInfo.UseYesterdayAsLatest, userInfo.WeekStartDay);
            Dates = new DatesModel(dates, this.CultureInfo);
        }
    }
}