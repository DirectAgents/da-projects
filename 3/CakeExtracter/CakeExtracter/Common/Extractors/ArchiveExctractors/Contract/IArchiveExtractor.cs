using System.IO;

namespace CakeExtracter.Common.Extractors.ArchiveExctractors.Contract
{
    /// <summary>
    /// Archive extractor interface
    /// </summary>
    public interface IArchiveExtractor
    {
        /// <summary>
        /// Tries the unzip stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        string TryUnzipStream(Stream stream);
    }
}
