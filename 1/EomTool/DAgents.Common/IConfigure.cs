namespace DAgents.Common
{
    public interface IConfigure<T>
    {
        void Configure(T target);
    }
}
