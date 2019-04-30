using System.Collections.Generic;

namespace CakeExtracter.Common.Extractors.CsvExtractors.Contracts
{
    internal interface ICsvExtractor<T>
    {
        List<T> EnumerateRows();
    }
}
