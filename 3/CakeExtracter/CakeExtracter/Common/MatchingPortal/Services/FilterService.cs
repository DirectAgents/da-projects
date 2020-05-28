using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;
using CakeExtracter.Helpers;

using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.MatchPortal;

namespace CakeExtracter.Common.MatchingPortal.Services
{
    public class FilterService : IFilterService
    {
        private static readonly object RequestLocker = new object();

        public IReadOnlyCollection<int> ApplyMatchFilter(MatchFilter filter)
        {
            IReadOnlyCollection<int> results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetMatchFilterResults(dbContext, filter),
                RequestLocker,
                "Applying match filter");
            return results;
        }

        public IReadOnlyCollection<MatchResult> ApplyResultsFilter(ResultFilter filter)
        {
            IReadOnlyCollection<MatchResult> results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetResultFilterResults(dbContext, filter),
                RequestLocker,
                "Applying result filter");
            return results;
        }

        private IReadOnlyCollection<int> GetMatchFilterResults(MatchPortalContext dataContext, MatchFilter filter)
        {
            IQueryable<MatchingProduct> query = dataContext.MatchingProducts;

            if (!string.IsNullOrWhiteSpace(filter.ProductId))
            {
                query = query.Where(x => x.BuymaId == filter.ProductId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
            {
                query = query.Where(x => x.OldTitle.Contains(filter.ProductTitle));
            }

            if (filter.Brands?.Any() ?? false)
            {
                query = query.Where(x => filter.Brands.Contains(x.Brand));
            }

            if (filter.Categories?.Any() ?? false)
            {
                query = query.Where(x => filter.Categories.Contains(x.Category));
            }

            var result = query.Select(x => x.UId).Distinct().ToList();
            filter.IsFilterApplied = true;
            filter.IsBrandMatched = "NO";
            filter.NumberOfProductsToMatch = result.Count;
            return result;
        }

        private IReadOnlyCollection<MatchResult> GetResultFilterResults(MatchPortalContext dataContext, ResultFilter filter)
        {
            IQueryable<MatchingResult> query = dataContext.MatchingResults;

            if (!string.IsNullOrWhiteSpace(filter.ProductId))
            {
                query = query.Where(x => x.BuymaId == filter.ProductId);
            }

            if (!string.IsNullOrWhiteSpace(filter.ProductTitle))
            {
                query = query.Where(x => x.SearchResult.OldTitle.Contains(filter.ProductTitle));
            }

            if (filter.Brands?.Any() ?? false)
            {
                query = query.Where(x => filter.Brands.Contains(x.SearchResult.Brand));
            }

            if (filter.Categories?.Any() ?? false)
            {
                query = query.Where(x => filter.Categories.Contains(x.SearchResult.Category));
            }

            if ((filter.MatchedStatus?.Any() ?? false) && filter.MatchedStatus.Length == 1)
            {
                if (filter.MatchedStatus.Contains("Matched"))
                {
                    query = query.Where(x => x.SearchResultId != null && x.SearchResultId != 0);
                }

                if (filter.MatchedStatus.Contains("No Match"))
                {
                    query = query.Where(x => x.SearchResultId == null || x.SearchResultId == 0);
                }
            }

            if (filter.DateRangeFrom != null)
            {
                query = query.Where(x => x.MatchedDate >= filter.DateRangeFrom);
            }

            if (filter.DateRangeTo != null)
            {
                query = query.Where(x => x.MatchedDate <= filter.DateRangeTo);
            }

            var results = new List<MatchResult>();

            foreach (var item in query.ToList())
            {
                var product = dataContext.MatchingProducts.FirstOrDefault(x => x.BuymaId == item.BuymaId);
                var matchResult = new MatchResult
                                  {
                                      ProductId = item.BuymaId,
                                      MatchingDate = item.MatchedDate,
                                      ProductDescription = item.ProductDescription,
                                      ProductImage = product.BuymaImageUrl,
                                      OldProductTitle = product.OldTitle,
                                      NewProductTitle = item.ProductName,
                                      Brand = product.Brand,
                                      Category = product.Category,
                                      MatchedStatus = item.SearchResultId == null || item.SearchResultId == 0 ? "No Match" : "Matched",
                                      Url = product.SerItemUrl,
                                      RedirectId = product.UId,
                                  };
                results.Add(matchResult);
            }

            return results;
        }
    }
}