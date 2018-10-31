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
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.DailySummaryMetric>()
                    .ForMember(s => s.ExtAccount, map => map.AllowNull());

                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.StrategySummary, DirectAgents.Domain.Entities.CPProg.StrategySummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.StrategySummary, DirectAgents.Domain.Entities.CPProg.Strategy>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.StrategyId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.StrategyName))
                    .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.StrategyEid))
                    .ForMember(d => d.Type, opt => opt.MapFrom(s => new EntityType {Name = s.StrategyType}));
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.StrategySummaryMetric>()
                    .ForMember(s => s.Strategy, map => map.AllowNull());

                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.AdSetSummary, DirectAgents.Domain.Entities.CPProg.AdSetSummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.AdSetSummary, DirectAgents.Domain.Entities.CPProg.AdSet>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.AdSetId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.AdSetName))
                    .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.AdSetEid))
                    .ForMember(d => d.Strategy, opt => opt.MapFrom(s => new Strategy {Name = s.StrategyName, ExternalId = s.StrategyEid}));
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.AdSetSummaryMetric>()
                    .ForMember(s => s.AdSet, map => map.AllowNull());

                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.TDadSummary, DirectAgents.Domain.Entities.CPProg.TDadSummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.TDadSummary, DirectAgents.Domain.Entities.CPProg.TDad>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.TDadId))
                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.TDadName))
                    .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.TDadEid))
                    .ForMember(d => d.ExternalIds, opt => opt.MapFrom(s => s.ExternalIds))
                    .ForMember(d => d.AdSet, opt => opt.MapFrom(s => new AdSet { Name = s.AdSetName, ExternalId = s.AdSetEid }));
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SummaryMetric, DirectAgents.Domain.Entities.CPProg.TDadSummaryMetric>()
                    .ForMember(s => s.TDad, map => map.AllowNull());

                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.SiteSummary, DirectAgents.Domain.Entities.CPProg.SiteSummary>()
                    .ForMember(s => s.SiteName, opt => opt.Ignore());
                cfg.CreateMap<DirectAgents.Domain.Entities.CPProg.Conv, DirectAgents.Domain.Entities.CPProg.Conv>();
                cfg.CreateMap<DirectAgents.Domain.Entities.AdRoll.AdDailySummary, DirectAgents.Domain.Entities.AdRoll.AdDailySummary>();
                cfg.CreateMap<DirectAgents.Domain.Entities.DBM.CreativeDailySummary, DirectAgents.Domain.Entities.DBM.CreativeDailySummary>();
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