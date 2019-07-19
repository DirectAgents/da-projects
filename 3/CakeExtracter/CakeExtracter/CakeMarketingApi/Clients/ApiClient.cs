using System;
using CakeExtracter.Helpers;
using Polly;
using RestSharp;
using RestSharp.Deserializers;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    abstract public class ApiClient
    {
        protected const string ApiKey = "FCjdYAcwQE";
        protected const string Domain = "login.directagents.com";
        protected readonly string BaseUrl;

        private readonly int maxRetryAttempt =
            ConfigurationHelper.GetIntConfigurationValue("CakeEventConversions_MaxRetryAttempts");

        private readonly TimeSpan pauseBetweenAttempts =
            TimeSpan.FromSeconds(ConfigurationHelper.GetIntConfigurationValue("CakeEventConversions_PauseBetweenAttemptsInSeconds"));

        protected ApiClient(int version, string asmx, string operation)
        {
            BaseUrl = "https://" + Domain + "/api/" + version + "/" + asmx + ".asmx/" + operation;
        }

        public T TryGetResponse<T>(ApiRequest apiRequest, IDeserializer deserializer = null)
            where T : new()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetry(maxRetryAttempt,
                    i => pauseBetweenAttempts,
                    (exception, timeSpan, retryCount, context) =>
                        Logger.Warn($"Failed. Will repeat request. Waiting {timeSpan}", retryCount))
                .Execute(() => GetResponse<T>(apiRequest, deserializer));
        }

        private T GetResponse<T>(ApiRequest apiRequest, IDeserializer deserializer = null)
            where T : new()
        {
            var restRequest = new RestRequest();
            //restRequest.Timeout = 300000; // testing!

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
                Logger.Error(response.ErrorException);
                throw new ApplicationException("Error retrieving response.", response.ErrorException);
            }

            return response.Data;
        }
    }
}