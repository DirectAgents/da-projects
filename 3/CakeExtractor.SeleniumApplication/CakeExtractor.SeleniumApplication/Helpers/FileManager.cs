using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    public class FileManager
    {
        private static string _cookieFileName = "Cookie ({0}).txt";

        public static string GetAssemblyRelativePath(string itemName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            return CombinePath(assemblyDir, itemName);
        }

        public static string GetApplicationName()
        {
            return AppDomain.CurrentDomain.FriendlyName;
        }

        public static string CombinePath(string dirPath, string fileName)
        {
            return Path.Combine(dirPath, fileName);
        }

        public static void CreateDirectoryIfNotExist(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Console.WriteLine($"Create the directory [{path}]");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not create the directory [{path}]: {e.Message}", e);
            }
        }

        public static void CleanDirectory(string dirPath, string templateFileName)
        {
            try
            {
                var dir = new DirectoryInfo(dirPath);
                var files = dir.GetFiles(templateFileName);
                foreach (var file in files)
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error: Could not delete the file [{file.Name}]: {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Could not clear the directory [{dirPath}]: {e.Message}", e);
            }
        }

        public static string[] GetFilesFromPath(string dirPath, string templateFileName, string fileName)
        {
            try
            {
                var dir = new DirectoryInfo(dirPath);
                var i = 0;
                var ext = Path.GetExtension(templateFileName);
                var fileNameMask = Path.GetFileNameWithoutExtension(templateFileName);
                var formatTemplate = fileName
                    .Replace('/', '-')
                    .Replace("\"", "")
                    .Replace(" ", "-")
                    .Replace(".", "-");
                var files = dir.GetFiles($"{formatTemplate}{fileNameMask}{ext}");
                var result = new string[files.Length];
                foreach (var file in files)
                {
                    result[i] = file.FullName;
                    i++;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(
                    $"Could not get path of the file using template [{templateFileName}] in the directory [{dirPath}]: {e.Message}",
                    e);
            }
        }

        public static void SaveCookiesToFiles(IEnumerable<Cookie> cookies, string directoryName)
        {
            var i = 0;
            foreach (var cookie in cookies)
            {
                SaveCookieToFile(cookie, CombinePath(directoryName, string.Format(_cookieFileName, i++)));
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
            var files = dir.GetFiles(string.Format(_cookieFileName, "*"));
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
                Console.WriteLine($"Warning: {e.Message}");
                return null;
            }
        }
    }
}
