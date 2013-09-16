namespace CakeExtracter.Reports
{
    public partial class SearchReportRuntimeTextTemplate : SearchReportRuntimeTextTemplateBase
    {
        public string AdvertiserName { get; set; }

        public string Week { get; set; }

        public decimal Revenue { get; set; }

        public decimal Cost { get; set; }

        public int ROAS { get; set; }

        public decimal Margin { get; set; }

        public int Orders { get; set; }

        public decimal CPO { get; set; }
    }
}
