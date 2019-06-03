using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.Entities;
using Amazon.Entities.Summaries;
using Amazon.Enums;
using Amazon.Exceptions;
using CakeExtracter.Commands;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Services;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Services.JobRequestSchedulers;
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
    [Description("Test proper behavior of Amazon relaunch mechanism.")]
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
                        extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Throws<Exception>().Verifiable();
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
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateRange>())).Throws<Exception>().Verifiable();
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
        [TestCase("1/4/2018", "1/21/2019", null)]
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
                        extractorMock.Setup(m => m.GetDailySummaryDataFromDataBase()).Returns(new List<DailySummary> { new DailySummary() }).Verifiable();
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateDailyLoader(It.IsAny<ExtAccount>()))
                .Returns<ExtAccount>(
                    account =>
                    {
                        var loaderMock = new Mock<AmazonDailySummaryLoader>(account.Id) {CallBase = true};
                        loaderMock.Setup(m => m.LoadItems(It.IsAny<List<DailySummary>>())).Throws<Exception>().Verifiable();
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
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Throws(exception).Verifiable();
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
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
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Throws(exception).Verifiable();
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
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

        [Test(Description = "Checks if all required job requests are created if any error occurs when a target keyword report is received.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInTargetKeywordReportReceiving(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.KeywordArg);
            var utilityMock = new Mock<AmazonUtility>();
            var exception = new ExtractDataException(
                It.IsAny<Exception>(),
                It.IsAny<string>(),
                It.IsAny<EntitesType>(),
                It.IsAny<CampaignType>(),
                It.IsAny<string>());
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Returns(new List<AmazonCampaign>());
            utilityMock.Setup(m => m.ReportKeywords(It.IsAny<CampaignType>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonKeywordDailySummary>());
            utilityMock.Setup(m => m.ReportTargetKeywords(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>())).Throws(exception).Verifiable();
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
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
            AssertRelaunchIsValid(commandMock, false);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs when a keyword report is received.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInKeywordReportReceiving(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.KeywordArg);
            var utilityMock = new Mock<AmazonUtility>();
            var exception = new ExtractDataException(
                It.IsAny<Exception>(),
                It.IsAny<string>(),
                It.IsAny<EntitesType>(),
                It.IsAny<CampaignType>(),
                It.IsAny<string>());
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Returns(new List<AmazonCampaign>());
            utilityMock.Setup(m => m.ReportKeywords(It.IsAny<CampaignType>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Throws(exception).Verifiable();
            utilityMock.Setup(m => m.ReportTargetKeywords(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonTargetKeywordDailySummary>());
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
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
            AssertRelaunchIsValid(commandMock, false);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs in the keyword database cleaner.")]
        [TestCase(null, null, 1)]
        [TestCase(null, null, 10)]
        [TestCase("12/1/2018", null, 10)]
        [TestCase("1/1/2019", "1/31/2019", null)]
        [TestCase("1/1/2019", "1/31/2019", 4)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInKeywordCleaner(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.KeywordArg);
            var utilityMock = new Mock<AmazonUtility>();
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Returns(new List<AmazonCampaign>());
            utilityMock.Setup(m => m.ReportKeywords(It.IsAny<CampaignType>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonKeywordDailySummary>());
            utilityMock.Setup(m => m.ReportTargetKeywords(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonTargetKeywordDailySummary>());
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
            commandMock.Setup(m => m.CreateKeywordExtractor(It.IsAny<DateRange>(), It.IsAny<ExtAccount>(), It.IsAny<AmazonUtility>()))
                .Returns<DateRange, ExtAccount, AmazonUtility>(
                    (dateRange, account, utility) =>
                    {
                        var extractorMock =
                            new Mock<AmazonApiKeywordExtractor>(utility, dateRange, account, null, null)
                            {
                                CallBase = true,
                            };
                        extractorMock.Setup(m => m.RemoveOldData(It.IsAny<DateTime>())).Throws<Exception>().Verifiable();
                        return extractorMock.Object;
                    });
            commandMock.Setup(m => m.CreateKeywordLoader(It.IsAny<ExtAccount>())).Returns(() =>
            {
                var loaderMock = new Mock<AmazonKeywordSummaryLoader>(It.IsAny<int>()) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock, false);
        }

        [Test(Description = "Checks if all required job requests are created if any error occurs in the keyword loader.")]
        [TestCase(null, null, 20)]
        [TestCase(null, null, 4)]
        [TestCase("12/1/2018", null, 1)]
        [TestCase("1/4/2019", "1/21/2019", null)]
        [TestCase("6/1/2018", "1/31/2019", 40)]
        [TestCase(null, null, null)]
        public void AmazonRelaunchMechanism_RelaunchCommandAfterExceptionInKeywordLoader(DateTime? fromDateTime, DateTime? toDateTime, int? daysAgo)
        {
            var commandMock = CreateCommandMock(null, fromDateTime, toDateTime, daysAgo, StatsTypeAgg.KeywordArg);
            var utilityMock = new Mock<AmazonUtility>();
            utilityMock.Setup(m => m.GetCampaigns(It.IsAny<CampaignType>(), It.IsAny<string>())).Returns(new List<AmazonCampaign>());
            utilityMock.Setup(m => m.ReportKeywords(It.IsAny<CampaignType>(), It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonKeywordDailySummary> { new AmazonKeywordDailySummary{ Clicks = 1 } });
            utilityMock.Setup(m => m.ReportTargetKeywords(It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new List<AmazonTargetKeywordDailySummary>());
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(utilityMock.Object);
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
            commandMock.Setup(m => m.CreateKeywordLoader(It.IsAny<ExtAccount>())).Returns<ExtAccount>(account =>
            {
                var loaderMock = new Mock<AmazonKeywordSummaryLoader>(account.Id) { CallBase = true };
                loaderMock.Setup(m => m.LoadItems(It.IsAny<List<KeywordSummary>>())).Throws<Exception>().Verifiable();
                return loaderMock.Object;
            });

            commandMock.Object.Run();
            AssertRelaunchIsValid(commandMock);
        }

        private void AssertRelaunchIsValid(Mock<DASynchAmazonStats> commandMock, bool forDateRange = true)
        {
            var command = commandMock.Object;
            List<DASynchAmazonStats> expectedCommands;
            if (forDateRange)
            {
                expectedCommands = testAccounts.Select(x => CopyCommandForAccountAndDateRange(command, x.Id)).ToList();
            }
            else
            {
                var commandDateRange = GetCommandDateRange(command);
                expectedCommands = commandDateRange.Dates.SelectMany(date =>
                    testAccounts.Select(x => CopyCommandForAccountAndDate(command, x.Id, date))).ToList();
            }

            var expectedResult = expectedCommands.Select(CommandArgumentsConverter.GetCommandArgumentsAsLine).ToList();
            AssertRelaunchIsValid(expectedResult);
        }

        private void AssertRelaunchIsValid(List<string> expectedResult)
        {
            Assert.AreEqual(expectedResult.Count, requestRepository.ScheduledRequests.Count);
            Assert.IsTrue(expectedResult.TrueForAll(
                res => requestRepository.ScheduledRequests.Any(
                    x => string.Equals(x.CommandName, "DASynchAmazonStats", StringComparison.OrdinalIgnoreCase) &&
                         string.Equals(x.CommandExecutionArguments, res, StringComparison.OrdinalIgnoreCase))));
        }

        private DASynchAmazonStats CopyCommandForAccountAndDateRange(DASynchAmazonStats sourceCommand, int accountId)
        {
            var command = CopyCommandForAccount(sourceCommand, accountId);
            var dateRange = GetCommandDateRange(command);
            SetCommandDateParameters(command, dateRange.FromDate, dateRange.ToDate);
            return command;
        }

        private DASynchAmazonStats CopyCommandForAccountAndDate(DASynchAmazonStats sourceCommand, int accountId, DateTime date)
        {
            var command = CopyCommandForAccount(sourceCommand, accountId);
            SetCommandDateParameters(command, date, date);
            return command;
        }

        private DASynchAmazonStats CopyCommandForAccount(DASynchAmazonStats sourceCommand, int accountId)
        {
            var command = CopyCommand(sourceCommand);
            command.AccountId = accountId;
            return command;
        }

        private void SetCommandDateParameters(DASynchAmazonStats sourceCommand, DateTime fromDate, DateTime toDate)
        {
            sourceCommand.StartDate = fromDate;
            sourceCommand.EndDate = toDate;
            sourceCommand.DaysAgoToStart = null;
        }

        private DateRange GetCommandDateRange(DASynchAmazonStats command)
        {
            return CommandHelper.GetDateRange(
                command.StartDate,
                command.EndDate,
                command.DaysAgoToStart,
                DASynchAmazonStats.DefaultDaysAgo);
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
            commandMock.Setup(m => m.PrepareCommandExecutionContext())
                .Callback(() => ResetCommandExecutionContext(commandMock.Object));
            commandMock.Setup(m => m.GetAccounts()).Returns(testAccounts);
            commandMock.Setup(m => m.SynchAsinAnalyticTables(It.IsAny<int>()));
            commandMock.Setup(m => m.GetTokens()).Returns(new string[0]);
            commandMock.Setup(m => m.SaveTokens(It.IsAny<string[]>())).Verifiable();
            commandMock.Setup(m => m.CreateUtility(It.IsAny<ExtAccount>())).Returns(new AmazonUtility());
        }

        private void ResetCommandExecutionContext(ConsoleCommand currentCommand)
        {
            var jobExecutionItemService = new JobExecutionItemService(executionItemRepository, requestRepository);
            var jobExecutionRequestService = new JobExecutionRequestScheduler(requestRepository);
            CommandExecutionContext.ResetContext(currentCommand, jobExecutionItemService, jobExecutionRequestService);
        }
    }
}
