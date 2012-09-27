using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiClient.Models.Cake;
using System.Threading;
using Common;
using Extensions;

namespace ApiClient.Etl.Cake
{
    public class DailySummariesToDatabase : IDestination<DailySummary>
    {
        ISource<DailySummary> source;
        static int LoadBatchSize = 2000;

        public Thread Load(ISource<DailySummary> source)
        {
            this.source = source;
            var thread = new Thread(DoLoad);
            thread.Start();
            return thread;
        }

        void DoLoad()
        {
            while ((!source.Done) || (source.Total != Loaded))
            {
                IEnumerable<DailySummary> dailySummaries = null;
                lock (source.Locker)
                {
                    if (source.Items.Count() > Loaded)
                        dailySummaries = source.Items.Skip(Loaded).ToList();
                }
                if (dailySummaries != null)
                {
                    foreach (var batch in dailySummaries.InSetsOf(LoadBatchSize))
                    {
                        using (var db = new CakeDbContext())
                        {
                            Load(db, batch);
                            Logger.Log("Saving..");
                            db.SaveChanges();
                            Logger.Log("Saved.");
                            Loaded += batch.Count();
                        }
                    }
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
            Logger.Log("DailySummary loading complete.");
        }

        void LogStatus()
        {
            Logger.Log("{0}/{1}, Done: {2}", Loaded, source.Total, source.Done);
        }

        static void Load(CakeDbContext db, List<DailySummary> batch)
        {
            Logger.Log("Loading batch of {0}..", batch.Count);
            batch.ForEach(d =>
            {
                Logger.Progress();
                db.DailySummaries.Add(d);
            });
            Logger.Done();
        }

        int loaded = 0;
        public int Loaded
        {
            get { return loaded; }
            set
            {
                loaded = value;
                LogStatus();
            }
        }
    }
}
