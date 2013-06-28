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
            Mapper.CreateMap<DailySummary, DailySummary>();
        }
    }
}