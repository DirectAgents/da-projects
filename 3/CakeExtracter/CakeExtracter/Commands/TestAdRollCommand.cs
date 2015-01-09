using CakeExtracter.Common;
using LocalConnex;
using System;
using System.ComponentModel.Composition;
using AdRoll;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class TestAdRollCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public TestAdRollCommand()
        {
            IsCommand("testAdRoll");
        }

        public override int Execute(string[] remainingArguments)
        {
            var arUtility = new AdRollUtility();
            var advertisableId = "HUULCFSLCVEBHPBXVDCIEN"; // AHS
            var date = DateTime.Today.AddDays(-1);
            var adSummaries = arUtility.AdSummaries(date, advertisableId);

            return 0;
        }
    }
}
