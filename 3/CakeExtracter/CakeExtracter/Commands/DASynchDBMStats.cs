using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.DBM;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchDBMStats : ConsoleCommand
    {
        const int DaysPerReport = 30;

        public int? InsertionOrderID { get; set; }
        public DateTime? EndDate { get; set; }
        public bool UseEarliestDateForUpdate { get; set; }

        public override void ResetProperties()
        {
            InsertionOrderID = null;
            EndDate = null;
            UseEarliestDateForUpdate = false;
        }

        public DASynchDBMStats()
        {
            IsCommand("daSynchDBMStats", "synch DBM Stats");
            HasOption<int>("i|insertionOrderID=", "InsertionOrder ID (default = all)", c => InsertionOrderID = c);
            HasOption<DateTime>("e|endDate=", "End Date (default is yesterday)", c => EndDate = c);
            // Note: endDate is the last day of the desired stats (a report goes back 'DaysPerReport' days)
            HasOption<bool>("u|useEarliest=", "Use earliest stats date for update (default is false)", c => UseEarliestDateForUpdate = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            // Note: The reportDate will be one day after the endDate of the desired stats
            DateTime endDate = EndDate ?? DateTime.Today.AddDays(-1);
            var reportDate = endDate.AddDays(1);
            var dateRange = new DateRange(reportDate, reportDate);

            var insertionOrders = GetInsertionOrders();
            var bucketNames = insertionOrders.Select(i => i.Bucket);

            var extracter = new DbmCloudStorageExtracter(dateRange, bucketNames);
            var loader = new DBMCreativeDailySummaryLoader();
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            var updateStart = endDate.AddDays(1 - DaysPerReport);
            if (UseEarliestDateForUpdate && loader.EarliestDate.HasValue)
                updateStart = loader.EarliestDate.Value;
            var updateDateRange = new DateRange(updateStart, endDate);
            UpdateTDTablesFromDBMTables(updateDateRange, InsertionOrderID);

            return 0;
        }

        public IEnumerable<InsertionOrder> GetInsertionOrders()
        {
            using (var db = new DATDContext())
            {
                var IOs = db.InsertionOrders.AsQueryable();
                if (InsertionOrderID.HasValue)
                    IOs = IOs.Where(io => io.ID == InsertionOrderID.Value);
                return IOs.Where(io => io.Bucket != null).ToList();
            }
        }

        // Note this will update DailySummaries for all insertion orders (Accounts) with CreativeDailySummary stats in the specified range
        public static void UpdateTDTablesFromDBMTables(DateRange dateRange, int? insertionOrderID)
        {
            Logger.Info("Updating Accounts and DailySummaries for dateRange {0:d} to {1:d}", dateRange.FromDate, dateRange.ToDate);

            using (var db = new DATDContext())
            {
                var dbmPlatformId = db.Platforms.Where(p => p.Code == Platform.Code_DBM).First().Id;
                var dbmAccounts = db.Accounts.Where(a => a.PlatformId == dbmPlatformId);
                var dbmExternalIds = dbmAccounts.Select(a => a.ExternalId).ToList();

                // 1) InsertionOrders (with stats in this dateRange) -> Accounts
                var cdSums = db.DBMCreativeDailySummaries.Where(cds => cds.Date >= dateRange.FromDate && cds.Date <= dateRange.ToDate);
                if (insertionOrderID.HasValue)
                    cdSums = cdSums.Where(cds => cds.Creative.InsertionOrderID == insertionOrderID.Value);
                var insertionOrders = cdSums.Select(cds => cds.Creative.InsertionOrder).Distinct().ToList();
                foreach (var io in insertionOrders)
                {
                    if (!dbmExternalIds.Contains(io.ID.ToString()))
                    { // add
                        var newAccount = new Account
                        {
                            PlatformId = dbmPlatformId,
                            ExternalId = io.ID.ToString(),
                            Name = io.Name
                        };
                        db.Accounts.Add(newAccount);
                    }
                    // Note: skipping update
                }
                db.SaveChanges();

                // 2) Add/Update DailySummaries
                foreach (var io in insertionOrders)
                {
                    var accountId = db.Accounts.Where(a => a.ExternalId == io.ID.ToString()).First().Id;

                    var cdSumsForIO = cdSums.Where(cds => cds.Creative.InsertionOrderID == io.ID);
                    var statDates = cdSumsForIO.Select(cds => cds.Date).Distinct().ToList();
                    foreach (var date in dateRange.Dates)
                    {
                        var existingDS = db.DailySummaries.Find(date, accountId);

                        if (!statDates.Contains(date))
                        { // No stats. Make sure there's no td.DailySummary for this date
                            if (existingDS != null)
                                db.DailySummaries.Remove(existingDS);
                        }
                        else
                        { // Add or update tdDailySummary
                            var cds1day = cdSumsForIO.Where(cds => cds.Date == date);
                            var newDS = new DailySummary
                            {
                                Date = date,
                                AccountId = accountId,
                                Impressions = cds1day.Sum(cds => cds.Impressions),
                                Clicks = cds1day.Sum(cds => cds.Clicks),
                                PostClickConv = cds1day.Sum(cds => cds.PostClickConv),
                                PostViewConv = cds1day.Sum(cds => cds.PostViewConv),
                                Cost = cds1day.Sum(cds => cds.Revenue)
                            };

                            if (existingDS == null)
                            {
                                db.DailySummaries.Add(newDS);
                            }
                            else
                            {
                                var entry = db.Entry(existingDS);
                                entry.State = EntityState.Detached;
                                AutoMapper.Mapper.Map(newDS, existingDS);
                                entry.State = EntityState.Modified;
                            }
                        }
                    }
                    db.SaveChanges();
                } //foreach insertionOrder
            } //using db
        }

    }
}
