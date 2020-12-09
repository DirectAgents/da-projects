using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectAgents.Domain.Entities.CPProg.Adform
{
    /// <inheritdoc />
    /// <summary>
    /// Adform Tracking Point database entity.
    /// </summary>
    public class AdfTrackingPoint : AdfBaseEntity
    {
        /// <summary>
        /// Gets or sets the database ID of parent Line Item.
        /// </summary>
        public int LineItemId { get; set; }

        /// <summary>
        /// Gets or sets the parent Line Item.
        /// </summary>
        public virtual AdfLineItem LineItem { get; set; }
    }
}
