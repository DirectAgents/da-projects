using DirectAgents.Domain.Entities.Cake;
using System;
using System.Collections.Generic;

namespace DirectAgents.Domain.Abstract
{
    public interface IMainRepository : IDisposable
    {
        IEnumerable<Advertiser> GetAdvertisers();
    }
}
