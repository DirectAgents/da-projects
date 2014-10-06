using System.Net.Mail;
namespace CakeExtracter.Reports
{
    interface IReport
    {
        string Subject { get; }
        string Generate();

        AlternateView GenerateView();
        void DisposeResources();
    }
}
