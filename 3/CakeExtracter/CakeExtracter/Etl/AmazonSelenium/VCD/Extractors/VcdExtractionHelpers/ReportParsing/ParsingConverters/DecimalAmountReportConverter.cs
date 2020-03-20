﻿using System.Globalization;
using CsvHelper.TypeConversion;

namespace CakeExtracter.Etl.AmazonSelenium.VCD.Extractors.VcdExtractionHelpers.ReportParsing.ParsingConverters
{
    internal sealed class DecimalAmountReportConverter : StringConverter
    {
        private const string EmptyValue = "—";

        public override object ConvertFromString(TypeConverterOptions options, string text)
        {
            if (text == EmptyValue)
            {
                return 0M;
            }
            var d = decimal.Parse(
                text,
                NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowThousands);
            return d;
        }
    }
}