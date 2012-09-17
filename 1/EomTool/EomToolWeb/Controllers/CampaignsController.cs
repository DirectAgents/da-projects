﻿using System;
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

        public ActionResult List(string country, string pid, string vertical)
        {
            var viewModel = new CampaignsListViewModel();
            var campaigns = campaignRepository.Campaigns;
            
            if (!string.IsNullOrWhiteSpace(country))
            {
                viewModel.Country = campaignRepository.Countries.Where(c => c.CountryCode == country).FirstOrDefault();
                if (viewModel.Country != null)
                    campaigns = campaigns.Where(camp => camp.Countries.Select(c => c.CountryCode).Contains(country));
            }
            int pidInt;
            if (Int32.TryParse(pid, out pidInt))
            {
                campaigns = campaigns.Where(c => c.Pid == pidInt);
            }
            if (!string.IsNullOrWhiteSpace(vertical))
            {
                viewModel.Vertical = campaignRepository.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
                if (viewModel.Vertical != null)
                    campaigns = campaigns.Where(c => c.Vertical.Name == vertical);
            }

            viewModel.Campaigns = campaigns.AsEnumerable();
            return View(viewModel);
        }

        public ActionResult ListByCountry()
        {
            var countries = campaignRepository.Countries.OrderByDescending(c => c.Campaigns.Count());
            return View(countries);
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
