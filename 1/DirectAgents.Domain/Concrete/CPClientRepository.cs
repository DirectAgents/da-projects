using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DirectAgents.Domain.Abstract;
using DirectAgents.Domain.Contexts;
using DirectAgents.Domain.Entities.CPSearch;

namespace DirectAgents.Domain.Concrete
{
    public partial class CPSearchRepository
    {
        public IQueryable<ClientReport> ClientReports()
        {
            return context.ClientReports;
        }

        public ClientReport GetClientReport(int id)
        {
            return context.ClientReports.Find(id);
        }

        public bool SaveClientReport(ClientReport clientReport, bool saveIfExists = true, bool createIfDoesntExist = false)
        {
            if (context.ClientReports.Any(x => x.Id == clientReport.Id))
            {
                if (saveIfExists)
                {
                    var entry = context.Entry(clientReport);
                    entry.State = EntityState.Modified;
                    context.SaveChanges();
                    return true;
                }
            }
            else if (createIfDoesntExist)
            {
                context.ClientReports.Add(clientReport);
                context.SaveChanges();
                return true;
            }
            return false; // not saved/created
        }

        public bool DeleteClientReport(int id)
        {
            var rep = context.ClientReports.Find(id);
            if (rep == null)
                return false;
            context.ClientReports.Remove(rep);
            context.SaveChanges();
            return true;
        }
    }
}
