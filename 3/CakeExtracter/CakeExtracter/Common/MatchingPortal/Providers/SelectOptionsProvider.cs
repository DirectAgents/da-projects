using System.Collections;
using System.Collections.Generic;
using System.Linq;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;

using WatiN.Core;

namespace CakeExtracter.Common.MatchingPortal.Providers
{
    public static class SelectOptionsProvider
    {
        private static readonly object RequestLocker = new object();

        public static IEnumerable<SearchResult> GetSearchResults(string buymaId)
        {
            IEnumerable<SearchResult> results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetSearchResultsById(dbContext, buymaId),
                RequestLocker,
                $"Get SearchResults by Id - {buymaId}");
            return results;
        }

        public static IEnumerable<string> GetCategories()
        {
            IEnumerable<string> categoryList = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => { categoryList = dbContext.MatchingProducts.Select(x => x.Category).Distinct().ToList(); },
                RequestLocker,
                $"Get SearchResults by Id");
            return categoryList;
        }

        public static IEnumerable<string> GetBrands()
        {
            IEnumerable<string> brandList = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => { brandList = dbContext.MatchingProducts.Select(x => x.Brand).Distinct().ToList(); },
                RequestLocker,
                $"Get SearchResults by Id");
            return brandList;
        }

        private static IEnumerable<SearchResult> GetSearchResultsById(MatchPortalContext dbContext, string buymaId)
        {
            var query = dbContext.MatchingProducts.Where(x => x.BuymaId == buymaId);
            var resultList = query.ToList();
            var results = resultList.Select(AutoMapper.Mapper.Map<SearchResult>).ToList();
            return results;
        }

        public static IEnumerable<string> GetMatchedStatuses()
        {
            return new[] { "Matched", "No Match" };
        }
    }
}