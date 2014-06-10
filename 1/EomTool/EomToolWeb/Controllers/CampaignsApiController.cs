using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Concrete;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.Wiki;
using EomToolWeb.Models;
using System.Linq;
using System.Web.Http;

namespace EomToolWeb.Controllers
{
    public class CampaignsApiController : ApiController
    {
        private ICampaignRepository campaignRepository;
        private IMainRepository mainRepo;
        // TODO: used ninject
        //public CampaignsApiController(ICampaignRepository campaignRepository)
        //{
        //    this.campaignRepository = campaignRepository;
        //}
        public CampaignsApiController()
        {
            this.campaignRepository = new CampaignRepository(new WikiContext());
            this.mainRepo = new MainRepository(new DAContext());
        }

        public IQueryable<CampaignViewModel> Get()
        {
            return Get(null, null, null, null, null, null);
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
        public IQueryable<CampaignViewModel> Get(string search, string country, string vertical, string traffictype, string mobilelp, int? pid)
        {
            IQueryable<Campaign> campaigns;

            if (pid != null)
            {
                campaigns = campaignRepository.Campaigns.Where(c => c.Pid == pid.Value);
            }
            else
            {
                var settings = SessionUtility.WikiSettings;
                if (search != null && search.ToLower() == "cpm")
                    settings.ExcludeCPM = false;

                var excludeStrings = settings.ExcludeStrings().ToArray();
                bool? mobilelpBool = (mobilelp == null) ? (bool?)null : (mobilelp.ToLower() == "yes");
                campaigns = campaignRepository.CampaignsFiltered(excludeStrings, search, country, vertical, traffictype, mobilelpBool, settings.ExcludeHidden, settings.ExcludeInactive);

                var isValidCountry = campaignRepository.Countries.Where(c => c.CountryCode == country).Any();

                if (isValidCountry) // show campaigns for the specified country first, then multi-country campaigns
                    campaigns = campaigns.OrderBy(c => c.Countries.Count() > 1).ThenBy(c => c.Name);
                else
                    campaigns = campaigns.OrderBy(c => c.Name);
            }

            var query = campaigns
                       .AsEnumerable()
                       .Select(c => new CampaignViewModel(c, mainRepo.GetOffer(c.Pid, false, true)))
                       .AsQueryable();

            return query;
        }
    }
}
