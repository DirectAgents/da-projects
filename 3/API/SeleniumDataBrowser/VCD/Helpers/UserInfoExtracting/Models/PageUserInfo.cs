using System.Collections.Generic;

namespace SeleniumDataBrowser.VCD.Helpers.UserInfoExtracting.Models
{
    public class PageUserInfo
    {
        public string id { get; set; }
        public string product { get; set; }
        public string marketplace { get; set; }
        public int activeVendorGroupId { get; set; }
        public int activeMcId { get; set; }
        public List<PageAccountInfo> subAccounts { get; set; }
        public string retailSubscriptionLevel { get; set; }
        public bool spoofingAllowed { get; set; }
        public string spoofedVendorGroupIdsText { get; set; }
        public string spoofedMerchantIdsText { get; set; }
        public string userPortal { get; set; }
    }
}
