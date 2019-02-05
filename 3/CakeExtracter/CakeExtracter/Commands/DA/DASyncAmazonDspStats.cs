using CakeExtracter.Common;
using CakeExtracter.Etl.DSP.Configuration;
using CakeExtracter.Etl.DSP.Extractors;
using CakeExtracter.Etl.DSP.Loaders;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace CakeExtracter.Commands.DA
{
    /// <summary>DSP stat syncer job command.</summary>
    [Export(typeof(ConsoleCommand))]
    public class DASyncAmazonDspStats : ConsoleCommand
    {
        private readonly AmazonDspExtractor extractor;
        private readonly AmazonDspConfigurationProvider configurationProvider;
        private readonly AmazonDspDataLoader loader;
        private readonly AmazonDspAccountsProvider accountsProvider;

        private int? AccountId { get; set; }

        /// <summary>Initializes a new instance of the <see cref="DASyncAmazonDspStats"/> class.
        /// Initialises cmd params values.</summary>
        public DASyncAmazonDspStats()
        {
            IsCommand("daSyncAmazonDspStats", "Synch Amazon DSP Stats");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
            configurationProvider = new AmazonDspConfigurationProvider();
            extractor = new AmazonDspExtractor(configurationProvider);
            loader = new AmazonDspDataLoader();
            accountsProvider = new AmazonDspAccountsProvider();
        }

        /// <summary>Resets command properties.</summary>
        public override void ResetProperties()
        {
            AccountId = null;
        }

        /// <summary>Executes the command logic.</summary>
        /// <param name="remainingArguments">The remaining arguments.</param>
        /// <returns>Execution status.</returns>
        public override int Execute(string[] remainingArguments)
        {
            try
            {
                ProcessDailyEtl();
                return 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return 0;
            }
        }

        private void ProcessDailyEtl()
        {
            var accounts = accountsProvider.GetAccountsToProcess(AccountId);
            var reportData = ProcessDailyDataExtraction(accounts);
            if (reportData != null)
            {
                loader.LoadData(reportData);
            }
            else
            {
                Logger.Warn("DSP - Data loading canceled because report data extraction failed");
            }
        }

        private List<AmazonDspAccauntReportData> ProcessDailyDataExtraction(List<ExtAccount> accounts)
        {
            try
            {
                var dailyReportData = extractor.ExtractDailyData(accounts);
                return dailyReportData;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
    }
}
