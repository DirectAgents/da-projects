﻿using System.ComponentModel.Composition;
using CakeExtracter.Bootstrappers;
using CakeExtracter.Common;
using CakeExtracter.Etl.CakeMarketing.DALoaders;
using CakeExtracter.Etl.CakeMarketing.Extracters;

namespace CakeExtracter.Commands
{
    [Export(typeof(ConsoleCommand))]
    public class DASynchAffiliates : ConsoleCommand
    {
        public static int RunStatic(int affiliateId)//, bool includeContacts)
        {
            AutoMapperBootstrapper.CheckRunSetup();
            var cmd = new DASynchAffiliates
            {
                AffiliateId = affiliateId,
                //IncludeContacts = includeContacts,
            };
            return cmd.Run();
        }

        public int AffiliateId { get; set; }
        //public bool IncludeContacts { get; set; }

        public override void ResetProperties()
        {
            AffiliateId = 0;
            //IncludeContacts = false;
        }

        public DASynchAffiliates()
        {
            IsCommand("daSynchAffiliates", "synch Cake Affiliates");
            HasOption<int>("a|affiliateId=", "Affiliate Id (0 = all (default))", c => AffiliateId = c);
            //HasOption("c|contacts=", "synch Contacts also (default is false)", c => IncludeContacts = bool.Parse(c));
        }

        public override int Execute(string[] remainingArguments)
        {
            var extracter = new AffiliatesExtracter(new int[] { AffiliateId });
            var loader = new DAAffiliatesLoader();// IncludeContacts);
            var extracterThread = extracter.Start();
            var loaderThread = loader.Start(extracter);
            extracterThread.Join();
            loaderThread.Join();

            return 0;
        }
    }
}
