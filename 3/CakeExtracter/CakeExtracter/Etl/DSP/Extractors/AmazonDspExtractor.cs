using System;
using System.Collections.Generic;
using Amazon;
using CakeExtracter.Common.Extractors.ArchiveExctractors;
using CakeExtracter.Common.Extractors.ArchiveExctractors.Contract;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Exceptions;
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
        private readonly IArchiveExtractor reportArchiveExtractor;
        private readonly int? accountId;

        private const string DspReportDefaultBucketName = "amazonselfservicedsp";

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<DspFailedEtlException> ProcessFailedExtraction;

        /// <summary>Initializes a new instance of the <see cref="AmazonDspExtractor"/> class.</summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        /// <param name="accountId">The account Id.</param>
        public AmazonDspExtractor(AmazonDspConfigurationProvider configurationProvider, int? accountId)
        {
            this.configurationProvider = configurationProvider;
            this.accountId = accountId;
            awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            reportsParser = new DspReportCsvParser();
            reportComposer = new DspReportDataComposer();
            reportArchiveExtractor = new GzipArchiveExtractor();
        }

        /// <summary>Extracts the daily data.</summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Can't compose report data. DSP report content is null or empty</exception>
        public List<AmazonDspAccountReportData> ExtractDailyData(List<ExtAccount> accounts)
        {
            try
            {
                var reportName = configurationProvider.GetDspReportName();
                var reportContentStream = awsFilesDownloader.GetLatestFileContentByFilePrefix(reportName, DspReportDefaultBucketName);
                if (reportContentStream != null)
                {
                    var reportTextContent = reportArchiveExtractor.TryUnzipStream(reportContentStream);
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
            catch (Exception ex)
            {
                Logger.Error(accountId.HasValue ? accountId.Value : 0, ex);
                ProcessFailedStatsExtraction(ex, accountId);
                return null;
            }
        }

        private void ProcessFailedStatsExtraction(Exception e, int? accountId)
        {
            Logger.Error(e);
            var exception = new DspFailedEtlException(null, null, accountId, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
