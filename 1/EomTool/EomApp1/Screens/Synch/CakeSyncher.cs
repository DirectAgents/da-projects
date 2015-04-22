using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DAgents.Common;
using EomApp1.Cake.WebServices._4.Reports;
using EomApp1.Screens.Synch.Models.Cake;
using EomApp1.Screens.Synch.Models.Eom;
using EomApp1.Screens.Synch.Services.Cake;

namespace EomApp1.Screens.Synch
{
    class CakeSyncher
    {
        /// <summary>
        /// Parameters for the conversion extraction and Item creation.
        /// </summary>
        public struct Parameters
        {
            /// <summary>
            /// The ID of the EOM campaign that receives the Items which are created from the extracted conversions.
            /// </summary>
            public int CampaignId { get; set; }

            /// <summary>
            /// The ID of the corresponding Offer in Cake which serves as the source of the conversions.
            /// </summary>
            public int CampaignExternalId { get; set; }

            /// <summary>
            /// The year to extract conversions from.
            /// </summary>
            public int Year { get; set; }

            /// <summary>
            /// The month to extract conversions from.
            /// </summary>
            public int Month { get; set; }

            /// <summary>
            /// The starting day to extract conversions.
            /// </summary>
            public int FromDay { get; set; }

            /// <summary>
            /// The ending day to extract conversions.
            /// </summary>
            public int ToDay { get; set; }

            /// <summary>
            /// Gets Year, Month and FromDay as a DateTime.
            /// </summary>
            public DateTime FromDate { get { return new DateTime(this.Year, this.Month, this.FromDay); } }

            /// <summary>
            /// Gets Year, Month and ToDay as a DateTime.
            /// </summary>
            public DateTime ToDate { get { return new DateTime(this.Year, this.Month, this.ToDay); } }

            /// <summary>
            /// If true, create all items as a summary of the items for the month instead of
            /// an item for each day with stats.
            /// </summary>
            public bool GroupItemsToFirstDayOfMonth { get; set; }
        }

        private readonly Parameters parameters;
        private readonly ILogger logger;
        private readonly CakeWebService cakeService;
        private List<conversion> extractedConversions;
        private CakeEntities cakeEntities;
        private EomDatabaseEntities eomEntities;
        private List<RegroupedCakeConversionSummary> conversionSummaries;
        private Dictionary<RegroupedCakeConversionSummary, Item> itemsFromConversionSummaries;

        public CakeSyncher(ILogger logger, Parameters parameters)
        {
            this.parameters = parameters;
            this.logger = logger;
            this.cakeService = new CakeWebService(this.logger);
        }

        public void SynchStatsForOfferId()
        {
            using (this.cakeEntities = new CakeEntities())
            {
                this.cakeEntities.CommandTimeout = 60;
                this.DeleteExistingConversions(); // 1
                this.cakeEntities.SaveChanges();
            }
            using (this.eomEntities = EomDatabaseEntities.Create())
            {
                this.DeleteExistingItems(); // 2
                this.eomEntities.SaveChanges();
            }
            using (this.cakeEntities = new CakeEntities())
            {
                this.cakeEntities.CommandTimeout = 60;
                // 3, 4
                this.ExtractConversions();
                // 5
                this.StageExtractedConversions();
                // 6 - save changes
                this.cakeEntities.SaveChanges();
                using (this.eomEntities = EomDatabaseEntities.Create())
                {
                    // 7, 8
                    this.ReadConversionSummaries();
                    // 9
                    this.TransformConversionSummariesToItems();
                    // 10, 11
                    this.LoadItems();
                    this.eomEntities.SaveChanges();
                }
            }
        }

