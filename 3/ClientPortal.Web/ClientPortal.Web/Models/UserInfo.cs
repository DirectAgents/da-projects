using ClientPortal.Data.Contexts;
using ClientPortal.Data.Entities.TD;
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
            this.TDAccount = tradingDeskAccount;
        }

        private UserProfile UserProfile { get; set; }
        private Advertiser Advertiser { get; set; }
        public TradingDeskAccount TDAccount { get; set; }

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
            get {
                if (UserProfile != null && UserProfile.SearchProfile != null)
                    return true;
                return (Advertiser == null) ? false : Advertiser.HasSearch;
            }
        }

        public SearchProfile SearchProfile
        {
            get { return UserProfile.SearchProfile; }
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

        public DateTime? LatestDaySums
        {
            get { return (Advertiser == null) ? null : Advertiser.LatestDaySums; }
        }
        public DateTime? LatestClicks
        {
            get { return (Advertiser == null) ? null : Advertiser.LatestClicks; }
        }

        public bool UseYesterdayAsLatest
        {
            get
            {
                bool useYesterdayAsLatest;
                if (Boolean.TryParse(WebConfigurationManager.AppSettings["UseYesterdayAsLatest"], out useYesterdayAsLatest))
                    return useYesterdayAsLatest;
                else
                    return true; // (if not specified)
            }
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return (Advertiser == null) ? DayOfWeek.Sunday : (DayOfWeek)Advertiser.StartDayOfWeek; }
        }

        public DayOfWeek SearchStartDayOfWeek
        {
            get { return (SearchProfile == null) ? DayOfWeek.Sunday : (DayOfWeek)SearchProfile.StartDayOfWeek; }
        }

        private Dates _dates;
        internal Dates Dates
        {
            get
            {
                if (_dates == null) _dates = new Dates(this.UseYesterdayAsLatest, this.StartDayOfWeek);
                return _dates;
            }
        }
        public Dates DatesForSearch()
        {
            return new Dates(this.UseYesterdayAsLatest, this.SearchStartDayOfWeek);
        }


        public bool HasTradingDesk(bool only = false)
        {
            bool hasTradingDesk = (this.TDAccount != null);
            if (only)
                return hasTradingDesk && (Advertiser == null);
            else
                return hasTradingDesk;
        }

        public int? InsertionOrderID
        {
            get { return (TDAccount == null) ? null : TDAccount.InsertionOrderID(); }
        }
    }
}