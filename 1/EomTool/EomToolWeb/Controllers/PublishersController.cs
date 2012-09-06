using System;
using System.Linq;
using System.Web.Mvc;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;
using System.Net.Mail;
using EomTool.Domain.Entities;
using DAgents.Common;
using System.Web.Configuration;

namespace EomToolWeb.Controllers
{
    public class PublishersController : Controller
    {
        public int PageSize = 25;
        private IAffiliateRepository affiliateRepository;
        private IMainRepository mainRepository;

        public PublishersController(IAffiliateRepository affiliateRepository, IMainRepository mainRepository)
        {
            this.affiliateRepository = affiliateRepository;
            this.mainRepository = mainRepository;
        }

        public ActionResult List(int page=1)
        {
            AffiliatesListViewModel viewModel = new AffiliatesListViewModel
            {
                Affiliates = affiliateRepository.Affiliates.OrderBy(a => a.name).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = affiliateRepository.Affiliates.Count()
                }
            };
            return View(viewModel);
        }

        public ActionResult Approve(int affid, string note, bool releasing)
        {
            mainRepository.ApproveItemsByAffId(affid, note, User.Identity.Name);
            if (releasing)
            {
                var affiliate = affiliateRepository.AffiliateByAffId(affid);
                SendReleaseEmail(affiliate, note, User.Identity.Name);
            }
            if (Request.IsAjaxRequest())
                return Content(releasing ? "(released)" : "(approved)");
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

        static void SendReleaseEmail(Affiliate affiliate, string note, string mediaBuyer)
        {
            string from = WebConfigurationManager.AppSettings["EmailFromDefault"];
            string to = WebConfigurationManager.AppSettings["EmailToEOM"];
            string subject = "Payments Released by Media Buyer";
            string body = (@"The following payments were released on [[Date]]:
<p>
<b>Media Buyer:</b> [[MediaBuyer]]<br />
<b>Affiliate:</b> [[Affiliate]]<br />
<b>Affid:</b> [[Affid]]<br />
<b>Note:</b> [[Note]]
</p>")
               .Replace("[[Date]]", DateTime.Now.ToString())
               .Replace("[[MediaBuyer]]", mediaBuyer)
               .Replace("[[Affiliate]]", affiliate.name2)
               .Replace("[[Affid]]", affiliate.affid.ToString())
               .Replace("[[Note]]", note);

            EmailUtility.SendEmail(from, new string[] { to }, new string[] { }, subject, body, true);
        }
    }
}