        #region Prepare
        // step 1
        private void DeleteExistingConversions()
        {
            this.logger.Log("Deleting existing conversions for offer " + this.parameters.CampaignExternalId + "...");

            var existingConversions = this.cakeEntities.CakeConversions.ByOfferIdAndDateRange(this.parameters.CampaignExternalId,
                                                                                              this.parameters.FromDate,
                                                                                              this.parameters.ToDate);

            int numConversions = existingConversions.Count();
            if (numConversions > 0)
            {
                foreach (var conversion in existingConversions)
                {
                    this.cakeEntities.CakeConversions.DeleteObject(conversion);
                }
            }
            this.logger.Log("Deleted " + numConversions + " conversions.");
        }

        // step 2
        private void DeleteExistingItems()
        {
            this.logger.Log("Deleting Items with default accounting/reporting status...");

            var existingItems = from c in this.eomEntities.Items
                                where
                                   c.campaign_status_id == 1 &&
                                   c.item_accounting_status_id == 1 &&
                                   c.item_reporting_status_id == 1 &&
                                   c.pid == this.parameters.CampaignId &&
                                   c.Source.name == "Cake"
                                select c;

            existingItems.ToList().ForEach(c => this.eomEntities.Items.DeleteObject(c));

            this.logger.Log("Deleted " + existingItems.Count() + " items.");
        }
        #endregion

        #region Extract
        // step 3
        private void ExtractConversions()
        {
            logger.Log("Extracting conversions...");

            var extracted = this.cakeService.Conversions(this.parameters.CampaignExternalId,
                                                         this.parameters.FromDate,
                                                         this.parameters.ToDate).ToList();

            var validations = new Func<conversion, bool>[] { 
                 c => (c.affiliate != null) && (c.affiliate.affiliate_id > 0),
                 c => c.campaign_id > 0
            };

            var partitioned = extracted.ToLookup(c => validations.All(valid => valid(c))); // if any are invalid go to the false bucket

            if (partitioned[false].Any())
            {
                logger.Log("Ignoring " + partitioned[false].Count() + " conversions.");
            }

            // step 4 - SET extractedConversions to list of valid conversions
            this.extractedConversions = partitioned[true].ToList();

            logger.Log("Extracted " + this.extractedConversions.Count + " conversions.");
        }

        List<CakeConversion> updatedCakeConversions = new List<CakeConversion>();
        // step 5
        private void StageExtractedConversions()
        {
            this.logger.Log("Staging extracted conversions...");

            this.extractedConversions.ForEach(extractedConversion =>
            {
                var cakeConversion = this.cakeEntities.CakeConversions.Create(extractedConversion.IdAsInt);
                cakeConversion.Update(extractedConversion);
                updatedCakeConversions.Add(cakeConversion);
            });

            this.logger.Log("Staged " + this.extractedConversions.Count + ".");
        }
        #endregion

