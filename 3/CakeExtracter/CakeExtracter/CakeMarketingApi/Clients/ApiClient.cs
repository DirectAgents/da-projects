using System;
using CakeExtracter.Helpers;
using Polly;
using RestSharp;
using RestSharp.Deserializers;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public abstract class ApiClient
    {
        protected const string ApiKey = "FCjdYAcwQE";
        protected const string Domain = "login.directagents.com";
        protected readonly string BaseUrl;

        private int maxRetryAttempt;
        private TimeSpan pauseBetweenAttempts;

        protected ApiClient(int version, string asmx, string operation)
        {
            BaseUrl = $"https://{Domain}/api/{version}/{asmx}.asmx/{operation}";
            SetConfigurationValues();
        }

        public T TryGetResponse<T>(ApiRequest apiRequest, IDeserializer deserializer = null)
            where T : new()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    maxRetryAttempt,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                        Logger.Warn($"Failed. Will repeat request. Waiting {timeSpan}", retryCount))
                .Execute(() => GetResponse<T>(apiRequest, deserializer));
        }

        private void SetConfigurationValues()
        {
            maxRetryAttempt = ConfigurationHelper.GetIntConfigurationValue("CakeEventConversions_MaxRetryAttempts");
            var pauseBetweenAttemptsInSeconds = ConfigurationHelper.GetIntConfigurationValue("CakeEventConversions_PauseBetweenAttemptsInSeconds");
            pauseBetweenAttempts = TimeSpan.FromSeconds(pauseBetweenAttemptsInSeconds);
        }

        private T GetResponse<T>(ApiRequest apiRequest, IDeserializer deserializer = null)
            where T : new()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("api_key", ApiKey);
            apiRequest.AddParameters(restRequest);

            var restClient = new RestClient { BaseUrl = new Uri(BaseUrl) };
            if (deserializer != null)
            {
                restClient.AddHandler("text/xml", deserializer);
            }

            var response = restClient.ExecuteAsGet<T>(restRequest, "GET");
            if (response.ErrorException != null)
            {
                Logger.Warn(response.ErrorException.Message);
                throw new ApplicationException("Error retrieving response.", response.ErrorException);
            }
            return response.Data;
        }
    }
}