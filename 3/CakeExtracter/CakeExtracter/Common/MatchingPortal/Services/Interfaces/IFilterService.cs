using System.Collections.Generic;
using System.Collections.ObjectModel;

using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.MatchingPortal.Services.Interfaces
{
    public interface IFilterService
    {
        IReadOnlyCollection<int> ApplyMatchFilter(MatchFilter filter);

        IReadOnlyCollection<MatchResult> ApplyResultsFilter(ResultFilter filter);
    }
}