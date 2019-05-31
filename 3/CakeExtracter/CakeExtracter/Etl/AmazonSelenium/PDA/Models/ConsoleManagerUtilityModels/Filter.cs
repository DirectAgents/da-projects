namespace CakeExtracter.Etl.AmazonSelenium.PDA.Models.ConsoleManagerUtilityModels
{
    internal class Filter
    {
        public string field { get; set; }

        public bool not { get; set; }

        public string @operator { get; set; }

        public string[] values { get; set; }

    }
}
