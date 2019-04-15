using DirectAgents.Domain.Entities.CPProg.Facebook.Campaign;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook.Ad
{
    public class FbAd
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        /// <value>
        /// The external identifier.
        /// </value>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the ext account.
        /// </summary>
        /// <value>
        /// The ext account.
        /// </value>
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        /// <summary>
        /// Gets or sets the campaign identifier.
        /// </summary>
        /// <value>
        /// The campaign identifier.
        /// </value>
        public int? CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the campaign.
        /// </summary>
        /// <value>
        /// The campaign.
        /// </value>
        [ForeignKey("CampaignId")]
        public virtual FbCampaign Campaign { get; set; }

        /// <summary>
        /// Gets or sets the ad set identifier.
        /// </summary>
        /// <value>
        /// The ad set identifier.
        /// </value>
        public int? AdSetId { get; set; }

        /// <summary>
        /// Gets or sets the ad set.
        /// </summary>
        /// <value>
        /// The ad set.
        /// </value>
        [ForeignKey("AdSetId")]
        public virtual FbCampaign AdSet { get; set; }

        /// <summary>
        /// Gets or sets the creative identifier.
        /// </summary>
        /// <value>
        /// The creative identifier.
        /// </value>
        public int? CreativeId { get; set; }

        /// <summary>
        /// Gets or sets the creative.
        /// </summary>
        /// <value>
        /// The creative.
        /// </value>
        [ForeignKey("CreativeId")]
        public virtual FbCreative Creative { get; set; }
    }
}
