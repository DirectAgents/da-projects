using CakeExtracter.Common.ArchiveExtractors.Contract;
using System.IO;
using System.IO.Compression;

namespace CakeExtracter.Common.ArchiveExtractors
{
    /// <summary>
    /// Archive extractor for Gzip stream
    /// </summary>
    /// <seealso cref="Amazon.ArchiveExctractors.IArchiveExtractor" />
    public class GzipArchiveExtractor : IArchiveExtractor
    {
        /// <summary>
        /// Tries the unzip stream.
        /// </summary>
        /// <param name="stream">The archive stream.</param>
        /// <returns></returns>
        public string TryUnzipStream(Stream stream)
        {
            using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (StreamReader reader = new StreamReader(gzip))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
