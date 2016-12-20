using System;
using System.Collections.Generic;
using DirectAgents.Domain.DTO;

namespace DirectAgents.Domain.Abstract
{
    public interface IDepartmentRepository
    {
        IEnumerable<IRTLineItem> StatsByClient(DateTime monthStart, bool includeZeros = false, int? maxClients = null);
    }
}
