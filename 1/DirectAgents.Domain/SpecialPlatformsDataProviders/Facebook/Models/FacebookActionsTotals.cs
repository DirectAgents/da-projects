namespace DirectAgents.Domain.SpecialPlatformsDataProviders.Facebook.Models
{
    /// <summary>
    /// Facebook Actions Totals Info.
    /// </summary>
    public class FacebookActionsTotals
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the post click.
        /// </summary>
        /// <value>
        /// The post click.
        /// </value>
        public int PostClick { get; set; }

        /// <summary>
        /// Gets or sets the post view.
        /// </summary>
        /// <value>
        /// The post view.
        /// </value>
        public int PostView { get; set; }

        /// <summary>
        /// Gets or sets the post click value.
        /// </summary>
        /// <value>
        /// The post click value.
        /// </value>
        public decimal PostClickVal { get; set; }

        /// <summary>
        /// Gets or sets the post view value.
        /// </summary>
        /// <value>
        /// The post view value.
        /// </value>
        public decimal PostViewVal { get; set; }

        /// <summary>
        /// Gets or sets the click attribute window.
        /// </summary>
        /// <value>
        /// The click attribute window.
        /// </value>
        public string ClickAttrWindow { get; set; }

        /// <summary>
        /// Gets or sets the view attribute window.
        /// </summary>
        /// <value>
        /// The view attribute window.
        /// </value>
        public string ViewAttrWindow { get; set; }
    }
}
