using System;
using System.Linq;
using DAgents.Common;
using EomApp1.Cake.WebServices._4.Export;
using EomApp1.Screens.Synch.Services.Cake;
using System.Data.Objects;

namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class Affiliate
    {
        public Affiliate()
        {
        }

        public Affiliate(EomDatabaseEntities eom, int cakeAffiliateID, CakeWebService cakeService, ILogger logger)
        {
            logger.Log("Creating new Affiliate in database from cakeAffiliateID " + cakeAffiliateID + "...");
            var affiliate = cakeService.AffiliateById(cakeAffiliateID);
            this.affid = affiliate.affiliate_id;
            Populate(eom, this, affiliate);
            AttachTo(eom, this);
        }

        public static Affiliate CreateFromCakeWithSafeId(EomDatabaseEntities eom, int cakeAffiliateID)
        {
            var affiliate = new Affiliate();
            var cakeService = new CakeWebService(new DAgents.Common.Utilities.NullLogger()); // todo: use real logger in the add affiliate setup dialog
            var extracted = cakeService.AffiliateById(cakeAffiliateID);
            affiliate.affid = SafeId(eom, extracted);
            Populate(eom, affiliate, extracted);
            affiliate.tracking_system_id = eom.TrackingSystems.Cake().id;
            AttachTo(eom, affiliate);
            return affiliate;
        }

        private static void Populate(EomDatabaseEntities eom, Affiliate affiliate, affiliate extracted)
        {
            affiliate.name = extracted.affiliate_name;
            affiliate.media_buyer_id = eom.MediaBuyers.IdOrCreate(extracted);
            affiliate.add_code = "CA" + extracted.affiliate_id;
            affiliate.currency_id = eom.Currencies.IdByName(extracted.Currency);
            affiliate.email = extracted.contacts[0].email_address;
            affiliate.net_term_type_id = eom.NetTermTypes.IdByName("Net 30");
            affiliate.payment_method_id = eom.AffiliatePaymentMethods.IdByName("default");
        }

        private static void AttachTo(EomDatabaseEntities eom, Affiliate affiliate)
        {
            using (var context = EomDatabaseEntities.Create())
            {
                context.Affiliates.AddObject(affiliate);
                context.SaveChanges();
                context.Detach(affiliate);
            }
            eom.Attach(affiliate);
        }

        private static int SafeId(EomDatabaseEntities eom, affiliate extracted)
        {
            int safeID = FirstSafeId + extracted.affiliate_id;

            var query = from c in eom.Affiliates
                        where c.affid == safeID
                        select c.affid;

            if (query.Any())
            {
                throw new Exception("Cannot find safe ID for affiliate ID " + extracted.affiliate_id);
            }

            return safeID;
        }

        private static int FirstSafeId { get { return 500000; } }

        private static int IncrementForNextSafeId { get { return 100000; } }
    }

    public static class AffiliateExtensions
    {
        public static int IdOrCreate(this ObjectSet<Affiliate> affiliates, int cakeAffiliateID, CakeWebService cakeService, ILogger logger)
        {
            Affiliate affiliate;

            // Match tracking system and external id.
            affiliate = (from c in affiliates
                         where c.external_id == cakeAffiliateID && c.TrackingSystem.name == "Cake Marketing"
                         select c).SingleOrDefault();

            if (affiliate == null)
            {
                // Match affid.
                affiliate = (from c in affiliates
                             where c.affid == cakeAffiliateID
                             select c).SingleOrDefault();

                if (affiliate == null)
                {
                    // Create new affiliate.
                    affiliate = new Affiliate(affiliates.Context as EomDatabaseEntities, cakeAffiliateID, cakeService, logger);
                }
            }

            return affiliate.affid;
        }
    }
}
