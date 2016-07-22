using System.Globalization;
using ClientPortal.Data.Contexts;
using ClientPortal.Web.Controllers;

namespace ClientPortal.Web.Models
{
    public class SearchVM
    {
        public SearchVM(UserInfo userInfo)
        {
            this.UserInfo = userInfo;
            this.Dates = new DatesModel(userInfo.Search_Dates, userInfo.CultureInfo);
        }

        public UserInfo UserInfo { get; set; }
        public DatesModel Dates { get; set; }

        public SearchProfile SearchProfile
        {   // Instantiate one if necessary
            get { return (UserInfo != null && UserInfo.SearchProfile != null) ? UserInfo.SearchProfile : new SearchProfile(); }
        }
    }

    public class SearchIndexModel
    {
        // TODO: store UserInfo here; get Culture/CultureInfo from UserInfo...
        public string Culture { get; set; }
        public CultureInfo CultureInfo { get { return string.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); } }

        public bool HasLogo { get; set; }
        public DatesModel Dates { get; set; }

        public SearchIndexModel(UserInfo userInfo)
        {
            Culture = userInfo.Culture;
            HasLogo = (userInfo.Logo != null);

            Dates = new DatesModel(userInfo.Search_Dates, this.CultureInfo);
        }
    }

    public class SearchReportModel
    {
        public SearchProfile SearchProfile { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}