using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.DBM.Entities
{
    /// <inheritdoc />
    /// <summary>
    /// DBM Creative DB entity.
    /// </summary>
    public class DbmCreative : DbmEntity
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

        /// <summary>
        /// Gets or sets the height of creative.
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Gets or sets the width of creative.
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Gets or sets the size of creative.
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the type of creative.
        /// </summary>
        public string Type { get; set; }
    }
}