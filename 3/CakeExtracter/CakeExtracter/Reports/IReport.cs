namespace CakeExtracter.Reports
{
    interface IReport
    {
        string Subject { get; }
        string Generate();
    }
}
