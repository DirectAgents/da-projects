using Amazon;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Models;

namespace CakeExtracter.Etl.DSP.Extractors
{
    internal class AmazonDspExtractor
    {
        private AmazonDspConfigurationProvider configurationProvider;

        private AwsDspReportsDownloader reportsDownloader;

        private DspReportCsvParser reportsParser;

        public AmazonDspExtractor(AmazonDspConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            reportsDownloader = new AwsDspReportsDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            reportsParser = new DspReportCsvParser();
        }

        public AmazonDspReportData ExtractDailyData()
        {
            var reportName = configurationProvider.GetDspReportName();
            var reportTextContent = reportsDownloader.GetLatestDspReportContent(reportName);
            if (!string.IsNullOrEmpty(reportTextContent))
            {
                var reportCreativeEntries = reportsParser.GetReportCreatives(reportTextContent);
                return new AmazonDspReportData();
            }
            return null;
        }
    }
}
