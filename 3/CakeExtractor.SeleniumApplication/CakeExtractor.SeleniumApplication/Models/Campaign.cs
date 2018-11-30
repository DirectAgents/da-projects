
namespace CakeExtractor.SeleniumApplication.Models
{
    public class Campaign
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public string Targeting { get; set; }
        public string ReportPath { get; set; }

        public Campaign() { }
        
        public override string ToString()
        {
            return $"{Name} ({Id})";
        }
    }
}
