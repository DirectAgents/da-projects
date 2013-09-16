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
            get 
            {
                if (Advertiser == null)
                    return "en-US";
                return Advertiser.Culture; 
            }
        }
        public CultureInfo CultureInfo
        {
            get { return String.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); }
        }
        public bool ShowCPMRep
        {
            get 
            {
                if (Advertiser == null)
                    return false;
                return Advertiser.ShowCPMRep;
            }
        }
        public bool ShowConversionData
        {
            get 
            {
                if (Advertiser == null)
                    return false;
                return Advertiser.ShowConversionData; 
            }
        }
        public string ConversionValueName
        {
            get
            {
                if (Advertiser == null)
                    return null;
                return Advertiser.ConversionValueName;
            }
        }
        public bool ConversionValueIsNumber
        {
            get
            {
                if (Advertiser == null)
                    return false;
                return Advertiser.ConversionValueIsNumber; 
            }
        }

        public bool HasSearch
        {
            get 
            {
                if (Advertiser == null)
                    return false;
                return Advertiser.HasSearch; 
            }
        }

        public byte[] Logo
        {
            get 
            { 
                return Advertiser == null ? null : Advertiser.Logo; 
            }
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