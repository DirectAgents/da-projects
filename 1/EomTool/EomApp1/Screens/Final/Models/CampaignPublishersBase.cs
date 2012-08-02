using System.Linq;
using System;

namespace EomApp1.Screens.Final.Models
{
    public class CampaignPublishersBase
    {
        private bool initialized = false;

        public CampaignPublishersBase(int pid)
        {
            this.pid = pid;
        }

        private void Initialize()
        {
            if (!this.initialized)
            {
                using (var db = Eom.Create())
                {
                    var campaign = db.Campaigns.Single(c => c.pid == this.pid);
                    this.campaignName = campaign.campaign_name;
                    this.advertiserName = campaign.Advertiser.name;
                }
                this.initialized = true;
            }
        }

        private int pid;
        public int Pid { get { return this.pid; } }

        private string campaignName;
        public string CampaignName { get { Initialize(); return this.campaignName; } }

        private string advertiserName;
        public string AdvertiserName { get { Initialize(); return this.advertiserName; } }
    }
}
