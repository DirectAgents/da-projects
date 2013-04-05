using ClientPortal.Data.Contexts;
using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class IndexModel
    {
        public CultureInfo CultureInfo { get; set; }
        public CakeAdvertiser Advertiser { get; set; }
    }
}