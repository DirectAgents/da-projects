using System.ComponentModel.Composition;
using AutoMapper;
using ClientPortal.Data.Contexts;

namespace CakeExtracter.Bootstrappers
{
    [Export(typeof(IBootstrapper))]
    public class AutoMapperBootstrapper : IBootstrapper
    {
        public void Run()
        {
            Mapper.CreateMap<SearchDailySummary, SearchDailySummary>();
            Mapper.CreateMap<SearchDailySummary2, SearchDailySummary2>();
            //Mapper.CreateMap<OfferDailySummary, OfferDailySummary>();
            Mapper.CreateMap<GoogleAnalyticsSummary, GoogleAnalyticsSummary>();
            Mapper.CreateMap<CallDailySummary, CallDailySummary>();

            Mapper.CreateMap<ClientPortal.Data.Entities.TD.DBM.CreativeDailySummary, ClientPortal.Data.Entities.TD.DBM.CreativeDailySummary>();
            Mapper.CreateMap<ClientPortal.Data.Entities.TD.DBM.DBMDailySummary, ClientPortal.Data.Entities.TD.DBM.DBMDailySummary>();
            Mapper.CreateMap<ClientPortal.Data.Entities.TD.DBM.DBMConversion, ClientPortal.Data.Entities.TD.DBM.DBMConversion>();
            Mapper.CreateMap<ClientPortal.Data.Entities.TD.DBM.UserListStat, ClientPortal.Data.Entities.TD.DBM.UserListStat>();
            Mapper.CreateMap<ClientPortal.Data.Entities.TD.AdRoll.AdDailySummary, ClientPortal.Data.Entities.TD.AdRoll.AdDailySummary>();

            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.DailySummary, DirectAgents.Domain.Entities.TD.DailySummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.AdRoll.AdDailySummary, DirectAgents.Domain.Entities.AdRoll.AdDailySummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.DBM.CreativeDailySummary, DirectAgents.Domain.Entities.DBM.CreativeDailySummary>();
        }
    }
}