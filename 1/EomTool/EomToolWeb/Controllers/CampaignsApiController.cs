using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Entities;
using EomToolWeb.Models;

namespace EomToolWeb.Controllers
{
    public class CampaignsApiController : ApiController
    {
        private ICampaignRepository campaignRepository;
        // TODO: used ninject
        //public CampaignsApiController(ICampaignRepository campaignRepository)
        //{
        //    this.campaignRepository = campaignRepository;
        //}
        public CampaignsApiController()
        {
            EFDbContext db = new EFDbContext();
            this.campaignRepository = new CampaignRepository(db);
        }

        public IQueryable<CampaignViewModel> Get()
        {
            return Get(null, null, null, null, null);
        }

        public IQueryable<CampaignViewModel> Get(string vertical, string traffictype, string search, string country, int? pid)
        {
            var excludeStrings = SessionUtility.WikiSettings.ExcludeStrings().ToArray();
            var campaigns = campaignRepository.CampaignsExcluding(excludeStrings).Where(c => c.StatusId != Status.Inactive);

            if (pid != null)
            {
                campaigns = campaignRepository.Campaigns.Where(c => c.Pid == pid.Value);
            }

            if (!string.IsNullOrWhiteSpace(vertical))
            {
                var cr = campaignRepository.Verticals.Where(v => v.Name == vertical).FirstOrDefault();
                if (cr != null)
                    campaigns = campaigns.Where(c => c.Vertical.Name == vertical);
            }

            if (!string.IsNullOrWhiteSpace(traffictype))
            {
                var tt = campaignRepository.TrafficTypes.Where(t => t.Name == traffictype).FirstOrDefault();
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
    }
}
