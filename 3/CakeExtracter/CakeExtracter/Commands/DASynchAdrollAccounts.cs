using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AdRoll;
using CakeExtracter.Common;
using CakeExtracter.Etl.TradingDesk.Extracters;
using CakeExtracter.Etl.TradingDesk.LoadersDA;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.AdRoll;
using DirectAgents.Domain.Entities.TD;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAdrollAccounts : ConsoleCommand
    {
        public override void ResetProperties()
        {
        }

        public DASynchAdrollAccounts()
        {
            IsCommand("daSynchAdrollAccounts", "synch AdRoll Accounts(Advertisables)");
        }

        public override int Execute(string[] remainingArguments)
        {
            var arUtility = new AdRollUtility(m => Logger.Info(m), m => Logger.Warn(m));
            var freshAdvertisables = arUtility.GetAdvertisables();
            using (var db = new DATDContext())
            {
                var dbAdvertisables = db.Advertisables.ToList();
                var dbAdvEids = dbAdvertisables.Select(a => a.Eid).ToArray();

                var platformId_AdRoll = db.Platforms.Where(p => p.Code == Platform.Code_AdRoll).First().Id;
                var dbAccounts = db.Accounts.Where(a => a.PlatformId == platformId_AdRoll).ToList();
                var dbAcctEids = dbAccounts.Select(a => a.ExternalId).ToArray();

                foreach (var adv in freshAdvertisables)
                {
                    // Check/update adr.Advertisable table
                    if (!dbAdvEids.Contains(adv.eid))
                    { // add
                        Logger.Info("Adding new Advertisable '{0}' ({1})", adv.name, adv.eid);
                        var newAdv = new Advertisable
                        {
                            Eid = adv.eid,
                            Name = adv.name
                        };
                        db.Advertisables.Add(newAdv);
                    }
                    else
                    { // update
                        var dbAdv = dbAdvertisables.First(a => a.Eid == adv.eid);
                        dbAdv.Name = adv.name;
                    }

                    // Check/update td.Account table
                    if (!dbAcctEids.Contains(adv.eid))
                    { // add
                        Logger.Info("Adding new Account '{0}' ({1})", adv.name, adv.eid);
                        var newAcct = new Account
                        {
                            PlatformId = platformId_AdRoll,
                            ExternalId = adv.eid,
                            Name = adv.name
                        };
                        db.Accounts.Add(newAcct);
                    }
                    else
                    { // update
                        var dbAcct = dbAccounts.First(a => a.ExternalId == adv.eid);
                        dbAcct.Name = adv.name;
                    }
                }
                db.SaveChanges();
            }
            return 0;
        }
    }
}
