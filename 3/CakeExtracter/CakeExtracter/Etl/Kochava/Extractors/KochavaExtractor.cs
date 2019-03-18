using System;
using System.Collections.Generic;
using Amazon;
using CakeExtracter.Common.ArchiveExtractors.Contract;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Extractors.Parsers;
using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Kochava.Extractors
{
    /// <summary>
    /// Kochava Data Extractor
    /// </summary>
    internal class KochavaExtractor
    {
        private readonly KochavaConfigurationProvider configurationProvider;
        private readonly AwsFilesDownloader awsFilesDownloader;
        private readonly KochavaReportParser reportParser;
        private readonly IArchiveExtractor reportArchiveExtractor;

        private const string KochavaReportPrefixForm = "{0}_quey_da";

        public KochavaExtractor(KochavaConfigurationProvider configurationProvider, KochavaReportParser reportParser,
            AwsFilesDownloader awsFilesDownloader, IArchiveExtractor reportArchiveExtractor)
        {
            this.configurationProvider = configurationProvider;
            this.reportParser = reportParser;
            this.awsFilesDownloader = awsFilesDownloader;
            this.reportArchiveExtractor = reportArchiveExtractor;
        }

        public List<KochavaReportItem> ExtractLatestAccountData(ExtAccount account)
        {
            var reportPrefixName = GetReportPrefixName(account);
            var reportBucketName = configurationProvider.GetAwsBucketName();
            var reportContentStream = awsFilesDownloader.GetLatestFileContentByFilePrefix(reportPrefixName, reportBucketName);
            if (reportContentStream != null)
            {
                var reportTextContent = reportArchiveExtractor.TryUnzipStream(reportContentStream);
                Logger.Info(account.Id, "Kochava. Report downloaded from s3 bucket. Size {0}", reportTextContent.Length);
                var reportEntries = reportParser.ParseKochavaReport(reportTextContent);
                Logger.Info(account.Id, "Kochava. Report data was parsed.  Entries number: {0}", reportEntries.Count);
                return reportEntries;
            }
            else
            {
                throw new Exception("Can't compose report data. Kochava report content is null or empty");
            }
        }

        private string GetReportPrefixName(ExtAccount account)
        {
            return string.Format(KochavaReportPrefixForm, account.ExternalId);
        }
    }
}
