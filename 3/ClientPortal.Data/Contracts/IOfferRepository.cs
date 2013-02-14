using ClientPortal.Data.Contexts;
using ClientPortal.Data.DTOs;
using System;
using System.Linq;

namespace ClientPortal.Data.Contracts
{
    public interface IOfferRepository
    {
        IQueryable<OfferInfo> GetOfferInfos(DateTime since);
    }
}
