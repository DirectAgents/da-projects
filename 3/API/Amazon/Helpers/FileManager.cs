using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;

namespace Amazon.Helpers
{
    public static class FileManager
    {
        private const string JsonExtension = "json";
        private const string GzipExtension = "gzip";

        private const string BaseFileName = "download";

        public static string ReadJsonFromDecompressedStream(string folder, string uniqueFileName, Stream sourceStream)
        {
            var compressedFilePath = GetBaseDirectoryFilePath(uniqueFileName, GzipExtension);
            var decompressedFilePath = GetDirectoryFilePath(folder, uniqueFileName, JsonExtension);
            var json = ReadJsonFromDecompressedStream(sourceStream, compressedFilePath, decompressedFilePath);
            File.Delete(compressedFilePath);
            return json;
        }

        public static string ReadJsonFromDecompressedStream(Stream sourceStream)
        {
            var fileNameWithoutExtension = GetFileNameForCurrentProcess();
            var compressedFilePath = GetBaseDirectoryFilePath(fileNameWithoutExtension, GzipExtension);
            var decompressedFilePath = GetBaseDirectoryFilePath(fileNameWithoutExtension, JsonExtension);
            var json = ReadJsonFromDecompressedStream(sourceStream, compressedFilePath, decompressedFilePath);
            File.Delete(compressedFilePath);
            File.Delete(decompressedFilePath);
            return json;
        }

        public static void SaveToFileInExecutionFolder(string fileName, string fileContent)
        {
            var path = GetBaseDirectoryFilePath(fileName);
            File.WriteAllText(path, fileContent);
        }

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

        private static string ReadJsonFromDecompressedStream(Stream sourceStream, string compressedFilePath, string decompressedFilePath)
        {
            CopyStream(sourceStream, compressedFilePath);
            DecompressStream(compressedFilePath, decompressedFilePath);
            var json = File.ReadAllText(decompressedFilePath);
            return json;
        }

        private static void CopyStream(Stream sourceStream, string destFilePath)
        {
            using (var destStream = File.Create(destFilePath))
            {
                sourceStream.CopyTo(destStream);
            }
        }

        private static void DecompressStream(string sourceFilePath, string destFilePath)
        {
            using (var compressedFileStream = File.OpenRead(sourceFilePath))
            {
                using (var decompressedFileStream = File.Create(destFilePath))
                {
                    DecompressStream(compressedFileStream, decompressedFileStream);
                }
            }
        }

        private static void DecompressStream(Stream sourceStream, Stream destStream)
        {
            using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(destStream);
            }
        }

        private static string GetFileNameForCurrentProcess()
        {
            var uniquePart = Process.GetCurrentProcess().Id;
            return BaseFileName + uniquePart;
        }

        private static string GetDirectoryFilePath(string directoryPath, string fileNameWithoutExtension, string extension)
        {
            var fileName = GetFileNameWithExtension(fileNameWithoutExtension, extension);
            return GetDirectoryFilePath(directoryPath, fileName);
        }

        private static string GetBaseDirectoryFilePath(string fileNameWithoutExtension, string extension)
        {
            var fileName = GetFileNameWithExtension(fileNameWithoutExtension, extension);
            return GetBaseDirectoryFilePath(fileName);
        }

        private static string GetDirectoryFilePath(string directoryPath, string fileName)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var filePath = Path.Combine(directoryPath, fileName);
            return filePath;
        }

        private static string GetBaseDirectoryFilePath(string fileName)
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(exePath, fileName);
            return filePath;
        }

        private static string GetFileNameWithExtension(string fileName, string extension)
        {
            return $"{fileName}.{extension}";
        }
    }
}
