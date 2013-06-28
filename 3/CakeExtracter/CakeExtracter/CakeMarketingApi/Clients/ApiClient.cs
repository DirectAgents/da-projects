using System;
using CakeExtracter.Common;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    abstract public class ApiClient
    {
        protected const string ApiKey = "FCjdYAcwQE";
        protected const string Domain = "login.directagents.com";
        protected readonly int Version;
        protected readonly string ASMX;
        protected readonly string Operation;

        protected ApiClient(int version, string asmx, string operation)
        {
            Version = version;
            ASMX = asmx;
            Operation = operation;
        }

        public string BaseUrl
        {
            get { return "https://" + Domain + "/api/" + Version + "/" + ASMX + ".asmx/" + Operation; }
        }

        public T Execute<T>(ApiRequest apiRequest) where T : new()
        {
            var restRequest = new RestRequest();
            restRequest.AddParameter("api_key", ApiKey);            
            apiRequest.AddParameters(restRequest);

            var restClient = new RestClient { BaseUrl = BaseUrl };
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