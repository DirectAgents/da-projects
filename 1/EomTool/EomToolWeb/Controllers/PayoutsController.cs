using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomToolWeb.Models;
using EomTool.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class PayoutsController : Controller
    {
        public int PageSize = 15;
        private IMainRepository mainRepository;
        public PayoutsController(IMainRepository mainRepository)
        {
            this.mainRepository = mainRepository;
        }

        public ActionResult List(string mediabuyer, string mode, int page=1)
        {
            //testing
            if (Request["test"] != null)
                Session["TestMode"] = Request["test"];
            if (String.IsNullOrWhiteSpace(mediabuyer))
                mediabuyer = "Lyle Srebnick";

            var payouts = mainRepository.PublisherPayouts;
            PayoutsListViewModel viewModel = new PayoutsListViewModel();
            if (mode == "preapproval")
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Default || p.status_id == CampaignStatus.Active
                    || (p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id < MediaBuyerApprovalStatus.Sent));
            }
            else if (mode == "held")
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Hold);
            }
            else if (mode == "approved")
            {
                payouts = payouts.Where(p => (p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Approved)
                    || p.status_id == CampaignStatus.Verified);
            }
            else // normal view - show payouts needing action
            {
                payouts = payouts.Where(p => p.status_id == CampaignStatus.Finalized && p.media_buyer_approval_status_id == MediaBuyerApprovalStatus.Sent);
                viewModel.ShowActions = true;
            }

            if (!String.IsNullOrWhiteSpace(mediabuyer))
                payouts = payouts.Where(p => p.Media_Buyer == mediabuyer);
            
            viewModel.Payouts = payouts.OrderBy(p => p.Publisher).ThenBy(p => p.Campaign_Name).Skip((page - 1) * PageSize).Take(PageSize);
            viewModel.PagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = payouts.Count()
            };
            viewModel.TestMode = (Session["TestMode"] != null && Session["TestMode"].ToString() == "1");

            return View(viewModel);
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
