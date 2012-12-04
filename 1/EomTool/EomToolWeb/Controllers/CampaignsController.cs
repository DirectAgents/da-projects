using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class CampaignsController : Controller
    {
        private ICampaignRepository campaignRepository;

        public CampaignsController(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        // non-Kendo
        public ActionResult List(string searchstring, string pid, string country, string vertical, string traffictype)
        {
            var viewModel = new CampaignsListViewModel();
            var campaigns = campaignRepository.Campaigns.Where(c => c.StatusId != Status.Inactive);

            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                viewModel.Pid = pidInt;
                campaigns = campaignRepository.Campaigns.Where(c => c.Pid == pidInt);
            }

            if (!string.IsNullOrWhiteSpace(searchstring))
            {
                viewModel.SearchString = searchstring;
                campaigns = campaigns.Where(c => c.Name.Contains(searchstring) || c.Description.Contains(searchstring));
            }
            if (!string.IsNullOrWhiteSpace(vertical))
            {
                viewModel.Vertical = campaignRepository.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
                if (viewModel.Vertical != null)
                    campaigns = campaigns.Where(c => c.Vertical.Name == vertical);
            }
            if (!string.IsNullOrWhiteSpace(traffictype))
            {
                viewModel.TrafficType = campaignRepository.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
                if (viewModel.TrafficType != null)
                    campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(traffictype));
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                viewModel.Country = campaignRepository.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
            }
            if (viewModel.Country != null)
            {
                campaigns = campaigns.Where(camp => camp.Countries.Select(c => c.CountryCode).Contains(country))
                    .OrderBy(c => c.Countries.Count() > 1)
                    .ThenBy(c => c.Name);
            }
            else
            {
                campaigns = campaigns.OrderBy(c => c.Name);
            }

            viewModel.Campaigns = campaigns.AsEnumerable();
            return View(viewModel);
        }

        private void SetMode(string mode)
        {
            Session["ListViewMode"] = (mode.ToLower() == "brief") ?
                new ListViewMode()
                {
                    TemplateName = "Brief",
                    ItemsPerPage = 500,
                    EditHeight = 750,
                    EditWidth = 1100,
                } :
                new ListViewMode() // default: "List" mode
                {
                    TemplateName = "List",
                    ItemsPerPage = 30,
                    EditHeight = 500,
                    EditWidth = 800,
                };
        }
        private ListViewMode GetMode()
        {
            var listViewMode =  Session["ListViewMode"] as ListViewMode;
            if (listViewMode == null)
            {
                SetMode("List");
            }
            return Session["ListViewMode"] as ListViewMode;
        }

        // for Kendo
        public ActionResult List2(string search, string pid, string country, string vertical, string traffictype, string mode)
        {
            if (!string.IsNullOrWhiteSpace(mode))
                SetMode(mode);
            var viewModel = new CampaignsListViewModel()
            {
                ListViewMode = GetMode()
            };

            if (!string.IsNullOrWhiteSpace(search))
            {
                viewModel.SearchString = search;
            }
            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                viewModel.Pid = pidInt;
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                viewModel.Country = campaignRepository.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(vertical))
            {
                viewModel.Vertical = campaignRepository.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(traffictype))
            {
                viewModel.TrafficType = campaignRepository.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
            }
            return View(viewModel);
        }

        private IEnumerable<SelectListItem> ExcludeSelectList(string[] excludeStrings)
        {
            string[] excludes = new string[] { "PAUSED", "NOT LIVE YET", "CPM" };
            IEnumerable<SelectListItem> selectList =
                from e in excludes
                select new SelectListItem
                {
                    Selected = excludeStrings.Contains(e),
                    Text = e,
                    Value = e
                };
            return selectList;
        }

        public ActionResult ListByCountry(string[] exclude)
        {
            if (exclude == null) exclude = new string[] { };

            var countries = campaignRepository.CountriesWithActiveCampaigns;
            var model = countries.ToList();
            foreach (var country in model)
            {
                var filteredCampaigns = country.ActiveCampaigns;
                foreach (var excludeString in exclude)
                {
                    filteredCampaigns = filteredCampaigns.Where(c => !c.Name.Contains(excludeString));
                }
                country.Campaigns = filteredCampaigns.ToList();
            }
            model = model.OrderByDescending(c => c.Campaigns.Count()).ThenBy(c => c.CountryCode).ToList();
            ViewBag.Exclude = ExcludeSelectList(exclude);
            return View(model);
        }

        // old
        public ActionResult Search()
        {
            var countryCodes = campaignRepository.AllCountryCodes;
            return View(countryCodes);
        }
        // old
        public ActionResult ShowCountries()
        {
            var countryCodes = campaignRepository.AllCountryCodes;
            return View(countryCodes);
        }

        // non-Kendo
        public ActionResult Show(int pid)
        {
            var campaign = campaignRepository.FindById(pid);
            if (campaign == null)
            {
                return Content("campaign not found");
            }
            return PartialView(new CampaignViewModel(campaign));
        }

        // non-Kendo
        [HttpGet]
        public ActionResult Edit(int pid)
        {
            var campaign = campaignRepository.FindById(pid);
            if (campaign == null)
            {
                return Content("campaign not found");
            }
            return PartialView(campaign);
        }

        // non-Kendo
        [HttpPost]
        public ActionResult Edit(Campaign campaign)
        {
            if (ModelState.IsValid)
            {
                var existingCampaign = campaignRepository.FindById(campaign.Pid);
                if (existingCampaign != null)
                {
                    TryUpdateModel(existingCampaign);
                    campaignRepository.SaveChanges();
                }
                // else... set a ModelState error?
            }

            if (Request.IsAjaxRequest())
                return Json(new { IsValid = ModelState.IsValid });
            else
                return View(campaign);
        }

        // for Kendo
        [HttpPost]
        public ActionResult Edit2(Campaign campaign)
        {
            var c = campaignRepository.FindById(campaign.Pid);
            if (c != null)
            {
                c.PayableAction = campaign.PayableAction;
                c.ImportantDetails = campaign.ImportantDetails;
                c.Budget = campaign.Budget;
                c.PassedInfo = campaign.PassedInfo;
                c.CampaignCap = campaign.CampaignCap;
                c.ScrubPolicy = campaign.ScrubPolicy;
                c.EomNotes = campaign.EomNotes;
                campaignRepository.SaveChanges();
            }
            var campaignViewModel = (c != null) ? new CampaignViewModel(c) : null;
            var json = Json(campaignViewModel);
            return json;
        }

        public ActionResult Top(TopCampaignsBy by, string traffictype)
        {
            TrafficType trafficTypeEntity = null;

            if (traffictype != null)
                trafficTypeEntity = campaignRepository.TrafficTypes.SingleOrDefault(t => t.Name == traffictype);

            IEnumerable<CampaignSummary> campaignSummaries;

            if (trafficTypeEntity == null) // no matching traffic type
                campaignSummaries = campaignRepository.TopCampaigns(20, by, null);
            else
                campaignSummaries = campaignRepository.TopCampaigns(20, by, traffictype);

            var model = new TopViewModel { CampaignSummaries = campaignSummaries, By = by, TrafficType = trafficTypeEntity };

            return View(model);
        }
    }
}
