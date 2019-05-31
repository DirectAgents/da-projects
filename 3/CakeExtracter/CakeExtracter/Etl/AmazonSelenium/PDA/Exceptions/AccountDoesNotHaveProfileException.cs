using System;

namespace CakeExtracter.Etl.AmazonSelenium.PDA.Exceptions
{
    public class AccountDoesNotHaveProfileException : Exception
    {
        public AccountDoesNotHaveProfileException(string accountName, string profileName) 
            : base($"The account {accountName} does not have the following profile: {profileName}")
        {
        }
    }
}
