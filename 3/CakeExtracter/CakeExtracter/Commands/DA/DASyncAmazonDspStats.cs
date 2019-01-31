using CakeExtracter.Common;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors;
using CakeExtracter.Etl.DSP.Loaders;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASyncAmazonDspStats : ConsoleCommand
    {
        private readonly AmazonDspExtractor extractor;
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AmazonDspDataLoader loader;
        private readonly AmazonDspAccountsProvider accountsProvider;

        public DASyncAmazonDspStats()
        {
            IsCommand("daSyncAmazonDspStats", "Synch Amazon DSP Stats");
            configurationProvider = new AmazonDspConfigurationProvider();
            extractor = new AmazonDspExtractor(configurationProvider);
            loader = new AmazonDspDataLoader();
            accountsProvider = new AmazonDspAccountsProvider();
        }

        public override int Execute(string[] remainingArguments)
        {
            ProcessDailyEtl();
            return 1;
        }

        private void ProcessDailyEtl()
        {
            var accounts = accountsProvider.GetAccountsToProcess(0);
            var dailyReportData = extractor.ExtractDailyData(accounts);
            loader.LoadData(dailyReportData);
        }

        public override void ResetProperties()
        {
            throw new System.NotImplementedException();
        }
    }
}
