using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.PDA.Configuration;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.CommonHelperModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels;
using CakeExtracter.Etl.AmazonSelenium.PDA.PageActions;
using DirectAgents.Domain.Entities.CPProg;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Utilities
{
    public class PdaDataProvider
    {
        private AmazonPdaPageActions pageActions;
        private AuthorizationModel authorizationModel;
        private string downloadDir;
        private Dictionary<string, string> availableProfileUrls;
        private readonly ExtAccount account;

        public PdaDataProvider(ExtAccount account)
        {
            this.account = account;
        }

        public IEnumerable<AmazonCmApiCampaignSummary> GetCampaignApiFullSummaries(DateRange dateRange)
        {
            var cmApiUtility = GetAmazonConsoleManagerUtility();
            var campaignsInfos = cmApiUtility.GetPdaCampaignsSummaries(dateRange, account.Name);
            var campaignType = AmazonApiHelper.GetCampaignTypeName(CampaignType.ProductDisplay);
            campaignsInfos.ForEach(x => x.Type = campaignType);
            return campaignsInfos.ToList();
        }

        private AmazonConsoleManagerUtility GetAmazonConsoleManagerUtility()
        {
            var cookies = pageActions.GetAllCookies();
            var maxRetryAttempts = PdaConfigurationHelper.GetMaxRetryAttempts();
            var pauseBetweenAttempts = PdaConfigurationHelper.GetPauseBetweenAttempts();

            var cmApiUtility = new AmazonConsoleManagerUtility(
                pageActions,
                authorizationModel,
                cookies,
                maxRetryAttempts,
                pauseBetweenAttempts,
                x => Logger.Info(account.Id, x),
                x => Logger.Error(account.Id, new Exception(x)),
                x => Logger.Warn(account.Id, x));
            return cmApiUtility;
        }
    }
}
