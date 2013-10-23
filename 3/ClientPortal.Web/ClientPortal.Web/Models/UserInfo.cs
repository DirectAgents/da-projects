using ClientPortal.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ClientPortal.Web.Models
{
    public class UserInfo
    {
        public UserInfo(UserProfile userProfile, Advertiser advertiser)
        {      
            this.UserProfile = userProfile;
            this.Advertiser = advertiser;
        }

        private UserProfile UserProfile { get; set; }
        private Advertiser Advertiser { get; set; }

        public bool HasUserProfile
        {
            get { return (UserProfile != null); }
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

        public bool UseAnalytics
        {
            get { return (Advertiser == null) ? false : !String.IsNullOrWhiteSpace(Advertiser.AnalyticsProfileId); }
        }

        public byte[] Logo
        {
            get { return Advertiser == null ? null : Advertiser.Logo; }
        }

        public Tuple<DayOfWeek, DayOfWeek> SearchWeekDays
        {
            get
            {
                var firstDayOfWeek = (DayOfWeek)this.UserProfile.SearchWeekStartDay;
                var secondDayOfWeek = (DayOfWeek)this.UserProfile.SearchWeekEndDay;
                return Tuple.Create(firstDayOfWeek, secondDayOfWeek);
            }
        }

    }
}