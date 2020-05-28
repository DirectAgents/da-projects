using System;
using System.IO;

namespace CakeExtracter.Common.MatchingPortal.Models
{
    /// <summary>
    /// Class representing a data frame export.
    /// </summary>
    public class DataFrameExport
    {
        /// <summary>
        /// Gets or sets contents of the data frame.
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets content type of the file.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets timestamp when the report was built.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
