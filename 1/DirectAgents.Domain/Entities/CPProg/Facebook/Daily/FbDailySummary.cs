namespace DirectAgents.Domain.Entities.CPProg.Facebook.Daily
{
    /// <summary>
    /// Facebook daily summary entity
    /// </summary>
    /// <seealso cref="DirectAgents.Domain.Entities.CPProg.Facebook.FbBaseSummary" />
    public class FbDailySummary : FbBaseSummary
    {
        /// <summary>
        /// Gets or sets the Account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public int AccountId { get; set; }
    }
}
