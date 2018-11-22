using AmazonAdvertisingNavigationTest;
using System;
using System.IO;
using System.Reflection;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    public class FileManager
    {
        public static string GetAssemblyRelativePath(string itemName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            return Path.Combine(assemblyDir, itemName);
        }

        public static void CreateDirectoryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CleanDirectory(string dirPath, string fileName)
        {
            var dir = new DirectoryInfo(dirPath);
            var files = dir.GetFiles(fileName);
            foreach (var file in files)
            {
                file.Delete();
            }
        }

        public static void ParsingCsvFiles(string dirPath, string fileName)
        {
            try
            {
                var di = new DirectoryInfo(dirPath);
                foreach (var file in di.GetFiles(fileName))
                {
                    var parsingFile = new ParsingCSV(Path.Combine(dirPath, file.Name));
                    var campaignsInfo = parsingFile.Parse(','); // <-- INFORMATION            
                }
            }
            catch (Exception e)
            {
                throw new Exception($"The parsing of csv files failed: {e.Message}", e);
            }
        }
    }
}
