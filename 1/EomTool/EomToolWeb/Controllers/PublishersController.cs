using System;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class PublishersController : Controller
    {
        public int PageSize = 25;
        private IAffiliateRepository affiliatesRepository;
        private IMainRepository mainRepository;
        public PublishersController(IAffiliateRepository affiliatesRepository, IMainRepository mainRepository)
        {
            this.affiliatesRepository = affiliatesRepository;
            this.mainRepository = mainRepository;
        }

        public ActionResult List(int page=1)
        {
            AffiliatesListViewModel viewModel = new AffiliatesListViewModel
            {
                Affiliates = affiliatesRepository.Affiliates.OrderBy(a => a.name).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = affiliatesRepository.Affiliates.Count()
                }
            };
            return View(viewModel);
        }

        public ActionResult Approve(int affid, string note)
        {
            mainRepository.ApproveItemsByAffId(affid, note, User.Identity.Name);
            if (Request.IsAjaxRequest())
                return Content("(approved)");
            else
                return RedirectToAction("Summary", "Payouts");
        }

        public ActionResult Hold(int affid, string note)
        {
            mainRepository.HoldItemsByAffId(affid, note, User.Identity.Name);
            if (Request.IsAjaxRequest())
                return Content("(held)");
            else
                return RedirectToAction("Summary", "Payouts");
        }
    }
}
