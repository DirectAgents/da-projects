using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Etl.Amazon.Exceptions;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors;
using CakeExtracter.Helpers;
using CakeExtracter.Logging.TimeWatchers.Amazon;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Amazon.Extractors.AmazonApiExtractors
{
    public class AmazonSponsoredDisplayMetadataExtractor
    {
        protected readonly AmazonUtility AmazonUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonSponsoredDisplayMetadataExtractor"/> class.
        /// </summary>
        /// <param name="amazonUtility">The amazon utility.</param>
        public AmazonSponsoredDisplayMetadataExtractor(AmazonUtility amazonUtility)
        {
            AmazonUtility = amazonUtility;
        }

        /// <summary>
        /// Loads the campaigns metadata for account.
        /// </summary>
        /// <param name="accountId">The account identifier.</param>
        /// <param name="accountExternalId">The account external identifier.</param>
        /// <returns></returns>
        public IEnumerable<AmazonCampaign> LoadSponsoredDisplayMetadata(int accountId, string accountExternalId)
        {
            var campaigns = LoadSponsoredDisplayFromAmazonApi(accountExternalId);
            return campaigns;
        }
        
        private IEnumerable<AmazonCampaign> LoadSponsoredDisplayFromAmazonApi(string accountExternalId)
        {
            var sdCampaigns = AmazonUtility.GetCampaigns(CampaignType.ProductDisplay, accountExternalId);
            var campaigns = sdCampaigns;
            return campaigns.ToList();
        }
    }
}
