using System;
using Amazon.Enums;

namespace Amazon.Exceptions
{
    public class ExtractDataException : Exception
    {
        private const string ExceptionMessage = "Exception in data extraction";

        public ExtractDataException(Exception exception, string details, EntitesType entitiesType,
            CampaignType campaignType, string profileId) : base(
            $"{ExceptionMessage} - {details} ({campaignType}, {entitiesType}, EID - {profileId}): {exception.Message}")
        {
        }
    }
}
