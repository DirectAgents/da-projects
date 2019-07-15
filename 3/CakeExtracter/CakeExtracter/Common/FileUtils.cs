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
            var assembly = Assembly.GetExecutingAssembly();
            var assemblyDir = Path.GetDirectoryName(assembly.Location);
            string fullFilePath = Path.Combine(assemblyDir, relativePath);
            return File.ReadAllText(fullFilePath);
        }
    }
}
