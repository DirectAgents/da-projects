using System;

namespace CakeExtracter.Etl.Kochava.Models
{
    /// <summary>
    /// Report entity for kochava report
    /// </summary>
    public class KochavaReportItem
    {
        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the name of the network.
        /// </summary>
        /// <value>
        /// The name of the network.
        /// </value>
        public string NetworkName { get; set; }

        /// <summary>
        /// Gets or sets the name of the event.
        /// </summary>
        /// <value>
        /// The name of the event.
        /// </value>
        public string EventName { get; set; }

        /// <summary>
        /// Gets or sets the campaignid.
        /// </summary>
        /// <value>
        /// The campaignid.
        /// </value>
        public string CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the creative identifier.
        /// </summary>
        /// <value>
        /// The creative identifier.
        /// </value>
        public string CreativeId { get; set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        /// <value>
        /// The site identifier.
        /// </value>
        public string SiteId { get; set; }

        /// <summary>
        /// Gets or sets the ad group identifier.
        /// </summary>
        /// <value>
        /// The ad group identifier.
        /// </value>
        public string AdGroupId { get; set; }

        /// <summary>
        /// Gets or sets the keyword.
        /// </summary>
        /// <value>
        /// The keyword.
        /// </value>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets the device identifier.
        /// </summary>
        /// <value>
        /// The device identifier.
        /// </value>
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string CountryCode { get; set; }
    }
}
