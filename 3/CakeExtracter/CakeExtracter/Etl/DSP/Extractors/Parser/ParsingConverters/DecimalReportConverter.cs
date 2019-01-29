using System;
using System.Globalization;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.DSP.Extractors.Parser.ParsingConverters
{
    internal sealed class DecimalReportConverter : DecimalConverter
    {
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
