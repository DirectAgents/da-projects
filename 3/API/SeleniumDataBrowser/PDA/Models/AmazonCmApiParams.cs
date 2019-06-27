namespace SeleniumDataBrowser.PDA.Models
{
    internal class AmazonCmApiParams
    {
        public double startDateUTC { get; set; }

        public double endDateUTC { get; set; }

        public string[] fields { get; set; }

        public Filter[] filters { get; set; }

        public string interval { get; set; }

        public string period { get; set; }

        public string programType { get; set; }

        public int pageOffset { get; set; }

        public int pageSize { get; set; }

        public string[] queries { get; set; }

        public string sort { get; set; }
    }
}