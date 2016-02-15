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
            CreateMaps();
        }

        public static void CreateMaps()
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

            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.Conv, DirectAgents.Domain.Entities.TD.Conv>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.DailySummary, DirectAgents.Domain.Entities.TD.DailySummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.StrategySummary, DirectAgents.Domain.Entities.TD.StrategySummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.TDadSummary, DirectAgents.Domain.Entities.TD.TDadSummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.SiteSummary, DirectAgents.Domain.Entities.TD.SiteSummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.TD.Conv, DirectAgents.Domain.Entities.TD.Conv>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.AdRoll.AdDailySummary, DirectAgents.Domain.Entities.AdRoll.AdDailySummary>();
            Mapper.CreateMap<DirectAgents.Domain.Entities.DBM.CreativeDailySummary, DirectAgents.Domain.Entities.DBM.CreativeDailySummary>();
        }

        // Called from the commands' RunStatic() methods
        // TODO? A static method that checks all bootstrappers. e.g. ServicePointBootstrapper ... DefaultConnectionLimit
        public static void CheckRunSetup()
        {
            if (Mapper.GetAllTypeMaps().Length == 0)
            {
                CreateMaps();
            }
        }
    }
}