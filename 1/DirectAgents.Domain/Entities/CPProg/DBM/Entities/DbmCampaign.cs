using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Campaign DB entity.
    /// </summary>
    public class DbmCampaign : DbmEntity
    {
        /// <summary>
        /// Gets or sets the advertiser identifier.
        /// </summary>
        public int? AdvertiserId { get; set; }

        /// <summary>
        /// Gets or sets the advertiser.
        /// </summary>
        [ForeignKey("AdvertiserId")]
        public virtual DbmAdvertiser Advertiser { get; set; }
    }
}