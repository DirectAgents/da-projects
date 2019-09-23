using System;
using System.IO;
using System.Reflection;

namespace SeleniumDataBrowser.Helpers
{
    /// <summary>
    /// Class to help work with directory and files.
    /// </summary>
    public static class PathToFileDirectoryHelper
    {
        /// <summary>
        /// Gets the relative path to the current execution assembly.
        /// </summary>
        /// <param name="itemName">Name of the file or directory that will be combined with the relative path.</param>
        /// <returns>Relative path to the current execution assembly.</returns>
        public static string GetAssemblyRelativePath(string itemName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            return CombinePath(assemblyDir, itemName);
        }

        /// <summary>
        /// Combines the specified file name with the specified path to the directory.
        /// </summary>
        /// <param name="dirPath">Path to the directory.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Full path to the specified file name.</returns>
        public static string CombinePath(string dirPath, string fileName)
        {
            return Path.Combine(dirPath, fileName);
        }

        /// <summary>
        /// Creates a directory at the specified path if it does not exist.
        /// </summary>
        /// <param name="path">Full path to the exist directory.</param>
        public static void CreateDirectoryIfNotExist(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    return;
                }
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not create the directory [{path}]: {e.Message}", e);
            }
        }
    }
}
