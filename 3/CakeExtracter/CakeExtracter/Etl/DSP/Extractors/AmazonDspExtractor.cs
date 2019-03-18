using System;
using System.Collections.Generic;
using Amazon;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors.Composer;
using CakeExtracter.Etl.DSP.Extractors.Parser;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DSP.Extractors
{
    /// <summary>Dsp data extractor. Downloads and extracts dsp report data.</summary>
    internal class AmazonDspExtractor
    {
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AwsFilesDownloader awsFilesDownloader;
        private readonly DspReportCsvParser reportsParser;
        private readonly DspReportDataComposer reportComposer;

        /// <summary>Initializes a new instance of the <see cref="AmazonDspExtractor"/> class.</summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        public AmazonDspExtractor(AmazonDspConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            reportsParser = new DspReportCsvParser();
            reportComposer = new DspReportDataComposer();
        }

        /// <summary>Extracts the daily data.</summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Can't compose report data. DSP report content is null or empty</exception>
        public List<AmazonDspAccountReportData> ExtractDailyData(List<ExtAccount> accounts)
        {
            var reportName = configurationProvider.GetDspReportName();
            var reportTextContent = awsFilesDownloader.GetLatestFileContentByFilePrefix(reportName);
            if (!string.IsNullOrEmpty(reportTextContent))
            {
                Logger.Info("DSP. All Data Report downloaded. Size {0}", reportTextContent.Length);
                var reportCreativeEntries = reportsParser.GetReportCreatives(reportTextContent);
                var accountsReportData = reportComposer.ComposeReportData(reportCreativeEntries, accounts);
                Logger.Info("DSP. Report Data Composed.  Advertisers number: {0}", accountsReportData.Count);
                return accountsReportData;
            }
            else
            {
                throw new Exception("Can't compose report data. DSP report content is null or empty");
            }
        }
    }
}
