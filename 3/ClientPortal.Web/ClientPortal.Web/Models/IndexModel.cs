using System.Globalization;

namespace ClientPortal.Web.Models
{
    public class IndexModel
    {
        public CultureInfo CultureInfo { get; set; }
        public bool HasLogo { get; set; }
        public bool ShowCPMRep { get; set; }
    }
}