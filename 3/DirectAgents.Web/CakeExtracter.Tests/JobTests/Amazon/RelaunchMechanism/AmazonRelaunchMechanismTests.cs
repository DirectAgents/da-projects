using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Enums;
using Amazon.Exceptions;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestManagers;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Utils;
using CakeExtracter.Etl.TradingDesk.Extracters.AmazonExtractors.AmazonApiExtractors;
using CakeExtracter.Etl.TradingDesk.LoadersDA.AmazonLoaders;
using CakeExtracter.Helpers;
using CakeExtracter.Tests.Helpers;
using CakeExtracter.Tests.JobTests.Amazon.TestImplementations;
using DirectAgents.Domain.Entities.CPProg;
using Moq;
using NUnit.Framework;

namespace CakeExtracter.Tests.JobTests.Amazon.RelaunchMechanism
{
    [TestFixture(TestName = "Amazon relaunch mechanism tests.")]
    [Category("Jobs")]
    [Description("Test proper behaviour of Amazon relaunch mechanism.")]
    public class AmazonRelaunchMechanismTests
    {
        private TestJobExecutionItemRepository executionItemRepository;
        private TestJobRequestRepository requestRepository;

        private readonly List<ExtAccount> testAccounts = new List<ExtAccount>
        {
            PredefinedTestObjectsStorage.TestAccount1,
            PredefinedTestObjectsStorage.TestAccount2,
            PredefinedTestObjectsStorage.TestAccount3,
        };

