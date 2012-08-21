using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EomTool.Domain.Concrete;
using EomTool.Domain.Abstract;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class PublishersController : Controller
    {
        public int PageSize = 10;
        private IAffiliateRepository affiliatesRepository;
        public PublishersController(IAffiliateRepository affiliatesRepository)
        {
            this.affiliatesRepository = affiliatesRepository;
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

    }
}
