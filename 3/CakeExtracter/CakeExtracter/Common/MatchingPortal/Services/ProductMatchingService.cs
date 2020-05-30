using System;
using System.Linq;

using CakeExtracter.Common.MatchingPortal.Models;
using CakeExtracter.Common.MatchingPortal.Services.Interfaces;
using CakeExtracter.Helpers;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.MatchPortal;

namespace CakeExtracter.Common.MatchingPortal.Services
{
    /// <inheritdoc />
    public class ProductMatchingService : IProductMatchingService
    {
        private static readonly object RequestLocker = new object();

        /// <inheritdoc />
        public void SaveMatch(Product product)
        {
            var matchingResult = AutoMapper.Mapper.Map<MatchingResult>(product);
            matchingResult.MatchedDate = DateTime.Now;
            if (matchingResult.SearchResultId == 0)
            {
                matchingResult.SearchResultId = null;
            }

            UpsertMatchingResult(matchingResult);
        }

        /// <inheritdoc />
        public Product GetProduct(int id)
        {
            Product results = null;
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext => results = GetProductById(dbContext, id),
                RequestLocker,
                $"Get Product by Id - {id}");
            return results;
        }

        private Product GetProductById(MatchPortalContext dbContext, int id)
        {
            var product = dbContext.MatchingProducts.FirstOrDefault(x => (int)x.UId == id);
            var result = AutoMapper.Mapper.Map<Product>(product);
            return result;
        }

        private static void UpsertMatchingResult(MatchingResult matchingResult)
        {
            SafeContextWrapper.TryMakeTransactionWithLock<MatchPortalContext>(
                dbContext =>
                {
                    dbContext.MatchingResults.AddOrUpdateExtension(matchingResult);
                    dbContext.SaveChanges();
                },
                RequestLocker,
                $"Upsert MatchingResult Id - {matchingResult.BuymaId}");
        }
    }
}