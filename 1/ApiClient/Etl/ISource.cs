using System.Collections.Generic;
using System.Threading;

namespace ApiClient.Etl
{
    public interface ISource<T>
    {
        IEnumerable<T> Items { get; }
        Thread Extract();
        int Total { get; }
        bool Done { get; }
        object Locker { get; set; }
    }
}
