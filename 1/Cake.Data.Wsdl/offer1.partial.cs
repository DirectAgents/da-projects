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
    }
}
