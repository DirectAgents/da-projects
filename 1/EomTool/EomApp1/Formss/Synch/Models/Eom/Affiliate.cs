using DAgents.Common;
using EomApp1.Formss.Synch.Services.Cake;
namespace EomApp1.Formss.Synch.Models.Eom
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

            this.name = affiliate.affiliate_name;
            this.media_buyer_id = eom.MediaBuyers.GetOrCreate(affiliate);
            this.affid = affiliate.affiliate_id;
            this.add_code = "CA" + affiliate.affiliate_id;
            this.currency_id = eom.Currencies.IdByName(affiliate.Currency);
            this.email = affiliate.contacts[0].email_address;
            this.net_term_type_id = eom.NetTermTypes.IdByName("Net 30");
            this.payment_method_id = eom.AffiliatePaymentMethods.IdByName("default");

            using (var context = EomDatabaseEntities.Create())
            {
                context.Affiliates.AddObject(this);
                context.SaveChanges();
                context.Detach(this);
            }
            eom.Attach(this);
        }
    }
}
