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
            this.CakeAdvertiser = cpAdvertiser;
            this.TDAccount = tradingDeskAccount;
            this.ProgAdvertiser = progAdvertiser;
        }

        private UserProfile UserProfile { get; set; }
        private ClientPortal.Data.Contexts.Advertiser CakeAdvertiser { get; set; }
        public TradingDeskAccount TDAccount { get; set; } // deprecated
        public DirectAgents.Domain.Entities.TD.Advertiser ProgAdvertiser { get; set; }

        public bool HasUserProfile
        {
            get { return (UserProfile != null); }
        }

        // For the combined portal...
        public string ClientName
        {
            get { return (UserProfile != null && UserProfile.ClientInfo != null) ? UserProfile.ClientInfo.Name : null; }
        }
        public byte[] ClientLogo
        {
            get { return (UserProfile != null && UserProfile.ClientInfo != null) ? UserProfile.ClientInfo.Logo : null; }
        }

        public bool IsAdmin
        {
            get { return !HasUserProfile ? false : CheckIsAdmin(UserProfile.UserName); }
        }
        public static bool CheckIsAdmin(string userName)
        {
            return (userName == "admin");
        }

        public string Culture
        {
            get { return (CakeAdvertiser == null) ? "en-US" : CakeAdvertiser.Culture; } // TODO: get from SearchProfile/TDAccount where appropriate
        }
        public CultureInfo CultureInfo
        {
            get { return String.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); }
        }
        public bool ShowCPMRep
        {
            get { return (CakeAdvertiser == null) ? false : CakeAdvertiser.ShowCPMRep; }
        }
        public bool ShowConversionData
        {
            get { return (CakeAdvertiser == null) ? false : CakeAdvertiser.ShowConversionData; }
        }
        public string ConversionValueName
        {
            get { return (CakeAdvertiser == null) ? null : CakeAdvertiser.ConversionValueName; }
        }
        public bool ConversionValueIsNumber
        {
            get { return (CakeAdvertiser == null) ? false : CakeAdvertiser.ConversionValueIsNumber; }
        }

        public int? CakeAdvertiserId
        {
            get { return UserProfile.CakeAdvertiserId; }
        }

        public bool HasCake
        {
            get { return (CakeAdvertiser != null); }
        }

        public bool HasSearch
        {
            get {
                if (UserProfile != null && UserProfile.SearchProfile != null)
                    return true;
                return (CakeAdvertiser == null) ? false : CakeAdvertiser.HasSearch;
            }
        }

        public SearchProfile SearchProfile
        {
            get { return (UserProfile != null ? UserProfile.SearchProfile : null); }
        }

        public bool ShowSearchChannels
        {
            get { return (SearchProfile == null) ? false : SearchProfile.ShowSearchChannels; }
        }

        public bool UseAnalytics
        {
            get { return (CakeAdvertiser == null) ? false : !String.IsNullOrWhiteSpace(CakeAdvertiser.AnalyticsProfileId); }
        }

        public byte[] Logo
        {
            get { return CakeAdvertiser == null ? null : CakeAdvertiser.Logo; }
        }

        public DateTime? LatestDaySums
        {
            get { return (CakeAdvertiser == null) ? null : CakeAdvertiser.LatestDaySums; }
        }
        public DateTime? LatestClicks
        {
            get { return (CakeAdvertiser == null) ? null : CakeAdvertiser.LatestClicks; }
        }

        public bool UseYesterdayAsLatest
        {
            get { return UserSettings.UseYesterdayAsLatest; }
        }
        public bool Search_UseYesterdayAsLatest
        {
            get { return UserSettings.Search_UseYesterdayAsLatest; }
        }
        public bool TD_UseYesterdayAsLatest
        {
            get { return UserSettings.TD_UseYesterdayAsLatest; }
        }

        public DayOfWeek StartDayOfWeek
        {
            get { return (CakeAdvertiser == null) ? DayOfWeek.Sunday : (DayOfWeek)CakeAdvertiser.StartDayOfWeek; }
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

        public bool HasProgrammatic(bool only = false)
        {
            bool hasProgrammatic = (ProgAdvertiser != null);
            if (only)
                return hasProgrammatic && !HasCake && !HasSearch;
            else
                return hasProgrammatic;
        }

        // --- to be eliminated ---

        public bool HasTradingDesk(bool only = false)
        {
            bool hasTradingDesk = (this.TDAccount != null);
            if (only)
                return hasTradingDesk && !HasCake && !HasSearch;
            else
                return hasTradingDesk;
        }

        public int? InsertionOrderID
        {
            get { return (TDAccount == null) ? null : TDAccount.InsertionOrderID(); }
        }
    }

    public class UserSettings
    {
        public static bool UseYesterdayAsLatest
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
        public static bool Search_UseYesterdayAsLatest
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
        public static bool TD_UseYesterdayAsLatest
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
    }
}