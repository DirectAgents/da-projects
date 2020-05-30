using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Providers;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.MatchPortal;

namespace CakeExtracter.Common.MatchingPortal.Services
{
    /// <inheritdoc/>
    public class FilterService : IFilterService
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc/>
        public IReadOnlyCollection<int> ApplyMatchFilter(MatchFilter filter)
        {
            IReadOnlyCollection<int> results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetMatchFilterResults(dbContext, filter),
                RequestLocker,
                "Applying match filter");
            return results;
        }

        /// <inheritdoc/>
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
            var matchedItems = dataContext.MatchingResults.Select(x => x.BuymaId);

            var query = dataContext.MatchingProducts.Where(x => !matchedItems.Contains(x.BuymaId));

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

            var result = query.Select(x => (int)x.UId).Distinct().ToList();
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
                query = query.Where(x => x.ProductName.Contains(filter.ProductTitle));
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
                var dateRangeTo = filter.DateRangeTo.Value.AddDays(1);
                query = query.Where(x => x.MatchedDate < dateRangeTo);
            }

            var results = GetResultsByQuery(dataContext, query);

            if (filter.Brands?.Any() ?? false)
            {
                results = results.Where(x => filter.Brands.Contains(x.Brand)).ToList();
            }

            if (filter.Categories?.Any() ?? false)
            {
                results = results.Where(x => filter.Categories.Contains(x.Category)).ToList();
            }

            double matchedCount = results.Count(x => x.MatchedStatus == SelectOptionsProvider.MatchedStatusValue);
            filter.NumberOfResults = results.Count;
            filter.PercentMatched = matchedCount / filter.NumberOfResults;
            return results;
        }

        private static IReadOnlyCollection<MatchResult> GetResultsByQuery(MatchPortalContext dataContext, IQueryable<MatchingResult> query)
        {
            var results = new List<MatchResult>();

            foreach (var item in query.ToList())
            {
                var product = dataContext.MatchingProducts.First(x => x.BuymaId == item.BuymaId);
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
                    MatchedStatus =
                      item.SearchResultId == null || item.SearchResultId == 0
                          ? SelectOptionsProvider.NotMatchedStatusValue
                          : SelectOptionsProvider.MatchedStatusValue,
                    Url = product.SerItemUrl,
                    RedirectId = (int)product.UId,
                };
                results.Add(matchResult);
            }

            return results;
        }
    }
}