using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SeleniumDataBrowser.VCD.Helpers
{
    public static class VcdReportFolderHelper
    {
        public static IEnumerable<DirectoryInfo> GetSubdirectories(DirectoryInfo directory)
        {
            var subdirectories = directory.GetDirectories();
            return subdirectories;
        }

        public static IEnumerable<DirectoryInfo> GetSubdirectories(string folderPath)
        {
            var directory = new DirectoryInfo(folderPath);
            var subdirectories = directory.GetDirectories();
            return subdirectories;
        }

        public static List<string> GetFilesFromDirectory(string directoryPath)
        {
            return Directory.GetFiles(directoryPath).ToList();
        }
    }
}