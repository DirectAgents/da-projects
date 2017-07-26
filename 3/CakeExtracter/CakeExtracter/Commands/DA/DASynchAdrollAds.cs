﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPProg;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdrollAds : ConsoleCommand
    {
        public int? AccountId { get; set; }

        public override void ResetProperties()
        {
            AccountId = null;
        }

        public DASynchAdrollAds()
        {
            IsCommand("daSynchAdrollAds", "synch AdRoll Ads");
            HasOption<int>("a|accountId=", "Account Id (default = all)", c => AccountId = c);
        }

        public override int Execute(string[] remainingArguments)
        {
            using (var db = new ClientPortalProgContext())
            {
                var extAccounts = GetAccounts();

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

        public IEnumerable<ExtAccount> GetAccounts()
        {
            using (var db = new ClientPortalProgContext())
            {
                var accounts = db.ExtAccounts
                    .Where(a => a.Platform.Code == Platform.Code_AdRoll && !a.Disabled);
                if (AccountId.HasValue)
                    return accounts.Where(a => a.Id == AccountId.Value).ToList();
                else
                    return accounts.ToList().Where(a => !string.IsNullOrWhiteSpace(a.ExternalId));
            }
        }
    }
}
