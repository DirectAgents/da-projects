using System;

namespace DirectAgents.Domain.Entities.CPProg
{
    public class DatedStatsSummary : StatsSummary, IDatedObject
    {
        public DateTime Date { get; set; }
    }
}