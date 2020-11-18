using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Exceptions;

namespace CakeExtracter.Etl.AmazonAttribution.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// The common exception for cases where something is wrong in Amazon Attribution ETL.
    /// </summary>
    public class AttributionFailedEtlException : FailedEtlException
    {
        /// <inheritdoc cref="FailedEtlException"/>
        /// <summary>
        /// Initializes a new instance of the <see cref="AttributionFailedEtlException"/> class.
        /// </summary>
        /// <param name="startDate">Start date from which statistics was extracted.</param>
        /// <param name="endDate">End date to which statistics was extracted.</param>
        /// <param name="accountId">Account Id in the database for which statistics was extracted.</param>
        /// <param name="innerException">The source exception.</param>
        public AttributionFailedEtlException(DateTime? startDate, DateTime? endDate, int? accountId, Exception innerException)
            : base(startDate, endDate, accountId, innerException)
        {
        }
    }
}
