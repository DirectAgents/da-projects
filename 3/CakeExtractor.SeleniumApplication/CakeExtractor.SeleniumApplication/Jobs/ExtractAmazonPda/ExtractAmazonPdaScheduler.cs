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
            var trigger = TriggerBuilder.Create()
                .WithIdentity("Extract PDA Campaign stats", "Amazon")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .Build();
            return trigger;
        }
    }
}
