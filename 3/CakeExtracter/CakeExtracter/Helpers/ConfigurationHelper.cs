﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CakeExtracter.Helpers
{
    public class ConfigurationHelper
    {
        private const char Separator = ',';

        private static readonly char[] ConfigurationValuesSeparators = { Separator };

        public static List<string> ExtractEnumerableFromConfig(string configValueName)
        {
            var configValue = ConfigurationManager.AppSettings[configValueName] ?? string.Empty;
            var values = ExtractEnumerableFromConfigValue(configValue);
            return values.ToList();
        }

        public static Dictionary<string, string> ExtractDictionaryFromConfigValue(string configKeysName, string configValuesName)
        {
            var keys = ExtractEnumerableFromConfig(configKeysName);
            var values = ExtractEnumerableFromConfig(configValuesName);
            var dictionary = new Dictionary<string, string>();
            for (var i = 0; i < keys.Count; i++)
            {
                dictionary.Add(keys[i], values[i]);
            }

            return dictionary;
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

        public static string GetValueWithLeadingAndTrailingSeparator(string configValueName)
        {
            var value = ConfigurationManager.AppSettings[configValueName];
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return GetSeparatorOrEmptyString(value.First()) + value + GetSeparatorOrEmptyString(value.Last());
        }

        private static IEnumerable<string> ExtractEnumerableFromConfigValue(string configValue)
        {
            return configValue.Split(ConfigurationValuesSeparators);
        }

        private static string GetSeparatorOrEmptyString(char symbol)
        {
            return symbol == Separator ? string.Empty : Separator.ToString();
        }
    }
}
