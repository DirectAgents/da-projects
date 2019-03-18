using CakeExtracter.Etl.Kochava.Models;
using CsvHelper.Configuration;

namespace CakeExtracter.Etl.Kochava.Extractors.Parsers.ParsingConverters
{
    /// <summary>
    /// Kochava report entity row map. Used for csv parsing configuration.
    /// </summary>
    /// <seealso cref="CsvHelper.Configuration.CsvClassMap{CakeExtracter.Etl.Kochava.Models.KochavaReportItem}" />
    internal class KochavaReportEntityRowMap : CsvClassMap<KochavaReportItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KochavaReportEntityRowMap"/> class.
        /// </summary>
        public KochavaReportEntityRowMap()
        {
            Map(m => m.AppName).Name("app_name");
            Map(m => m.Date).Name("date_utc"); ;
            Map(m => m.NetworkName).Name("network_name");
            Map(m => m.EventName).Name("event_name");
            Map(m => m.CampaignId).Name("campaign_id");
            Map(m => m.CreativeId).Name("creative_id");
            Map(m => m.SiteId).Name("site_id");
            Map(m => m.AdGroupId).Name("adgroup_id");
            Map(m => m.Keyword).Name("keyword");
            Map(m => m.DeviceId).Name("device_id");
            Map(m => m.CountryCode).Name("country_code");
        }
    }
}
