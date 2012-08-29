using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EomToolWeb.Controllers
{
    public class PayoutsController : Controller
    {
        public int PageSize = 100;
        private IMainRepository mainRepository;
        private ISecurityRepository securityRepository;
        public PayoutsController(IMainRepository mainRepository, ISecurityRepository securityRepository)
        {
            this.mainRepository = mainRepository;
            this.securityRepository = securityRepository;
        }

        private List<int> AffIdsForCurrentUser()
        {
            var affIds = new List<int>();
            var group = securityRepository.WindowsIdentityGroup(User.Identity.Name);
            if (group != null)
            {
                var roleNames = group.Roles.Select(r => r.Name).ToArray();
                affIds = mainRepository.AffiliatesForMediaBuyers(roleNames).Select(a => a.affid).ToList();
            }
            return affIds;
        }

        public ActionResult Summary(string mode)
        {
            var affIds = AffIdsForCurrentUser();

            var viewModel = new PublisherSummaryViewModel
            {
                PublisherSummaries = mainRepository.PublisherSummariesByMode(mode).Where(ps => affIds.Contains(ps.affid)).OrderBy(p => p.PublisherName),
                Mode = mode
            };

            return View(viewModel);
        }

        public ActionResult Details(string mode, int? affid, int page = 1)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page);
            return PartialView(model);
        }

        public JsonResult DetailsJson(string mode, int? affid, int page = 1)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page);
            return Json(model.Payouts, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List(string mode, int? affid, int page = 1)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page);
            return View(model);
        }

        public ActionResult ListPartial(string mode, int? affid, int page = 1)
        {
            var model = CreatePayoutsListViewModel(mode, affid, page);
            return PartialView("PayoutsGrid", model);
        }

        private PayoutsListViewModel CreatePayoutsListViewModel(string mode, int? affid, int page)
        {
            //testing
            if (Request["test"] != null)
                Session["TestMode"] = Request["test"];

            var affIds = AffIdsForCurrentUser();
            if (affid != null)
                affIds = affIds.Where(a => a == affid).ToList();

            var payouts = mainRepository.PublisherPayoutsByMode(mode).Where(p => affIds.Contains(p.affid));

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

            viewModel.PublisherReport = MvcHtmlString.Create(GetPublisherReport(payouts));

            return viewModel;
        }

        private string GetPublisherReport(IEnumerable<EomTool.Domain.Entities.PublisherPayout> payouts)
        {
            var table = new Eom.Common.PublisherReportDataSet1.CampaignsPublisherReportDetailsDataTable();
            string publisherName = null;

            foreach (var payout in payouts/*.Where(c => c.Pub_Payout > 0)*/)
            {
                var rx = new Regex(@"(?'name'((\w+)\W+)+)\((?'addcode'((CD|CA)(?'cdnumber'([0-9]+))))\)");
                var publisher = rx.Match(payout.Publisher).Groups;
                if (publisherName == null)
                    publisherName = publisher["name"].Value;

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

            // TODO: get the correct FromDate and ToDate (should not be based on current date...)
            var report = new Eom.Common.PublisherReports.PubRepTemplate(Eom.Common.PublisherReports.PubRepTemplateHtmlMode.InnerHtml)
            {
                Publisher = publisherName,
                Data = table,
                FromDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
                ToDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month))
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
