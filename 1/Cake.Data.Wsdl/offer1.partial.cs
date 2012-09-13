using System.Linq;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Cake.Data.Wsdl.ExportService
{
    public partial class offer1
    {
        public string StatusName
        {
            get
            {
                return this.offer_status.offer_status_name;
            }
        }

        public string AdvertiserId
        {
            get
            {
                return this.advertiser.advertiser_id.ToString();
            }
        }

        public string AllowedCountries
        {
            get
            {
                string[] result = this.offer_contracts.SelectMany(oc => oc.geo_targeting.allowed_countries.Select(c => c.country.country_code)).ToArray();
                string resultCSV = string.Join(",", result);
                return resultCSV;
            }
        }

        public string VerticalName
        {
            get
            {
                return this.vertical.vertical_name;
            }
        }

        public string DefaultPriceFormat
        {
            get
            {
                return this.offer_contracts[0].price_format.price_format_name;
            }
        }

        public string AllowedMediaTypeNames
        {
            get
            {
                return string.Join(",", this.allowed_media_types.Select(c => c.media_type_name));
            }
        }

        public string Xml
        {
            get
            {
                var serializer = new XmlSerializer(typeof(offer1));
                var xdoc = new XDocument();
                XmlWriter writer = xdoc.CreateWriter();
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("offer", "http://cakemarketing.com/api/3/");
                serializer.Serialize(writer, this, ns);
                writer.Close();
                string result = xdoc.ToString();
                return result;
            }
        }
    }
}
