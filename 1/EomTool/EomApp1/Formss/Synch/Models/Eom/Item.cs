using System.Linq;
using DAgents.Common;
using EomApp1.Formss.Synch.Models.Cake;
using EomApp1.Formss.Synch.Services.Cake;
namespace EomApp1.Formss.Synch.Models.Eom
{
    public partial class Item
    {
        public void Update(EomDatabaseEntities eomEntities, CakeConversionSummary conversion, CakeWebService cakeService, ILogger logger)
        {
            if (this.id == 0)
            {
                this.name = conversion.Name;
                this.notes = "From Cake";
                this.accounting_notes = "From Cake";
                this.item_accounting_status_id = 1;
                this.item_reporting_status_id = 1;
                this.source_id = eomEntities.Sources.Where(c => c.name == "Cake").Select(c => c.id).Single();
            }
            this.unit_type_id = eomEntities.UnitTypes.Where(c => c.name == conversion.ConversionType).Select(c => c.id).Single();
            this.revenue_currency_id = eomEntities.Currencies.Where(c => c.name == conversion.PriceReceivedCurrency).Select(c => c.id).Single();
            this.cost_currency_id = eomEntities.Currencies.Where(c => c.name == conversion.PricePaidCurrency).Select(c => c.id).Single();
            this.pid = eomEntities.Campaigns.GetOrCreate(conversion.Offer_Id.Value, cakeService, logger);
            this.affid = eomEntities.Affiliates.GetOrCreate(conversion.Affiliate_Id.Value, cakeService, logger);
            this.num_units = conversion.Units ?? 0;
            this.revenue_per_unit = conversion.PriceReceived ?? 0;
            this.cost_per_unit = conversion.PricePaid ?? 0;
        }
    }
}
