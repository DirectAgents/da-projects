﻿using System;
using System.Collections.Generic;
using Amazon;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors.Composer;
using CakeExtracter.Etl.DSP.Extractors.Parser;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DSP.Extractors
{
    internal class AmazonDspExtractor
    {
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AwsFilesDownloader awsFilesDownloader;
        private readonly DspReportCsvParser reportsParser;
        private readonly DspReportDataComposer reportComposer;

        public AmazonDspExtractor(AmazonDspConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
            awsFilesDownloader = new AwsFilesDownloader(configurationProvider.GetAwsAccessToken(),
                configurationProvider.GetAwsSecretValue(), m => Logger.Info(m), (ex) => Logger.Error(ex));
            reportsParser = new DspReportCsvParser();
            reportComposer = new DspReportDataComposer();
        }

        public List<AmazonDspAccauntReportData> ExtractDailyData(List<ExtAccount> accounts)
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
