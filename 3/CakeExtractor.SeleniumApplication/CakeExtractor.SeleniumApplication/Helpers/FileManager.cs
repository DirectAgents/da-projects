using System;
using System.IO;
using System.Reflection;
using CakeExtracter;

namespace CakeExtractor.SeleniumApplication.Helpers
{
    public class FileManager
    {
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
                if (Directory.Exists(path))
                {
                    return;
                }
                Directory.CreateDirectory(path);
                Logger.Info("Create the directory [{0}]", path);
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
                        throw new Exception($"Error: Could not delete the file [{file.Name}]: {e.Message}", e);
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
                    .Replace('|', '-')
                    .Replace('/', '-')
                    .Replace("\"", "")
                    .Replace(" ", "-")
                    .Replace(".", "-")
                    .Replace("(", "-")
                    .Replace(")", "-");
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
                    $"Could not get path of the file using template [{templateFileName}] in the directory [{dirPath}]: {e.Message}", e);
            }
        }       
    }
}
