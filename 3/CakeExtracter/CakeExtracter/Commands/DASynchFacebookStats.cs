using System.ComponentModel.Composition;
using CakeExtracter.Common;
using FacebookAPI;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchFacebookStats : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public DASynchFacebookStats()
        {
            IsCommand("daSynchFacebookStats", "synch Facebook stats");
        }

        public override int Execute(string[] remainingArguments)
        {
            var fbUtility = new FacebookUtility(m => Logger.Info(m), m => Logger.Warn(m));
            fbUtility.Test();

            return 0;
        }
    }
}
