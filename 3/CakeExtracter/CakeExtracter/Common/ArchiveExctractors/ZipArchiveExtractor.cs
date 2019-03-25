using CakeExtracter.Common.ArchiveExtractors.Contract;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace CakeExtracter.Common.ArchiveExtractors
{
    /// <summary>
    /// Archive extractor for Zip stream
    /// </summary>
    /// <seealso cref="Amazon.ArchiveExctractors.IArchiveExtractor" />
    public class ZipArchiveExtractor : IArchiveExtractor
    {
        /// <summary>
        /// Tries the unzip stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public string TryUnzipStream(Stream stream)
        {
            using (var archive = new ZipArchive(stream))
            {
                var entry = archive.Entries.FirstOrDefault();
                if (entry != null)
                {
                    using (var unzippedEntryStream = entry.Open())
                    {
                        using (var ms = new MemoryStream())
                        {
                            unzippedEntryStream.CopyTo(ms);
                            var unzippedArray = ms.ToArray();
                            return Encoding.Default.GetString(unzippedArray);
                        }
                    }
                }
                return null;
            }
        }
    }
}
