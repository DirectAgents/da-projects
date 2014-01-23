namespace CakeExtracter.Reports
{
    public partial class CPMReportTemplate : CPMReportTemplateBase
    {
        public string PortalUrl { get; set; }

        private string _summary;
        public string Summary
        {
            set { _summary = value; }
            get { return ReplaceSpecialChars(_summary); }
        }

        private string _optimization;
        public string Optimization
        {
            set { _optimization = value; }
            get { return ReplaceSpecialChars(_optimization); }
        }

        private string _nextSteps;
        public string NextSteps
        {
            set { _nextSteps = value; }
            get { return ReplaceSpecialChars(_nextSteps); }
        }

        private string ReplaceSpecialChars(string text)
        {
            text = text.Replace("\r\n", "<br />\r\n");
            text = text.Replace(">>", "<img src=\"" + PortalUrl + "Images/bullet_triangle.gif\" width=\"17\" height=\"9\">");
            return text;
        }
    }
}
        //public ActionResult PreviewTempl(int id)
        //{
        //    var report = cpRepo.GetCPMReport(id, true);
        //    if (report == null)
        //        return HttpNotFound();

        //    var template = new CPMReportTemplate()
        //    {
        //        PortalUrl = WebConfigurationManager.AppSettings["PortalUrl"],
        //        Summary = report.Summary,
        //        NextSteps = report.Conclusion
        //    };
        //    var content = template.TransformText();
        //    return Content(content);
        //}
