using System;
using CakeExtracter.Etl.Facebook.Exceptions;

namespace CakeExtracter.Etl.Facebook.Interfaces
{
    /// <summary>
    /// Interface to provide event of failed loading process.
    /// </summary>
    public interface IFacebookLoadingErrorHandler
    {
        /// <summary>
        /// Action for exception of failed loading process.
        /// </summary>
        event Action<FacebookFailedEtlException> ProcessFailedLoading;
    }
}