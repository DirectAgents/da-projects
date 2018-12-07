using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Amazon;
using CakeExtracter.Common;
using System.Configuration;
using System.Linq;
using Amazon.Enums;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestAmazonCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestAmazonCommand()
        {
            IsCommand("TestAmazonCommand");
        }

        //NOTE: Need to add "[STAThread]" to Main method in CakeExtracter.ConsoleApplication

        public override int Execute(string[] remainingArguments)
        {
            //string clientId = ConfigurationManager.AppSettings["AmazonClientId"];
            //string clientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
            //string accessCode = ConfigurationManager.AppSettings["AmazonAccessCode"];

            //_amazonAuth = new AmazonAuth(clientId, clientSecret, accessCode);

            //var tokens = Platform.GetPlatformTokens(Platform.Code_Amazon);
            //AmazonUtility.TokenSets = tokens;

            //ListProfiles();

            //tokens = AmazonUtility.TokenSets;
            //Platform.SavePlatformTokens(Platform.Code_Amazon, tokens);

            SetCampaignTypeInStrategy();

            Console.ReadKey();
            return 0;
        }

        public void ListProfiles()
        {
            var amazonUtil = new AmazonUtility();

            //get profiles
            var profiles = amazonUtil.GetProfiles();

            //list profiles
            foreach (var profile in profiles)
            {
                System.Console.WriteLine("Profile ID:{0}", profile.ProfileId);
                System.Console.WriteLine("Account ID:{0}", profile.AccountInfo.Id);
                System.Console.WriteLine("Account Name:{0}", profile.AccountInfo.Name);
            }
        }

        public void TestFillStrategy()
        {

        }
        public void TestFillAdSet()
        {

        }
        public void TestFillCreative()
        {

        }

        public void SetCampaignTypeInStrategy()
        {
            using (var db = new ClientPortalProgContext())
            {
                var strategies = db.Strategies;
                var accounts = strategies.Select(x => x.ExtAccount);

                AmazonUtility.TokenSets = GetTokens();

                foreach (var account in accounts)
                {
                    var amazonUtility = new AmazonUtility(m => Logger.Info(account.Id, m), m => Logger.Warn(account.Id, m));
                    amazonUtility.SetWhichAlt(account.ExternalId);

                    var extracter = new AmazonCampaignSummaryExtracter(new AmazonUtility(), new DateRange(),
                        new ExtAccount { ExternalId = account.ExternalId });

                    var campaigns = extracter.LoadCampaignsFromAmazonApi();
                    if (campaigns.Any(x => !string.IsNullOrEmpty(x.CampaignId)))
                    {
                        foreach (var campaign in campaigns)
                        {
                            var strategy = db.Strategies.FirstOrDefault(x => x.ExternalId == campaign.CampaignId);
                            if (strategy != null)
                            {
                                strategy.CampaignType =
                                    campaign.CampaignType?.ToLower() == CampaignType.SponsoredProducts.ToString().ToLower()
                                        ? CampaignType.SponsoredProducts.ToString()
                                        : CampaignType.SponsoredBrands.ToString();
                            }
                        }
                    }
                }

                db.SaveChanges();
            }
        }

        private string[] GetTokens()
        {
            return Platform.GetPlatformTokens(Platform.Code_Amazon);
        }
    }
}