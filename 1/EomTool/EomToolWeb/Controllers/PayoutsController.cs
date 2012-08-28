using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomToolWeb.Models;
using EomTool.Domain.Entities;
using System.Reflection;

namespace EomToolWeb.Controllers
{
    public class PayoutsController : Controller
    {
        public int PageSize = 15;
        private IMainRepository mainRepository;
        private ISecurityRepository securityRepository;
        public PayoutsController(IMainRepository mainRepository, ISecurityRepository securityRepository)
        {
            this.mainRepository = mainRepository;
            this.securityRepository = securityRepository;
        }

        public ActionResult Summary(string mode)
        {
            var groups = securityRepository.GroupsByWindowsIdentity(User.Identity.Name);
            var roleNames = groups.SelectMany(g => g.Roles).Select(r => r.Name).ToArray();
            var affIds = mainRepository.AffiliatesForMediaBuyers(roleNames).Select(a => a.affid).ToList();

            var model = new PublisherSummaryViewModel
            {
                PublisherSummaries = mainRepository.PublisherSummariesByMode(mode).Where(ps => affIds.Contains(ps.affid)).OrderBy(p => p.PublisherName),
                Mode = mode
            };

            return View(model);
        }

        public ActionResult List(string mode, int? affid, int page = 1)
        {
            var model = DoList(mode, affid, page);
            return View(model);
        }

        public ActionResult ListPartial(string mode, int? affid, int page = 1)
        {
            var model = DoList(mode, affid, page);
            return PartialView("PayoutsGrid", model);
        }

        private PayoutsListViewModel DoList(string mode, int? affid, int page)
        {
            //testing
            if (Request["test"] != null)
                Session["TestMode"] = Request["test"];

            var groups = securityRepository.GroupsByWindowsIdentity(User.Identity.Name);
            var roleNames = groups.SelectMany(g => g.Roles).Select(r => r.Name).ToArray();
            var affIds = mainRepository.AffiliatesForMediaBuyers(roleNames).Select(a => a.affid).ToList();

            if (affid != null)
                // TESTING
                affIds = (new int[] { affid.Value }).ToList();
            //affIds = affIds.Where(a => a == affid).ToList();

            var payouts = mainRepository.PublisherPayoutsByMode(mode).Where(p => affIds.Contains(p.affid));

            PayoutsListViewModel viewModel = new PayoutsListViewModel();

            if (string.IsNullOrWhiteSpace(mode))
            {
                viewModel.ShowActions = true;
            }

            viewModel.Payouts = payouts.OrderBy(p => p.Publisher).ThenBy(p => p.Campaign_Name).Skip((page - 1) * PageSize).Take(PageSize);
            viewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = payouts.Count()
            };
            viewModel.TestMode = (Session["TestMode"] != null && Session["TestMode"].ToString() == "1");

            // HACK: converting publisher name2 field to publisher name fields by splitting on left paren
            //       a lookup using affid might be better but then again we don't want to tie a publisher name to a single affid...
            string publisherName = viewModel.Payouts.First().Publisher.Split('(').First();

            viewModel.PublisherReport = MvcHtmlString.Create(GetPublisherReport(publisherName));

            return viewModel;
        }

        private string GetPublisherReport(string publisherName)
        {
            var table = new Eom.Common.PublisherReportDataSet1.CampaignsPublisherReportDetailsDataTable();
            var details = this.mainRepository.CampaignPublisherReportDetails.Where(c => c.Publisher == publisherName);
            foreach (var detail in details)
            {
                var row = table.NewCampaignsPublisherReportDetailsRow();
                Copy(detail, row, false);

            }
            //table.NewCampaignsPublisherReportDetailsRow()
            return "foo";
        }

        static void Copy(object sourceObject, object targetObject, bool deepCopy = true)
        {
            if (sourceObject != null && targetObject != null)
            {
                (from sourceProperty in sourceObject.GetType().GetProperties().AsEnumerable()
                 from targetProperty in targetObject.GetType().GetProperties().AsEnumerable()
                 where sourceProperty.Name == targetProperty.Name
                 let sourceValue = sourceProperty.GetValue(sourceObject, null)
                 let targetValue = targetProperty.GetValue(targetObject, null)
                 where sourceValue != null && !sourceValue.Equals(targetValue)
                 select Action(targetProperty, targetObject, sourceValue, deepCopy))
                .ToList()
                .ForEach(c => c());
            }
        }

        static Action Action(PropertyInfo propertyInfo, object targetObject, object sourceValue, bool deepCopy)
        {
            Action action;
            if (sourceValue == null)
                action = () => { };
            else if (!deepCopy || sourceValue.GetType().FullName.StartsWith("System."))
                action = () => propertyInfo.SetValue(targetObject, sourceValue, null);
            else
                action = () => Copy(sourceValue, propertyInfo.GetValue(targetObject, null));
            return action;
        }

        public ActionResult Approve(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            mainRepository.Media_ApproveItems(itemIdsArray);

            return RedirectToAction("List");
        }

        public ActionResult Hold(string itemids)
        {
            int[] itemIdsArray = itemids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            mainRepository.Media_HoldItems(itemIdsArray);

            return RedirectToAction("List");
        }
    }
}
