using System.Collections.Generic;
using System.Threading;

namespace CakeExtracter.Etl
{
    public interface IExtracter<out T>
    {
        object Locker { get; set; }

        Thread BeginExtracting();

        IEnumerable<T> ExtractedItems { get; }

        int TotalExtracted { get; }

        bool IsComplete { get; }
    }
}