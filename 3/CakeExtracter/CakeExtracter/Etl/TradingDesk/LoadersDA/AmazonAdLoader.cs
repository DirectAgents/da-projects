using Amazon.Entities;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CakeExtracter.Etl.TradingDesk.LoadersDA
{
    public class AmazonAdLoader : Loader<TDad>
    {
        private readonly int accountId;
        private Dictionary<string, int> _strategyIdLookup = new Dictionary<string, int>(); // by StrategyEid and DB Id
        private Dictionary<string, int> adsetIdLookup = new Dictionary<string, int>(); // by StrategyEid + StrategyName + AdSetEid + AdSetName

        public AmazonAdLoader(int accountId = -1)
        {
            this.accountId = accountId;
        }

        protected override int Load(List<TDad> items)
        {
            Logger.Info("Loading {0} Amazon Ad data:", items.Count);

            AddUpdateAds(items);
            return items.Count;
        }

        private void AddUpdateAds(List<TDad> items)
        {
            using (var db = new ClientPortalProgContext())
            {
                foreach (var item in items)
                {
                    #region add adset to db
                    var tdAd = new TDad
                    {
                        AccountId = this.accountId,
                        ExternalId = item.ExternalId,
                        Name = item.Name
                    };
                    db.TDads.Add(tdAd);
                    db.SaveChanges();
                    Logger.Info("Saved new AdSet: {0} ({1}), ExternalId={2}", tdAd.Name, tdAd.Id, tdAd.ExternalId);

                    #endregion
                }
            }
        }
    }
}
