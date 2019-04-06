using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CakeExtracter;
using CakeExtracter.Common;
using CakeExtractor.SeleniumApplication.Models.CommonHelperModels;
using Quartz;
using Quartz.Impl;

namespace CakeExtractor.SeleniumApplication.Jobs
{
    internal class AmazonSeleniumCommandsJobScheduler
    {
        public static async Task ConfigureJobSchedule(string[] args, IEnumerable<ConsoleCommand> Commands)
        {
            var firstRunTime = Properties.Settings.Default.StartExtractionDateTime != DateTime.MinValue ?
                Properties.Settings.Default.StartExtractionDateTime :
                DateTime.Now;
            var scheduling = new JobScheduleModel
            {
                DaysInterval = Properties.Settings.Default.ExtractionIntervalsInDays,
                StartExtractionTime = firstRunTime
            };
            Logger.Info("Next run time {0}", firstRunTime);
            var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Context.Put(JobConstants.CommandLineArgs, args);
            scheduler.Context.Put(JobConstants.AllCommands, Commands);
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
                .WithIdentity("Selenium Jobs", "Amazon")
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
            var interval = new TimeSpan(0, 0, 1, 0);
            return interval;
        }
    }
}
