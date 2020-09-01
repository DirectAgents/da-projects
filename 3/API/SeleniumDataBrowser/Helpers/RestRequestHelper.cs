using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using OpenQA.Selenium;
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

        public static IRestResponse<T> SendGetRequest<T>(string baseUrl, RestRequest request)
            where T : new()
        {
            var restClient = new RestClient(baseUrl);
            var response = restClient.ExecuteAsGet<T>(request, "GET");
            return response;
        }

        public static RestRequest CreateRestRequest(
            string relativePath,
            IEnumerable<Cookie> cookies,
            Dictionary<string, string> queryParams = null,
            object body = null,
            string token = null)
        {
            var request = CreateSimpleRequestObject(relativePath);
            SetCookies(request, cookies);
            SetQueryParameters(request, queryParams);
            SetBodyParameters(request, body);
            SetToken(request, token);
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

        private static void SetCookies(IRestRequest request, IEnumerable<Cookie> cookies)
        {
            cookies?.ForEach(x => request.AddCookie(x.Name, x.Value));
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

        private static void SetToken(IRestRequest request, string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.AddHeader("X-CSRF-token", token);
            }
        }
    }
}
