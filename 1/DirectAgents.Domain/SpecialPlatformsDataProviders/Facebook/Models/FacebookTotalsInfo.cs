using DirectAgents.Domain.Entities.CPProg;

namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models
{
    /// <summary>
    /// Facebook Level Totals Info.
    /// </summary>
    public class FacebookTotalsInfo
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public ExtAccount Account { get; set; }

        /// <summary>
        /// Gets or sets the impressions total.
        /// </summary>
        /// <value>
        /// The impressions total.
        /// </value>
        public int ImpressionsTotal { get; set; }

        /// <summary>
        /// Gets or sets the clicks total.
        /// </summary>
        /// <value>
        /// The clicks total.
        /// </value>
        public int ClicksTotal { get; set; }

        /// <summary>
        /// Gets or sets all clicks total.
        /// </summary>
        /// <value>
        /// All clicks total.
        /// </value>
        public int AllClicksTotal { get; set; }

        /// <summary>
        /// Gets or sets the post click conv total.
        /// </summary>
        /// <value>
        /// The post click conv total.
        /// </value>
        public int PostClickConvTotal { get; set; }

        /// <summary>
        /// Gets or sets the post view conv total.
        /// </summary>
        /// <value>
        /// The post view conv total.
        /// </value>
        public int PostViewConvTotal { get; set; }

        /// <summary>
        /// Gets or sets the post click rev total.
        /// </summary>
        /// <value>
        /// The post click rev total.
        /// </value>
        public decimal PostClickRevTotal { get; set; }

        /// <summary>
        /// Gets or sets the post view rev total.
        /// </summary>
        /// <value>
        /// The post view rev total.
        /// </value>
        public decimal PostViewRevTotal { get; set; }

        /// <summary>
        /// Gets or sets the cost total.
        /// </summary>
        /// <value>
        /// The cost total.
        /// </value>
        public decimal CostTotal { get; set; }
    }
}
