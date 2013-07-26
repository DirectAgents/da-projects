using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using CakeExtracter.Common;
using Quartz;

namespace CakeExtracter.Commands
{
    public class ScheduledCommand : ConsoleCommand
    {
        public int IntervalCount { get; set; }
        public char IntervalUnit { get; set; }
        public int StartHoursFromNow { get; set; }

        private IScheduler scheduler;
        private ConsoleCommand consoleCommandToExecute;

        public ScheduledCommand(IScheduler scheduler, ConsoleCommand consoleCommandToExecute)
        {
            this.scheduler = scheduler;
            this.consoleCommandToExecute = consoleCommandToExecute;
            IsCommand(this.consoleCommandToExecute.Command, "scheduled command: " + this.consoleCommandToExecute.Command);
        }

        public override int Execute(string[] remainingArguments)
        {
            return this.IntervalCount > 0
                            ? RunSchedule()
                            : this.consoleCommandToExecute.Execute(null);
        }

        private int RunSchedule()
        {
            string thisTypeName = this.GetType().FullName;

            var jobDetail = JobBuilder.Create<ScheduledCommandJob>()
                    .WithIdentity("Job_" + thisTypeName, "Group_" + thisTypeName)
                    .Build();

            var jobDataMap = jobDetail.JobDataMap;
            Type consoleCommandToExecuteType = this.consoleCommandToExecute.GetType();
            jobDataMap["typeName"] = consoleCommandToExecuteType.FullName;
            jobDataMap["assemblyName"] = consoleCommandToExecuteType.Assembly.FullName;
            foreach (var argumentProperty in this.consoleCommandToExecute.GetArgumentProperties())
            {
                jobDetail.JobDataMap["property." + argumentProperty.Name] = consoleCommandToExecuteType
                                                                                    .GetProperty(argumentProperty.Name)
                                                                                    .GetValue(this.consoleCommandToExecute);
            }

            var triggerBuilder = TriggerBuilder
                                    .Create()
                                    .WithIdentity("Trigger_" + thisTypeName, "Group_" + thisTypeName)
                                    .WithSimpleSchedule(c => c.WithInterval(GetInterval()).RepeatForever());
            if (StartHoursFromNow > 0)
                triggerBuilder.StartAt(DateTimeOffset.UtcNow.AddHours(StartHoursFromNow));
            else
                triggerBuilder.StartNow();

            var trigger = triggerBuilder.Build();
            this.scheduler.ScheduleJob(jobDetail, trigger);
            SleepLoopForever();
            return 0;
        }

        private TimeSpan GetInterval()
        {
            TimeSpan result;
            switch (IntervalUnit)
            {
                case 'h':
                    result = TimeSpan.FromHours(IntervalCount);
                    break;
                case 'm':
                    result = TimeSpan.FromMinutes(IntervalCount);
                    break;
                case 's':
                    result = TimeSpan.FromSeconds(IntervalCount);
                    break;
                default:
                    throw new Exception("invalid interval unit");
            }
            return result;
        }

        private static void SleepLoopForever()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                Logger.Info("ScheduledSynchDailySummariesCommand running for {0}", stopwatch.Elapsed.ToString());
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }

        public override IEnumerable<PropertyInfo> GetArgumentProperties()
        {
            return this.consoleCommandToExecute.GetArgumentProperties().Concat(base.GetArgumentProperties());
        }

        public override bool TrySetProperty(string propertyName, object propertyValue)
        {
            return this.consoleCommandToExecute.TrySetProperty(propertyName, propertyValue) ||
                    base.TrySetProperty(propertyName, propertyValue);
        }
    }
}
