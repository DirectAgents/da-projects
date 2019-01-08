using System;
using CakeExtractor.SeleniumApplication.Commands;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using Quartz;
using Quartz.Impl;

namespace CakeExtractor.SeleniumApplication.Jobs.ExtractAmazonPda
{
    internal class ExtractAmazonPdaScheduler
    {
        public static async void Start(SyncAmazonPdaCommand command, JobScheduleModel scheduling)
        {
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Context.Put("SyncAmazonPdaCommand", command);
            await scheduler.Start();

            var job = JobBuilder.Create<ExtractAmazonPdaJob>().Build();
            var trigger = CreateTrigger(scheduling);
            await scheduler.ScheduleJob(job, trigger);
        }

        private static ITrigger CreateTrigger(JobScheduleModel scheduling)
        {
            var startTime = GetStartTime(scheduling);            
            var interval = GetInterval(scheduling);
            var trigger = TriggerBuilder.Create()
                .WithIdentity("Extract PDA Campaign stats", "Amazon")
                .StartAt(startTime)
                .WithSimpleSchedule(x => x
                    .WithInterval(interval) 
                    .RepeatForever())
                .Build();
            return trigger;
        }

        private static DateTimeOffset GetStartTime(JobScheduleModel scheduling)
        {
            var startTimeConfig = new DateTimeOffset(scheduling.StartExtractionTime);
            return startTimeConfig;
        }

        private static TimeSpan GetInterval(JobScheduleModel scheduling)
        {
            var interval = new TimeSpan(scheduling.DaysInterval, 0, 0, 0);
            return interval;
        }
    }
}
