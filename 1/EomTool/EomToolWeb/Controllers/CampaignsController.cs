using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.DTO;
using DirectAgents.Domain.Entities.Wiki;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class CampaignsController : Controller
    {
        private ICampaignRepository campaignRepo;
        private IMainRepository mainRepo;

        public CampaignsController(ICampaignRepository campaignRepository, IMainRepository mainRepository)
        {
            this.campaignRepo = campaignRepository;
            this.mainRepo = mainRepository;
        }

        // non-Kendo
        public ActionResult List(string searchstring, string country, string vertical, string traffictype, string mobilelp, int? pid)
        {
            if (string.IsNullOrWhiteSpace(searchstring)) searchstring = null;
            var viewModel = new CampaignsListViewModel
            {
                Pid = pid,
                SearchString = searchstring,
                MobileLP = mobilelp
            };

            IQueryable<Campaign> campaigns;
            if (pid != null)
            {
                campaigns = campaignRepo.Campaigns.Where(c => c.Pid == pid.Value);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(country))
                    viewModel.Country = campaignRepo.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
                if (viewModel.Country == null) country = null;

                if (!string.IsNullOrWhiteSpace(vertical))
                    viewModel.Vertical = campaignRepo.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
                if (viewModel.Vertical == null) vertical = null;

                if (!string.IsNullOrWhiteSpace(traffictype))
                    viewModel.TrafficType = campaignRepo.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
                if (viewModel.TrafficType == null) traffictype = null;

                var excludeStrings = WikiSettings.ExcludeStrings().ToArray();
                bool? mobilelpBool = (mobilelp == null) ? (bool?)null : (mobilelp.ToLower() == "yes");

                campaigns = campaignRepo.CampaignsFiltered(excludeStrings, searchstring, country, vertical, traffictype, mobilelpBool, WikiSettings.ExcludeHidden, WikiSettings.ExcludeInactive);

                if (viewModel.Country != null) // show campaigns for the specified country first, then multi-country campaigns
                    campaigns = campaigns.OrderBy(c => c.Countries.Count() > 1).ThenBy(c => c.Name);
                else
                    campaigns = campaigns.OrderBy(c => c.Name);
            }
            viewModel.CampaignVMs = campaigns.AsEnumerable().Select(c => new CampaignViewModel(c, mainRepo.GetOffer(c.Pid, false, true)));
            return View(viewModel);
        }

        private void SetMode(string mode)
        {
            Session["ListViewMode"] = (mode.ToLower() == "brief") ?
                new ListViewMode()
                {
                    TemplateName = "Brief",
                    ItemsPerPage = 1000,
                    EditWidth = 1100,
                    EditHeight = 840, // 880? if can't put two on one line
                } :
                new ListViewMode() // default: "List" mode
                {
                    TemplateName = "List",
                    ItemsPerPage = 30,
                    EditWidth = 800,
                    EditHeight = 530,
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
        public ActionResult List2(string search, string pid, string country, string vertical, string traffictype, string mobilelp, string mode)
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
                if (search.ToLower() == "cpm")
                {
                    var settings = SessionUtility.WikiSettings;
                    settings.ExcludeCPM = false;
                }
            }
            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                viewModel.Pid = pidInt;
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                viewModel.Country = campaignRepo.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(vertical))
            {
                viewModel.Vertical = campaignRepo.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(traffictype))
            {
                viewModel.TrafficType = campaignRepo.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
            }
            if (!string.IsNullOrWhiteSpace(mobilelp))
            {
                viewModel.MobileLP = mobilelp;
            }
            return View(viewModel);
        }

        public ActionResult ListByCountry()
        {
            List<Country> countries;
            if (WikiSettings.ExcludeInactive)
                countries = campaignRepo.Countries.ToList();
            else
                countries = campaignRepo.CountriesWithActiveCampaigns.ToList();

            var excludeStrings = WikiSettings.ExcludeStrings().ToArray();
            foreach (var country in countries)
            {
                country.Campaigns = country.FilteredCampaigns(excludeStrings, WikiSettings.ExcludeHidden, WikiSettings.ExcludeInactive).ToList();
            }
            countries = countries.Where(c => c.Campaigns.Count() > 0).OrderByDescending(c => c.Campaigns.Count()).ThenBy(c => c.CountryCode).ToList();
            return View(countries);
        }

        public ActionResult ListSortable(string sort, bool? desc, string vertical, string traffictype, string mobilelp, bool? inner)
        {
            var viewModel = new CampaignsListViewModel()
            {
                Sort = sort,
                SortDesc = desc,
                MobileLP = mobilelp
            };
            if (!string.IsNullOrWhiteSpace(vertical))
                viewModel.Vertical = campaignRepo.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
            if (viewModel.Vertical == null) vertical = null;

            if (!string.IsNullOrWhiteSpace(traffictype))
                viewModel.TrafficType = campaignRepo.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
            if (viewModel.TrafficType == null) traffictype = null;

            bool? mobilelpBool = (mobilelp == null) ? (bool?)null : (mobilelp.ToLower() == "yes");

            var excludeStrings = WikiSettings.ExcludeStrings().ToArray();
            var campaigns = campaignRepo.CampaignsFiltered(excludeStrings, null, null, vertical, traffictype, mobilelpBool, WikiSettings.ExcludeHidden, WikiSettings.ExcludeInactive);
            var campaignVMs = campaigns.AsEnumerable().Select(c => new CampaignViewModel(c, mainRepo.GetOffer(c.Pid, false, true)));

            bool descending = desc.HasValue && desc.Value;
            switch (sort)
            {
                case "budget":
                    campaignVMs = descending ? campaignVMs.OrderByDescending(c => c.Budget) : campaignVMs.OrderBy(c => c.Budget);
                    break;
                case "availbudget":
                    campaignVMs = descending ? campaignVMs.OrderByDescending(c => c.AvailableBudget) : campaignVMs.OrderBy(c => c.AvailableBudget);
                    break;
                case "budgetend":
                    campaignVMs = descending ? campaignVMs.OrderByDescending(c => c.BudgetEnd) : campaignVMs.OrderBy(c => c.BudgetEnd);
                    break;
                default: // "name"
                    sort = "name";
                    campaignVMs = descending ? campaignVMs.OrderByDescending(c => c.Name) : campaignVMs.OrderBy(c => c.Name);
                    break;
            }

            viewModel.CampaignVMs = campaignVMs;
            if (inner.HasValue && inner.Value)
                return PartialView("ListSortableInner", viewModel);
            else
                return View(viewModel);
        }

        // old
        public ActionResult Search()
        {
            var countryCodes = campaignRepo.AllCountryCodes;
            return View(countryCodes);
        }
        // old
        public ActionResult ShowCountries()
        {
            var countryCodes = campaignRepo.AllCountryCodes;
            return View(countryCodes);
        }

        // non-Kendo
        public ActionResult Show(int pid)
        {
            var campaign = campaignRepo.FindById(pid);
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
            var campaign = campaignRepo.FindById(pid);
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
                var existingCampaign = campaignRepo.FindById(campaign.Pid);
                if (existingCampaign != null)
                {
                    TryUpdateModel(existingCampaign);
                    campaignRepo.SaveChanges();
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
            var c = campaignRepo.FindById(campaign.Pid);
            if (c != null)
            {
                c.PayableAction = campaign.PayableAction;
                c.ImportantDetails = campaign.ImportantDetails;
                c.Budget = campaign.Budget;
                c.PassedInfo = campaign.PassedInfo;
                c.CampaignCap = campaign.CampaignCap;
                c.ScrubPolicy = campaign.ScrubPolicy;
                c.EomNotes = campaign.EomNotes;
//                c.MobileAllowed = campaign.MobileAllowed;
                c.MobileLP = campaign.MobileLP;
                campaignRepo.SaveChanges();
            }
            var campaignViewModel = (c != null) ? new CampaignViewModel(c) : null;
            var json = Json(campaignViewModel);
            return json;
        }

        public ActionResult Top(TopCampaignsBy by, string traffictype)
        {
            TrafficType trafficTypeEntity = null;

            if (traffictype != null)
                trafficTypeEntity = campaignRepo.TrafficTypes.SingleOrDefault(t => t.Name == traffictype);

            IEnumerable<CampaignSummary> campaignSummaries;

            if (trafficTypeEntity == null) // no matching traffic type
                campaignSummaries = campaignRepo.TopCampaigns(20, by, null);
            else
                campaignSummaries = campaignRepo.TopCampaigns(20, by, traffictype);

            var model = new TopViewModel { CampaignSummaries = campaignSummaries, By = by, TrafficType = trafficTypeEntity };

            return View(model);
        }

        WikiSettings WikiSettings { get { return _WikiSettings.Value; } }
        Lazy<WikiSettings> _WikiSettings = new Lazy<WikiSettings>(() => SessionUtility.WikiSettings);
    }
}
