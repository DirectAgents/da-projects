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
        private ISource<conversion> source;
        static int LoadBatchSize = 100;

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
                    using (var db = new CakeDbContext())
                    {
                        foreach (var batch in conversions.InSetsOf(LoadBatchSize))
                        {
                            Load(db, batch);
                            db.SaveChanges();
                            Loaded += batch.Count();
                        }
                    }
                }
                else
                {
                    Thread.Sleep(250);
                }
                Logger.Log("{0}/{1}, Done: {2}", Loaded, source.Total, source.Done);
            }
            System.Console.WriteLine("here");
        }

        private static void Load(CakeDbContext db, List<conversion> batch)
        {
            Logger.Log("Loading batch of {0}..", batch.Count);
            batch.ForEach(c =>
            {
                var existing = db.Conversions.Where(d => d.conversion_id == c.conversion_id);

                if (existing.SingleOrDefault() != null)
                    db.Conversions.Remove(existing.First());

                db.Conversions.Add(c);
            });     
        }

        public int Loaded { get; set; }
    }
}
