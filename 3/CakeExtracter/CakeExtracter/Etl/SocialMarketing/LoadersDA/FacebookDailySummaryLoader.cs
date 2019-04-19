using CakeExtracter.Common;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg.Facebook.Daily;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookDailySummaryLoader : Loader<FbDailySummary>
    {
        private readonly DateRange dateRange;

        private static object lockObj = new object();

        private List<FbDailySummary> latestSummaries = new List<FbDailySummary>();

        public FacebookDailySummaryLoader(int accountId, DateRange dateRange)
            : base(accountId)
        {
            this.dateRange = dateRange;
        }

        protected override int Load(List<FbDailySummary> items)
        {
            latestSummaries.AddRange(items);
            return items.Count;
        }

        protected override void AfterLoadAction()
        {
            DeleteOldSummariesFromDb();
            LoadLatestSummariesToDb(latestSummaries);
        }

        private void LoadSummaries(List<FbDailySummary> summaries)
        {
            latestSummaries.AddRange(summaries);
        }

        private void DeleteOldSummariesFromDb()
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.FbDailySummaries.Where(x => (x.Date >= dateRange.FromDate && x.Date <= dateRange.ToDate)
                    && x.AccountId == accountId).DeleteFromQuery();
            }, lockObj, "BulkDeleteByQuery");
        }

        private void LoadLatestSummariesToDb(List<FbDailySummary> summaries)
        {
            SafeContextWrapper.TryMakeTransactionWithLock((ClientPortalProgContext db) =>
            {
                db.BulkInsert(summaries);
            }, lockObj, "BulkInsert");
        }
    }
}
