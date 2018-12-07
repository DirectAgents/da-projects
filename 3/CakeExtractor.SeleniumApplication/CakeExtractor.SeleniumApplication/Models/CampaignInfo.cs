
namespace CakeExtractor.SeleniumApplication.Models
{
    public class CampaignInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public string Targeting { get; set; }
        public string ReportPath { get; set; }

        public CampaignInfo() { }
        
        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}
