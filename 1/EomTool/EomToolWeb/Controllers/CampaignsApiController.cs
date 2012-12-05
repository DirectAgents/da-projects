using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class CampaignsApiController : ApiController
    {
        private EFDbContext db = new EFDbContext();
        private IQueryable<Campaign> Campaigns(bool includeInactive)
        {
            IQueryable<Campaign> campaigns = null;
            if (includeInactive)
                campaigns = db.Campaigns.AsQueryable();
            else
                campaigns = db.Campaigns.Where(c => c.StatusId != Status.Inactive);

            var settings = SessionUtility.WikiSettings;
            foreach (var excludeString in settings.ExcludeStrings())
            {
                campaigns = campaigns.Where(c => !c.Name.Contains(excludeString));
            }
            return campaigns;
        }

        public IQueryable<CampaignViewModel> Get()
        {
            var query = Campaigns(false)
                          .OrderBy(c => c.Name)
                          .AsEnumerable()
                          .Select(c => new CampaignViewModel(c));

            return query.AsQueryable();
        }

        public IQueryable<CampaignViewModel> Get(string vertical, string traffictype, string search, string country, int? pid)
        {
            var campaigns = Campaigns(false);

            if (pid != null)
            {
                campaigns = Campaigns(true).Where(c => c.Pid == pid.Value);
            }

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

            if (!string.IsNullOrWhiteSpace(search))
            {
                campaigns = campaigns.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                campaigns = campaigns.Where(camp => camp.Countries.Select(c => c.CountryCode).Contains(country))
                       .OrderBy(c => c.Countries.Count() > 1)
                       .ThenBy(c => c.Name);
            }
            else
            {
                campaigns = campaigns.OrderBy(c => c.Name);
            }

            var query = campaigns
                       .AsEnumerable()
                       .Select(c => new CampaignViewModel(c))
                       .AsQueryable();

            return query;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
