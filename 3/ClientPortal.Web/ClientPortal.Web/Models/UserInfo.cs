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

        // TODO: convert to getting the following from Advertiser instead of UserProfile
        public string Culture
        {
            get { return UserProfile.Culture; }
        }
        public CultureInfo CultureInfo
        {
            get { return String.IsNullOrWhiteSpace(Culture) ? CultureInfo.InvariantCulture : CultureInfo.CreateSpecificCulture(Culture); }
        }
        public bool ShowCPMRep
        {
            get { return UserProfile.ShowCPMRep; }
        }
        public bool ShowConversionData
        {
            get { return UserProfile.ShowConversionData; }
        }
        public string ConversionValueName
        {
            get { return UserProfile.ConversionValueName; }
        }
        public bool ConversionValueIsNumber
        {
            get { return UserProfile.ConversionValueIsNumber; }
        }

        public byte[] Logo
        {
            get { return Advertiser == null ? null : Advertiser.Logo; }
        }

    }
}