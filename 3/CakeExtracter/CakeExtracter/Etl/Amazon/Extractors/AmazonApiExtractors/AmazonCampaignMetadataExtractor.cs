using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Enums;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors
{
    /// <summary>
    /// Amazon campaign metadata extractor
    /// </summary>
    public class AmazonCampaignMetadataExtractor
    {
        private const string SdCompaignMark = "offAmazon";

        protected readonly AmazonUtility AmazonUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonCampaignMetadataExtractor"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        public AmazonCampaignMetadataExtractor(AmazonUtility amazonUtility)
        {
            AmazonUtility = amazonUtility;
        }

        /// <summary>
        /// Loads the campaigns metadata for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="accountExternalId">The account external identifier.</param>
        /// <returns></returns>
        public IEnumerable<AmazonCampaign> LoadCampaignsMetadata(int accountId, string accountExternalId)
        {
            var campaigns = LoadCampaignsFromAmazonApi(accountExternalId);
            return campaigns;
        }

        private IEnumerable<AmazonCampaign> LoadCampaignsFromAmazonApi(string accountExternalId)
        {
            var sbCampaigns = AmazonUtility.GetCampaigns(CampaignType.SponsoredBrands, accountExternalId);
            var spCampaigns = AmazonUtility.GetCampaigns(CampaignType.SponsoredProducts, accountExternalId);
            var campaigns = spCampaigns.Concat(sbCampaigns);
            return campaigns.Where(c => !c.Networks?.Contains(SdCompaignMark) ?? true).ToList();
        }
    }
}
