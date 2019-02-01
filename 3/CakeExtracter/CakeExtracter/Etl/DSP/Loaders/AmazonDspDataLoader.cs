using CakeExtracter.Etl.DSP.Loaders.ReportEntriesDataLoaders;
using CakeExtracter.Etl.DSP.Models;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.DSP.Loaders
{
    internal class AmazonDspDataLoader
    {
        public AmazonDspDataLoader()
        {
        }

        public void LoadData(List<AmazonDspAccauntReportData> accountsReportData)
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
                throw ex;
            }
        }

        private void LoadAccountData(AmazonDspAccauntReportData accountReportData)
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
            Logger.Info("Amazon VCD, Finished loading advertisers data. Loaded metrics of {0} advertisers", dbAdvertisers.Count);
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
            Logger.Info("Amazon VCD, Finished loading orders data. Loaded metrics of {0} orders", dbOrders.Count);
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
            Logger.Info("Amazon VCD, Finished loading lineitems data. Loaded metrics of {0} lineitems", dbLineItems.Count);
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
            Logger.Info("Amazon VCD, Finished loading lineitems data. Loaded metrics of {0} lineitems", dbCreatives.Count);
        }
    }
}
