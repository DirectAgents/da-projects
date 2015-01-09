using System;
using System.Configuration;
using RestSharp;
using RestSharp.Deserializers;

namespace AdRoll.Clients
{
    abstract public class ApiClient
    {
        protected const string Domain = "api.adroll.com";
        protected readonly string BaseUrl;

        // ?move these to AdRollUtility and pass them in?
        private readonly string Username = ConfigurationManager.AppSettings["AdRollUsername"];
        private readonly string Password = ConfigurationManager.AppSettings["AdRollPassword"];

        protected ApiClient(int version, string service, string method)
        {
            BaseUrl = "https://" + Domain + "/v" + version + "/" + service + "/" + method;
        }

        public T Execute<T>(ApiRequest apiRequest, IDeserializer deserializer = null) where T : new()
        {
            var restRequest = new RestRequest();
            //{
            //    RequestFormat = DataFormat.Json
            //};
            apiRequest.AddParameters(restRequest);

            var restClient = new RestClient {
                BaseUrl = BaseUrl,
                Authenticator = new HttpBasicAuthenticator(Username, Password)
            };

            if (deserializer != null)
            {
                restClient.AddHandler("text/plain", deserializer);
            }

            var response = restClient.ExecuteAsGet<T>(restRequest, "GET");

            if (response.ErrorException != null)
            {
                //Logger.Error(response.ErrorException);
                throw new ApplicationException("Error retrieving response.", response.ErrorException);
            }

            return response.Data;
        }
    }
}
