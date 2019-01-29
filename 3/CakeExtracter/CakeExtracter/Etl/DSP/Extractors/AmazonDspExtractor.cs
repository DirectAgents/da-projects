using System.Collections.Generic;
using Amazon;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors.Composer;
using CakeExtracter.Etl.DSP.Extractors.Parser;
using CakeExtracter.Etl.DSP.Models;

namespace CakeExtracter.Etl.DSP.Extractors
{
    internal class AmazonDspExtractor
    {
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AwsDspReportsDownloader reportsDownloader;
        private readonly DspReportCsvParser reportsParser;
        private readonly DspReportDataComposer reportComposer;

        public AmazonDspExtractor(AmazonDspConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            reportsDownloader = new AwsDspReportsDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            reportsParser = new DspReportCsvParser();
            reportComposer = new DspReportDataComposer();
        }

        public List<AmazonDspDailyReportData> ExtractDailyData()
        {
            var reportName = configurationProvider.GetDspReportName();
            var reportTextContent = reportsDownloader.GetLatestDspReportContent(reportName);
            if (!string.IsNullOrEmpty(reportTextContent))
            {
                var reportCreativeEntries = reportsParser.GetReportCreatives(reportTextContent);
                var allDailyReportData = reportComposer.ComposeReportData(reportCreativeEntries);
                return allDailyReportData;
            }
            return null;
        }
    }
}
