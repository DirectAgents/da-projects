using System.Net;
using ApiClient.Etl.Cake;
using System;
using Common;
using DirectTrack;
using System.Diagnostics;
//using IronPython.Hosting;
//using Microsoft.Scripting;
//using Microsoft.Scripting.Hosting;

namespace ApiClient
{
    class Program
    {
        static Program()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
        }

        static void Main(string[] args)
        {
            string command = args[0];

            if (command == "DailySummaries")
            {
                var source = new DailySummariesFromWebService();
                var dest = new DailySummariesToDatabase();
                var extract = source.Extract();
                var load = dest.Load(source);
                extract.Join();
                load.Join();
            }
            else if (command == "DT")
            {
                DT(args);
            }
            else
            {
                Console.WriteLine("Invalid Command");
            }
        }

        // DT <accessID> <pointsThreshold>
        static void DT(string[] args)
        {
            ApiInfo.LoginAccessId = int.Parse(args[1]);
            ResourceGetter.PointsThreshold = int.Parse(args[2]);
            int year = int.Parse(args[3]);

            Stopwatch sw = Stopwatch.StartNew();

            //var source = new ConversionsFromWebService();
            //var dest = new ConversionsToStaging();

            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/campaign/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/advertiser/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/affiliate/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/creative/campaign/[campaign_id]/");
            
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/payout/campaign/[campaign_id]/", DirectTrack.ResourceGetterMode.ResourceList);
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/payout/campaign/[campaign_id]/", DirectTrack.ResourceGetterMode.Resource, 10000);

            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack(
            //                                                    url: "1/leadDetail/campaign/[campaign_id]/[yyyy]-[mm]-[dd]/",
            //                                                    mode: DirectTrack.ResourceGetterMode.ResourceList,
            //                                                    dateRange: new DateRange(new DateTime(2002, 1, 1), DateTime.Now),
            //                                                    threadMode: ThreadMode.Multiple);

            var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack(
                                                    // url: "3/statCampaign/cumulative/campaign/[campaign_id]/[yyyy]-[mm]",
                                                    //url: "1/statCampaign/cumulative/campaign/1077/[yyyy]-[mm]",
                                                    //url: "3/statCampaign/daily/campaign/1077/[yyyy]-[mm]",
                                                    url: "1/statCampaign/affiliate/campaign/[campaign_id]/[yyyy]-[mm]",
                                                    //statCampaign/affiliate/campaign/[campaign_id]/[yyyy]-[mm]
                                                    mode: DirectTrack.ResourceGetterMode.ResourceList,
                                                    dateRange: new DateRange(new DateTime(2007, 1, 1), new DateTime(2011, 1, 1), x => x.AddMonths(1)),
                                                    threadMode: ThreadMode.Multiple);

            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack(
            //                            //url: "1/programLead/campaign/[campaign_id]/[yyyy]-[mm]-[dd]/",
            //                            url: "1/programLead/campaign/[campaign_id]/[yyyy]-[mm]-[dd]/",
            //                            mode: DirectTrack.ResourceGetterMode.Resource,
            //                            dateRange: new DateRange(new DateTime(year, 1, 1), new DateTime(year, 1, 1).AddYears(1), x => x.AddDays(1)),
            //                            threadMode: ThreadMode.Multiple);
            
            //leadDetail/campaign/[campaign_id]/[yyyy]-[mm]-[dd]/
            var dest = new ApiClient.Etl.DirectTrack.ResourcesToDatabase();

            //var source = new DailySummariesFromWebService();
            //var dest = new DailySummariesToDatabase();
		
            var extract = source.Extract();
            var load = dest.Load(source);
            extract.Join();
            load.Join();

            sw.Stop();
            TimeSpan elapsed = sw.Elapsed;
            Console.WriteLine("Time Elapsed: " + elapsed.ToString());

            Console.WriteLine("Press a key...");
            Console.ReadKey();
        }

        //static void Main()
        //{
        //    var p = new Program();
        //    p.foo();
        //}
        //void foo()
        //{
        //    scope = engine.CreateScope();
        //    scope.SetVariable("s", "Hello");
        //    string code = "print s";
        //    ScriptSource source = engine.CreateScriptSourceFromString(code, SourceCodeKind.SingleStatement);
        //    source.Execute(scope); 
        //}
        //private ScriptEngine engine = Python.CreateEngine();
        //private ScriptScope scope = null;
    }
}
