using System;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Entities;
using System.Data.Objects.SqlClient;
using System.Collections.Generic;
using Cake.Model;
using System.Net;

namespace DirectAgents.Domain.Concrete
{
    public class AdminImpl : IAdmin
    {
        public void CreateDatabaseIfNotExists()
        {
            using (var context = new EFDbContext())
            {
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                }
            }
        }

        public void ReCreateDatabase()
        {
            using (var context = new EFDbContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                context.Database.Create();
            }
        }

        public void LoadCampaigns()
        {
            using (var cake = new Cake.Model.Staging.CakeStagingEntities())
            using (var daDomain = new EFDbContext())
            {

                foreach (var item in from offer in cake.CakeOffers.ToList()
                                     from advertiser in cake.CakeAdvertisers.ToList()
                                     where advertiser.Advertiser_Id == int.Parse(offer.Advertiser_Id)
                                     select new { Offer = offer, Advertiser = advertiser })
                {
                    var campaign = daDomain.Campaigns.FirstOrDefault(c => c.Pid == item.Offer.Offer_Id);
                    if (campaign == null)
                    {
                        campaign = new Campaign
                        {
                            Pid = item.Offer.Offer_Id,
                        };
                        daDomain.Campaigns.Add(campaign);
                    }
                    campaign.Name = item.Offer.OfferName;
                    campaign.TrafficType = item.Offer.AllowedMediaTypeNames;

                    campaign.Countries.Clear();
                    var countryCodes = item.Offer.AllowedCountries.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    foreach (var countryCode in countryCodes)
                    {
                        var country = daDomain.Countries.Where(c => c.CountryCode == countryCode).FirstOrDefault();
                        if (country == null)
                        {
                            country = new Country { CountryCode = countryCode, Name = countryCode + "." };
                        }
                        campaign.Countries.Add(country);
                    }

                    var am = daDomain.People.Where(p => p.Name == item.Advertiser.AccountManagerName).FirstOrDefault();
                    if (am == null)
                    {
                        am = new Person { Name = item.Advertiser.AccountManagerName };
                    }
                    // TODO: support multiple accountmangers
                    if (campaign.AccountManagers != null)
                    {
                        campaign.AccountManagers.Clear();
                        campaign.AccountManagers.Add(am);
                    }

                    var ad = daDomain.People.Where(p => p.Name == item.Advertiser.AdManagerName).FirstOrDefault();
                    if (ad == null)
                    {
                        ad = new Person { Name = item.Advertiser.AdManagerName };
                    }
                    // TODO: support multiple admangers
                    if (campaign.AdManagers != null)
                    {
                        campaign.AdManagers.Clear();
                        campaign.AdManagers.Add(ad);
                    }

                    var offer = (Cake.Data.Wsdl.ExportService.offer1)item.Offer;
                    if (offer != null)
                    {
                        campaign.Cost = offer.offer_contracts[0].payout.amount;
                        campaign.ImageUrl = offer.offer_image_link;
                        campaign.Description = string.IsNullOrWhiteSpace(offer.offer_description) ? "no description" : offer.offer_description;
                        campaign.Link = offer.offer_contracts[0].offer_link;
                    }
                    if (string.IsNullOrWhiteSpace(campaign.PayableAction))
                        campaign.PayableAction = "Not Specified";

                    daDomain.SaveChanges(); // Do this here so that any people that were created can be reused for other campaigns.
                }
            }
        }
    }
}
