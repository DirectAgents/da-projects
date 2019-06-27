namespace SeleniumDataBrowser.PDA.Models
{
    internal class Filter
    {
        public string field { get; set; }

        public bool not { get; set; }

        public string @operator { get; set; }

        public string[] values { get; set; }
    }
}