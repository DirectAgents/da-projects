using System;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters
{
    /// <summary>CSV parsing helper for report decimal values.</summary>
    internal sealed class DecimalReportConverter : DecimalConverter
    {
        /// <summary>Converts the string to an decimal value.</summary>
        /// <param name="options">The options to use when converting.</param>
        /// <param name="text">The string to convert to an object.</param>
        /// <returns>The object created from the string.</returns>
        /// <exception cref="Exception">Could not convert string value [{text}] to decimal: {e.Message}</exception>
        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            try
            {
                options.NumberStyle = NumberStyles.Number;
                return base.ConvertFromString(options, text);
            }
            catch (Exception e)
            {
                throw new Exception($"Could not convert string value [{text}] to decimal: {e.Message}", e);
            }
        }
    }
}
