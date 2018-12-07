﻿using System;
using CakeExtractor.SeleniumApplication.Commands;
using Quartz;
using Quartz.Impl;

namespace CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda
{
    internal class ExtractAmazonPdaScheduler
    {
        public static async void Start(SyncAmazonPdaCommand command)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Context.Put("SyncAmazonPdaCommand", command);
            await scheduler.Start();

            var job = JobBuilder.Create<ExtractAmazonPdaJob>().Build();
            var trigger = CreateTrigger();
            await scheduler.ScheduleJob(job, trigger);
        }

        private static ITrigger CreateTrigger()
        {
            var startTime = GetStartTime();            
            var interval = GetInterval();
            var trigger = TriggerBuilder.Create()
                .WithIdentity("Extract PDA Campaign stats", "Amazon")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x
                    .WithInterval(interval) 
                    //.WithInterval(new TimeSpan(0,0,1,0))
                    .RepeatForever())
                .Build();
            return trigger;
        }

        private static DateTimeOffset GetStartTime()
        {
            var startTimeConfig = new DateTimeOffset(Properties.Settings.Default.StartExtractionDateTime);
            
            return startTimeConfig < DateTimeOffset.Now ? DateTimeOffset.Now : startTimeConfig;
        }

        private static TimeSpan GetInterval()
        {
            var daysInterval = Properties.Settings.Default.ExtractionIntervalsInDays;
            return new TimeSpan(daysInterval, 0, 0, 0);
        }
    }
}
