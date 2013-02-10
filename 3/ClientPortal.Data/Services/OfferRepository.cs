using ClientPortal.Data.Contexts;
using ClientPortal.Data.Contracts;
using System.Linq;

namespace ClientPortal.Data.Services
{
    public class OfferRepository : IOfferRepository
    {
        CakeContext cakeContext;

        public OfferRepository(CakeContext cakeContext)
        {
            this.cakeContext = cakeContext;
        }

        public IQueryable<IOffer> GetAll()
        {
            return cakeContext.CakeOffers;
        }
    }
}
