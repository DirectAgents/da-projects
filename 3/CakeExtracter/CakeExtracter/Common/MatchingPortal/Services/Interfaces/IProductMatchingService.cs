using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.MatchingPortal.Services.Interfaces
{
    /// <summary>
    /// Service to provide matching functionality.
    /// </summary>
    public interface IProductMatchingService
    {
        /// <summary>
        /// A method save result of product matching.
        /// </summary>
        /// <param name="product">Result of product matching.</param>
        void SaveMatch(Product product);

        /// <summary>
        /// A method to get product for matching.
        /// </summary>
        /// <param name="id">Buyma product unique identifier.</param>
        /// <returns>Product for matching.</returns>
        Product GetProduct(int id);
    }
}