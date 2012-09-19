using System.Threading;

namespace ApiClient.Etl
{
    public interface IDestination<T>
    {
        Thread Load(ISource<T> source);
        int Loaded { get; set; }
    }
}
