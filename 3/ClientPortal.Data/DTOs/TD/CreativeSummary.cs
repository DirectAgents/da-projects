
namespace ClientPortal.Data.DTOs.TD
{
    public class CreativeSummary
    {
        public int CreativeID { get; set; }
        public string CreativeName { get; set; }

        //public string Currency { get; set; }
        public int Impressions { get; set; }
        public int Clicks { get; set; }
        public int Conversions { get; set; }
        public decimal Revenue { get; set; }
    }
}
