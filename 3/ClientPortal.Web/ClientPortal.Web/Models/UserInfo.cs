using System;
using System.Globalization;
using System.Web.Configuration;
using ClientPortal.Data.Contexts;
using ClientPortal.Data.Entities.TD;
using ClientPortal.Web.Controllers;
using DirectAgents.Domain.Entities.TD;

namespace ClientPortal.Web.Models
{
    public class UserInfo
    {
        public UserInfo(UserProfile userProfile, ClientPortal.Data.Contexts.Advertiser cpAdvertiser, TradingDeskAccount tradingDeskAccount = null, DirectAgents.Domain.Entities.TD.Advertiser progAdvertiser = null)
        {
            this.UserProfile = userProfile;
            this.Advertiser = cpAdvertiser;
            this.TDAccount = tradingDeskAccount;
            this.ProgAdvertiser = progAdvertiser;
        }

        private UserProfile UserProfile { get; set; }
        private ClientPortal.Data.Contexts.Advertiser Advertiser { get; set; }
        public TradingDeskAccount TDAccount { get; set; }
        public DirectAgents.Domain.Entities.TD.Advertiser ProgAdvertiser { get; set; }

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
            get { return (Advertiser == null) ? "en-US" : Advertiser.Culture; } // TODO: get from SearchProfile/TDAccount where appropriate
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
            get { return (SearchProfile == null) ? false : SearchProfile.ShowSearchChannels; }
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
        public bool Search_UseYesterdayAsLatest
        {
            get
            {
                bool useYesterdayAsLatest;
                if (Boolean.TryParse(WebConfigurationManager.AppSettings["Search_UseYesterdayAsLatest"], out useYesterdayAsLatest))
                    return useYesterdayAsLatest;
                else
                    return true; // (if not specified)
            }
        }
        public bool TD_UseYesterdayAsLatest
        {
            get
            {
                bool useYesterdayAsLatest;
                if (Boolean.TryParse(WebConfigurationManager.AppSettings["TD_UseYesterdayAsLatest"], out useYesterdayAsLatest))
                    return useYesterdayAsLatest;
                else
                    return true; // (if not specified)
            }
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return (Advertiser == null) ? DayOfWeek.Sunday : (DayOfWeek)Advertiser.StartDayOfWeek; }
        }
        public DayOfWeek Search_StartDayOfWeek
        {
            get { return (SearchProfile == null) ? DayOfWeek.Sunday : (DayOfWeek)SearchProfile.StartDayOfWeek; }
        }
        public DayOfWeek TD_StartDayOfWeek
        {
            get { return (TDAccount == null) ? DayOfWeek.Monday : (DayOfWeek)TDAccount.StartDayOfWeek; }
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
        private Dates _searchDates;
        internal Dates Search_Dates
        {
            get
            {
                if (_searchDates == null) _searchDates = new Dates(this.Search_UseYesterdayAsLatest, this.Search_StartDayOfWeek);
                return _searchDates;
            }
        }
        private Dates _tdDates;
        internal Dates TD_Dates
        {
            get
            {
                if (_tdDates == null) _tdDates = new Dates(this.TD_UseYesterdayAsLatest, this.TD_StartDayOfWeek);
                return _tdDates;
            }
        }

        public bool HasProgrammatic
        {
            get { return (ProgAdvertiser != null); }
        }

        // --- to be eliminated ---

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