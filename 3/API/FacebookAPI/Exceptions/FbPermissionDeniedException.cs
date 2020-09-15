using System;

namespace FacebookAPI.Exceptions
{
    /// <summary>
    /// Class represents an permission denied exception for Facebook.
    /// </summary>
    public class FbPermissionDeniedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FbPermissionDeniedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public FbPermissionDeniedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FbPermissionDeniedException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FbPermissionDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}