using System.ComponentModel.Composition;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.Extracters;
using CakeExtracter.Etl.CakeMarketing.Loaders;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class SynchCampaignsCommand : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public SynchCampaignsCommand()
        {
            IsCommand("synchCampaigns", "synch Campaigns");
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new CampaignsExtracter(30530);
            var loader = new CampaignsLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();
            return 0;
        }
    }
}
