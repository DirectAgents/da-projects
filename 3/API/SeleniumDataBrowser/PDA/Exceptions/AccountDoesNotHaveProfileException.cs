using System;

namespace SeleniumDataBrowser.PDA.Exceptions
{
    /// <inheritdoc />
    /// <summary>
    /// Exception for cases when portal profile doesn't have account.
    /// </summary>
    public class AccountDoesNotHaveProfileException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountDoesNotHaveProfileException"/> class.
        /// </summary>
        /// <param name="accountName">Name of account.</param>
        /// <param name="profileName">Name of profile.</param>
        public AccountDoesNotHaveProfileException(string accountName, string profileName)
            : base($"The account {accountName} does not have the following profile: {profileName}")
        {
        }
    }
}
