using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;

namespace CakeExtracter.Common.MatchingPortal.Providers
{
    /// <summary>
    /// A class to provide options for dropdowns, list-boxes, radiobuttons, etc.
    /// </summary>
    public static class SelectOptionsProvider
    {
        /// <summary>
        /// A display value for matched status.
        /// </summary>
        public const string MatchedStatusValue = "Matched";

        /// <summary>
        /// A display value for not matched status.
        /// </summary>
        public const string NotMatchedStatusValue = "No Match";

        private const string NotValuableResult = "https://www.buyma.com";

        private static readonly object RequestLocker = new object();

        /// <summary>
        /// A method provide search results by buyma_id.
        /// </summary>
        /// <param name="buymaId">buyma_id of the product to match.</param>
        /// <returns>Collection of the search results.</returns>
        public static IEnumerable<SearchResult> GetSearchResults(string buymaId)
        {
            IEnumerable<SearchResult> results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetSearchResultsById(dbContext, buymaId),
                RequestLocker,
                $"Get SearchResults by Id - {buymaId}");
            return results;
        }

        /// <summary>
        /// A method provide collection of the product categories.
        /// </summary>
        /// <returns>Collection of the product categories.</returns>
        public static IEnumerable<string> GetCategories()
        {
            IEnumerable<string> categoryList = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => { categoryList = dbContext.MatchingProducts.Select(x => x.Category).Distinct().ToList(); },
                RequestLocker,
                $"Get SearchResults by Id");
            return categoryList;
        }

        /// <summary>
        /// A method provide collection of the product brands.
        /// </summary>
        /// <returns>Collection of the product brands.</returns>
        public static IEnumerable<string> GetBrands()
        {
            IEnumerable<string> brandList = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => { brandList = dbContext.MatchingProducts.Select(x => x.Brand).Distinct().ToList(); },
                RequestLocker,
                $"Get SearchResults by Id");
            return brandList;
        }

        /// <summary>
        /// A method provide collection of the matched status.
        /// </summary>
        /// <returns>Collection of the matched status.</returns>
        public static IEnumerable<string> GetMatchedStatuses()
        {
            return new[] { MatchedStatusValue, NotMatchedStatusValue };
        }

        private static IEnumerable<SearchResult> GetSearchResultsById(MatchPortalContext dbContext, string buymaId)
        {
            var query = dbContext.MatchingProducts.Where(x => x.BuymaId == buymaId && !x.SerItemUrl.Contains(NotValuableResult));
            var resultList = query.ToList();
            var results = resultList.Select(AutoMapper.Mapper.Map<SearchResult>).ToList();
            return results;
        }
    }
}