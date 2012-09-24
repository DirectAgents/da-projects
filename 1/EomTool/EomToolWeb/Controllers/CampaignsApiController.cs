﻿using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Concrete;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class CampaignsApiController : ApiController
    {
        private EFDbContext db = new EFDbContext();

        public IQueryable<CampaignViewModel> Get()
        {
            var query = db.Campaigns
                          .AsEnumerable()
                          .Select(c => new CampaignViewModel(c));

            return query.AsQueryable();
        }

        public IQueryable<CampaignViewModel> Get(string vertical, string traffictype)
        {
            var campaigns = db.Campaigns.AsQueryable();

            if (!string.IsNullOrWhiteSpace(vertical))
            {
                var cr = db.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
                if (cr != null)
                    campaigns = campaigns.Where(c => c.Vertical.Name == vertical);
            }

            if (!string.IsNullOrWhiteSpace(traffictype))
            {
                var tt = db.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
                if (tt != null)
                    campaigns = campaigns.Where(c => c.TrafficTypes.Select(t => t.Name).Contains(traffictype));
            }

            var query = campaigns
                       .AsEnumerable()
                       .Select(c => new CampaignViewModel(c))
                       .AsQueryable();

            return query.AsQueryable();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
