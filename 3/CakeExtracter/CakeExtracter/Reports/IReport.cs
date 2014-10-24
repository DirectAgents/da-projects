using System.Net.Mail;

namespace CakeExtracter.Reports
{
    public interface IReport
    {
        string Subject { get; }
        string Generate();

        AlternateView GenerateView();
        void DisposeResources();
    }
}
