namespace CakeExtracter.Etl.SearchMarketing.Extracters.BingExtractors.Models
{
    /// <inheritdoc />
    /// <summary>
    /// Model for Bing row with goal that extracted from CSV report.
    /// </summary>
    public class BingGoalRow : BingBaseRow
    {
        /// <summary>
        /// Gets or sets the GoalId column.
        /// </summary>
        public string GoalId { get; set; }

        /// <summary>
        /// Gets or sets the Goal column.
        /// </summary>
        public string Goal { get; set; }

        /// <summary>
        /// Gets or sets the AllConversions column.
        /// </summary>
        public int AllConversions { get; set; }

        /// <summary>
        /// Gets or sets the AllRevenue column.
        /// </summary>
        public decimal AllRevenue { get; set; }
    }
}
