﻿using System;
using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// Vcd configuration helper. Prepare config values for using in application.
    /// </summary>
    internal class VcdCommandConfigurationHelper : SeleniumCommandConfigurationHelper
    {
        private const string WaitPageTimeoutConfigurationKey = "SeleniumWaitPageTimeoutInMinutes";

        /// <summary>
        /// Gets the date ranges to process.
        /// If start date and end date specified only one date range with these start date and end date should be returned.
        /// </summary>
        /// <returns>List of date ranges where vcd job should be executed.</returns>
        public static IEnumerable<DateRange> GetDateRangesToProcess()
        {
            var executionProfileConfig = VcdExecutionProfileManger.Current.ProfileConfiguration;
            return executionProfileConfig.StartDate.HasValue && executionProfileConfig.EndDate.HasValue
                ? new List<DateRange> { new DateRange(executionProfileConfig.StartDate.Value, executionProfileConfig.EndDate.Value) }
                : executionProfileConfig.DayIntervalsToProcess.Select(interval => 
                    CommandHelper.GetDateRange(default(DateTime), default(DateTime), interval, 0));
        }
    }
}