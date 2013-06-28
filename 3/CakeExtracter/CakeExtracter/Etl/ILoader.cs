using System.Threading;

namespace CakeExtracter.Etl
{
    public interface ILoader<in T>
    {
        int TotalLoaded { get; set; }

        int LoadBatchSize { get; set; }

        Thread BeginLoading(IExtracter<T> extracter);
    }
}