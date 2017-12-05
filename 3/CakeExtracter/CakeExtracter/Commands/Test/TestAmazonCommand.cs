using System;
using System.ComponentModel.Composition;
using Amazon;
using CakeExtracter.Common;

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
            IsCommand("testAmazon");
        }

        //NOTE: Need to add "[STAThread]" to Main method in CakeExtracter.ConsoleApplication

        public override int Execute(string[] remainingArguments)
        {
            var username = "portal@directagents.com";
            var password = "dir3ct@pi!"; 
            var clientId = "amzn1.application-oa2-client.4171f7a48d214b5b859604a1302615fa";
            var clientSecret = "d65e5fe10f136e888277f251042521d4d22494a9bcab5dc6369fc2c21326eca7";

            var amazonAuth = new AmazonAuth(username, password, clientId, clientSecret);
            var tokens = amazonAuth.GetInitialTokens();

            return 0;
        }

        public void TestFillDailySummaries()
        {

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


