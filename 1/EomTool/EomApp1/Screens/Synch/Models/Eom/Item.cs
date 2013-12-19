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
            // Campaign (offer) gets created on demand
            eomEntities.Campaigns.IdOrCreate(pid, cakeService, logger);

            var splitType = conversion.ConversionType.Split(new string[] { " - " }, StringSplitOptions.None);
            string unitTypeString = "";
            if (splitType.Length > 1) unitTypeString = splitType[1];

            var unitType = eomEntities.UnitTypes.Where(c => c.name == unitTypeString).SingleOrDefault();
            if (unitType == null)
            {   // unitType not found from the conversion; try from the campaign (offer)
                var campaign = eomEntities.Campaigns.Where(c => c.pid == this.pid).FirstOrDefault();
                if (campaign != null)
                    unitType = eomEntities.UnitTypes.Where(c => c.name == campaign.campaign_type).SingleOrDefault();
                if (unitType == null)
                {
                    string notFoundName = unitTypeString + (campaign != null ? "\" or \"" + (campaign.campaign_type ?? "[NULL]") : "");
                    throw new EntityNotFoundException("UnitType", notFoundName); //TODO: allow to pass in multiple notFoundName's
                }
            }
            var revenueCurrency = eomEntities.Currencies.Where(c => c.name == conversion.PriceReceivedCurrency).SingleOrDefault();
            if (revenueCurrency == null)
                throw new EntityNotFoundException("Revenue Currency", conversion.PriceReceivedCurrency);

            var costCurrency = eomEntities.Currencies.Where(c => c.name == conversion.PricePaidCurrency).SingleOrDefault();
            if (costCurrency == null)
                throw new EntityNotFoundException("Cost Currency", conversion.PricePaidCurrency);

            if (this.id == 0) // new item.  Affiliate gets created on demand here
            {
                this.name = conversion.Name;
                this.notes = "From Cake";
                this.accounting_notes = "From Cake";
                this.item_accounting_status_id = 1;
                this.item_reporting_status_id = 1;
                this.source_id = eomEntities.Sources.Where(c => c.name == "Cake").Select(c => c.id).Single();
                this.pid = pid;
                this.affid = eomEntities.Affiliates.IdOrCreate(conversion.Affiliate_Id.Value, cakeService, logger);
            }
            else // existing item - once items statuses are out of initial state, they may not be altered, so an exception may be thrown
            {
                // if item has been promoted
                if (this.campaign_status_id != 1 || this.item_accounting_status_id != 1 || this.item_reporting_status_id != 1)
                {
                    // if item has changed
                    if (this.unit_type_id != unitType.id ||
                        this.revenue_currency_id != revenueCurrency.id ||
                        this.cost_currency_id != costCurrency.id ||
                        this.num_units != (conversion.Units ?? 0) ||
                        this.revenue_per_unit != (conversion.PriceReceived ?? 0) ||
                        this.cost_per_unit != (conversion.PricePaid ?? 0)
                       )
                        throw new CannotChangePromotedItemException(this);
                }
            }

            // since no exception was thrown, update the other fields
            this.unit_type_id = unitType.id;
            this.revenue_currency_id = revenueCurrency.id;
            this.cost_currency_id = costCurrency.id;
            this.num_units = conversion.Units ?? 0;
            this.revenue_per_unit = conversion.PriceReceived ?? 0;
            this.cost_per_unit = conversion.PricePaid ?? 0;
        }
    }
}
