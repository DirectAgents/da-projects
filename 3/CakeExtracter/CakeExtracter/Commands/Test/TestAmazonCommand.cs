using System;
using System.ComponentModel.Composition;
using Amazon;
using CakeExtracter.Common;
using System.Configuration;
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

            var tokens = Platform.GetPlatformTokens(Platform.Code_Amazon);
            AmazonUtility.TokenSets = tokens;

            ListProfiles();

            tokens = AmazonUtility.TokenSets;
            Platform.SavePlatformTokens(Platform.Code_Amazon, tokens);

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
                System.Console.WriteLine("Profile Name:{0}", profile.AccountInfo.BrandName);
                System.Console.WriteLine("Profile ID:{0}", profile.ProfileId);
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

    }
}


