using System;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;
using System.Text;

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

        public ActionResult ShowNotes(string batchids)
        {
            int[] batchIdsArray = batchids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(id => Convert.ToInt32(id)).ToArray();
            var batches = mainRepository.BatchesByBatchIds(batchIdsArray);
            StringBuilder notes = new StringBuilder();
            foreach (var batch in batches.OrderBy(b => b.id))
            {
                foreach (var batchNote in batch.BatchNotes.OrderBy(bn => bn.date_created))
                {
                    notes.Append("BATCH " + batch.id + "<br/>");
                    notes.Append(batchNote.date_created.ToString() + "<br/>");
                    notes.Append(batchNote.author + "<br/>");
                    notes.Append(batchNote.note + "<br/><br/>");
                }
            }
            return Content(notes.ToString());
        }
    }
}
