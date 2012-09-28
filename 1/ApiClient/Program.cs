﻿using System.Net;
using ApiClient.Etl.Cake;
using System;
//using IronPython.Hosting;
//using Microsoft.Scripting;
//using Microsoft.Scripting.Hosting;

namespace ApiClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 50;

            //var source = new ConversionsFromWebService();
            //var dest = new ConversionsToStaging();

            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/campaign/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/advertiser/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/affiliate/");
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/creative/campaign/[campaign_id]/");
            
            //var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/payout/campaign/[campaign_id]/", DirectTrack.ResourceGetterMode.ResourceList);
            var source = new ApiClient.Etl.DirectTrack.ResourcesFromDirectTrack("1/payout/campaign/[campaign_id]/", DirectTrack.ResourceGetterMode.Resource, 10000);

            var dest = new ApiClient.Etl.DirectTrack.ResourcesToDatabase();

            //var source = new DailySummariesFromWebService();
            //var dest = new DailySummariesToDatabase();
		
            var extract = source.Extract();
            var load = dest.Load(source);
            extract.Join();
            load.Join();

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
