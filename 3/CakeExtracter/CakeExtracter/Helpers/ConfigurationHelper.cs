using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CakeExtracter.Helpers
{
    public class ConfigurationHelper
    {
        private static readonly char[] ConfigurationValuesSeparators = {','};

        public static List<string> ExtractEnumerableFromConfig(string configValueName)
        {
            var configValue = ConfigurationManager.AppSettings[configValueName] ?? string.Empty;
            var values = ExtractEnumerableFromConfigValue(configValue);
            return values.ToList();
        }

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

        private static IEnumerable<string> ExtractEnumerableFromConfigValue(string configValue)
        {
            return configValue.Split(ConfigurationValuesSeparators);
        }
    }
}