        #region Transform
        // step 7
        private void ReadConversionSummaries()
        {
            // step 8
            // CakeConversionSummaries is a view that groups conversions by offerId and Date, and gives them a unique string name key
            // SET conversionSummaries to rows from special view for offerId and date range
            var conversionSummariesFromView = cakeEntities.CakeConversionSummaries.ByOfferIdAndDateRange(
                                                                                this.parameters.CampaignExternalId,
                                                                                this.parameters.FromDate,
                                                                                this.parameters.ToDate).ToList();

            if (this.parameters.GroupItemsToFirstDayOfMonth)
            {
                int conversionSummaryCountBeforeGrouping = conversionSummariesFromView.Count;
                var rx = new Regex(@"Cake/(\d+-\d+-\d+)/aff");
                Func<DateTime, DateTime> firstOfMonth = dt => dt.AddDays(-1 * (dt.Day - 1));

                this.conversionSummaries = conversionSummariesFromView
                        .Select(c => new RegroupedCakeConversionSummary
                        {
                            Name = rx.Replace(c.Name, "Cake/" + firstOfMonth(c.ConversionDate.Value).ToString("yyyy-MM-dd") + "/aff"),
                            ConversionDate = firstOfMonth(c.ConversionDate.Value),
                            Affiliate_Id = c.Affiliate_Id,
                            Offer_Id = c.Offer_Id,
                            ConversionType = c.ConversionType,
                            Units = c.Units,
                            PricePaid = c.PricePaid,
                            PriceReceived = c.PriceReceived,
                            PricePaidCurrency = c.PricePaidCurrency,
                            PriceReceivedCurrency = c.PriceReceivedCurrency,
                            Paid = c.Paid,
                            Received = c.Received
                        })
                        .GroupBy(c => new
                        {
                            c.Name,
                            c.ConversionDate,
                            c.Affiliate_Id,
                            c.Offer_Id,
                            c.ConversionType,
                            c.PricePaid,
                            c.PricePaidCurrency,
                            c.PriceReceived,
                            c.PriceReceivedCurrency
                        })
                        .Select(c => new RegroupedCakeConversionSummary
                        {
                            Name = c.Key.Name,
                            ConversionDate = c.Key.ConversionDate,
                            Affiliate_Id = c.Key.Affiliate_Id,
                            Offer_Id = c.Key.Offer_Id,
                            ConversionType = c.Key.ConversionType,
                            Units = c.Sum(s => s.Units),
                            PricePaid = c.Key.PricePaid,
                            PriceReceived = c.Key.PriceReceived,
                            PricePaidCurrency = c.Key.PricePaidCurrency,
                            PriceReceivedCurrency = c.Key.PriceReceivedCurrency,
                            Paid = c.Sum(s => s.Paid),
                            Received = c.Sum(s => s.Received)
                        }).ToList();

                this.logger.Log(string.Format("Regrouped to {0} conversion summaries to {1} with 1st of month conversion date..",
                    conversionSummaryCountBeforeGrouping, conversionSummaries.Count));
            }
            else
            {
                this.conversionSummaries = conversionSummariesFromView
                    .Select(c => new RegroupedCakeConversionSummary
                    {
                        Name = c.Name,
                        ConversionDate = c.ConversionDate,
                        Affiliate_Id = c.Affiliate_Id,
                        Offer_Id = c.Offer_Id,
                        ConversionType = c.ConversionType,
                        Units = c.Units,
                        PricePaid = c.PricePaid,
                        PriceReceived = c.PriceReceived,
                        PricePaidCurrency = c.PricePaidCurrency,
                        PriceReceivedCurrency = c.PriceReceivedCurrency,
                        Paid = c.Paid,
                        Received = c.Received
                    }).ToList();
            }
        }

        private void TransformConversionSummariesToItems()
        {
            // step 9
            // TRICKY - we get a dictionary where the key is a summary and the value is an existing item which matches
            // date, affiliate id, offer id, and offer type (e.g. CPA), thus matching everything BUT the currency types and amounts
            // for cost and revenue.  If there is not matching item, the value is set to null.
            this.itemsFromConversionSummaries = this.conversionSummaries.ToDictionary(c => c, c => c.MatchingItem(this.eomEntities));
        }
        #endregion

