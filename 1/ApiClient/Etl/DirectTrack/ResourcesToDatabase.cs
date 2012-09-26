using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading;
using ApiClient.Models.DirectTrack;
using Common;
using Extensions;

namespace ApiClient.Etl.DirectTrack
{
    public class ResourcesToDatabase : IDestination<DirectTrackResource>
    {
        ISource<DirectTrackResource> source;
        static int LoadBatchSize = 2000;

        public Thread Load(ISource<DirectTrackResource> source)
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
                IEnumerable<DirectTrackResource> itemToLoad = null;
                lock (source.Locker)
                {
                    if (source.Items.Count() > Loaded)
                        itemToLoad = source.Items.Skip(Loaded).ToList();
                }
                if (itemToLoad != null)
                {
                    foreach (var batch in itemToLoad.InSetsOf(LoadBatchSize))
                    {
                        using (var db = new DirectTrackDbContext())
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
            Logger.Log("Loading complete.");
        }

        void LogStatus()
        {
            Logger.Log("{0}/{1}, Done: {2}", Loaded, source.Total, source.Done);
        }

        static void Load(DirectTrackDbContext db, List<DirectTrackResource> batch)
        {
            Logger.Log("Loading batch of {0}..", batch.Count);
            batch.ForEach(c =>
            {
                Logger.Progress();
                db.DirectTrackResources.AddOrUpdate(c);
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
