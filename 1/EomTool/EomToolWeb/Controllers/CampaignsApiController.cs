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

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="search">a search string</param>
        /// <param name="country">a country code</param>
        /// <param name="vertical">a vertical</param>
        /// <param name="traffictype">a traffic type</param>
        /// <param name="pid">if specified, other parameters will be ignored</param>
        /// <returns></returns>
        public IQueryable<CampaignViewModel> Get(string search, string country, string vertical, string traffictype, int? pid)
        {
            IQueryable<Campaign> campaigns;

            if (pid != null)
            {
                campaigns = campaignRepository.Campaigns.Where(c => c.Pid == pid.Value);
            }
            else
            {
                var settings = SessionUtility.WikiSettings;
                var excludeStrings = settings.ExcludeStrings().ToArray();
                campaigns = campaignRepository.CampaignsFiltered(excludeStrings, search, country, vertical, traffictype, settings.ExcludeHidden, settings.ExcludeInactive);

                var isValidCountry = campaignRepository.Countries.Where(c => c.CountryCode == country).Any();

                if (isValidCountry) // show campaigns for the specified country first, then multi-country campaigns
                    campaigns = campaigns.OrderBy(c => c.Countries.Count() > 1).ThenBy(c => c.Name);
                else
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
