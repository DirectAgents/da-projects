using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DirectAgents.Domain.Abstract;
using EomToolWeb.Models;
using DirectAgents.Domain.Entities;

namespace EomToolWeb.Controllers
{
    public class CampaignsController : Controller
    {
        private ICampaignRepository campaignRepository;

        public CampaignsController(ICampaignRepository campaignRepository)
        {
            this.campaignRepository = campaignRepository;
        }

        public ActionResult List(string country, string pid)
        {
            var campaigns = campaignRepository.Campaigns;
            if (!string.IsNullOrWhiteSpace(country))
            {
                campaigns = campaigns.Where(c => c.Countries.Contains(country));
//                campaigns = campaigns.Where(c => c.CountryCodes.Contains(country));
            }
            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                campaigns = campaigns.Where(c => c.Pid == pidInt);
            }
            return View(campaigns);
        }

        public ActionResult ListByCountry()
        {
            var campaigns = campaignRepository.Campaigns;
            var countryCodes = campaignRepository.AllCountryCodes;
            var model = new CampaignsByCountryViewModel(campaigns, countryCodes);
            return View(model);
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
