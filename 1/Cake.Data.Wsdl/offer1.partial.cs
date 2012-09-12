using System.Linq;

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
    }
}
