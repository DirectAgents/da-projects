using System.Linq;
using DAgents.Common;
using EomApp1.Screens.Synch.Models.Cake;
using EomApp1.Screens.Synch.Services.Cake;
using System.Collections.Generic;
using System;

namespace EomApp1.Screens.Synch.Models.Eom
{
    public partial class Item
    {
        public void Update(EomDatabaseEntities eomEntities, RegroupedCakeConversionSummary conversion, int pid, CakeWebService cakeService, ILogger logger)
        {
            if (this.id == 0) // new item.  Campaigns (offer) and Affiliates get created on demand here
            {
                this.name = conversion.Name;
                this.notes = "From Cake";
                this.accounting_notes = "From Cake";
                this.item_accounting_status_id = 1;
                this.item_reporting_status_id = 1;
                this.source_id = eomEntities.Sources.Where(c => c.name == "Cake").Select(c => c.id).Single();
                this.pid = eomEntities.Campaigns.IdOrCreate(pid, cakeService, logger);
                this.affid = eomEntities.Affiliates.IdOrCreate(conversion.Affiliate_Id.Value, cakeService, logger);
            }
            else // existing item - once items statuses are out of initial state, they may not be altered, so an exception may be thrown
            {
                // if item has been promoted
                if (this.campaign_status_id != 1 || this.item_accounting_status_id != 1 || this.item_reporting_status_id != 1) 
                {
                    // if item has changed
                    if (this.unit_type_id != eomEntities.UnitTypes.Where(c => c.name == conversion.ConversionType).Select(c => c.id).Single() ||
                        this.cost_currency_id != eomEntities.Currencies.Where(c => c.name == conversion.PricePaidCurrency).Select(c => c.id).Single() ||
                        this.num_units != (conversion.Units ?? 0) ||
                        this.revenue_per_unit != (conversion.PriceReceived ?? 0) ||
                        this.cost_per_unit != (conversion.PricePaid ?? 0) 
                       )
                        throw new CannotChangePromotedItemException(this);
                }
            }
            // since no exception was thrown, update the other fields
            this.unit_type_id = eomEntities.UnitTypes.Where(c => c.name == conversion.ConversionType).Select(c => c.id).Single();
            this.revenue_currency_id = eomEntities.Currencies.Where(c => c.name == conversion.PriceReceivedCurrency).Select(c => c.id).Single();
            this.cost_currency_id = eomEntities.Currencies.Where(c => c.name == conversion.PricePaidCurrency).Select(c => c.id).Single();
            this.num_units = conversion.Units ?? 0;
            this.revenue_per_unit = conversion.PriceReceived ?? 0;
            this.cost_per_unit = conversion.PricePaid ?? 0;
        }
    }
}
