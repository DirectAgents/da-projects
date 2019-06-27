using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CakeExtracter.Common;
using CakeExtracter.Etl.AmazonSelenium.Configuration;
using CakeExtracter.Helpers;
using SeleniumDataBrowser.Helpers;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Configuration
{
    /// <inheritdoc />
    /// <summary>
    /// Vcd configuration helper. Prepare config values for using in application.
    /// </summary>
    internal class VcdCommandConfigurationManager : SeleniumCommandConfigurationManager
    {
        private const string SyncScriptPathConfigurationKey = "VCD_SyncScriptPath";
        private const string SyncScriptNameConfigurationKey = "VCD_SyncScriptName";
        private const string IntervalBetweenUnsuccessfulAndNewRequestsConfigurationKey = "VCD_IntervalBetweenUnsuccessfulAndNewRequestsInMinutes";

        private const int DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes = 30;

        /// <summary>
        /// Gets the full path to sync SQL script.
        /// </summary>
        /// <returns>Full path to sync SQL script.</returns>
        public static string GetSyncScriptPath()
        {
            var path = ConfigurationManager.AppSettings[SyncScriptPathConfigurationKey];
            var name = ConfigurationManager.AppSettings[SyncScriptNameConfigurationKey];
            return PathToFileDirectoryHelper.GetAssemblyRelativePath(PathToFileDirectoryHelper.CombinePath(path, name));
        }

        /// <summary>
        /// Gets the number minutes of interval between unsuccessful and new job execution requests
        /// from the config application setting.
        /// </summary>
        /// <returns>Number minutes of interval between unsuccessful and new job execution requests.</returns>
        public static int GetIntervalBetweenUnsuccessfulAndNewRequest()
        {
            return ConfigurationHelper.GetIntConfigurationValue(
                IntervalBetweenUnsuccessfulAndNewRequestsConfigurationKey,
                DefaultIntervalBetweenUnsuccessfulAndNewRequestsInMinutes);
        }
    }
}