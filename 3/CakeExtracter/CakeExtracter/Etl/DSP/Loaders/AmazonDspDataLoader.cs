using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Etl.DSP.Exceptions;
using CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Etl.DSP.Loaders
{
    /// <summary>Dsp data loader.</summary>
    internal class AmazonDspDataLoader
    {
        /// <summary>
        /// Action for exception of failed loading.
        /// </summary>
        public event Action<DspFailedEtlException> ProcessFailedLoading;

        /// <summary>Initializes a new instance of the <see cref="AmazonDspDataLoader"/> class.</summary>
        public AmazonDspDataLoader()
        {
        }

        /// <summary>Loads the dsp data.</summary>
        /// <param name="accountsReportData">The accounts report data.
        /// (included data for all advertisers for all dates)</param>
        public void LoadData(List<AmazonDspAccountReportData> accountsReportData)
        {
            try
            {
                Logger.Info("Started Loading Data.");
                accountsReportData.ForEach(accountReportData =>
                {
                    Logger.Info("Loading data for {0} account", accountReportData.Account.Name);
                    LoadAccountData(accountReportData);
                });
                Logger.Info("Finished loading data");
            }
            catch (Exception ex)
            {
                Logger.Warn("DSP: Failed to load dsp data.");
                Logger.Error(ex);
                ProcessFailedStatsLoading(ex, null);
            }
        }

        private void LoadAccountData(AmazonDspAccountReportData accountReportData)
        {
            LoadAdvertisersData(accountReportData.Account, accountReportData.DailyDataCollection);
            LoadOrdersData(accountReportData.Account, accountReportData.DailyDataCollection);
            LoadLineItemsData(accountReportData.Account, accountReportData.DailyDataCollection);
            LoadCreativesData(accountReportData.Account, accountReportData.DailyDataCollection);
        }

        private void LoadAdvertisersData(ExtAccount account, List<AmazonDspDailyReportData> accountDailyData)
        {
            var advertisersDataLoader = new AdvertisersDataLoader();
            var allReportAdvertisers = accountDailyData.SelectMany(ad => ad.Advertisers).ToList();
            var dbAdvertisers = advertisersDataLoader.EnsureDspEntitiesInDataBase(allReportAdvertisers, account);
            accountDailyData.ForEach(dailyData =>
            {
                advertisersDataLoader.UpdateAccountSummaryMetricsDataForDate(dailyData.Advertisers, dbAdvertisers, dailyData.Date, account);
            });
            Logger.Info("DSP, Finished loading advertisers data. Loaded metrics of {0} advertisers", dbAdvertisers.Count);
        }

        private void LoadOrdersData(ExtAccount account, List<AmazonDspDailyReportData> accountDailyData)
        {
            var ordersDataLoader = new OrdersDataLoader();
            var allReportOrders = accountDailyData.SelectMany(ad => ad.Orders).ToList();
            var dbOrders = ordersDataLoader.EnsureDspEntitiesInDataBase(allReportOrders, account);
            accountDailyData.ForEach(dailyData =>
            {
                ordersDataLoader.UpdateAccountSummaryMetricsDataForDate(dailyData.Orders, dbOrders, dailyData.Date, account);
            });
            Logger.Info("DSP, Finished loading orders data. Loaded metrics of {0} orders", dbOrders.Count);
        }

        private void LoadLineItemsData(ExtAccount account, List<AmazonDspDailyReportData> accountDailyData)
        {
            var lineitemsDataLoader = new LineItemsDataLoader();
            var allReportLineItems = accountDailyData.SelectMany(ad => ad.LineItems).ToList();
            var dbLineItems = lineitemsDataLoader.EnsureDspEntitiesInDataBase(allReportLineItems, account);
            accountDailyData.ForEach(dailyData =>
            {
                lineitemsDataLoader.UpdateAccountSummaryMetricsDataForDate(dailyData.LineItems, dbLineItems, dailyData.Date, account);
            });
            Logger.Info("DSP, Finished loading lineitems data. Loaded metrics of {0} lineitems", dbLineItems.Count);
        }

        private void LoadCreativesData(ExtAccount account, List<AmazonDspDailyReportData> accountDailyData)
        {
            var creativesDataLoader = new CreativesDataLoader();
            var allReportCreatives = accountDailyData.SelectMany(ad => ad.Creatives).ToList();
            var dbCreatives = creativesDataLoader.EnsureDspEntitiesInDataBase(allReportCreatives, account);
            accountDailyData.ForEach(dailyData =>
            {
                creativesDataLoader.UpdateAccountSummaryMetricsDataForDate(dailyData.Creatives, dbCreatives, dailyData.Date, account);
            });
            Logger.Info("DSP, Finished loading creatives data. Loaded metrics of {0} creatives", dbCreatives.Count);
        }

        private void ProcessFailedStatsLoading(Exception e, AmazonDspAccountReportData accountReportData)
        {
            Logger.Error(e);
            var exception = new DspFailedEtlException(null, null, accountReportData.Account.Id, e);
            ProcessFailedLoading?.Invoke(exception);
        }
    }
}
