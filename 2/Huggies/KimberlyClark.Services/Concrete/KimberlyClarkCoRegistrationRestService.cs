using System.Xml.Linq;
using KimberlyClark.Services.Abstract;
using RestSharp;

namespace KimberlyClark.Services.Concrete
{
    public class KimberlyClarkCoRegistrationRestService : IKimberlyClarkCoRegistrationRestService
    {
        public KimberlyClarkCoRegistrationRestService(string baseUrl, string userName, string password)
        {
            BaseUrl = baseUrl;
            UserName = userName;
            Password = password;
        }

        public string Password { get; set; }

        public string UserName { get; set; }

        public string BaseUrl { get; set; }

        public bool CheckIfConsumerExists(string email)
        {
            var request = new RestRequest(Method.POST)
                {
                    Resource = "CheckIfConsumerExists",
                };

#if DEBUG
            var element = new XElement("string", email);

#else
            var element = new XElement("string", email);
            //XNamespace ns = "http://schemas.microsoft.com/2003/10/Serialization/";
            //var element = new XElement(ns + "string", email);
#endif

            request.AddParameter("text/xml", element.ToString(), ParameterType.RequestBody);

            return Execute<bool>(request);
        }

        public IProcessResult ProcessConsumerInformation(IConsumer consumer)
        {
            IRestResponse<ProcessResult> restResponse;
            return ProcessConsumerInformation(consumer, out restResponse);
        }
        public IProcessResult ProcessConsumerInformation(IConsumer consumer, out IRestResponse<ProcessResult> restResponse)
        {
            var request = new RestRequest(Method.POST)
                {
                    Resource = "ProcessConsumerInformation",
                };

            request.AddParameter("text/xml", consumer.ToString(), ParameterType.RequestBody);

            return Execute<ProcessResult>(request, out restResponse);
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            IRestResponse<T> restResponse;
            return Execute<T>(request, out restResponse);
        }
        public T Execute<T>(RestRequest request, out IRestResponse<T> restResponse) where T : new()
        {
            var client = new RestClient
                {
                    BaseUrl = BaseUrl,
                    Authenticator = new HttpBasicAuthenticator(UserName, Password)
                };

            restResponse = client.Execute<T>(request);

            if (restResponse.ErrorException != null)
            {
                throw restResponse.ErrorException;
            }

            return restResponse.Data;
        }
    }
}