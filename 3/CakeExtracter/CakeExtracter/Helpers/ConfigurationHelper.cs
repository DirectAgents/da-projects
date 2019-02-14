using System.Collections.Generic;
using System.Configuration;

namespace CakeExtracter.Helpers
{
    internal class ConfigurationHelper
    {
        private static readonly char[] ConfigurationValuesSeparators = {','};

        public static IEnumerable<string> ExtractEnumerableFromConfig(string configValueName)
        {
            var configValue = ConfigurationManager.AppSettings[configValueName] ?? string.Empty;
            var values = configValue.Split(ConfigurationValuesSeparators);
            return values;
        }
    }
}
