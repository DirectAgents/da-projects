using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CakeExtracter.Helpers
{
    /// <summary>
    /// Configuration extractor helper.
    /// </summary>
    public class ConfigurationHelper
    {
        private const char Separator = ',';

        private static readonly char[] ConfigurationValuesSeparators = { Separator };

        /// <summary>
        /// Extracts the enumerable from configuration.
        /// </summary>
        /// <param name="configKey">The configuration key.</param>
        /// <param name="removeEmptyEntries">Option for removing or leaving empty entries.</param>
        /// <returns>Configuration values collection.</returns>
        public static List<string> ExtractEnumerableFromConfig(string configKey, bool removeEmptyEntries = true)
        {
            var configValue = ConfigurationManager.AppSettings[configKey] ?? string.Empty;
            var values = removeEmptyEntries
                        ? ExtractEnumerableFromConfigValue(configValue, StringSplitOptions.RemoveEmptyEntries)
                        : ExtractEnumerableFromConfigValue(configValue, StringSplitOptions.None);
            return values.ToList();
        }

        /// <summary>
        /// Extracts the dictionary from configuration value.
        /// </summary>
        /// <param name="configKeysName">Name of the configuration keys.</param>
        /// <param name="configValuesName">Name of the configuration values.</param>
        /// <param name="removeEmptyEntries">Option for removing or leaving empty entries.</param>
        /// <returns>Dictionary configuration value.</returns>
        public static Dictionary<string, string> ExtractDictionaryFromConfigValue(string configKeysName, string configValuesName, bool removeEmptyEntries = true)
        {
            var keys = ExtractEnumerableFromConfig(configKeysName, removeEmptyEntries);
            var values = ExtractEnumerableFromConfig(configValuesName, removeEmptyEntries);
            var dictionary = new Dictionary<string, string>();
            for (var i = 0; i < keys.Count; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }

            return dictionary;
        }

        /// <summary>
        /// Extracts the numbers from configuration value.
        /// </summary>
        /// <param name="configValue">The configuration value.</param>
        /// <returns>Number value from configuration.</returns>
        /// <exception cref="System.Exception">The configuration value \"{configValue}\" is invalid: {e}</exception>
        public static List<int> ExtractNumbersFromConfigValue(string configValue)
        {
            var values = ExtractEnumerableFromConfigValue(configValue);
            try
            {
                var numbers = values.Select(int.Parse).ToList();
                return numbers;
            }
            catch (Exception e)
            {
                throw new Exception($"The configuration value \"{configValue}\" is invalid: {e}");
            }
        }

        /// <summary>
        /// Gets the value with leading and trailing separator.
        /// </summary>
        /// <param name="configValueName">Name of the configuration value.</param>
        /// <returns>Processed values.</returns>
        public static string GetValueWithLeadingAndTrailingSeparator(string configValueName)
        {
            var value = ConfigurationManager.AppSettings[configValueName];
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return GetSeparatorOrEmptyString(value.First()) + value + GetSeparatorOrEmptyString(value.Last());
        }

        /// <summary>
        /// Returns the configuration value, if it exists, or the default value, if it does not.
        /// </summary>
        /// <param name="configurationValueName">Configuration name of the value.</param>
        /// <param name="defaultValue">Default value.</param>
        /// <returns>Result value.</returns>
        public static int GetIntConfigurationValue(string configurationValueName, int defaultValue = 0)
        {
            var configurationValue = GetConfigurationValue(configurationValueName, defaultValue.ToString());
            return int.Parse(configurationValue);
        }

        private static IEnumerable<string> ExtractEnumerableFromConfigValue(string configValue, StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
        {
            return configValue.Split(ConfigurationValuesSeparators, option);
        }

        private static string GetSeparatorOrEmptyString(char symbol)
        {
            return symbol == Separator ? string.Empty : Separator.ToString();
        }

        private static string GetConfigurationValue(string configurationValueName, string defaultValue = "")
        {
            var configurationValue = ConfigurationManager.AppSettings[configurationValueName];
            return configurationValue ?? defaultValue;
        }
    }
}
