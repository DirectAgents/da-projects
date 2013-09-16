using System.Globalization;
using CakeExtracter.CakeMarketingApi.Entities;
using RestSharp;
using RestSharp.Deserializers;

namespace CakeExtracter.CakeMarketingApi.Clients
{
    public class ConversionsClient : ApiClient
    {
        public ConversionsClient()
            : base(8, "reports", "Conversions")
        {
        }

        public ConversionReportResponse Conversions(ConversionsRequest request)
        {
            var result = Execute<ConversionReportResponse>(request, new ConversionsDeserializer());
            return result;
        }

        public class ConversionsDeserializer : IDeserializer
        {
            private XmlDeserializer restSharpXmlDeserializer;

            public ConversionsDeserializer()
            {
                Culture = CultureInfo.InvariantCulture;
                restSharpXmlDeserializer = new XmlDeserializer();
            }

            public T Deserialize<T>(IRestResponse response)
            {
                response.Content = response.Content.Replace(@"xsi:nil=""true""", "");
                return restSharpXmlDeserializer.Deserialize<T>(response);
            }

            public string RootElement { get; set; }

            public string Namespace { get; set; }

            public string DateFormat { get; set; }

            public CultureInfo Culture { get; set; }
        }
    }
}