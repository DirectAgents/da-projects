using System;
using System.ComponentModel.Composition;
using AutoMapper;
using ClientPortal.Data.Contexts;
using DirectAgents.Domain.Entities.CPProg;

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
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SearchDailySummary, SearchDailySummary>();
                cfg.CreateMap<SearchDailySummary2, SearchDailySummary2>();
                //cfg.CreateMap<OfferDailySummary, OfferDailySummary>();
                cfg.CreateMap<GoogleAnalyticsSummary, GoogleAnalyticsSummary>();
                cfg.CreateMap<CallDailySummary, CallDailySummary>();
                cfg.CreateMap<SearchConvSummary, SearchConvSummary>();

                cfg.CreateMap<ClientPortal.Data.Entities.TD.DBM.CreativeDailySummary, ClientPortal.Data.Entities.TD.DBM.CreativeDailySummary>();
                cfg.CreateMap<ClientPortal.Data.Entities.TD.DBM.DBMDailySummary, ClientPortal.Data.Entities.TD.DBM.DBMDailySummary>();
                cfg.CreateMap<ClientPortal.Data.Entities.TD.DBM.DBMConversion, ClientPortal.Data.Entities.TD.DBM.DBMConversion>();
                cfg.CreateMap<ClientPortal.Data.Entities.TD.DBM.UserListStat, ClientPortal.Data.Entities.TD.DBM.UserListStat>();
                cfg.CreateMap<ClientPortal.Data.Entities.TD.AdRoll.AdDailySummary, ClientPortal.Data.Entities.TD.AdRoll.AdDailySummary>();

                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.DailySummary, DirectAgents.Domain.Entities.CPProg.DailySummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.StrategySummary, DirectAgents.Domain.Entities.CPProg.StrategySummary>()
                    .ForMember(s => s.StrategyName, opt => opt.Ignore())
                    .ForMember(s => s.StrategyEid, opt => opt.Ignore());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.StrategySummary, DirectAgents.Domain.Entities.CPProg.Strategy>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.StrategyId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.StrategyName))
                    .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.StrategyEid))
                    .ForMember(d => d.Type, opt => opt.MapFrom(s => new EntityType {Name = s.StrategyType}));
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.AdSetSummary, DirectAgents.Domain.Entities.CPProg.AdSetSummary>()
                    .ForMember(s => s.AdSetName, opt => opt.Ignore())
                    .ForMember(s => s.AdSetEid, opt => opt.Ignore())
                    .ForMember(s => s.StrategyName, opt => opt.Ignore())
                    .ForMember(s => s.StrategyEid, opt => opt.Ignore());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.TDadSummary, DirectAgents.Domain.Entities.CPProg.TDadSummary>()
                    .ForMember(s => s.TDadName, opt => opt.Ignore())
                    .ForMember(s => s.TDadEid, opt => opt.Ignore());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SiteSummary, DirectAgents.Domain.Entities.CPProg.SiteSummary>()
                    .ForMember(s => s.SiteName, opt => opt.Ignore());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.Conv, DirectAgents.Domain.Entities.CPProg.Conv>();
                cfg.CreateMap<DirectAgents.Domain.Entities.AdRoll.AdDailySummary, DirectAgents.Domain.Entities.AdRoll.AdDailySummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.DBM.CreativeDailySummary, DirectAgents.Domain.Entities.DBM.CreativeDailySummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.DailySummaryMetric>()
                    .ForMember(s => s.ExtAccount, map => map.AllowNull());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.StrategySummaryMetric>()
                    .ForMember(s => s.Strategy, map => map.AllowNull());
            });
        }

        // Called from the commands' RunStatic() methods
        // TODO? A static method that checks all bootstrappers. e.g. ServicePointBootstrapper ... DefaultConnectionLimit
        public static void CheckRunSetup()
        {
            try
            {
                Mapper.Configuration.AssertConfigurationIsValid();
            }
            //catch (AutoMapperConfigurationException) // didn't get caught
            catch (Exception)
            {
                CreateMaps();
            }
        }
    }
}