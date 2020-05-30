using AutoMapper;

using CakeExtracter.Common.MatchingPortal.Models;
using DirectAgents.Domain.Entities.MatchPortal;

namespace DirectAgents.Web
{
    public class AutoMapperConfig
    {
        public static void CreateMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MatchingProduct, Product>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => (int)s.UId))
                    .ForMember(d => d.ProductImageLink, opt => opt.MapFrom(s => s.BuymaImageUrl))
                    .ForMember(d => d.OriginalProductTitle, opt => opt.MapFrom(s => s.OldTitle))
                    .ForMember(d => d.ProductId, opt => opt.MapFrom(s => s.BuymaId));

                cfg.CreateMap<MatchingProduct, SearchResult>()
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                    .ForMember(d => d.ImageLink, opt => opt.MapFrom(s => s.ImageUrl))
                    .ForMember(d => d.Headline, opt => opt.MapFrom(s => s.SrTitle))
                    .ForMember(d => d.ResultLink, opt => opt.MapFrom(s => s.SerItemUrl));

                cfg.CreateMap<Product, MatchingResult>()
                    .ForMember(d => d.BuymaId, opt => opt.MapFrom(s => s.ProductId))
                    .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.NewProductTitle))
                    .ForMember(d => d.SearchResultId, opt => opt.MapFrom(s => s.MatchedResultId));
            });
        }

    }
}