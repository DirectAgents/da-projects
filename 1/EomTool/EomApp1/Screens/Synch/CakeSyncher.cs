using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private readonly Parameters parameters;
        private readonly ILogger logger;
        private readonly CakeWebService cakeService;
        private List<conversion> extractedConversions;
        private CakeEntities cakeEntities;
        private EomDatabaseEntities eomEntities;
        private List<CakeConversionSummary> conversionSummaries;
        private Dictionary<CakeConversionSummary, Item> itemsFromConversionSummaries;

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
                this.DeleteExistingConversions();
                this.cakeEntities.SaveChanges();
            }
            using (this.eomEntities = EomDatabaseEntities.Create())
            {
                this.DeleteExistingItems();
                this.eomEntities.SaveChanges();
            }
            using (this.cakeEntities = new CakeEntities())
            {
                this.ExtractConversions();
                this.StageExtractedConversions();
                this.cakeEntities.SaveChanges();
                using (this.eomEntities = EomDatabaseEntities.Create())
                {
                    this.ReadConversionSummaries();
                    this.TransformConversionSummariesToItems();
                    this.LoadItems();
                    this.eomEntities.SaveChanges();
                }
            }
        }

        #region Prepare
        private void DeleteExistingConversions()
        {
            this.logger.Log("Deleting existing conversions...");

            var existingConversions = this.cakeEntities.CakeConversions.ByOfferIdAndDateRange(this.parameters.CampaignExternalId,
                                                                                              this.parameters.FromDate,
                                                                                              this.parameters.ToDate);

            foreach (var conversion in existingConversions)
            {
                this.cakeEntities.CakeConversions.DeleteObject(conversion);
            }

            this.logger.Log("Deleted " + existingConversions.Count() + " conversions.");
        }

        private void DeleteExistingItems()
        {
            this.logger.Log("Deleting Items with default accounting/reporting status...");

            var existingItems = from c in this.eomEntities.Items
                                where
                                  (c.campaign_status_id == 1 || c.campaign_status_id == 2) &&
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

            this.extractedConversions = partitioned[true].ToList();

            logger.Log("Extracted " + this.extractedConversions.Count + " conversions.");
        }


        List<CakeConversion> updatedCakeConversions = new List<CakeConversion>();
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
        private void ReadConversionSummaries()
        {
            this.conversionSummaries = cakeEntities.CakeConversionSummaries.ByOfferIdAndDateRange(
                                                                                this.parameters.CampaignExternalId,
                                                                                this.parameters.FromDate,
                                                                                this.parameters.ToDate).ToList();
        }

        private void TransformConversionSummariesToItems()
        {
            this.itemsFromConversionSummaries = this.conversionSummaries.ToDictionary(c => c, c => c.MatchingItem(this.eomEntities));
        }
        #endregion

        #region Load
        private void LoadItems()
        {
            logger.Log("Loading items from conversion summaries...");

            foreach (var conversionSummary in this.itemsFromConversionSummaries.Keys.ToList())
            {
                var item = this.itemsFromConversionSummaries[conversionSummary];

                if (item == null)
                {
                    item = CreateItem(conversionSummary, item);
                }
                else
                {
                    logger.Log(string.Format("Updating item {0} from conversion {1}.", item.name, conversionSummary.Name));
                }

                if (this.parameters.CampaignId != this.parameters.CampaignExternalId)
                {
                    logger.Log(string.Format("Cake Offer {0} is redirecting to PID {1}.", this.parameters.CampaignExternalId, this.parameters.CampaignId));
                }

                try
                {
                    item.Update(this.eomEntities, conversionSummary, this.parameters.CampaignId, this.cakeService, this.logger);
                }
                catch (CannotChangePromotedItemException)
                {
                    logger.Log(string.Format("Item for Pid {0} not being updated; already promoted.", this.parameters.CampaignId));
                    continue;
                }

                if (item.affid != conversionSummary.Affiliate_Id)
                {
                    logger.Log(string.Format("Cake Affiliate {0} is redirecting to ID {1}.", conversionSummary.Affiliate_Id, item.affid));
                }
            }

            logger.Log("Items loaded.");
        }

        private Item CreateItem(CakeConversionSummary conversionSummary, Item item)
        {
            logger.Log("Creating item: " + conversionSummary.Name);

            item = new Item() { media_buyer_approval_status_id = 1, campaign_status_id = 1 };
            item.pid = this.parameters.CampaignId;
            eomEntities.Items.AddObject(item);

            return item;
        }
        #endregion
    }
}
