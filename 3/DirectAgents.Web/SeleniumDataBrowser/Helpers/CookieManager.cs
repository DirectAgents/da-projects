using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace SeleniumDataBrowser.Helpers
{
    public class CookieManager
    {
        private const string cookieFileName = "Cookie_{0}.json";
        private static int startNumberCookieFile = 1;

        public static void SaveCookiesToFiles(IEnumerable<Cookie> cookies, string directoryName)
        {
            foreach (var cookie in cookies)
            {
                SaveCookieToFile(cookie, FileManager.CombinePath(directoryName, string.Format(cookieFileName, startNumberCookieFile++)));
            }
        }

        public static IEnumerable<Cookie> GetCookiesFromFiles(string directoryName)
        {
            var dir = new DirectoryInfo(directoryName);
            var files = dir.GetFiles(string.Format(cookieFileName, "*"));
            return files.Select(file => GetCookieFromFile(file.FullName)).Where(cookie => cookie != null).ToList();
        }

        private static Cookie GetCookieFromFile(string pathToFile)
        {
            try
            {
                var strings = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(pathToFile));
                var expiryDate = string.IsNullOrEmpty(strings["Expiry"]) ? (DateTime?) null : DateTime.Parse(strings["Expiry"]);
                return new Cookie(strings["Name"], strings["Value"], strings["Domain"], strings["Path"], expiryDate);
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message);
                return null;
            }
        }

        private static void SaveCookieToFile(Cookie cookie, string pathToFile)
        {
            File.WriteAllText(pathToFile, JsonConvert.SerializeObject(cookie));
        }
    }
}
