using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IOfferRepository
    {
        IQueryable<IOffer> GetAll();
    }
}
