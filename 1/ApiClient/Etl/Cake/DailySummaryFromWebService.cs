namespace ApiClient.Etl.Cake
{
    public class DailySummaryFromWebService : Source<string>
    {
        private string url;

        public DailySummaryFromWebService(string url)
        {
            this.url = url;
        }

        public override void DoExtract()
        {
            //var logger = new ConsoleLogger();
            //var restCall = new RestCall(new ApiInfo());
            //var getter = new ResourceGetter(logger, url, restCall);
            //getter.GotResource += (sender, uri, url2, doc) =>
            //{
            //    AddItems(new[] { new DirectTrackResource { Name = url2, Content = doc.ToString() } });
            //};
            //getter.Error += (sender, uri, ex) =>
            //{
            //    Logger.Log("Exception: ", ex.Message);
            //};
            //getter.GetResources();
            //Done = true;
        }
    }
}
