using System;
using System.Linq;
using System.Web.Mvc;

using DirectAgents.Domain.Entities.MatchPortal;
using DirectAgents.Web.Areas.MatchPortal.Helpers;
using DirectAgents.Web.Areas.MatchPortal.Models;

namespace DirectAgents.Web.Areas.MatchPortal.Controllers
{
    public class ProductsController : Controller
    {
        public ActionResult Index()
        {
            return View(new MatchFilterVM());
        }

        [HttpPost]
        public ActionResult Index(MatchFilterVM filter, string action)
        {
            // TODO: Replace action param with a new control action
            if (action == "filter")
            {
                Session["ProductsController.MatchFilter"] = filter;
                filter.IsFilterApplied = true;

                // TODO: Apply filter and update model, extend model to store filter results
                // var filterResult = _filterService.ApplyMatchFilter(filter);
                // return View(filterResult);
            }
            if (action == "match")
            {
                return RedirectToAction("Match", "Products", new { id = 1 });
            }
            return View(filter);
        }

        public ActionResult Match(int? id)
        {
            // TODO: Get filter from session and find result, save next and previous Id
            // var product = _dataContext.BuymaHandbags.FirstOrDefault(x => x.Id == id);
            // var productIds = ((MatchFilterVM)Session["ProductsController.MatchFilter"]).Results.ToList();
            // var nextId = productIds
            //     .SkipWhile(i => i != id)
            //     .Skip(1)
            //     .FirstOrDefault();
            // var previousId = productIds
            //     .TakeWhile(i => i != id)
            //     .LastOrDefault();
            // ViewBag.NextId = nextId;
            // ViewBag.PreviousId = previousId;
            var product = MpTempDataContext.Procucts.FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Match(BuymaHandbag model, int? id)
        {
            // TODO: Save matching result into db and redirect to a new product, replace BuymaHandbag model with a relevant
            // _productMatchingService.SaveMatch(model);
            // var product = _dataContext.BuymaHandbags.FirstOrDefault(x => x.Id == id);
            // var productIds = ((MatchFilterVM)Session["ProductsController.MatchFilter"]).Results.ToList();
            // var nextId = productIds
            //     .SkipWhile(i => i != id)
            //     .Skip(1)
            //     .FirstOrDefault();
            // var previousId = productIds
            //     .TakeWhile(i => i != id)
            //     .LastOrDefault();
            // ViewBag.NextId = nextId;
            // ViewBag.PreviousId = previousId;
            var product = MpTempDataContext.Procucts.FirstOrDefault(x => x.Id == id);
            return View(product);
        }

        public ActionResult Result()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Result(ResultFilterVM filter)
        {
            // TODO: Get results from db
            // var filterResults = _filterService.ApplyResultsFilter(filter);
            // filter.Results = filterResults;
            Session["ProductsController.ResultsFilter"] = filter;
            filter.Results = new[]
                            {
                                new MatchResultVM
                                {
                                    Brand = "test",
                                    MatchedStatus = "matched",
                                    NewProductTitle = "new title",
                                    MatchingDate = DateTime.Today,
                                    OldProductTitle = "old title",
                                    ProductDescription = "description",
                                    ProductId = 1,
                                    ProductImage =
                                        "https://static-buyma-com.akamaized.net/imgdata/item/191122/0049220739/228872974/org.jpg",
                                    Url = "https://www.buyma.us/items/fe7591d8-c3e8-47c5-ba7d-29c9e2675e7d/",
                                },
                            };
            return View(filter);
        }
    }
}