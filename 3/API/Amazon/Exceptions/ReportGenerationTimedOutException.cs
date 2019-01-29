using System;

namespace CakeExtracter.Exceptions
{
    public class ReportGenerationTimedOutException : Exception
    {
        private const string ExceptionMessage = "Generation timed out";

        public ReportGenerationTimedOutException(string message) : base($"{ExceptionMessage}: {message}")
        {
        }
    }
}
