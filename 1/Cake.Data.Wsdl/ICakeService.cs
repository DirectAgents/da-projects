using Cake.Data.Wsdl.ExportService;
using Cake.Data.Wsdl.ReportsService;
using System.Collections.Generic;

namespace Cake.Data.Wsdl
{
    //[LogMethodBoundary]
    public interface ICakeService
    {
        conversion[] Conversions();
        Cake.Data.Wsdl.ExportService.advertiser[] ExportAdvertisers();
        Cake.Data.Wsdl.ExportService.affiliate[] ExportAffiliates();
        IEnumerable<campaign[]> ExportCampaigns();
        offer1[] ExportOffers();
    }
}
