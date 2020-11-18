using System;

namespace DirectAgents.Domain.Entities.CPSearch
{
    public interface ISearchGauge
    {
        DateTime? MinDaySum { get; set; }
        DateTime? MaxDaySum { get; set; }
        DateTime? MinConvSum { get; set; }
        DateTime? MaxConvSum { get; set; }
        DateTime? MinCallSum { get; set; }
        DateTime? MaxCallSum { get; set; }
        DateTime? MinVidSum { get; set; }
        DateTime? MaxVidSum { get; set; }
    }
}
