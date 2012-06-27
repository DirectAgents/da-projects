namespace DAgents.Common
{
    public interface IEnable
    {
        bool Enabled { get; set; }
        void EnabledChanged();
    }
}
