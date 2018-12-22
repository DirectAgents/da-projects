using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace Amazon.Helpers
{
    public static class FileManager
    {
        private const string JsonExtension = "json";
        private const string GzipExtension = "gzip";

        private const string BaseFileName = "download";

        public static string ReadJsonFromDecompressedStream(Stream sourceStream)
        {
            var fileNameWithoutExtension = GetFileNameForCurrentProcess();
            var compressedFilePath = GetBaseDirectoryFilePath(fileNameWithoutExtension, GzipExtension);
            var decompressedFilePath = GetBaseDirectoryFilePath(fileNameWithoutExtension, JsonExtension);

            CopyStream(sourceStream, compressedFilePath);
            DecompressStream(compressedFilePath, decompressedFilePath);
            var json = File.ReadAllText(decompressedFilePath);

            File.Delete(compressedFilePath);
            File.Delete(decompressedFilePath);
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

        private static string GetBaseDirectoryFilePath(string fileNameWithoutExtension, string extension)
        {
            var fileName = GetFileNameWithExtension(fileNameWithoutExtension, extension);
            return GetBaseDirectoryFilePath(fileName);
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
