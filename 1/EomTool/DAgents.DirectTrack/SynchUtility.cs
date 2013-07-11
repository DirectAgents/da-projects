using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DAgents.Common;
using DirectTrack.Rest;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;
namespace DAgents.Synch
{
    public static class SynchUtility
    {
        public enum TargetSystem
        {
            DirectTrack,
            Cake
        }

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programID"></param>
        /// <param name="synchDate"></param>
        /// <param name="logger"></param>
        /// <param name="db"></param>
        public static void SynchStats(int programID, DateTime synchDate, ILogger logger, SynchDBDataContext db, int redirectProgramID) // redirected pid
        {
            logger.Log("date " + synchDate.ToString());
            logger.Log("campaign pid " + programID);

            #region Delete stats and line items for current pid and date
            //{
            //    if (deleteStats)
            //    {
            //        using (var db2 = new SynchDBDataContext(true))
            //        {
            //            var stats = from c in db2.StatEntities
            //                        where c.pid == programID && c.stat_date == synchDate
            //                        select c;

            //            var badStats = from stat in stats
            //                           where stat.ItemEntities.Any(c => c.ItemAccountingStatusEntity.name != "default")
            //                           select stat;

            //            if (badStats.Count() > 0)
            //            {
            //                logger.LogError("cannot delete " + badStats.Count() + " items with status other than default");

            //                stats = stats.Except(badStats);
            //            }

            //            string statIDs = string.Join(",", stats.Select(c => c.id));

            //            logger.Log("delete stat ids: " + statIDs);

            //            db2.StatEntities.DeleteAllOnSubmit(stats);

            //            // TODO: key off of Source id, instead of glob matching to name
            //            var items = from c in db2.ItemEntities where c.pid == programID && c.name.Contains(synchDate.ToString("yyyy-MM-dd")) select c;
            //            db2.ItemEntities.DeleteAllOnSubmit(items);
            //            string itemIDs = string.Join(",", from c in items select c.id);

            //            logger.Log("delete item ids: " + itemIDs);

            //            logger.Log("cleaning stats/items");

            //            db2.SubmitChanges();
            //        }
            //    }
            //}
            #endregion

            // Get all the affiliates
            var affiliateIDs = db.AffiliateEntities.Select(c => c);

            // Get  DirectTrack stats for pid and date
            var stats2 = new StatList(XmlGetter.ByAffiliateForASingleDay(programID, synchDate));

            // List of stats to be turned into line items
            var statItems = new List<StatEntity>();

            #region Process stats
            foreach (var statDetail in stats2.StatsDetailList)
            {
                // Filter CD1
                // TODO: general mechanism for filtering at this level
                if (statDetail.affid == 1)
                {
                    continue;
                }
                // Skip stats with no leads
                // NOTE: this actually skips a very few instances
                // where there are sales and no stats, so it should be fixed
                if (statDetail.leads == 0 && statDetail.num_sales == 0 && statDetail.num_sub_sales == 0)
                {
                    try
                    {
                        // If there are at least clicks, then create the affiliate
                        if (statDetail.clicks > 0)
                        {
                            if (affiliateIDs.FirstOrDefault(c => c.affid == statDetail.affid) == null) // affid from stat not found in list of affiliates
                            {
                                AffiliateEntity.GetOrCreate(statDetail.affid, db);
                                SynchAffiliateToMediaBuyer(statDetail.affid, logger);
                                logger.Log("created affiliate with id " + statDetail.affid);
                            }

                        }
                    }
                    catch // TODO: improve (make it structured?) exception handling
                    {
                        logger.LogError("error creating affiliate with id " + statDetail.affid);
                    }
                    continue;
                }
                // Upsert stat to database
                var statEntity = StatEntity.GetOrCreateByApiUrl(statDetail.api_url, db);
                statEntity.pid = statDetail.pid;
                statEntity.affid = statDetail.affid;
                statEntity.apiurl = statDetail.api_url;
                statEntity.stat_date = statDetail.date;
                statEntity.clicks = statDetail.clicks;
                statEntity.leads = statDetail.leads;
                statEntity.num_sales = statDetail.num_sales;
                statEntity.num_sub_sales = statDetail.num_sub_sales;
                statEntity.num_post_impression_sales = 0;
                statEntity.sale_amount = 0;
                statEntity.num_post_impression_sales = 0;
                statEntity.num_post_impression_sub_sales = 0;
                statEntity.sub_sale_amount = 0;
                statEntity.num_post_impression_sub_sales = 0;
                try
                {
                    SetPayouts(statEntity, db);
                }
                catch (Exception setPayoutException)
                {
                    logger.LogError(setPayoutException.Message);
                    continue;
                }
                statItems.Add(statEntity);
                // TODO: is this needed?
                statEntity.AffiliateEntity = AffiliateEntity.GetOrCreate(statDetail.affid, db);
                if (affiliateIDs.FirstOrDefault(c => c.affid == statDetail.affid) == null) // affid from stat not found in list of affiliates
                {
                    SynchAffiliateToMediaBuyer(statDetail.affid, logger);
                }
            }
            logger.Log("submitting stats");
            db.SubmitChanges();
            #endregion

            #region Create line items
            foreach (var statEntity in statItems)
            {
                #region leads
                if (statEntity.leads > 0)
                {
                    logger.Log("line item for " + statEntity.leads + " leads");
                    var leadLineItemQuery = from c in db.ItemEntities
                                            where c.stat_id_n == statEntity.id &&
                                                  c.unit_type_id == 1
                                            select c;
                    var leadLineItem = leadLineItemQuery.FirstOrDefault();

                    if (leadLineItem == null)
                    {
                        leadLineItem = new ItemEntity();
                        leadLineItem.item_reporting_status_id = 1;
                        leadLineItem.item_accounting_status_id = 1;
                        db.ItemEntities.InsertOnSubmit(leadLineItem);

                    }
                    leadLineItem.name = statEntity.apiurl + ":Lead";
                    leadLineItem.pid = statEntity.pid;
                    leadLineItem.affid = statEntity.AffiliateEntity.affid;
                    leadLineItem.source_id = 1;
                    leadLineItem.unit_type_id = 1;
                    leadLineItem.stat_id_n = statEntity.id;
                    leadLineItem.RevenueCurrencyEntity = statEntity.RevenuePayout.CurrencyEntity;
                    leadLineItem.CostCurrencyEntity = statEntity.CostPayoutEntity.CurrencyEntity;
                    leadLineItem.revenue_per_unit = statEntity.RevenuePayout.lead;
                    leadLineItem.cost_per_unit = statEntity.CostPayoutEntity.lead;
                    leadLineItem.num_units = statEntity.leads;
                    leadLineItem.notes = "lead from dt stat " + DateTime.Now.ToString();
                    leadLineItem.accounting_notes = "lead from dt stat " + DateTime.Now.ToString();
                }
                #endregion

                #region sales
                if (statEntity.num_sales > 0)
                {
                    logger.Log("line item for " + statEntity.num_sales + " sales");
                    var salesLineItemQuery = from c in db.ItemEntities
                                             where c.stat_id_n == statEntity.id &&
                                                   c.unit_type_id == 2
                                             select c;
                    var salesLineItem = salesLineItemQuery.FirstOrDefault();
                    if (salesLineItem == null)
                    {
                        salesLineItem = new ItemEntity();
                        salesLineItem.item_reporting_status_id = 1;
                        salesLineItem.item_accounting_status_id = 1;
                        db.ItemEntities.InsertOnSubmit(salesLineItem);
                    }
                    salesLineItem.name = statEntity.apiurl + ":Sale";
                    salesLineItem.pid = statEntity.pid;
                    salesLineItem.affid = statEntity.AffiliateEntity.affid;
                    salesLineItem.source_id = 1;
                    salesLineItem.unit_type_id = 2;
                    salesLineItem.stat_id_n = statEntity.id;
                    salesLineItem.RevenueCurrencyEntity = statEntity.RevenuePayout.CurrencyEntity;
                    salesLineItem.CostCurrencyEntity = statEntity.CostPayoutEntity.CurrencyEntity;
                    salesLineItem.revenue_per_unit = statEntity.RevenuePayout.flat_sale;
                    salesLineItem.cost_per_unit = statEntity.CostPayoutEntity.flat_sale;
                    salesLineItem.num_units = statEntity.num_sales;
                    salesLineItem.notes = "sale from dt stat " + DateTime.Now.ToString();
                    salesLineItem.accounting_notes = "sale from dt stat " + DateTime.Now.ToString();
                }
                #endregion

                #region sub sales
                if (statEntity.num_sub_sales > 0)
                {
                    logger.Log("line item for " + statEntity.num_sales + " subsales");
                    var subSalesLineItemQuery = from c in db.ItemEntities
                                                where c.stat_id_n == statEntity.id &&
                                                      c.unit_type_id == 3
                                                select c;
                    var subSalesLineItem = subSalesLineItemQuery.FirstOrDefault();
                    if (subSalesLineItem == null)
                    {
                        subSalesLineItem = new ItemEntity();
                        subSalesLineItem.item_reporting_status_id = 1;
                        subSalesLineItem.item_accounting_status_id = 1;
                        db.ItemEntities.InsertOnSubmit(subSalesLineItem);
                    }
                    subSalesLineItem.name = statEntity.apiurl + ":Subsale";
                    subSalesLineItem.pid = statEntity.pid;
                    subSalesLineItem.affid = statEntity.AffiliateEntity.affid;
                    subSalesLineItem.source_id = 1;
                    subSalesLineItem.unit_type_id = 3;
                    subSalesLineItem.stat_id_n = statEntity.id;
                    subSalesLineItem.RevenueCurrencyEntity = statEntity.RevenuePayout.CurrencyEntity;
                    subSalesLineItem.CostCurrencyEntity = statEntity.CostPayoutEntity.CurrencyEntity;
                    subSalesLineItem.revenue_per_unit = statEntity.RevenuePayout.flat_sub_sale;
                    subSalesLineItem.cost_per_unit = statEntity.CostPayoutEntity.flat_sub_sale;
                    subSalesLineItem.num_units = statEntity.num_sub_sales;
                    subSalesLineItem.notes = "subsale from dt stat " + DateTime.Now.ToString();
                    subSalesLineItem.accounting_notes = "subsale from dt stat " + DateTime.Now.ToString();
                }
                #endregion
            }
            logger.Log("submitting line items");
            db.SubmitChanges();
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public static void SynchCampaignListFromDirectTrackToDatabaseWithPID(ILogger logger, int pid)
        {
            logger.Log("pulling campaign details");
            var campaignXML = XmlGetter.ViewCampaign(pid);
            var xElement = XElement.Parse(campaignXML);
            var campaignName = xElement.Element("campaignName").Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public static void UpdateCampaignsWithAdditionalDetailsFromDirectTrack(ILogger logger)
        {
            SynchDBDataContext db = new SynchDBDataContext(true);
            foreach (var campaignItem in from c in db.CampaignEntities
                                         where c.dt_campaign_url == null
                                         select c)
            {
                try
                {
                    SynchDBDataContext db2 = new SynchDBDataContext(true);
                    var campaign = CampaignEntity.Update(campaignItem.pid, db2);
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="affid"></param>
        /// <param name="logger"></param>
        public static void SynchAffiliateToMediaBuyer(int affid, ILogger logger)
        {
            SynchDBDataContext db = new SynchDBDataContext(true);
            var affDetail = AffiliateDetail.Create(affid);
            var affEntity = AffiliateEntity.GetOrCreate(affid, db);
            foreach (var id in affDetail.AffiliateGroupIDs)
            {
                var xml = XmlGetter.ViewAffiliateGroup(id);
                var affgroupdet = new AffiliateGroupDetail(xml);
                Match match = Regex.Match(affgroupdet.Name, @"(.*)\((MB)\)");
                if (match.Groups[2].Value == "MB")
                {
                    var mbe = MediaBuyerEntity.GetOrCreate(match.Groups[1].Value.Trim());
                    affEntity.media_buyer_id = mbe.id;
                    db.SubmitChanges();
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public static void SynchCampaignGroups(ILogger logger)
        {
            logger.Log("calling DirectTrack Rest API: campaignGroup/");
            string xml = XmlGetter.ListCampaignGroups();
            var campaignGroupList = new CampaignGroupList(xml, logger);
            // Iterate over the CampaignGroups in the CampaignGroupList
            foreach (var campaignGroup in campaignGroupList.CampaignGroups)
            {
                try
                {

                    // Iterate over the PIDs in the CampaignGroup
                    foreach (var pid in campaignGroup.CampaignPIDs)
                    {
                        logger.Log("campaign group " + pid + " --> " + campaignGroup.Name);
                        try
                        {
                            SynchDBDataContext db = new SynchDBDataContext(true);

                            var campaign = CampaignEntity.Get(pid, db);

                            string regex = @"(.*)\((AM|AD)\)";
                            Match match = Regex.Match(campaignGroup.Name, regex);
                            string personName = match.Groups[1].Value;
                            string personTypeCode = match.Groups[2].Value;
                            if (personTypeCode == "AM")
                            {
                                var ame = AccountManagerEntity.GetOrCreate(personName);
                                campaign.account_manager_id = ame.id;
                            }
                            else if (personTypeCode == "AD")
                            {
                                var ade = AdManagerEntity.GetOrCreate(personName);
                                campaign.ad_manager_id = ade.id;
                            }
                            logger.Log("submitting changes");
                            db.SubmitChanges();
                        }
                        catch (Exception ex)
                        {
                            logger.Log("exception:" + ex.Message);
                        }
                    }
                }
                catch (Exception ex2)
                {
                    logger.Log("exception2:" + ex2.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="pid"></param>
        public static void SynchPayoutsForCampaignPid(ILogger Logger, int pid)
        {
            Dictionary<string, int> currencyMap;
            using (SynchDBDataContext db = new SynchDBDataContext(true))
            {
                currencyMap = db.CurrencyEntities.ToDictionary(c => c.name, c => c.id);
            }
            SynchPayoutsForCampaignPid(Logger, currencyMap, pid);
        }

        private static void SynchPayoutsForCampaignPid(
            ILogger Logger,
            Dictionary<string, int> currMap,
            int pid)
        {
            try
            {
                Logger.Log("getting payout list for pid " + pid);
                PayoutList payoutList = PayoutList.FromPID(pid);
                Logger.Log("found " + payoutList.PayoutItems.Count() + " payouts for pid " + pid);
                int payoutsToGo = payoutList.PayoutItems.Count();
                int payoutsDone = 0;
                Logger.Log("parallel processing " + payoutsToGo + "payouts at " + DateTime.Now);
                payoutList.PayoutItems.AsParallel().ForAll(payoutItem =>
                {
                    using (SynchDBDataContext db = new SynchDBDataContext(true))
                    {
                        Logger.Log("+" + payoutItem.PayoutID);
                        try
                        {
                            PayoutDetail payoutDetail = PayoutDetail.FromPayoutId(payoutItem.PayoutID);
                            PayoutEntity payout = PayoutEntity.GetOrCreate(payoutItem.PayoutID, db);
                            payout.payout_type = payoutDetail.PayoutType;
                            payout.pid = payoutDetail.CampaignId;
                            payout.affid = payoutDetail.IsAllAffiliates ? 0 : payoutDetail.AffiliateId;
                            payout.impression = payoutDetail.Impression;
                            payout.click = payoutDetail.Click;
                            payout.lead = payoutDetail.Lead;
                            payout.percent_sale = payoutDetail.PercentSale;
                            payout.flat_sale = payoutDetail.FlatSale;
                            payout.percent_sub_sale = payoutDetail.PercentSubSale;
                            payout.flat_sub_sale = payoutDetail.FlatSubSale;
                            payout.effective_date = payoutDetail.EffectiveDate;
                            payout.modify_date = payoutDetail.ModifyDate;
                            try
                            {
                                payout.currency_id = currMap[payoutDetail.Curency];
                            }
                            catch
                            {
                                Logger.LogError("defaulting to USD for unknown currency id");
                                payout.currency_id = 1;
                            }
                            payout.product_id = payoutDetail.ProductId;
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e.Message);
                        }

                        Logger.Log("-" + payoutItem.PayoutID + " " + ++payoutsDone + "/" + --payoutsToGo);

                        try
                        {
                            db.SubmitChanges();
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(e.Message);
                        }
                    }
                }
                );
                Logger.Log("done parallel processing " + payoutsDone + " payouts at " + DateTime.Now);
            }
            catch
            {
                Logger.LogError(pid.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="pid"></param>
        /// <param name="affid"></param>
        public static void SynchPayoutsForCampaignPidForAffid(ILogger Logger, int pid, int affid)
        {
            SynchDBDataContext db = new SynchDBDataContext(true);
            Dictionary<string, int> currencyMap = db.CurrencyEntities.ToDictionary(c => c.name, c => c.id);
            SynchPayouts(Logger, db, currencyMap, pid, affid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logger"></param>
        //public static void SynchPayoutsForAllCampaigns(ILogger Logger)
        //{
        //    SynchDBDataContext db = new SynchDBDataContext(true);
        //    Dictionary<string, int> currencyNameToID =
        //        db.CurrencyEntities.ToDictionary(c => c.name, c => c.id);
        //    foreach (var pidWithStats in SynchUtilityHelper.GetPIDsForCampaignsWithStats())
        //    {
        //        SynchPayoutsForCampaignPid(Logger, db, currencyNameToID, pidWithStats);
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="pid"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sday"></param>
        /// <param name="eday"></param>
        public static void SynchStatsForPid( // redirect pid
            ILogger Logger,
            int pid,
            int year,
            int month,
            int sday,
            int eday,
            int redirectedPID)
        {
            var days = new List<int>();
            //var errors = new List<int>();
            for (int i = sday; i <= eday; i++)
            {
                days.Add(i);
            }
            //var exceptions = new ConcurrentQueue<Exception>();
            int done = 0;
            int toGo = days.Count;
            Logger.Log("parallel processing " + toGo + " days at " + DateTime.Now);
            days.AsParallel().ForAll(day =>
            {
                try
                {
                    using (var db = new SynchDBDataContext(true))
                    {
                        Logger.Log(string.Format("+synch stats for {0}/{1}/{2}", month, day, year));

                        // redirect pid
                        SynchStats(pid, new DateTime(year, month, day), Logger, db, redirectedPID);

                        Logger.Log(string.Format("-synch stats for {0}/{1}/{2} {3}", month, day, year, CounterString(++done, --toGo)));
                    }
                }
                catch (Exception e)
                {
                    //exceptions.Enqueue(e);
                    //errors.Add(day);
                    Logger.LogError(pid + ":" + e.Message);
                }
            });
            Logger.Log("done parallel processing " + done + " days at " + DateTime.Now
               // + " with " + errors.Count + " errors"
                );
            //if (errors.Count > 0)
            //{
            //    Logger.Log("non-parallel retry of " + errors.Count + " error days");
            //    foreach (var day in errors)
            //    {
            //        try
            //        {
            //            using (var db = new SynchDBDataContext(true))
            //            {
            //                Logger.Log(string.Format("synch stats for day {0}/{1}/{2}", month, day, year));
            //                SynchStats(pid, new DateTime(year, month, day), Logger, db, preDelete);
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            Logger.LogError("still error after retry for day " + day + ": " + e.Message);
            //        }
            //    }
            //}
            //if (exceptions.Count > 0)
            //{
            //    var messages = from e in exceptions
            //                   where e.Message.Contains("deadlock")
            //                   select e.Message;
            //    foreach (var e in exceptions)
            //    {
            //        Logger.LogError(pid + ":" + string.Join("\n", messages));
            //    }
            //}
        }

        private static string CounterString(int done, int toGo)
        {
            return string.Format("({0}/{1})", done, toGo);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Logger"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="sday"></param>
        /// <param name="eday"></param>
        //public static void SynchStatsForHardcodedPidList(
        //    ILogger Logger,
        //    int year,
        //    int month,
        //    int sday,
        //    int eday)
        //{
        //    foreach (var pidWithStats in SynchUtilityHelper.GetPIDsForCampaignsWithStats()
        //        .Where(
        //        c => c == 1349))
        //    {
        //        for (int i = sday; i <= eday; i++)
        //        {
        //            try
        //            {
        //                var db = new SynchDBDataContext(true);
        //                SynchStats(pidWithStats, new DateTime(year, month, i), Logger, db);
        //            }
        //            catch (Exception ex)
        //            {
        //                Logger.LogError(pidWithStats + ":" + ex.Message);
        //            }
        //        }
        //    }
        //}

        #endregion

        #region Private Methods

        private static void SetPayouts(StatEntity cur, SynchDBDataContext db)
        {
            var payouts = db.PayoutEntityList;
            var campaignPayout = (from c in payouts
                                  where c.payout_type == "campaign"
                                  && c.pid == cur.pid
                                  && c.effective_date <= cur.stat_date
                                  && (c.affid == cur.affid || c.affid == 0)
                                  select c)
                                  .OrderBy(s => s.effective_date)
                                  .OrderBy(s => s.affid)
                                  .LastOrDefault();
            if (campaignPayout != null)
            {
                cur.RevenuePayout = (from c in db.PayoutEntities
                                     where c.id == campaignPayout.id
                                     select c).First();
            }
            else
            {
                throw new Exception("no revenue payout");
            }

            var ap1 = from c in payouts
                      where c.payout_type == "affiliate"
                      && c.pid == cur.pid
                      && c.affid == cur.affid
                      && c.effective_date <= cur.stat_date
                      select c;

            int? affiliatePayoutID = null;

            if (ap1 != null)
            {
                var ap3 = ap1.OrderBy(s => s.effective_date).LastOrDefault();

                if (ap3 != null)
                {
                    affiliatePayoutID = ap3.id;
                }
                else
                {
                    var ap2 = (from c in payouts
                               where c.payout_type == "affiliate"
                               && c.pid == cur.pid
                               && c.affid == 0
                               select c)
                               .OrderBy(c => c.effective_date) // will always take default affiliate payout with latest date
                               .LastOrDefault();
                    if (ap2 != null)
                    {
                        affiliatePayoutID = ap2.id;
                    }
                    else
                    {
                        throw new Exception("no affiliate payout (affid=" + cur.affid + ")");
                    }
                }
            }

            cur.CostPayoutEntity = (from c in db.PayoutEntities
                                    where c.id == affiliatePayoutID.Value
                                    select c).First();
        }

        class Pair<TK, TV>
        {
            public TK K;
            public TV V;
        }

        class CountSet<T>
        {
            Dictionary<T, bool> Dictionary = new Dictionary<T, bool>();
            public CountSet(IEnumerable<T> items)
            {
            }
        }

        private static void SynchPayouts(
            ILogger Logger,
            SynchDBDataContext db,
            Dictionary<string, int> currMap,
            int pid,
            int affid)
        {
            Logger.Log("campaign pid " + pid);
            Logger.Log("affid " + affid);
            try
            {
                var payoutList = PayoutList.FromPID(pid);
                foreach (var payoutItem in payoutList.PayoutItems)
                {
                    Logger.Log("payout " + payoutItem.PayoutID);
                    PayoutDetail payoutDetail = PayoutDetail.FromPayoutId(payoutItem.PayoutID);
                    if (payoutDetail.IsAllAffiliates)
                    {
                        Logger.Log("skipping default payout");
                        continue;
                    }
                    if (payoutDetail.AffiliateId != affid)
                    {
                        Logger.Log("skipping payout that does not match affid");
                        continue;
                    }
                    PayoutEntity payout = PayoutEntity.GetOrCreate(payoutItem.PayoutID, db);
                    payout.payout_type = payoutDetail.PayoutType;
                    payout.pid = payoutDetail.CampaignId;
                    payout.affid = payoutDetail.IsAllAffiliates ? 0 : payoutDetail.AffiliateId;
                    payout.impression = payoutDetail.Impression;
                    payout.click = payoutDetail.Click;
                    payout.lead = payoutDetail.Lead;
                    payout.percent_sale = payoutDetail.PercentSale;
                    payout.flat_sale = payoutDetail.FlatSale;
                    payout.percent_sub_sale = payoutDetail.PercentSubSale;
                    payout.flat_sub_sale = payoutDetail.FlatSubSale;
                    payout.effective_date = payoutDetail.EffectiveDate;
                    payout.modify_date = payoutDetail.ModifyDate;
                    try
                    {
                        payout.currency_id = currMap[payoutDetail.Curency];
                    }
                    catch
                    {
                        Logger.LogError("defaulting to USD for unknown currency id");
                        payout.currency_id = 1;
                    }
                    payout.product_id = payoutDetail.ProductId;
                }
                Logger.Log("submitting changes");
                db.SubmitChanges();
            }
            catch
            {
                Logger.LogError(pid.ToString());
            }
        }
        #endregion
    }
}
