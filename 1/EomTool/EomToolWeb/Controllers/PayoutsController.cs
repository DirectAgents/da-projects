using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Entities;
using EomToolWeb.Models;
using DAgents.Common;

namespace EomToolWeb.Controllers
{
    public class PayoutsController : Controller
    {
        public int PageSize = 100;
        private IMainRepository mainRepository;
        private ISecurityRepository securityRepository;
        private IDAMain1Repository daMain1Repository;
        private IEomEntitiesConfig eomEntitiesConfig;

        public PayoutsController(IMainRepository mainRepository, ISecurityRepository securityRepository, IDAMain1Repository daMain1Repository, IEomEntitiesConfig eomEntitiesConfig)
        {
            this.mainRepository = mainRepository;
            this.securityRepository = securityRepository;
            this.daMain1Repository = daMain1Repository;
            this.eomEntitiesConfig = eomEntitiesConfig;
        }

        [HttpPost]
        public ActionResult ChooseMonth(DateTime selectedMonth)
        {
            eomEntitiesConfig.CurrentEomDate = selectedMonth;
            return Redirect(Request.UrlReferrer.ToString());
        }

        private List<int> AffIdsForCurrentUser()
        {
            var affIds = new List<int>();
            var groups = securityRepository.GroupsForUser(User);
            if (!groups.Any())
            {
                string ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                groups = securityRepository.GroupsForIpAddress(ipAddress);
                if (groups == null || !groups.Any())
                {
                    ipAddress = WindowsIdentityHelper.GetIpAddress();
                    groups = securityRepository.GroupsForIpAddress(ipAddress);
                }
            }
            if (groups != null && groups.Any())
            {
                var mediaBuyers = groups.SelectMany(g => g.Roles).Distinct().Where(r => r.Name.StartsWith("MB: ")).Select(r => r.Name.Substring(4)).ToArray();
                affIds = mainRepository.AffiliatesForMediaBuyers(mediaBuyers).Select(a => a.affid).ToList();
            }
            return affIds;
        }

        public ActionResult Summary(string mode, bool includeZero = true)
        {
            var affIds = AffIdsForCurrentUser();

            var viewModel = new PublisherSummaryViewModel
            {
                PublisherSummaries = mainRepository.PublisherSummariesByMode(mode, includeZero).Where(ps => affIds.Contains(ps.affid)).OrderBy(p => p.PublisherName),
                Mode = mode,
                IncludeZero = includeZero
            };

            ViewBag.ChooseMonthSelectList = new SelectList(ChooseMonthListItems, "Value", "Text", eomEntitiesConfig.CurrentEomDate.ToString());

            return View(viewModel);
        }

        IEnumerable<SelectListItem> ChooseMonthListItems
        {
            get
            {
                var listItems = from c in daMain1Repository.DADatabases
                                select new SelectListItem
                                {
                                    Text = c.name,
                                    Value = c.effective_date.Value.ToString()
                                };
                return listItems;
            }
        }

        public ActionResult Details(string mode, int? affid, int page = 1)
        {
            return PartialView("PayoutsGrid");
        }

        public JsonResult DetailsJson(string mode, int? affid, int page = 1, bool includeZero = true)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page, includeZero, false);
            return Json(model.Payouts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List(string mode, int? affid, int page = 1, bool includeZero = true)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page, includeZero, false);
            return View(model);
        }

        public ActionResult ListPartial(string mode, int? affid, int page = 1, bool includeZero = true)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page, includeZero, false);
            return PartialView("PayoutsGrid", model);
        }

        public ActionResult PublisherReport(string mode, int? affid, int page = 1)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page, false, true);
            return Content(model.PublisherReport.ToHtmlString());
        }

        private PayoutsListViewModel CreatePayoutsListViewModel(string mode, int? affid, int page, bool includeZero, bool includePublisherReport)
        {
            //testing
            if (Request["test"] != null)
                Session["TestMode"] = Request["test"];

            var affIds = AffIdsForCurrentUser();
            if (affid != null)
                affIds = affIds.Where(a => a == affid).ToList();

            var payouts = mainRepository.PublisherPayoutsByMode(mode, includeZero).Where(p => affIds.Contains(p.affid));

            PayoutsListViewModel viewModel = new PayoutsListViewModel()
            {
                ShowActions = string.IsNullOrWhiteSpace(mode) // default mode, ActionNeeded
            };

            viewModel.Payouts = payouts.OrderBy(p => p.Publisher).ThenBy(p => p.Campaign_Name).Skip((page - 1) * PageSize).Take(PageSize);
            viewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = payouts.Count()
            };
            viewModel.TestMode = (Session["TestMode"] != null && Session["TestMode"].ToString() == "1");

            if (includePublisherReport) viewModel.PublisherReport = MvcHtmlString.Create(GetPublisherReport(payouts));

            return viewModel;
        }

        private string GetPublisherReport(IEnumerable<EomTool.Domain.Entities.PublisherPayout> payouts)
        {
            var table = new Eom.Common.PublisherReportDataSet1.CampaignsPublisherReportDetailsDataTable();
            string publisherName = "";
            string mediaBuyer = "";

            foreach (var payout in payouts.Where(c => c.Pub_Payout != 0))
            {
                var rx = new Regex(@"(?'name'((\w+)\W+)+)\((?'addcode'((CD|CA)(?'cdnumber'([0-9]+))))\)");
                var publisher = rx.Match(payout.Publisher).Groups;
                if (string.IsNullOrEmpty(publisherName))
                    publisherName = publisher["name"].Value;
                if (string.IsNullOrEmpty(mediaBuyer))
                    mediaBuyer = payout.Media_Buyer;

                var row = table.NewCampaignsPublisherReportDetailsRow();
                row.ItemIDs = payout.ItemIds;
                row.IsCPM = payout.Unit_Type == "CPM" ? "Yes" : "No";
                row.Publisher = publisherName;
                row.CampaignStatus = "Verified";
                row.CampaignName = payout.Campaign_Name;
                row.AddCode = publisher["addcode"].Value;
                row.NumUnits = payout.Units ?? 0;
                row.CostPerUnit = row.NumUnits == 0 ? 0 : (payout.Pub_Payout ?? 0) / row.NumUnits;
                row.NetTerms = payout.Net_Terms;
                row.PayCurrency = payout.Pub_Pay_Curr;
                row.MediaBuyer = payout.Media_Buyer;
                row.ToBePaid = payout.Pub_Payout ?? 0;
                row.Paid = 0;
                row.Total = row.ToBePaid + row.Paid;

                table.AddCampaignsPublisherReportDetailsRow(row);
            }

            var eomDate = eomEntitiesConfig.CurrentEomDate;
            var report = new Eom.Common.PublisherReports.PubRepTemplate(Eom.Common.PublisherReports.PubRepTemplateHtmlMode.InnerHtml)
            {
                Publisher = publisherName,
                MediaBuyer = mediaBuyer,
                Data = table,
                FromDate = eomDate,
                ToDate = new DateTime(eomDate.Year, eomDate.Month, DateTime.DaysInMonth(eomDate.Year, eomDate.Month))
            };

            string reportHTML = report.TransformText();

            return reportHTML;
        }

        public ActionResult Approve(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            mainRepository.Media_ApproveItems(itemIdsArray);

            if (Request.IsAjaxRequest())
                return Content("(approved)");
            else
                return RedirectToAction("List");
        }

        public ActionResult Hold(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            mainRepository.Media_HoldItems(itemIdsArray);

            if (Request.IsAjaxRequest())
                return Content("(held)");
            else
                return RedirectToAction("List");
        }
    }
}
