using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DirectAgents.Domain.Entities.CPProg.Facebook
{
    /// <summary>
    /// Facebook Reach metric entity.
    /// </summary>
    public class FbReachMetric
    {
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        [Key]
        public int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the ext account.
        /// </summary>
        [ForeignKey("AccountId")]
        public virtual ExtAccount ExtAccount { get; set; }

        /// <summary>
        /// Gets or sets the period for which reach metrics are extracted.
        /// </summary>
        [Key]
        public string Period { get; set; }

        /// <summary>
        /// Gets or sets the reach metric value.
        /// The number of people who saw your ads at least once.
        /// </summary>
        public int Reach { get; set; }
    }
}
