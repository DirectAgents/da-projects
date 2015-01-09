using System;
using RestSharp;
using RestSharp.Deserializers;

namespace AdRoll.Clients
{
    abstract public class ApiClient
    {
        protected const string Domain = "api.adroll.com";
        protected readonly string BaseUrl;

        private string Username { get; set; }
        private string Password { get; set; }

        protected ApiClient(int version, string service, string method)
        {
            BaseUrl = "https://" + Domain + "/v" + version + "/" + service + "/" + method;
        }

        public void SetCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public T Execute<T>(ApiRequest apiRequest) where T : new()
        {
            var restRequest = new RestRequest();

            apiRequest.AddParametersTo(restRequest);

            var restClient = new RestClient {
                BaseUrl = BaseUrl,
                Authenticator = new HttpBasicAuthenticator(Username, Password)
            };
            restClient.AddHandler("text/plain", new JsonDeserializer());

            var response = restClient.ExecuteAsGet<T>(restRequest, "GET");

            if (response.ErrorException != null)
            {
                //Logger.Error(response.ErrorException);
                //throw new ApplicationException("Error retrieving response.", response.ErrorException);
                return default(T);
            }

            return response.Data;
        }
    }
}
