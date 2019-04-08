using AutoMapper;
using CakeExtracter.Etl.Kochava.Configuration;
using CakeExtracter.Etl.Kochava.Models;
using DirectAgents.Domain.Entities.CPProg;
using DirectAgents.Domain.Entities.CPProg.Kochava;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;

namespace CakeExtracter.Etl.Kochava.Loaders
{
    /// <summary>
    /// Kochava Data Loader
    /// </summary>
    public class KochavaLoader
    {
        private readonly KochavaItemsDbService kochavaDbService;

        private readonly KochavaConfigurationProvider configurationProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaLoader"/> class.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider.</param>
        /// <param name="kochavaCleaner">The kochava cleaner.</param>
        public KochavaLoader(KochavaConfigurationProvider configurationProvider, KochavaItemsDbService kochavaDbService)
        {
            this.configurationProvider = configurationProvider;
            this.kochavaDbService = kochavaDbService;
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="accountReportData">The accounts report data.</param>
        public void LoadData(List<KochavaReportItem> accountReportData, ExtAccount account)
        {
            try
            {
                CleanKochavaDataForReportingDateRange(account);
                Logger.Info(account.Id, $"Started Loading Kochava Items. Items count = {accountReportData.Count}");
                var dbEntries = GetKochavaDbItems(account, accountReportData);
                kochavaDbService.BulkInsertItems(account.Id, dbEntries);
                Logger.Info(account.Id, $"Finished Loading Kochava Items. Items count = {accountReportData.Count}");
            }
            catch (Exception ex)
            {
                Logger.Error(account.Id, new Exception("Error occured while loading kochava items.", ex));
                throw ex;
            }
        }

        private void CleanKochavaDataForReportingDateRange(ExtAccount account)
        {
            var reportPeriodDaysCount = configurationProvider.GetReportPeriodInDays();
            var fromDate = DateTime.Now.Date.AddDays(-reportPeriodDaysCount);
            var toDate = DateTime.Now.Date;
            kochavaDbService.BulkDeleteItems(account.Id, fromDate, toDate);
        }

        private List<KochavaItem> GetKochavaDbItems(ExtAccount account, List<KochavaReportItem> accountReportData)
        {
            var dbEntries = accountReportData.Select(reportItem =>
            {
                var dbEntity = new KochavaItem();
                Mapper.Map(reportItem, dbEntity);
                dbEntity.AccountId = account.Id;
                return dbEntity;
            });
            return dbEntries.ToList();
        }
    }
}
