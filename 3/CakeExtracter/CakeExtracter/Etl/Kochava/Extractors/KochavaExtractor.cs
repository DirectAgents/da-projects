using System;
using System.Collections.Generic;
using Amazon;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.Kochava.Extractors
{
    internal class KochavaExtractor
    {
        private readonly KochavaConfigurationProvider configurationProvider;
        private readonly AwsFilesDownloader awsFilesDownloader;
       
        public KochavaExtractor(KochavaConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            //reportsParser = new DspReportCsvParser();
            //reportComposer = new DspReportDataComposer();
        }

        public List<KochavaReportData> ExtractLatestAccountData(ExtAccount account)
        {
            var reportName = GetReportName(account);
            var reportTextContent = awsFilesDownloader.GetLatestFileContentByFilePrefix(reportName);
            if (!string.IsNullOrEmpty(reportTextContent))
            {
                Logger.Info("Kochava. Report downloaded from s3 bucket. Size {0}", reportTextContent.Length);
                //var reportCreativeEntries = reportsParser.GetReportCreatives(reportTextContent);
                var accountReportData = new List<KochavaReportData>();
                //Logger.Info("DSP. Report Data Composed.  Advertisers number: {0}", accountsReportData.Count);
                return accountReportData;
            }
            else
            {
                throw new Exception("Can't compose report data. Kochava report content is null or empty");
            }
        }

        private string GetReportName(ExtAccount account)
        {
            return string.Empty;
        }
    }
}
