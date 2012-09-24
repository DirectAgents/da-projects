using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ApiClient.Models;
using Common;
using Extensions;

namespace ApiClient.Etl.Cake
{
    public class ConversionsToStaging : IDestination<conversion>
    {
        ISource<conversion> source;
        static int LoadBatchSize = 2000;

        public Thread Load(ISource<conversion> source)
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
                IEnumerable<conversion> conversions = null;
                lock (source.Locker)
                {
                    if (source.Items.Count() > Loaded)
                        conversions = source.Items.Skip(Loaded).ToList();
                }
                if (conversions != null)
                {
                    foreach (var batch in conversions.InSetsOf(LoadBatchSize))
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
            Logger.Log("Conversion loading complete.");
        }

        void LogStatus()
        {
            Logger.Log("{0}/{1}, Done: {2}", Loaded, source.Total, source.Done);
        }

        static void Load(CakeDbContext db, List<conversion> batch)
        {
            Logger.Log("Loading batch of {0}..", batch.Count);
            batch.ForEach(c =>
            {
                Logger.Progress();
                db.Conversions.Add(c);
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
