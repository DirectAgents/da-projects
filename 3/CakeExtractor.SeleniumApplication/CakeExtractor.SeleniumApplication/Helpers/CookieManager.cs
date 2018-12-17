using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CakeExtracter;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    public class CookieManager
    {
        private static readonly string cookieFileName = "Cookie ({0}).txt";

        public static void SaveCookiesToFiles(IEnumerable<Cookie> cookies, string directoryName)
        {
            var i = 0;
            foreach (var cookie in cookies)
            {
                SaveCookieToFile(cookie, FileManager.CombinePath(directoryName, string.Format(cookieFileName, i++)));
            }
        }

        private static void SaveCookieToFile(Cookie cookie, string pathToFile)
        {
            string[] lines =
            {
                cookie.Name,
                cookie.Value,
                cookie.Domain,
                cookie.Path,
                cookie.Expiry.ToString()
            };
            File.WriteAllText(pathToFile, string.Join(";", lines));
        }

        public static IEnumerable<Cookie> GetCookiesFromFiles(string directoryName)
        {
            var dir = new DirectoryInfo(directoryName);
            var files = dir.GetFiles(string.Format(cookieFileName, "*"));
            return files.Select(file => GetCookieFromFile(file.FullName)).Where(cookie => cookie != null).ToList();
        }

        public static Cookie GetCookieFromFile(string file)
        {
            try
            {
                var strings = File.ReadAllText(file).Split(';');
                return new Cookie(strings[0], strings[1], strings[2], strings[3], DateTime.Parse(strings[4]));
            }
            catch (Exception e)
            {
                Logger.Warn(e.Message);
                return null;
            }
        }
    }
}
