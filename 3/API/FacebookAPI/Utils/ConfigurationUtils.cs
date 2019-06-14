using System.Configuration;

namespace FacebookAPI.Utils
{
    /// <summary>
    /// Facebook Configuration Utils.
    /// </summary>
    internal static class ConfigurationUtils
    {
        /// <summary>
        /// Gets the default or configuration int value.
        /// </summary>
        /// <param name="configValueName">Name of the configuration value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>Configuration value.</returns>
        public static int GetDefaultOrConfigIntValue(string configValueName, int defaultValue)
        {
            return int.TryParse(ConfigurationManager.AppSettings[configValueName], out var tmpInt)
                ? tmpInt
                : defaultValue;
        }
    }
}
