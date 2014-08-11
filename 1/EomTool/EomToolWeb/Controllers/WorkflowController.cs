using EomTool.Domain.Abstract;
using EomTool.Domain.DTOs;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EomToolWeb.Controllers
{
    public class WorkflowController : EOMController
    {
        public WorkflowController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepo = mainRepository;
            this.securityRepo = securityRepository;
            this.daMain1Repo = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        public ActionResult Index(int? am, int? cs, bool uninvoiced = false)
        {
            var campaignAmounts = mainRepo.CampaignAmounts(am, null, false, cs);

            if (uninvoiced) // only show the uninvoiced amounts
                campaignAmounts = campaignAmounts.Where(ca => ca.InvoicedAmount < ca.Revenue);

            var model = new WorkflowModel
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                CampaignAmounts = campaignAmounts.OrderBy(ca => ca.AdvertiserName).ThenBy(ca => ca.CampaignName),
                CampaignStatusId = cs,
                AccountManagerId = am
            };
            if (am.HasValue)
            {
                var accountManagerTeam = mainRepo.GetAccountManagerTeam(am.Value);
                if (accountManagerTeam != null)
                    model.AccountManagerName = accountManagerTeam.name;
            }
            return View(model);
        }

        public ActionResult AffiliateDrilldown(int advId, int? cs)
        {
            var advertiser = mainRepo.GetAdvertiser(advId);
            var model = new CampaignAffiliateAmountsModel()
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                AdvertiserId = advId,
                AdvertiserName = advertiser.name,
                CampaignAmounts = mainRepo.CampaignAmounts(null, advId, true, cs),
                CampaignStatusId = cs
            };
            return View(model);
        }

        // ---

        public ActionResult AffiliateCampaigns()
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var model = new AffiliateAmountsModel
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                CampaignAmounts = mainRepo.CampaignAmounts2(null)
            };
            return View("AffiliateCampaignAmounts", model);
        }

        public ActionResult AffiliateAmounts(string sort)
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var model = new AffiliateAmountsModel
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                CampaignAmounts = mainRepo.CampaignAmounts2(null),
                Sort = (sort != null ? sort.ToLower() : "")
            };
            switch (model.Sort)
            {
                case "campaign":
                    model.CampaignAmounts = model.CampaignAmounts.OrderBy(c => c.CampaignName).ThenBy(c => c.AffiliateName).ThenBy(c => c.UnitType.name);
                    break;
                case "pid":
                    model.CampaignAmounts = model.CampaignAmounts.OrderBy(c => c.Pid).ThenBy(c => c.AffiliateName).ThenBy(c => c.UnitType.name);
                    break;
                case "affiliate":
                    model.CampaignAmounts = model.CampaignAmounts.OrderBy(c => c.AffiliateName).ThenBy(c => c.AdvertiserName).ThenBy(c => c.CampaignName).ThenBy(c => c.UnitType.name);
                    break;
                default:
                    model.Sort = "advertiser";
                    model.CampaignAmounts = model.CampaignAmounts.OrderBy(c => c.AdvertiserName).ThenBy(c => c.CampaignName).ThenBy(c => c.AffiliateName).ThenBy(c => c.UnitType.name);
                    break;
            }
            return View(model);
        }

        public ActionResult UnitTypeDropDown(string name, string selected)
        {
            var unitTypeList = mainRepo.UnitTypeList;
            ViewBag.Name = name;
            ViewBag.Selected = selected;
            return PartialView(unitTypeList);
        }

        public ActionResult ChangeUnitType(string itemIds, int unitTypeId)
        {
            if (!securityRepo.IsAccountantOrAdmin(User))
                return Content("unauthorized");

            var itemIdStringArray = itemIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var itemIdInts = itemIdStringArray.Select(i => Convert.ToInt32(i));
            mainRepo.ChangeUnitType(itemIdInts, unitTypeId);

            return Content("ok");
        }

        // ---

        [HttpGet]
        public ActionResult EditCampaign(int pid)
        {
            var campaign = mainRepo.GetCampaign(pid);
            if (campaign == null)
                return Content("Campaign not found");
            return View(campaign);
        }

        [HttpPost]
        public ActionResult EditCampaign(Campaign campaign)
        {
            if (mainRepo.SaveCampaign(campaign))
                return Content("Saved. You may now close this tab.");
            else
                return null;
        }

        [HttpGet]
        public ActionResult MarginApproval(int pid, int[] affid, DateTime? period = null)
        {
            if (period.HasValue)
            {
                eomEntitiesConfig.CurrentEomDate = period.Value.FirstDayOfMonth();
                var url = Url.Action("MarginApproval", new { pid = pid }) + String.Join("", affid.Select(a => "&affid=" + a));
                return Redirect(url);
            }

            var campaign = mainRepo.GetCampaign(pid);
            if (campaign == null)
                return Content(String.Format("Campaign {0} not found", pid));

            int unfinalizedStatusId = CampaignStatus.Default;
            var campaignAmounts = mainRepo.CampaignAmounts(pid, unfinalizedStatusId);
            campaignAmounts = campaignAmounts.Where(ca => ca.AffId.HasValue && affid.Contains(ca.AffId.Value));

            var caList = campaignAmounts.ToList();
            Session["MarginApproval_CampaignAmounts"] = caList;

            decimal? minimumMargin = daMain1Repo.GetSettingDecimalValue("FinalizationWorkflow_MinimumMargin");
            var model = new CampaignAffiliateAmountsModel()
            {
                CurrentEomDateString = eomEntitiesConfig.CurrentEomDateString,
                AdvertiserId = campaign.advertiser_id,
                AdvertiserName = campaign.Advertiser.name,
                CampaignAmounts = campaignAmounts,
                CampaignStatusId = unfinalizedStatusId,
                ShowMarginCheckboxes = true,
                MinimumMarginPct = (minimumMargin.HasValue ? minimumMargin.Value / 100 : (decimal?)null)
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult MarginApproval(string[] idpairs2)
        {
            if (idpairs2 == null)
                return Content("Nothing approved. Please click \"Back\".");

            var campaignAmounts = (List<CampaignAmount>)Session["MarginApproval_CampaignAmounts"];
            if (campaignAmounts == null)
                return Content("Error retrieving CampaignAmounts from session. Please try again.");

            var now = DateTime.Now;
            var campAffIds = Util.ExtractCampAffIds(idpairs2);
            foreach (var campAffId in campAffIds)
            {
                var comment = Request.Form["comment" + campAffId.pid + "_" + campAffId.affid + "_" + campAffId.CostCurrId];

                var c = campaignAmounts.Where(ca => ca.Pid == campAffId.pid && ca.AffId == campAffId.affid && ca.CostCurrency.id == campAffId.CostCurrId).FirstOrDefault();
                if (c != null)
                {
                    var marginApproval = new MarginApproval
                    {
                        pid = c.Pid,
                        affid = c.AffId,
                        revenue_currency_id = c.RevenueCurrency.id,
                        total_revenue = c.Revenue,
                        cost_currency_id = c.CostCurrency.id,
                        total_cost = c.Cost,
                        comment = comment,
                        added_by = User.Identity.Name,
                        created = now,
                        margin_pct = c.MarginPct
                    };
                    mainRepo.SaveMarginApproval(marginApproval);
                }
            }
            mainRepo.SaveChanges();

            return Content("Approvals saved. Please close this window and return to the EOM Tool to finalize.");
        }

    }
}