        [Test(Description = "Checks if all required job requests are created if any error occurs in the daily database extractor.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInDailyExtractor(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.DailyArg);
            commandMock.Setup(m => m.CreateDailyExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>()))
                .Returns<DateRange, ExtAccount>(
                    (dateRange, account) =>
                    {
                        var extractorMock =
                            new Mock<AmazonDatabaseKeywordsToDailySummaryExtracter>(dateRange, account.Id)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
                        extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Throws<Exception>();
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateDailyLoader(It.IsAny<ExtAccount>())).Returns(() =>
            {
                var loaderMock = new Mock<AmazonDailySummaryLoader>(It.IsAny<int>()) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs in the daily database cleaner.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInDailyCleaner(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.DailyArg);
            commandMock.Setup(m => m.CreateDailyExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>()))
                .Returns<DateRange, ExtAccount>(
                    (dateRange, account) =>
                    {
                        var extractorMock =
                            new Mock<AmazonDatabaseKeywordsToDailySummaryExtracter>(dateRange, account.Id)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Throws<Exception>();
                        extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() });
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateDailyLoader(It.IsAny<ExtAccount>())).Returns(() =>
            {
                var loaderMock = new Mock<AmazonDailySummaryLoader>(It.IsAny<int>()) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs in the daily loader.")]
        [TestCase(null, null, 20)]
        [TestCase(null, null, 4)]
        [TestCase("12/1/2017", null, 1)]
        [TestCase("1/4/2019", "1/21/2019", null)]
        [TestCase("1/1/2016", "1/31/2019", 40)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInDailyLoader(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.DailyArg);
            commandMock.Setup(m => m.CreateDailyExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>()))
                .Returns<DateRange, ExtAccount>(
                    (dateRange, account) =>
                    {
                        var extractorMock =
                            new Mock<AmazonDatabaseKeywordsToDailySummaryExtracter>(dateRange, account.Id)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Verifiable();
                        extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() });
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateDailyLoader(It.IsAny<ExtAccount>()))
                .Returns<ExtAccount>(
                    account =>
                    {
                        var loaderMock = new Mock<AmazonDailySummaryLoader>(account.Id) {CallBase = true};
                        loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Throws<Exception>();
                        return loaderMock.Object;
                    });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs when a campaign info is received in the keyword extractor.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInCampaignsInfoReceivingOfKeywordExtractor(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.KeywordArg);
            var utilityMock = new Mock<AmazonUtility>();
            var exception = new ExtractDataException(
                It.IsAny<Exception>(),
                It.IsAny<string>(),
                It.IsAny<EntitesType>(),
                It.IsAny<CampaignType>(),
                It.IsAny<string>());
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Throws(exception);
            commandMock.Setup(m => m.CreateKeywordExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>(), It.IsAny<AmazonUtility>()))
                .Returns<DateRange, ExtAccount, AmazonUtility>(
                    (dateRange, account, utility) =>
                    {
                        var extractorMock =
                            new Mock<AmazonApiKeywordExtractor>(utility, dateRange, account, null, null)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateKeywordLoader(It.IsAny<ExtAccount>())).Returns(() =>
            {
                var loaderMock = new Mock<AmazonKeywordSummaryLoader>(It.IsAny<int>()) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs when a campaign info is received in the ad extractor.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInCampaignsInfoReceivingOfAdExtractor(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.CreativeArg);
            var utilityMock = new Mock<AmazonUtility>();
            var exception = new ExtractDataException(
                It.IsAny<Exception>(),
                It.IsAny<string>(),
                It.IsAny<EntitesType>(),
                It.IsAny<CampaignType>(),
                It.IsAny<string>());
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Throws(exception);
            commandMock.Setup(m => m.CreateCreativeExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>(), It.IsAny<AmazonUtility>()))
                .Returns<DateRange, ExtAccount, AmazonUtility>(
                    (dateRange, account, utility) =>
                    {
                        var extractorMock =
                            new Mock<AmazonApiAdExtrator>(utility, dateRange, account, null, null)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Verifiable();
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateCreativeLoader(It.IsAny<ExtAccount>())).Returns(() =>
            {
                var loaderMock = new Mock<AmazonAdSummaryLoader>(It.IsAny<int>()) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<TDadSummary>>())).Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }
        private void AssertRelaunchIsValid(Mock<DASynchAmazonStats> commandMock)
        {
            var command = commandMock.Object;
            var expectedCommands = new List<DASynchAmazonStats>
            {
                CopyCommandForAccountAndDateRange(command, PredefinedTestObjectsStorage.TestAccount1.Id),
                CopyCommandForAccountAndDateRange(command, PredefinedTestObjectsStorage.TestAccount2.Id),
                CopyCommandForAccountAndDateRange(command, PredefinedTestObjectsStorage.TestAccount3.Id),
            };
            var expectedResult = expectedCommands.Select(CommandArgumentsConverter.GetCommandArgumentsAsLine).ToList();
            AssertRelaunchIsValid(expectedResult);
        }

        private void AssertRelaunchIsValid(List<string> expectedResult)
        {
            Assert.IsTrue(requestRepository.ScheduledRequests.Count == expectedResult.Count);
            Assert.IsTrue(expectedResult.TrueForAll(
                res => requestRepository.ScheduledRequests.Any(
                    x => string.Equals(x.CommandName, "DASynchAmazonStats", StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(x.CommandExecutionArguments, res, StringComparison.OrdinalIgnoreCase))));
        }

        private DASynchAmazonStats CopyCommandForAccountAndDateRange(DASynchAmazonStats sourceCommand, int accountId)
        {
            var command = CopyCommand(sourceCommand);
            var dateRange = CommandHelper.GetDateRange(
                command.StartDate,
                command.EndDate,
                command.DaysAgoToStart,
                DASynchAmazonStats.DefaultDaysAgo);
            command.AccountId = accountId;
            command.StartDate = dateRange.FromDate;
            command.EndDate = dateRange.ToDate;
            command.DaysAgoToStart = null;
            return command;
        }

        private DASynchAmazonStats CopyCommand(DASynchAmazonStats sourceCommand)
        {
            return new DASynchAmazonStats
            {
                AccountId = sourceCommand.AccountId,
                DaysAgoToStart = sourceCommand.DaysAgoToStart,
                EndDate = sourceCommand.EndDate,
                StartDate = sourceCommand.StartDate,
                StatsType = sourceCommand.StatsType,
                KeepAmazonReports = sourceCommand.KeepAmazonReports,
                IntervalBetweenUnsuccessfulAndNewRequestInMinutes = sourceCommand.IntervalBetweenUnsuccessfulAndNewRequestInMinutes,
                DisabledOnly = sourceCommand.DisabledOnly,
                NoNeedToCreateRepeatRequests = sourceCommand.NoNeedToCreateRepeatRequests,
            };
        }

        private Mock<DASynchAmazonStats> CreateCommandMock(int? accountId, DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo, string statsType)
        {
            InitializeRepositories();
            var commandMock = new Mock<DASynchAmazonStats> { CallBase = true };
            SetupCommandMockProperties(commandMock, accountId, fromDateTime, toDateTime, daysAgo, statsType);
            SetupCommandMockBaseMethods(commandMock);
            return commandMock;
        }

        private void InitializeRepositories()
        {
            executionItemRepository = new TestJobExecutionItemRepository();
            requestRepository = new TestJobRequestRepository();
        }

        private void SetupCommandMockProperties(
            Mock<DASynchAmazonStats> commandMock,
            int? accountId,
            DateTime? fromDateTime,
            DateTime? toDateTime,
            int? daysAgo,
            string statsType)
        {
            commandMock.SetupProperty(x => x.AccountId, accountId);
            commandMock.SetupProperty(x => x.StartDate, fromDateTime);
            commandMock.SetupProperty(x => x.EndDate, toDateTime);
            commandMock.SetupProperty(x => x.DaysAgoToStart, daysAgo);
            commandMock.SetupProperty(x => x.StatsType, statsType);
        }

        private void SetupCommandMockBaseMethods(Mock<DASynchAmazonStats> commandMock)
        {
            commandMock.Setup(m => m.Clone()).Returns(() => CopyCommand(commandMock.Object));
            commandMock.Setup(m => m.ResetCommandExecutionContext())
                .Callback(() => ResetCommandExecutionContext(commandMock.Object));
            commandMock.Setup(m => m.GetAccounts()).Returns(testAccounts);
            commandMock.Setup(m => m.SynchAsinAnalyticTables(It.IsAny<int>()));
            commandMock.Setup(m => m.GetTokens()).Returns(new string[0]);
            commandMock.Setup(m => m.SaveTokens(It.IsAny<string[]>())).Verifiable();
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(new AmazonUtility());
        }

        private void ResetCommandExecutionContext(ConsoleCommand currentCommand)
        {
            var jobExecutionItemService = new JobExecutionItemService(executionItemRepository);
            var jobExecutionRequestService = new JobExecutionRequestService(requestRepository);
            CommandExecutionContext.ResetContext(currentCommand, jobExecutionItemService, jobExecutionRequestService);
        }
    }
}
