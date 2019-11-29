namespace FacebookAPI.Converters
{
    public interface IFacebookConverter<TSummary>
    {
        TSummary ParseSummaryFromRow(dynamic row);
    }
}
