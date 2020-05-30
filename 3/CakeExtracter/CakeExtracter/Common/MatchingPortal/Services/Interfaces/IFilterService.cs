using System.Collections.Generic;
using System.Collections.ObjectModel;

using CakeExtracter.Common.MatchingPortal.Models;

namespace CakeExtracter.Common.MatchingPortal.Services.Interfaces
{
    /// <summary>
    /// Service to provide filtered data.
    /// </summary>
    public interface IFilterService
    {
        /// <summary>
        /// A method filter data by <see cref="MatchFilter"/>.
        /// </summary>
        /// <param name="filter">Model of filter params.</param>
        /// <returns>Collection of the filtered results.</returns>
        IReadOnlyCollection<int> ApplyMatchFilter(MatchFilter filter);

        /// <summary>
        /// A method filter data by <see cref="ResultFilter"/>.
        /// </summary>
        /// <param name="filter">Model of filter params.</param>
        /// <returns>Collection of the filtered results.</returns>
        IReadOnlyCollection<MatchResult> ApplyResultsFilter(ResultFilter filter);
    }
}