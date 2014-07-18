using ClientPortal.Data.Contexts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Data.Entities.TD.DBM;
using ClientPortal.Web.Controllers;
using System;
using System.Globalization;
using System.Web.Configuration;

namespace ClientPortal.Web.Models
{
    public class UserInfo
    {
        public UserInfo(UserProfile userProfile, Advertiser advertiser, TradingDeskAccount tradingDeskAccount)
        {
            this.UserProfile = userProfile;
            this.Advertiser = advertiser;
            this.TradingDeskAccount = tradingDeskAccount;
        }

        private UserProfile UserProfile { get; set; }
        private Advertiser Advertiser { get; set; }
        private TradingDeskAccount TradingDeskAccount { get; set; }

        public bool HasUserProfile
        {
            get { return (UserProfile != null); }
        }

        public bool IsAdmin
        {
            get { return !HasUserProfile ? false : CheckIsAdmin(UserProfile.UserName); }
        }
        public static bool CheckIsAdmin(string userName)
        {
            return (userName == "admin");
        }

        public bool HasTradingDesk(bool only)
        {
            bool hasTradingDesk = (this.TradingDeskAccount != null);
            if (only)
                return hasTradingDesk && (Advertiser == null);
            else
                return hasTradingDesk;
        }

        public int? AdvertiserId
        {
            get { return UserProfile.CakeAdvertiserId; }
        }

        public string Culture
        {
            get { return (Advertiser == null) ? "en-US" : Advertiser.Culture; }
        }
        public CultureInfo CultureInfo
        {
            get { return String.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); }
        }
        public bool ShowCPMRep
        {
            get { return (Advertiser == null) ? false : Advertiser.ShowCPMRep; }
        }
        public bool ShowConversionData
        {
            get { return (Advertiser == null) ? false : Advertiser.ShowConversionData; }
        }
        public string ConversionValueName
        {
            get { return (Advertiser == null) ? null : Advertiser.ConversionValueName; }
        }
        public bool ConversionValueIsNumber
        {
            get { return (Advertiser == null) ? false : Advertiser.ConversionValueIsNumber; }
        }

        public bool HasSearch
        {
            get { return (Advertiser == null) ? false : Advertiser.HasSearch; }
        }

        public bool ShowSearchChannels
        {
            get { return (Advertiser == null) ? false : Advertiser.ShowSearchChannels; }
        }

        public bool UseAnalytics
        {
            get { return (Advertiser == null) ? false : !String.IsNullOrWhiteSpace(Advertiser.AnalyticsProfileId); }
        }

        public byte[] Logo
        {
            get { return Advertiser == null ? null : Advertiser.Logo; }
        }

        public DayOfWeek WeekStartDay
        {
            get { return (DayOfWeek)UserProfile.SearchWeekStartDay; }
        }
        //TODO: move Week start & end properties to Advertiser. change to WeekStartDay? (same for search & non-search?)

        //public DayOfWeek SearchWeekEndDay // compute this from StartDay ?

        public DateTime? LatestDaySums
        {
            get { return (Advertiser == null) ? null : Advertiser.LatestDaySums; }
        }
        public DateTime? LatestClicks
        {
            get { return (Advertiser == null) ? null : Advertiser.LatestClicks; }
        }

        private Dates _dates;
        internal Dates Dates
        {
            get
            {
                if (_dates == null) _dates = new Dates(this.UseYesterdayAsLatest, this.WeekStartDay);
                return _dates;
            }
        }

        public bool UseYesterdayAsLatest
        {
            get {
                bool useYesterdayAsLatest;
                if (Boolean.TryParse(WebConfigurationManager.AppSettings["UseYesterdayAsLatest"], out useYesterdayAsLatest))
                    return useYesterdayAsLatest;
                else
                    return true; // (if not specified)
            }
        }
    }
}