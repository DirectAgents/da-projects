using System;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdrollAds : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public DASynchAdrollAds()
        {
            IsCommand("daSynchAdrollAds", "synch AdRoll Ads");
        }

        public override int Execute(string[] remainingArguments)
        {
            using (var db = new DATDContext())
            {
                var platformId_AdRoll = db.Platforms.Where(p => p.Code == Platform.Code_AdRoll).First().Id;
                var extAccounts = db.ExtAccounts.Where(a => a.PlatformId == platformId_AdRoll && !String.IsNullOrEmpty(a.ExternalId));
                foreach (var extAcct in extAccounts)
                {
                    Logger.Info("Synch Ads for Account: {0} ({1})", extAcct.Name, extAcct.ExternalId);
                    var tdAds = db.TDads.Where(a => a.AccountId == extAcct.Id);
                    var adEids = tdAds.Select(a => a.ExternalId).ToArray();

                    var adExtracter = new AdRollAdExtracter(adEids);
                    var adLoader = new AdRollAdLoader(extAcct.Id);
                    var adExtracterThread = adExtracter.Start();
                    var adLoaderThread = adLoader.Start(adExtracter);
                    adExtracterThread.Join();
                    adLoaderThread.Join();
                }
            }
            return 0;
        }
    }
}
