using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Amazon;
using CakeExtracter.Common;
using System.Linq;
using Amazon.Entities;
using Amazon.Enums;
using Amazon.Helpers;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.SimpleRepositories;
using CakeExtracter.SimpleRepositories.Interfaces;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestAmazonCommand : ConsoleCommand
    {
        private readonly ISimpleRepository<EntityType> typeRepository;
        private readonly ISimpleRepository<Strategy> strategyRepository;

        public override void ResetProperties()
        {
            throw new NotImplementedException();
        }

        public TestAmazonCommand()
        {
            IsCommand("TestAmazonCommand");
            typeRepository = new TypeRepository();
            strategyRepository = new StrategyRepository();
        }

        //NOTE: Need to add "[STAThread]" to Main method in CakeExtracter.ConsoleApplication

        public override int Execute(string[] remainingArguments)
        {
            //ListProfiles();
            SetCampaignTypeInStrategy();
            Console.ReadKey();
            return 0;
        }

        public void ListProfiles()
        {
            SetUtilityTokens();
            var amazonUtil = new AmazonUtility();
            var profiles = amazonUtil.GetProfiles();
            profiles.ForEach(PrintProfileInfo);
            SaveUtilityTokens();
        }

        public void SetCampaignTypeInStrategy()
        {
            AddCampaignTypesInDb();
            SetUtilityTokens();
            var accounts = GetAmazonAccounts();
            accounts.ForEach(SetCampaignTypes);
            SaveUtilityTokens();
        }

        private void AddCampaignTypesInDb()
        {
            var types = new List<EntityType>
            {
                new EntityType {Name = AmazonApiHelper.GetCampaignTypeName(CampaignType.SponsoredProducts)},
                new EntityType {Name = AmazonApiHelper.GetCampaignTypeName(CampaignType.SponsoredBrands)}
            };
            typeRepository.AddItems(types);
        }

        private List<ExtAccount> GetAmazonAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts.Where(a => a.Platform.Code == Platform.Code_Amazon && !a.Disabled).ToList();
                var realAccounts = accounts.Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
                return realAccounts.ToList();
            }
        }

        private void SetCampaignTypes(ExtAccount account)
        {
            Logger.Info(account.Id, "Set Amazon campaign types for account - {0} ({1})", account.Name, account.ExternalId);
            var campaigns = GetCampaignsFromApi(account);
            var strategies = campaigns.Select(x => TransformApiCampaignToStrategy(account, x)).ToList();
            UpdateStrategiesInDb(account, strategies);
        }

        private IEnumerable<AmazonCampaign> GetCampaignsFromApi(ExtAccount account)
        {
            var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
            amazonUtility.SetWhichAlt(account.ExternalId);
            var extractor = new AmazonApiCampaignSummaryExtractor(amazonUtility, new DateRange(), account);
            var campaigns = extractor.LoadCampaignsFromAmazonApi();
            return campaigns;
        }

        private Strategy TransformApiCampaignToStrategy(ExtAccount account, AmazonCampaign campaign)
        {
            var campaignType = campaign.CampaignType?.ToLower() == CampaignType.SponsoredProducts.ToString().ToLower()
                ? CampaignType.SponsoredProducts
                : CampaignType.SponsoredBrands;
            var strategyType = new EntityType {Name = AmazonApiHelper.GetCampaignTypeName(campaignType)};
            var strategy = new Strategy
            {
                AccountId = account.Id,
                ExternalId = campaign.CampaignId,
                Name = campaign.Name,
                TypeId = typeRepository.IdStorage.GetEntityIdFromStorage(strategyType)
            };
            return strategy;
        }

        private void UpdateStrategiesInDb(ExtAccount account, IEnumerable<Strategy> strategies)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var strategy in strategies)
                {
                    var itemsInDb = strategyRepository.GetItems(db, strategy);
                    itemsInDb.ForEach(x => TryToUpdateStrategy(db, account, strategy, x));
                }
            }
        }

        private void TryToUpdateStrategy(ClientPortalProgContext db, ExtAccount account, Strategy strategyProps, Strategy strategy)
        {
            var numUpdates = strategyRepository.UpdateItem(db, strategyProps, strategy);
            if (numUpdates <= 0)
            {
                return;
            }

            Logger.Info(account.Id, "Updated Strategy: {0}, Eid = {1}, Type = {2}", strategyProps.Name, strategyProps.ExternalId, strategyProps.TypeId);
            if (numUpdates > 1)
            {
                Logger.Warn(account.Id, "Multiple entities in db ({0})", numUpdates);
            }
        }

        private void PrintProfileInfo(AmazonProfile profile)
        {
            Console.WriteLine($"Profile ID: {profile.ProfileId}");
            Console.WriteLine($"Account ID: {profile.AccountInfo.Id}");
            Console.WriteLine($"Account Name: {profile.AccountInfo.Name}");
            Console.WriteLine();
        }

        private void SetUtilityTokens()
        {
            var tokens = Platform.GetPlatformTokens(Platform.Code_Amazon);
            AmazonUtility.TokenSets = tokens;
        }

        private void SaveUtilityTokens()
        {
            var tokens = AmazonUtility.TokenSets;
            Platform.SavePlatformTokens(Platform.Code_Amazon, tokens);
        }
    }
}