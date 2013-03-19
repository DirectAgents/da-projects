using System;
using RestSharp;

namespace LTWeb.Service.Tracking
{
    public class TrackingService
    {
        public TrackingService(string baseUrl, string apiKey)
        {
            this.BaseUrl = baseUrl;
            this.ApiKey = apiKey;
        }

        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public IRestResponse Execute<T>(string url, T parameters)
        {
            var client = new RestClient(BaseUrl);
            var request = new RestRequest(url);

            request.AddParameter(new Parameter
                {
                    Name = "api_key",
                    Type = ParameterType.GetOrPost,
                    Value = ApiKey
                });

            var binder = new ParameterBinder();
            binder.BindParametersToRequest(parameters, request);

            IRestResponse response = client.Execute(request);
            if (response.ErrorException != null)
            {
                // TODO: do something if an error occurs
            }

            return response;
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class ParameterBinderAttribute : Attribute
    {
        private readonly string positionalString;

        // This is a positional argument
        public ParameterBinderAttribute(string positionalString)
        {
            this.positionalString = positionalString;

            // TODO: Implement code here
            throw new NotImplementedException();
        }

        public string PositionalString { get; private set; }

        // This is a named argument
        public int NamedInt { get; set; }
    }
}