using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using KendoGridBinderEx;
using KendoGridBinderEx.ModelBinder.Mvc;

namespace EomToolWeb.Controllers
{
    public class AdminController : EOMController
    {
        public AdminController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            SetAccountingPeriodViewData();
            return View();
        }

        public ActionResult Settings()
        {
            return View(GetSettingsVM());
        }

        [HttpGet]
        public ActionResult EditSettings()
        {
            return View(GetSettingsVM());
        }

        [HttpPost]
        public ActionResult EditSettings(SettingsVM settingsVM)
        {
            if (ModelState.IsValid)
            {
                daMain1Repo.SaveSetting("FinalizationWorkflow_MinimumMargin", settingsVM.FinalizationWorkflow_MinimumMargin);
                return RedirectToAction("Settings");
            }
            return View(settingsVM);
        }

        private SettingsVM GetSettingsVM()
        {
            var model = new SettingsVM
            {
                FinalizationWorkflow_MinimumMargin = daMain1Repo.GetSettingDecimalValue("FinalizationWorkflow_MinimumMargin")
            };
            return model;
        }

        // ---

        public ActionResult MarginApprovals()
        {
            var model = mainRepo.MarginApprovals(true);

            SetAccountingPeriodViewData();
            return View(model);
        }

        // ---

        public ActionResult AdvertiserMaintenance()
        {
            var advertisers = mainRepo.Advertisers().ToList();
            var advIds = advertisers.Select(a => a.id).ToList();

            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAdvertisers = prevRepo.Advertisers().ToList();
            var prevAdvIds = prevAdvertisers.Select(a => a.id).ToList();

            var newAdvertisers = advertisers.Where(a => !prevAdvIds.Contains(a.id));
            var expiredAdvertisers = prevAdvertisers.Where(a => !advIds.Contains(a.id)); // exist in prev month but not current

            List<Advertiser> changedAdvertisers = new List<Advertiser>();
            foreach (var adv in advertisers)
            {
                var prevAdv = prevAdvertisers.Where(a => a.id == adv.id).SingleOrDefault();
                if (prevAdv != null && adv.name != prevAdv.name)
                {
                    adv.PreviousMonthAdvertiser = prevAdv;
                    changedAdvertisers.Add(adv);
                }
            }

            var model = new PeriodMaintenanceVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                NewAdvertisers = newAdvertisers,
                ExpiredAdvertisers = expiredAdvertisers,
                ChangedAdvertisers = changedAdvertisers
            };
            return View(model);
        }

        public ActionResult AffiliateMaintenance()
        {
            var affiliates = mainRepo.Affiliates().ToList();
            var affIds = affiliates.Select(a => a.id).ToList();

            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAffiliates = prevRepo.Affiliates().ToList();
            var prevAffIds = prevAffiliates.Select(a => a.id).ToList();

            var newAffiliates = affiliates.Where(a => !prevAffIds.Contains(a.id));
            var expiredAffiliates = prevAffiliates.Where(a => !affIds.Contains(a.id));

            List<Affiliate> changedAffiliates = new List<Affiliate>();
            foreach (var aff in affiliates)
            {
                var prevAff = prevAffiliates.Where(a => a.id == aff.id).SingleOrDefault();
                if (prevAff != null && aff.name != prevAff.name)
                {
                    aff.PreviousMonthAffiliate = prevAff;
                    changedAffiliates.Add(aff);
                }
            }
            var model = new PeriodMaintenanceVM
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                NewAffiliates = newAffiliates,
                ExpiredAffiliates = expiredAffiliates,
                ChangedAffiliates = changedAffiliates
            };
            return View(model);
        }

        public ActionResult ClearInvoicingStatus()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var advertisers = mainRepo.Advertisers().Where(a => a.invoicing_status != null);
            foreach (var adv in advertisers)
                adv.invoicing_status = null;
            mainRepo.SaveChanges();

            return Content("cleared");
        }

        public ActionResult Copy1()
        {
            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAdvertisers = prevRepo.Advertisers().ToList();

            //TODO: check if ToListing is efficient & doesn't bring in extra stuff

            var advertisers = mainRepo.Advertisers();
            foreach (var adv in advertisers)
            {
                var prevAdv = prevAdvertisers.SingleOrDefault(a => a.id == adv.id);
                if (prevAdv != null)
                {
                    adv.status = prevAdv.status;
                    adv.payment_terms = prevAdv.payment_terms;
                    adv.invoicing_status = prevAdv.invoicing_status;
                }
            }
            mainRepo.SaveChanges();
            return Content("done");
        }
        public ActionResult Copy2()
        {
            var prevRepo = CreateMainRepository(eomEntitiesConfig.CurrentEomDate.AddMonths(-1));
            var prevAffiliates = prevRepo.Affiliates().ToList();

            var affiliates = mainRepo.Affiliates();
            foreach (var aff in affiliates)
            {
                var prevAff = prevAffiliates.SingleOrDefault(a => a.id == aff.id);
                if (prevAff != null)
                {
                    aff.status = prevAff.status;
                }
            }
            mainRepo.SaveChanges();
            return Content("done");
        }

        public ActionResult IncAdvStatus()
        {
            var advertisers = mainRepo.Advertisers();
            foreach (var adv in advertisers)
            {
                adv.status = NextStatus(adv.status);
            }
            mainRepo.SaveChanges();
            return Content("done");
        }
        public ActionResult IncAffStatus()
        {
            var affiliates = mainRepo.Affiliates();
            foreach (var aff in affiliates)
            {
                aff.status = NextStatus(aff.status);
            }
            mainRepo.SaveChanges();
            return Content("done");
        }

        private string NextStatus(string status)
        {
            if (status == "New")
                return "New1";
            if (status == "New1")
                return "New2";
            if (status == "New2")
                return "Existing";
            return status;
        }

        // ---

        public ActionResult AccountingSheet()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var model = new AccountingSheetModel {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult AccountingSheetData(KendoGridMvcRequest request)
        {
            // The "agg" aggregates will get computed outside of KendoGridEx
            request.AggregateObjects = request.AggregateObjects.Where(ao => ao.Aggregate != "agg");

            var affSort = request.SortObjects.SingleOrDefault(so => so.Field == "AffId");
            if (affSort != null) affSort.Field = "AffName";
            var advSort = request.SortObjects.SingleOrDefault(so => so.Field == "AdvId");
            if (advSort != null) advSort.Field = "AdvName";
            var unitTypeSort = request.SortObjects.SingleOrDefault(so => so.Field == "UnitTypeId");
            if (unitTypeSort != null) unitTypeSort.Field = "UnitTypeName";

            var data = mainRepo.CampAffItems(null);
            var kgrid = new KendoGridEx<CampAffItem>(request, data);

            return CreateJsonResult(kgrid);
        }

        [HttpPost]
        public ActionResult AccountingSheetUpdate(CampAffItem[] models)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder msgBuilder = new StringBuilder();
                    foreach (var modelError in ModelState.Values.SelectMany(ms => ms.Errors))
                    {
                        msgBuilder.Append(modelError.ErrorMessage + " ");
                    }
                    throw new Exception(msgBuilder.ToString());
                }
                foreach (var row in models)
                {
                    var revCurr = mainRepo.GetCurrency(row.RevCurr);
                    var costCurr = mainRepo.GetCurrency(row.CostCurr);

                    // Update each item (some rows represent multiple items that were grouped together)
                    var itemIds = row.GetItemIds();
                    var items = mainRepo.GetItems(itemIds);
                    if (items.Count() < itemIds.Count())
                        throw new Exception(String.Format("Only found {0} of {1} items", items.Count(), itemIds.Count()));
                    foreach (var item in items)
                    {
                        item.affid = row.AffId;
                        item.revenue_currency_id = revCurr.id;
                        item.cost_currency_id = costCurr.id;
                        item.revenue_per_unit = row.RevPerUnit;
                        item.cost_per_unit = row.CostPerUnit;
                        item.unit_type_id = row.UnitTypeId;
                        //item.campaign_status_id = row.CStatusId;
                        //item.item_accounting_status_id = row.AStatusId;
                    }
                    AdjustNumUnits(items, row);

                    // Recompute...
                    row.Rev = row.Units * row.RevPerUnit;
                    row.Cost = row.Units * row.CostPerUnit;
                    row.RevUSD = row.Rev * revCurr.to_usd_multiplier;
                    row.CostUSD = row.Cost * costCurr.to_usd_multiplier;
                    // (MarginPct is computed from RevUSD and CostUSD)
                }
                mainRepo.SaveChanges();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
                //TODO: rework. return a simpler json object with error info.
            }

            return Json(models);
        }
        private void AdjustNumUnits(IQueryable<Item> dbItems, CampAffItem row)
        {
            var oldNumUnits = dbItems.Sum(i => i.num_units);
            var difference = row.Units - oldNumUnits;
            if (difference == 0)
                return;

            // Order: extraItems first, then by name/descending(date for cake items), then by num_units/descending
            dbItems = dbItems.OrderBy(i => i.source_id).ThenByDescending(i => i.name).ThenByDescending(i => i.num_units);
            if (difference > 0)
            {   // Increasing numUnits
                var dbItem = dbItems.First();
                dbItem.num_units += difference;
            }
            else
            {   // Decreasing numUnits
                difference *= -1; // make it positive
                bool success = false;
                decimal counted = 0;
                foreach (var dbItem in dbItems)
                {
                    var needed = difference - counted;
                    if (needed < 0) {
                        throw new Exception("Error adjusting unit count. 'Needed' < 0");
                    }
                    else if (needed == 0)
                    {
                        success = true;
                        break;
                    }
                    var available = dbItem.num_units;
                    if (available > 0)
                    {
                        if (available >= needed)
                        {
                            dbItem.num_units -= needed;
                            counted += needed;
                            success = true;
                            break;
                        }
                        else
                        {
                            counted += dbItem.num_units;
                            dbItem.num_units = 0;
                        }
                    }
                }
                if (!success)
                    throw new Exception("Failed to adjust unit count.");
            }
        }

        // ---

        private JsonResult CreateJsonResult(KendoGridEx<CampAffItem> kgrid)
        {
            var kg = new KG<CampAffItem>();
            kg.data = kgrid.Data;
            kg.total = kgrid.Total;
            kg.aggregates = Aggregates(kgrid);

            var json = Json(kg);
            return json;
        }
        //private JsonResult CreateJsonResult(DataSourceResult result)
        //{
        //    result.Aggregates = Aggregates(result);
        //    var json = Json(result);
        //    return json;
        //}
        //var type2 = result.Aggregates.GetType();
        //var revAgg = type2.GetProperty("RevUSD").GetValue(result.Aggregates);
        //var costAgg = type2.GetProperty("CostUSD").GetValue(result.Aggregates);
        //var type1 = revAgg.GetType();
        //decimal sumRevUSD = (decimal)type1.GetProperty("sum").GetValue(revAgg);
        //decimal sumCostUSD = (decimal)type1.GetProperty("sum").GetValue(costAgg);

        private object Aggregates(KendoGridEx<CampAffItem> kgrid)
        {
            if (kgrid.Total == 0) return null;
            decimal sumRevUSD = ((dynamic)kgrid.Aggregates)["RevUSD"]["sum"];
            decimal sumCostUSD = ((dynamic)kgrid.Aggregates)["CostUSD"]["sum"];

            decimal? marginPct = null;
            if (sumRevUSD != 0)
                marginPct = 1 - sumCostUSD / sumRevUSD;

            var aggs = new
            {
                RevUSD = new { sum = sumRevUSD },
                RevCurr = new { agg = "USD" },
                CostUSD = new { sum = sumCostUSD },
                CostCurr = new { agg = "USD" },
                MarginPct = new { agg = marginPct }
            };
            return aggs;
        }
    }

    class KG<T>
    {
        public IEnumerable<T> data { get; set; }
        public int total { get; set; }
        public object aggregates { get; set; }
    }
}
