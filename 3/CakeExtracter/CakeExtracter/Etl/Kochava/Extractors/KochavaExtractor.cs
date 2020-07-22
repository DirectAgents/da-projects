using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using CakeExtracter.Common.Extractors.ArchiveExctractors.Contract;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Exceptions;
using CakeExtracter.Etl.Kochava.Extractors.Parsers;
using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Kochava.Extractors
{
    /// <summary>
    /// Kochava Data Extractor.
    /// </summary>
    internal class KochavaExtractor
    {
        private readonly KochavaConfigurationProvider configurationProvider;
        private readonly AwsFilesDownloader awsFilesDownloader;
        private readonly KochavaReportParser reportParser;
        private readonly IArchiveExtractor reportArchiveExtractor;

        private const string KochavaReportPrefixForm = "{0}_query_da";

        /// <summary>
        /// Action for exception of failed extraction.
        /// </summary>
        public event Action<KochavaFailedEtlException> ProcessFailedExtraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaExtractor"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        /// <param name="reportParser">The report parser.</param>
        /// <param name="awsFilesDownloader">The aws files downloader.</param>
        /// <param name="reportArchiveExtractor">The report archive extractor.</param>
        public KochavaExtractor(KochavaConfigurationProvider configurationProvider, KochavaReportParser reportParser,
            AwsFilesDownloader awsFilesDownloader, IArchiveExtractor reportArchiveExtractor)
        {
            this.configurationProvider = configurationProvider;
            this.reportParser = reportParser;
            this.awsFilesDownloader = awsFilesDownloader;
            this.reportArchiveExtractor = reportArchiveExtractor;
        }

        /// <summary>
        /// Extracts the latest account data.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Can't compose report data. Kochava report content is null or empty</exception>
        public List<KochavaReportItem> ExtractLatestAccountData(ExtAccount account)
        {
            try
            {
                var reportPrefixName = GetReportPrefixName(account);
                var reportBucketName = configurationProvider.GetAwsBucketName();
                var reportContentStream = awsFilesDownloader.GetLatestFileContentByFilePrefix(reportPrefixName, reportBucketName);
                if (reportContentStream != null)
                {
                    return ExtractReportContentFromReportStream(account, reportContentStream);
                }
                else
                {
                    throw new Exception("Can't compose report data. Kochava report content is null or empty");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, ex);
                ProcessFailedStatsExtraction(ex, account);
                return null;
            }
        }

        private List<KochavaReportItem> ExtractReportContentFromReportStream(ExtAccount account, Stream reportContentStream)
        {
            var reportTextContent = reportArchiveExtractor.TryUnzipStream(reportContentStream);
            Logger.Info(account.Id, "Kochava. Report downloaded from s3 bucket. Size {0}", reportTextContent.Length);
            var reportEntries = reportParser.ParseKochavaReport(reportTextContent);
            Logger.Info(account.Id, "Kochava. Report data was parsed.  Entries number: {0}", reportEntries.Count);
            return reportEntries;
        }

        private string GetReportPrefixName(ExtAccount account)
        {
            return string.Format(KochavaReportPrefixForm, account.ExternalId);
        }

        private void ProcessFailedStatsExtraction(Exception e, ExtAccount account)
        {
            Logger.Error(e);
            var exception = new KochavaFailedEtlException(null, null, account.Id, e);
            ProcessFailedExtraction?.Invoke(exception);
        }
    }
}
