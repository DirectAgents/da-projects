using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Helpers;
using System.Linq;

namespace CakeExtractor.SeleniumApplication.Configuration.Vcd
{
    /// <summary>
    /// Vcd configuration manager. Prepare config values for using in application.
    /// </summary>
    internal class VcdCommandConfigurationManager
    {
        /// <summary>
        /// Gets the date ranges to process. 
        /// If start date and end date specified only one date range with these start date and end date should be returned.
        /// </summary>
        /// <returns>
        /// List of date ranges where vcd job should be executed.
        /// </returns>
        public IEnumerable<DateRange> GetDateRangesToProcess()
        {
            var executionProfileConfig = VcdExecutionProfileManger.Current.ProfileConfiguration;
            return (executionProfileConfig.StartDate.HasValue && executionProfileConfig.EndDate.HasValue ?
                new List<DateRange> { new DateRange(executionProfileConfig.StartDate.Value, executionProfileConfig.EndDate.Value) } :
                executionProfileConfig.DayIntervalsToProcess.Select(interval => CommandHelper.GetDateRange(default(DateTime), default(DateTime), interval, 0)));
        }
    }
}

