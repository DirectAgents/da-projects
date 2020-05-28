using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.MatchingPortal.Services.Interfaces
{
    public interface IProductMatchingService
    {
        void SaveMatch(Product product);

        Product GetProduct(double id);
    }
}