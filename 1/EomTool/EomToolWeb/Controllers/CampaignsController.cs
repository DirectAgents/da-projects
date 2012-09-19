using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using EomToolWeb.Models;
using DirectAgents.Domain.Entities;
using System.Data.Entity;
namespace EomToolWeb.Controllers
{
    public class CampaignsController : Controller
    {
        private ICampaignRepository campaignRepository;

        public CampaignsController(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        public ActionResult List(string searchstring, string pid, string country, string vertical, string traffictype)
        {
            var viewModel = new CampaignsListViewModel();
            var campaigns = campaignRepository.Campaigns;

            if (!string.IsNullOrWhiteSpace(searchstring))
            {
                viewModel.SearchString = searchstring;
                campaigns = campaigns.Where(c => c.Name.Contains(searchstring));
            }
            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                viewModel.Pid = pidInt;
                campaigns = campaigns.Where(c => c.Pid == pidInt);
            }
            if (!string.IsNullOrWhiteSpace(country))
            {
                viewModel.Country = campaignRepository.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
                if (viewModel.Country != null)
                    campaigns = campaigns.Where(camp => camp.Countries.Select(c => c.CountryCode).Contains(country));
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

            viewModel.Campaigns = campaigns.AsEnumerable();
            return View(viewModel);
        }

        public ActionResult ListByCountry()
        {
            var countryCodes = campaignRepository.AllCountryCodes;
            ViewBag.CountryCodes = countryCodes.ToList();
            var countries = campaignRepository.Countries.OrderByDescending(c => c.Campaigns.Count());
            return View(countries);
        }

        public ActionResult Search()
        {
            var countryCodes = campaignRepository.AllCountryCodes;
            return View(countryCodes);
        }

        public ActionResult ShowCountries()
        {
            var countryCodes = campaignRepository.AllCountryCodes;
            return View(countryCodes);
        }

        public ActionResult Show(int pid)
        {
            var campaign = campaignRepository.Campaigns.Where(c => c.Pid == pid).FirstOrDefault();
            if (campaign == null)
            {
                return Content("campaign not found");
            }
            return PartialView(new CampaignViewModel(campaign));
        }

        [HttpGet]
        public ActionResult Edit(int pid)
        {
            var campaign = campaignRepository.Campaigns.Where(c => c.Pid == pid).FirstOrDefault();
            if (campaign == null)
            {
                return Content("campaign not found");
            }
            return PartialView(campaign);
        }

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
                return Json(new {IsValid = ModelState.IsValid});
            else
                return View(campaign);
        }
    }
}
