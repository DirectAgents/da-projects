using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomTool.Domain.Concrete;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class PayoutsController : Controller
    {
        public int PageSize = 25;
        private IMainRepository mainRepository;
        public PayoutsController(IMainRepository mainRepository)
        {
            this.mainRepository = mainRepository;
        }

        public ActionResult List(string mediabuyer, int page=1)
        {
            //testing
            if (String.IsNullOrWhiteSpace(mediabuyer))
                mediabuyer = "Lyle Srebnick";

            var payouts = mainRepository.PublisherPayouts.Where(p => p.Status == "Finalized");
            if (!String.IsNullOrWhiteSpace(mediabuyer))
                payouts = payouts.Where(p => p.Media_Buyer == mediabuyer);
            PayoutsListViewModel viewModel = new PayoutsListViewModel
            {
                Payouts = payouts.OrderBy(p => p.Publisher).ThenBy(p => p.Campaign_Name).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = payouts.Count()
                }
            };
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
