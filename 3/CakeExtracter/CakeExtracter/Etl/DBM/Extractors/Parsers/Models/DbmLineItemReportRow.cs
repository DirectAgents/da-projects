namespace CakeExtracter.Etl.DBM.Extractors.Parsers.Models
{
    public class DbmLineItemReportRow : DbmBaseReportRow
    {
        public string LineItemId { get; set; }

        public string LineItemName { get; set; }

        public string LineItemType { get; set; }

        public string LineItemStatus { get; set; }

        public string FloodlightActivityName { get; set; }
    }
}