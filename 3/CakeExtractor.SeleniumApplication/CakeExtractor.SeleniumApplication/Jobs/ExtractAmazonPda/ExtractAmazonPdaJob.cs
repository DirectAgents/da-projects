using System.Threading.Tasks;
using CakeExtractor.SeleniumApplication.Commands;
using Quartz;

namespace CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda
{
    internal class ExtractAmazonPdaJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var command = context.Scheduler.Context.Get("SyncAmazonPdaCommand") as SyncAmazonPdaCommand;
            //await Task.Factory.StartNew(command.ExtractCampaignsFromExportCsv);
            await Task.Factory.StartNew(command.ExtractCampaignsInfo);
        }
    }
}
