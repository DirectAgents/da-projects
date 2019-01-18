using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CakeExtractor.SeleniumApplication.Commands;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using Quartz;
using Quartz.Impl;

namespace CakeExtractor.SeleniumApplication.Jobs
{
    internal class AmazonSeleniumCommandsJobScheduler
    {
        public static async Task ConfigureJobSchedule(List<BaseAmazonSeleniumCommand> commands, string[] args)
        {
            var scheduling = new JobScheduleModel
            {
                DaysInterval = Properties.Settings.Default.ExtractionIntervalsInDays,
                //StartExtractionTime = Properties.Settings.Default.StartExtractionDateTime
                StartExtractionTime = DateTime.Now
            };
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Context.Put(JobConstants.CommandsJobContextValue, commands);
            scheduler.Context.Put("args", args);
            await scheduler.Start();

            var job = JobBuilder.Create<AmazonSeleniumCommandsJob>().Build();
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
