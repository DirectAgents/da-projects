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
                var cookieFileName = string.Format(CookieFileNameTemplate, startNumberCookieFile++);
                var cookieFilePath = FileManager.CombinePath(directoryName, cookieFileName);
                SaveCookieToFile(cookie, cookieFilePath);
            }
        }

        /// <summary>
        /// Gets cookies from JSON files of specified directory.
        /// </summary>
        /// <param name="cookiesDirectoryName">Name of the directory where JSON cookies files are stored.</param>
        /// <returns>Cookies.</returns>
        public static IEnumerable<Cookie> GetCookiesFromFiles(string cookiesDirectoryName)
        {
            var filesWithCookies = GetFilesWithCookiesFromDirectory(cookiesDirectoryName);
            var allCookies = filesWithCookies.Select(file => GetCookieFromFile(file.FullName));
            var notEmptyCookies = allCookies.Where(cookie => cookie != null).ToList();
            return notEmptyCookies;
        }

        private static IEnumerable<FileInfo> GetFilesWithCookiesFromDirectory(string cookiesDirectoryName)
        {
            const string cookieFileNumber = "*";
            var fileSearchPattern = string.Format(CookieFileNameTemplate, cookieFileNumber);
            var cookiesDirectoryInfo = new DirectoryInfo(cookiesDirectoryName);
            return cookiesDirectoryInfo.GetFiles(fileSearchPattern);
        }

        private static Cookie GetCookieFromFile(string pathToCookieFile)
        {
            try
            {
                var cookieFileContent = GetStringsFromCookieFile(pathToCookieFile);
                var cookie = GetCookieFromFileContent(cookieFileContent);
                return cookie;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Dictionary<string, string> GetStringsFromCookieFile(string pathToCookieFile)
        {
            var cookieFileContent = File.ReadAllText(pathToCookieFile);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(cookieFileContent);
        }

        private static Cookie GetCookieFromFileContent(IReadOnlyDictionary<string, string> fileContent)
        {
            var paramName = fileContent["Name"];
            var paramValue = fileContent["Value"];
            var paramDomain = fileContent["Domain"];
            var paramPath = fileContent["Path"];
            var paramExpiryDate = string.IsNullOrEmpty(fileContent["Expiry"])
                ? (DateTime?)null
                : DateTime.Parse(fileContent["Expiry"]);
            return new Cookie(paramName, paramValue, paramDomain, paramPath, paramExpiryDate);
        }

        private static void SaveCookieToFile(Cookie cookie, string pathToFile)
        {
            var cookieContent = JsonConvert.SerializeObject(cookie);
            File.WriteAllText(pathToFile, cookieContent);
        }
    }
}
