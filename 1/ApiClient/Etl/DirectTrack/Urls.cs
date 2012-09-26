
namespace DirectTrack
{
    public class  Urls
    {
        public string Creatives { get { return "{access_id}/creative/campaign/{campaign_id}/"; } }

        public string Campaigns { get { return "{access_id}/campaign/"; } }
        public string ActiveCampaigns { get { return "{access_id}/campaign/active/"; } }
        public string CampaignDetail { get { return "campaign/{campaign_id}"; } }

        public string CampaignGroups { get { return "campaignGroup/"; } }

        public string Affiliates { get { return "{access_id}/affiliate/"; } }
        public string ApprovedAffiliates { get { return "{access_id}/affiliate/approved/"; } }
        public string AffiliateDetail { get { return "affiliate/{affiliate_id}"; } }
        public string EditAffiliate { get { return "affiliate/{affiliate_id}"; } }
        
        public string AffiliateGroups { get { return "{access_id}/affiliateGroup/"; } }

        public string Advertisers { get { return "{access_id}/advertiser/"; } }
        public string AdvertiserGroups { get { return "{access_id}/advertiserGroup/"; } }

        public string ProgramLeads { get { return "{access_id}/programLead/campaign/{campaign_id}/{yyyy}-{mm}-{dd}/"; } }

        public string DetailedLeads { get { return "{access_id}/leadDetail/campaign/{campaign_id}/{yyyy}-{mm}-{dd}/"; } }

        public string DetailedSales { get { return "{access_id}/saleDetail/campaign/{campaign_id}/{yyyy}-{mm}-{dd}/"; } }

        public string CreativeDeployments { get { return "{access_id}/creativeDeployment/"; } }

        public string CampaignCategories { get { return "campaignCategory/"; } }

        public static string Payouts { get { return "{access_id}/payout/campaign/{campaign_id}/"; } }
    }
}
