using System.IO;
using System.IO.Compression;
using CakeExtracter.Common.Extractors.ArchiveExctractors.Contract;

namespace CakeExtracter.Common.Extractors.ArchiveExctractors
{
    /// <summary>
    /// Archive extractor for Gzip stream
    /// </summary>
    /// <seealso cref="IArchiveExtractor" />
    public class GzipArchiveExtractor : IArchiveExtractor
    {
        /// <summary>
        /// Tries the unzip stream.
        /// </summary>
        /// <param name="stream">The archive stream.</param>
        /// <returns></returns>
        public string TryUnzipStream(Stream stream)
        {
            using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
            {
                using (var reader = new StreamReader(gzip))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
