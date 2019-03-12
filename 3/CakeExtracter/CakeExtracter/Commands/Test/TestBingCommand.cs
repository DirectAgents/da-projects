using System;
using System.ComponentModel.Composition;
using System.Configuration;
using BingAds;
using CakeExtracter.Common;

namespace CakeExtracter.Commands.Test
{
    [Export(typeof(ConsoleCommand))]
    public class TestBingCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestBingCommand()
        {
            IsCommand("testBing", "The command for refreshing of token (Bing API)");
        }

        public override int Execute(string[] remainingArguments)
        {
            //NOTE: Need to fill user name and password in config
            var username = ConfigurationManager.AppSettings["BingApiUsername"];
            var password = ConfigurationManager.AppSettings["BingApiPassword"];
            var clientId = ConfigurationManager.AppSettings["BingClientId"];

            Console.WriteLine($"Bing API User name: {username}, Bing client Id: {clientId}");

            var bingAuth = new BingAuth(username, password, clientId);
            var tokens = bingAuth.GetInitialTokens();

            Console.WriteLine($"Result Bing refresh token (need to add in config): {tokens.RefreshToken}");
            return 0;
        }
    }
}
