using System.Collections.Generic;

namespace CakeExtractor.SeleniumApplication.SeleniumExtractors.VCD.VcdExtractionHelpers.UserInfoExtracting.Models
{
    //auto generated class from json value from amazon sales diagnostic page
    public class SubAccount
    {
        public string product { get; set; }
        public string marketplace { get; set; }
        public string name { get; set; }
        public bool spoofed { get; set; }
        public int vendorGroupId { get; set; }
        public string vendorCode { get; set; }
        public int primaryBusinessGroupId { get; set; }
        public string retailSubscriptionLevel { get; set; }
        public string status { get; set; }
        public int mcId { get; set; }
    }

    //auto generated class from json value from amazon sales diagnostic page
    public class UserInfo
    {
        public string id { get; set; }
        public string product { get; set; }
        public string marketplace { get; set; }
        public int activeVendorGroupId { get; set; }
        public int activeMcId { get; set; }
        public List<SubAccount> subAccounts { get; set; }
        public string retailSubscriptionLevel { get; set; }
        public bool spoofingAllowed { get; set; }
        public string spoofedVendorGroupIdsText { get; set; }
        public string spoofedMerchantIdsText { get; set; }
        public string userPortal { get; set; }
    }
}
