using System.Collections.Generic;
using System.IO;

namespace CakeExtracter.Common.Extractors.CsvExtractors.Contracts
{
    public interface ICsvExtractor<T>
    {
        string ItemsName { get; set; }

        List<T> EnumerateRows();

        List<T> EnumerateRows(string url);

        List<T> EnumerateRows(StreamReader streamReader);
    }
}
