using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using RestSharp;

namespace SeleniumDataBrowser.Helpers
{
    internal class RestRequestHelper
    {
        public static IRestResponse<T> SendPostRequest<T>(string baseUrl, RestRequest request)
            where T : new()
        {
            var restClient = new RestClient(baseUrl);
            var response = restClient.ExecuteAsPost<T>(request, "POST");
            return response;
        }

        public static RestRequest CreateRestRequest(
            string relativePath,
            Dictionary<string, string> cookies,
            Dictionary<string, string> queryParams = null,
            object body = null)
        {
            var request = CreateSimpleRequestObject(relativePath);
            SetCookies(request, cookies);
            SetQueryParameters(request, queryParams);
            SetBodyParameters(request, body);
            return request;
        }

        private static RestRequest CreateSimpleRequestObject(string resourceUri)
        {
            var request = new RestRequest(resourceUri)
            {
                OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; },
            };
            return request;
        }

        private static void SetCookies(IRestRequest request, Dictionary<string, string> cookies)
        {
            cookies?.ForEach(x => request.AddCookie(x.Key, x.Value));
        }

        private static void SetQueryParameters(IRestRequest request, Dictionary<string, string> queryParams)
        {
            queryParams?.ForEach(x => request.AddQueryParameter(x.Key, x.Value));
        }

        private static void SetBodyParameters(IRestRequest request, object body)
        {
            if (body != null)
            {
                request.AddJsonBody(body);
            }
        }
    }
}
