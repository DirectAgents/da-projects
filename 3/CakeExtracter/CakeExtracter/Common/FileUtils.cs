using System.IO;
using System.Reflection;

namespace CakeExtracter.Common
{
    /// <summary>
    /// File utils.
    /// </summary>
    public static class FileUtils
    {
        /// <summary>
        /// Gets the file content by relative path.
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns>File text content.</returns>
        public static string GetFileContentByRelativePath(string relativePath)
        {
            string currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string fullFilePath = CombinePath(currentDirectory, relativePath);
            return File.ReadAllText(fullFilePath);
        }

        private static string CombinePath(string path1, string path2)
        {
            // Ensure neither end of path1 or beginning of path2 have slashes
            path1 = path1.Trim().TrimEnd(System.IO.Path.DirectorySeparatorChar);
            path2 = path2.Trim().TrimStart(System.IO.Path.DirectorySeparatorChar);

            // Handle drive letters
            if (path1.Substring(path1.Length - 1, 1) == ":")
            {
                path1 += Path.DirectorySeparatorChar;
            }
            return Path.Combine(path1, path2);
        }
    }
}
