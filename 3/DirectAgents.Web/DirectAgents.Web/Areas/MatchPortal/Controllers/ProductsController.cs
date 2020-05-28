﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;

namespace DirectAgents.Web.Areas.MatchPortal.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IFilterService _filterService;

        private readonly IProductMatchingService _productMatchingService;

        public ProductsController(IFilterService filterService, IProductMatchingService productMatchingService)
        {
            _filterService = filterService;
            _productMatchingService = productMatchingService;
        }

        public ActionResult Index()
        {
            return View(new MatchFilter());
        }

        [HttpPost]
        public ActionResult Index(MatchFilter filter)
        {
            var filterResult = _filterService.ApplyMatchFilter(filter);
            filter.Results = filterResult;
            Session["ProductsController.MatchFilter"] = filter;
            return View(filter);
        }

        public ActionResult Match(int id)
        {
            var product = _productMatchingService.GetProduct(id);
            var productIds = ((MatchFilter)Session["ProductsController.MatchFilter"] ?? new MatchFilter()).Results?.ToList() ?? new List<int>();
            var nextId = GetNextId(id, productIds);
            var previousId = GetPreviousId(id, productIds);

            ViewBag.NextId = nextId;
            ViewBag.PreviousId = previousId;
            return View(product);
        }

        [HttpPost]
        public ActionResult Match(Product model, int id)
        {
            _productMatchingService.SaveMatch(model);
            var productIds = ((MatchFilter)Session["ProductsController.MatchFilter"] ?? new MatchFilter()).Results?.ToList() ?? new List<int>();
            var newId = GetNextId(id, productIds);
            return RedirectToAction("Match", "Products", new { id = newId });
        }

        public ActionResult Result()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result(ResultFilter filter)
        {
            // TODO: Get results from db
            var filterResults = _filterService.ApplyResultsFilter(filter);
            filter.Results = filterResults;
            Session["ProductsController.ResultsFilter"] = filter;
            return View(filter);
        }

        private static int GetNextId(int id, IEnumerable<int> productIds)
        {
            var nextId = productIds.SkipWhile(i => i != id).Skip(1).FirstOrDefault();
            return nextId;
        }

        private static int GetPreviousId(int id, IEnumerable<int> productIds)
        {
            var previousId = productIds.TakeWhile(i => i != id).LastOrDefault();
            return previousId;
        }
    }
}