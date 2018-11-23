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
            return CombinePath(assemblyDir, itemName);
        }

        public static string CombinePath(string dirPath, string fileName)
        {
            return Path.Combine(dirPath, fileName);
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
    }
}
