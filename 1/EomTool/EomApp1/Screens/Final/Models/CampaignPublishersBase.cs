﻿using System.Linq;
using System;

namespace EomApp1.Screens.Final.Models
{
    public class CampaignPublishersBase
    {
        private bool initialized = false;

        public CampaignPublishersBase(int pid, string currency)
        {
            this.pid = pid;
            this.currency = currency;
        }

        private void Initialize()
        {
            if (!this.initialized)
            {
                using (var db = Eom.Create())
                {
                    var campaign = db.Campaigns.Single(c => c.pid == this.pid);
                    this.campaignName = campaign.campaign_name;
                    this.accountManagerName = campaign.AccountManager.name;
                    this.advertiserName = campaign.Advertiser.name;
                }
                this.initialized = true;
            }
        }

        private int pid;
        public int Pid { get { return this.pid; } }

        private string currency;
        public string Currency { get { return currency; } }

        private string campaignName;
        public string CampaignName { get { Initialize(); return this.campaignName; } }

        private string advertiserName;
        public string AdvertiserName { get { Initialize(); return this.advertiserName; } }

        private string accountManagerName;
        public string AccountManagerName { get { Initialize(); return accountManagerName; } }
    }
}
