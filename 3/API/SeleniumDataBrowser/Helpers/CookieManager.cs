using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.Helpers
{
    /// <summary>
    /// Selenium helper for working with browser cookies.
    /// </summary>
    public class CookieManager
    {
        private const string CookieFileNameTemplate = "Cookie_{0}.json";

        private static int startNumberCookieFile = 1;

        /// <summary>
        /// Saves cookies to JSON files to the specified directory.
        /// </summary>
        /// <param name="cookies">List of cookies.</param>
        /// <param name="directoryName">Name of the directory which cookies will be saved.</param>
        public static void SaveCookiesToFiles(IEnumerable<Cookie> cookies, string directoryName)
        {
            foreach (var cookie in cookies)
            {
                SaveCookieToFile(cookie, FileManager.CombinePath(directoryName, string.Format(CookieFileNameTemplate, startNumberCookieFile++)));
            }
        }

        /// <summary>
        /// Gets cookies from JSON files of specified directory.
        /// </summary>
        /// <param name="directoryName">Name of the directory where JSON cookies files are stored.</param>
        /// <returns>Cookies.</returns>
        public static IEnumerable<Cookie> GetCookiesFromFiles(string directoryName)
        {
            var dir = new DirectoryInfo(directoryName);
            var files = dir.GetFiles(string.Format(CookieFileNameTemplate, "*"));
            return files.Select(file => GetCookieFromFile(file.FullName)).Where(cookie => cookie != null).ToList();
        }

        private static Cookie GetCookieFromFile(string pathToFile)
        {
            try
            {
                var strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(pathToFile));
                var expiryDate = string.IsNullOrEmpty(strings["Expiry"]) ? (DateTime?)null : DateTime.Parse(strings["Expiry"]);
                return new Cookie(strings["Name"], strings["Value"], strings["Domain"], strings["Path"], expiryDate);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void SaveCookieToFile(Cookie cookie, string pathToFile)
        {
            File.WriteAllText(pathToFile, JsonConvert.SerializeObject(cookie));
        }
    }
}
