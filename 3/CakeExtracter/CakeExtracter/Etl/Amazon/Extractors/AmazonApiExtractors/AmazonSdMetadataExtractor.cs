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
    public class AmazonSdMetadataExtractor
    {
        protected readonly AmazonUtility AmazonUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSdMetadataExtractor"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        public AmazonSdMetadataExtractor(AmazonUtility amazonUtility)
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
           var sdCampaigns = AmazonUtility.GetCampaigns(CampaignType.ProductDisplay, accountExternalId);
           return sdCampaigns.ToList();
        }
    }
}
