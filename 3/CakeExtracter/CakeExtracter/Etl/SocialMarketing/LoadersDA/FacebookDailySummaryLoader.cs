using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using FacebookAPI;
using FacebookAPI.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace CakeExtracter.Etl.SocialMarketing.LoadersDA
{
    public class FacebookDailySummaryLoader : Loader<FBSummary>
    {
        public FacebookDailySummaryLoader(int accountId)
            : base(accountId)
        {
            BatchSize = FacebookUtility.RowsReturnedAtATime; //FB API only returns 25 rows at a time
        }

        protected override int Load(List<FBSummary> items)
        {
            Logger.Info(accountId, "Loading {0} Facebook DailySummaries..", items.Count);
            var count = UpsertDailySummaries(items);
            return count;
        }

        private int UpsertDailySummaries(IEnumerable<FBSummary> items)
        {
            var progress = new LoadingProgress();
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    var source = new DailySummary
                    {
                        AccountId = accountId,
                        Date = item.Date,
                        Impressions = item.Impressions,
                        Clicks = item.LinkClicks,
                        AllClicks = item.AllClicks,
                        PostClickConv = item.Conversions_click,
                        PostViewConv = item.Conversions_view,
                        PostClickRev = item.ConVal_click,
                        PostViewRev = item.ConVal_view,
                        Cost = item.Spend
                    };
                    var target = db.Set<DailySummary>().Find(item.Date, accountId);
                    if (target == null)
                    {
                        if (!item.AllZeros())
                        {
                            db.DailySummaries.Add(source);
                            progress.AddedCount++;
                        }
                        else
                        {
                            progress.AlreadyDeletedCount++;
                        }
                    }
                    else // DailySummary already exists
                    {
                        var entry = db.Entry(target);
                        if (entry.State == EntityState.Unchanged)
                        {
                            if (!item.AllZeros())
                            {
                                entry.State = EntityState.Detached;
                                AutoMapper.Mapper.Map(source, target);
                                entry.State = EntityState.Modified;
                                progress.UpdatedCount++;
                            }
                            else
                            {
                                entry.State = EntityState.Deleted;
                            }
                        }
                        else
                        {
                            Logger.Warn(accountId, "Encountered duplicate DailySummary for {0:d}", item.Date);
                            progress.DuplicateCount++;
                        }
                    }

                    progress.ItemCount++;
                }

                db.SaveChanges();
            }

            Logger.Info(accountId, "Saving {0} DailySummaries ({1} updates, {2} additions, {3} duplicates, {4} deleted, {5} already-deleted)",
                progress.ItemCount, progress.UpdatedCount, progress.AddedCount, progress.DuplicateCount, progress.DeletedCount, progress.AlreadyDeletedCount);
            if (progress.DuplicateCount > 0)
            {
                Logger.Warn(accountId, "Encountered {0} duplicates which were skipped", progress.DuplicateCount);
            }
            return progress.ItemCount;
        }
    }
}
