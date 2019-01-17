using System;

namespace CakeExtractor.SeleniumApplication.Exceptions
{
    public class AccountDoesNotHaveProfileException : Exception
    {
        public AccountDoesNotHaveProfileException()
        {
        }

        public AccountDoesNotHaveProfileException(string message) : base(message)
        {
        }

        public AccountDoesNotHaveProfileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
