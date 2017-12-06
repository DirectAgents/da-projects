using System;
using System.ComponentModel.Composition;
using Amazon;
using CakeExtracter.Common;
using System.Configuration;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestAmazonCommand : ConsoleCommand
    {
        private AmazonAuth _amazonAuth = null;

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
            string clientId = ConfigurationManager.AppSettings["AmazonClientId"];
            string clientSecret = ConfigurationManager.AppSettings["AmazonClientSecret"];
            string username = ConfigurationManager.AppSettings["AmazonAPIUsername"];
            string password = ConfigurationManager.AppSettings["AmazonAPIPassword"];
            string refreshToken = ConfigurationManager.AppSettings["AmazonRefreshToken"];

            _amazonAuth = new AmazonAuth(username, password, clientId, clientSecret, refreshToken);

            

            ListProfiles();

            return 0;
        }

        public void ListProfiles()
        {
            var amazonUtil = new AmazonUtility();

            //get profiles
            amazonUtil.GetProfiles(); 
            //list profiles
            //foreach (var profile in listOfProfiles)
            //{

            //}
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


