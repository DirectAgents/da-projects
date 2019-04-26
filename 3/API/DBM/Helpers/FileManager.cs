using System;
using System.IO;

namespace DBM.Helpers
{
    public static class FileManager
    {
        public static void SaveToFileInExecutionFolder(string directoryName, string fileName, StreamReader fileContent)
        {
            var path = GetDirectoryFilePath(directoryName, fileName);
            File.WriteAllText(path, fileContent.ReadToEnd());
        }

        private static string GetDirectoryFilePath(string directoryName, string fileName)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(exePath, directoryName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filePath = Path.Combine(path, fileName);
            return filePath;
        }
    }
}
