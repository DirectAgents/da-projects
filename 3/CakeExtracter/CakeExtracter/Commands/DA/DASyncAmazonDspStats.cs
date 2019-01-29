using CakeExtracter.Common;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands.DA
{
    [Export(typeof(ConsoleCommand))]
    public class DASyncAmazonDspStats : ConsoleCommand
    {
        private readonly AmazonDspExtractor extractor;
        private readonly AmazonDspConfigurationProvider configurationProvider;

        public DASyncAmazonDspStats()
        {
            IsCommand("daSyncAmazonDspStats", "Synch Amazon DSP Stats");
            configurationProvider = new AmazonDspConfigurationProvider();
            extractor = new AmazonDspExtractor(configurationProvider);
        }

        public override int Execute(string[] remainingArguments)
        {
            ProcessDailyEtl();
            return 1;
        }

        private void ProcessDailyEtl()
        {
            var dailyReportData = extractor.ExtractDailyData();
        }

        public override void ResetProperties()
        {
            throw new System.NotImplementedException();
        }
    }
}