        #region Load
        // step 10
        private void LoadItems()
        {
            logger.Log("Loading items from conversion summaries...");
            var conversionTypes = this.itemsFromConversionSummaries.Keys.Select(s => s.ConversionType).Distinct();
            foreach (var conversionType in conversionTypes)
            {
                try
                {
                    var conversionSummaryKeys = this.itemsFromConversionSummaries.Keys.Where(s => s.ConversionType == conversionType).ToList();
                    foreach (var conversionSummary in conversionSummaryKeys)
                    {
                        // check the dictionary of conversionSummary to existing item
                        var item = this.itemsFromConversionSummaries[conversionSummary];

                        bool needToAddItem = false;
                        if (item == null)  // no existing item?
                        {
                            // ?10a
                            item = CreateItem(conversionSummary, item);
                            needToAddItem = true;
                        }
                        else
                        {
                            logger.Log(string.Format("Updating item {0} from conversion {1}.", item.name, conversionSummary.Name));
                        }

                        // Here's the logic that used to be required when there were two tracking systems and the campaign IDs could conflict
                        // Lesson to be learned, when there's an external system, use a key that will never conflict now or in the future
                        if (this.parameters.CampaignId != this.parameters.CampaignExternalId)
                        {
                            logger.Log(string.Format("Cake Offer {0} is redirecting to PID {1}.", this.parameters.CampaignExternalId, this.parameters.CampaignId));
                        }

                        try
                        {
                            // 11 - alot of stuff happens in the implementation of Update
                            item.Update(this.eomEntities, conversionSummary, this.parameters.CampaignId, this.cakeService, this.logger);
                        }
                        catch (CannotChangePromotedItemException)
                        {
                            logger.Log(string.Format("Item for Pid {0} not being updated; already promoted.", this.parameters.CampaignId));
                            continue;
                        }
                        if (needToAddItem)
                            this.eomEntities.Items.AddObject(item);

                        if (item.affid != conversionSummary.Affiliate_Id)
                            logger.Log(string.Format("Cake Affiliate {0} is redirecting to ID {1}.", conversionSummary.Affiliate_Id, item.affid));
                    }
                }
                catch (EntityNotFoundException ex)
                {
                    logger.LogError("error for stats(pid=" + this.parameters.CampaignId + ") - EntityNotFound: " + ex.Message);
                }
            }
            logger.Log("Items loaded.");
        }

        // ?10a
        private Item CreateItem(RegroupedCakeConversionSummary conversionSummary, Item item)
        {
            logger.Log("Creating item: " + conversionSummary.Name);

            item = new Item() { media_buyer_approval_status_id = 1, campaign_status_id = 1 };
            item.pid = this.parameters.CampaignId;

            return item;
        }
        #endregion
    }

    public class RegroupedCakeConversionSummary
    {
        public string Name { get; set; }

        public DateTime? ConversionDate { get; set; }

        public int? Affiliate_Id { get; set; }

        public int? Offer_Id { get; set; }

        public string ConversionType { get; set; }

        public int? Units { get; set; }

        public decimal? PricePaid { get; set; }

        public decimal? PriceReceived { get; set; }

        public string PricePaidCurrency { get; set; }

        public string PriceReceivedCurrency { get; set; }

        public decimal? Paid { get; set; }

        public decimal? Received { get; set; }

        //Cake/2012-05-18/aff:10901/offer:1539/type:CPA/paycur:EUR/paid:2/recvcur:EUR/recv:3
        public Item MatchingItem(EomDatabaseEntities eomEntities)
        {
            string name = this.Name;
            string keyPart = name.Substring(0, name.IndexOf("/type"));

            var matchingItem = from c in eomEntities.Items
                               where c.name.StartsWith(keyPart)
                               select c;

            return matchingItem.FirstOrDefault();
        }

        public string UnitTypeName()
        {
            string unitTypeName = "";
            var splitType = ConversionType.Split(new string[] { " - " }, StringSplitOptions.None);
            if (splitType.Length > 1)
                unitTypeName = splitType[1];
            return unitTypeName;
        }
        public UnitType GetUnitType(EomDatabaseEntities eomEntities, out string unitTypesTried)
        {
            unitTypesTried = null;
            string unitTypeName = this.UnitTypeName();
            var unitType = eomEntities.UnitTypes.Where(c => c.name == unitTypeName).SingleOrDefault();
            if (unitType == null)
            {   // unitType not found from the conversion; try from the campaign (offer)
                if (this.Offer_Id.HasValue)
                {
                    var campaign = eomEntities.Campaigns.Where(c => c.pid == this.Offer_Id.Value).FirstOrDefault();
                    if (campaign != null)
                        unitType = eomEntities.UnitTypes.Where(c => c.name == campaign.campaign_type).SingleOrDefault();
                    if (unitType == null)
                        unitTypesTried = unitTypeName + (campaign == null ? "" : "/" + (campaign.campaign_type ?? "[NULL]"));
                }
                else
                {
                    unitTypesTried = unitTypeName;
                }
            }
            return unitType;
        }
    }
}
