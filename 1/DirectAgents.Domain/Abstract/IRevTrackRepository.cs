using System;
using System.Linq;
using DirectAgents.Domain.Entities.RevTrack;

namespace DirectAgents.Domain.Abstract
{
    public interface IRevTrackRepository : IDisposable
    {
        void SaveChanges();

        IQueryable<Client> Clients();
        IQueryable<Vendor> Vendors();
    }
}
