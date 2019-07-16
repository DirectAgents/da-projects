using System;
using System.Collections.Generic;
using System.Threading;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
using CakeExtracter.Common.JobExecutionManagement.ProcessManagers.Interfaces;
using CakeExtracter.SimpleRepositories.BaseRepositories.Interfaces;
using CakeExtracter.Tests.Core.CommandExecutionContextTests.MockDIModules;
using CakeExtracter.Tests.Core.CommandExecutionContextTests.TestCommand;
using DirectAgents.Domain.Entities.Administration.JobExecution;
using DirectAgents.Domain.Entities.Administration.JobExecution.Enums;
using Moq;
using NUnit.Framework;

namespace CakeExtracter.Tests.Core.CommandExecutionContextTests
{
    [TestFixture(TestName = "Command Execution Context.")]
    [Category("Core")]
    [Description("Test proper behavior of command execution context.")]
    public class CommandExecutionContextTests
    {
        [Test(Description = "Creates new scheduled job request when request id not defined in command.")]
        public void ResetContext_JobCommandWithoutRequestId_ScheduledJobRequestCreated()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand();
            testCommand.PrepareCommandExecutionContext();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.AddItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled && jr.AttemptNumber == 0)),
                Times.Once);
        }

        [Test(Description = "Doesn't create job request when job request id defined in command but fetching existing one.")]
        public void ResetContext_JobCommandWithRequestId_JobRequestNotCreatedButFetchedExisting()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest { Id = 15 });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand
            {
                RequestId = 15,
            };
            testCommand.PrepareCommandExecutionContext();

            // Assert
            jobRequestsRepositoryMock.Verify(mock => mock.AddItem(It.IsAny<JobRequest>()), Times.Never);
            jobRequestsRepositoryMock.Verify(mock => mock.GetItem(15), Times.Once);
        }

        [Test(Description = "Updates new job request with processing state and increases attempt number after start request execution.")]
        public void StartRequestExecution_JobCommandWithoutRequestId_JobRequestToProcessingAndIncreaseAttempt()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand();
            testCommand.PrepareCommandExecutionContext();
            CommandExecutionContext.Current.StartRequestExecution();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr =>
                    jr.Status == JobRequestStatus.Processing && jr.AttemptNumber == 1)), Times.Once);
        }

        [Test(Description = "Updates existing job request with processing state and increases attempt number after start request execution.")]
        public void StartRequestExecution_JobCommandWithRequestId_JobRequestToProcessingAndIncreaseAttempt()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest
            {
                Id = 15,
                CommandName = "TestComamnd",
                AttemptNumber = 3,
                Status = JobRequestStatus.Scheduled,
            });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand
            {
                RequestId = 15,
            };
            testCommand.PrepareCommandExecutionContext();
            CommandExecutionContext.Current.StartRequestExecution();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr =>
                    jr.Status == JobRequestStatus.Processing && jr.AttemptNumber == 4)), Times.Once);
        }

        [Test(Description = "Creates job request execution item with processing state after command run.")]
        public void StartRequestExecution_CommandWithRequestId_ExecutionItemWasCreated()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest { Id = 15 });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand
            {
                RequestId = 15,
            };
            testCommand.PrepareCommandExecutionContext();
            CommandExecutionContext.Current.StartRequestExecution();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.AddItem(It.Is<JobRequestExecution>(jre => jre.Status == JobExecutionStatus.Processing && jre.StartTime != null)),
                Times.Once);
        }

        [Test(Description = "After successful request execution requests execution and request should have completed state.")]
        public void EndRequestExecution_CommandWithoutErrors_RequestExecutionAndRequestHaveCompletedState()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest
            {
                Id = 15,
                CommandName = "TestComamnd",
                AttemptNumber = 3,
                Status = JobRequestStatus.Scheduled,
            });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand
            {
                RequestId = 15,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Completed && jr.AttemptNumber == 4)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command without errors with retries differ with parent. NoNeedToCreateRepeatRequests = false. " +
                            "Job execution item to Completed. Job request item to PendingRetries. Retries created.")]
        public void EndRequestExecution_CommandWithoutErrorsWithRetriesDifferWithParentCreatedNeedToCreateRepeatRequests_CreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand(false)
            {
                NoNeedToCreateRepeatRequests = false,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.PendingRetries && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command with errors with retries differ with parent. NoNeedToCreateRepeatRequests = false. " +
                            "Job execution item to Failed. Job request item to PendingRetries. Retries created.")]
        public void EndRequestExecution_CommandWithErrorsWithRetriesDifferWithParentCreatedNeedToCreateRepeatRequests_CreatesRetriesAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand(true)
            {
                NoNeedToCreateRepeatRequests = false,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.PendingRetries && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command without errors with unique retries. NoNeedToCreateRepeatRequests = true. " +
                            "Job Execution item to Completed. Job request item to Failed. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithoutErrorsWithRetriesDifferWithParentCreatedNoNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand(false)
            {
                NoNeedToCreateRepeatRequests = true,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command with errors with unique retries. NoNeedToCreateRepeatRequests = true. " +
                            "Job Execution item to Failed. Job request item to Failed. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithErrorsWithRetriesDifferWithParentCreatedNoNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand(true)
            {
                NoNeedToCreateRepeatRequests = true,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command without errors with retries equal to parent. NoNeedToCreateRepeatRequests = false. " +
                            "Job execution item to Completed. Job request item to Scheduled. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithoutErrorsWithRetriesEqualToParentNeedToCreateRepeatRequests_DontCreatesRetriesCreateRescheduleAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand(false)
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command with errors with retries equal to parent. NoNeedToCreateRepeatRequests = false. " +
                            "Job execution item to Failed. Job request item to Scheduled. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithErrorsWithRetriesEqualToParentNeedToCreateRepeatRequests_DontCreatesRetriesCreateRescheduleAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand(true)
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command without errors with retries equal to parent. NoNeedToCreateRepeatRequests = true. " +
                            "Job execution item to Completed. Job request item to Failed. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithoutErrorsWithRetriesEqualToParentNoNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand(false)
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command with errors with retries equal to parent. NoNeedToCreateRepeatRequests = true. " +
                            "Job execution item to Failed. Job request item to Failed. Don't creates Retries.")]
        public void EndRequestExecution_CommandWithErrorsWithRetriesEqualToParentNoNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand(true)
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Command with errors without specific retries. NoNeedToCreateRepeatRequests = false. " +
                            "Job Execution item set to Failed, Job Request item set to Scheduled.")]
        public void EndRequestExecution_CommandWithErrorsWithoutSpecificRetriesNeedToCreateRepeatRequests_RescheduleRequestAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithErrorsTestCommand
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled && jr.AttemptNumber == 1)),
                Times.AtLeast(2));
        }

        [Test(Description = "Command with errors without specific retries. NoNeedToCreateRepeatRequests = true. " +
                            "Job Execution item set to Failed, Job Request item set to Failed.")]
        public void EndRequestExecution_CommandWithErrorsWithoutSpecificRetriesNoNeedToCreateRepeatRequests_DontRescheduleRequestAndStatusesFailed()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithErrorsTestCommand
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Failing command. NoNeedToCreateRepeatRequests = true. Job Execution item set to Failed, Job Request item set to Failed.")]
        public void SetAsFailedRequestExecution_FailingCommandNoNeedToCreateRepeatRequests_RequestExecutionAndRequestHaveFailedState()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new FailingTestCommand
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Failing command. NoNeedToCreateRepeatRequests = false. Job Execution item set to Failed, Job Request item set Scheduled.")]
        public void SetAsFailedRequestExecution_FailingCommandNeedToCreateRepeatRequests_RequestExecutionFailedRequestScheduledState()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new FailingTestCommand
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Aborted by timeout command. Job Execution item set to AbortedByTimeout, Job Request item set Failed.")]
        public void SetAsAbortedByTimeoutRequestExecution_AbortedByTimeoutCommand_RequestExecutionAbortedByTimeoutRequestFailedState()
        {
            // Arrange
            var commandThread = new Thread(() =>
            {
                var testCommand = new WithTimerTestCommand(true);
                testCommand.Run();
            });
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            var processManagerMock = new Mock<IProcessManager>();
            processManagerMock.Setup(manager => manager.EndCurrentProcess()).Callback(() => { commandThread.Abort(); });
            DIKernel.SetKernel(new TestExecutionContextDIModule(
                jobRequestExecutionRepositoryMock.Object,
                jobRequestsRepositoryMock.Object,
                processManagerMock.Object));

            // Act
            commandThread.Start();
            commandThread.Join();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.AbortedByTimeout && j.EndTime != null)),
                Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Close execution context when execution is in progress." +
                            "Job Execution item set to Aborted, Job Request item set Aborted.")]
        public void CloseContext_JobExecutionIsProcessing_AbortedStatuses()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand();
            testCommand.PrepareCommandExecutionContext();
            CommandExecutionContext.Current.StartRequestExecution();

            CommandExecutionContext.Current.CloseContext();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.AddItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Aborted && j.EndTime != null)),
                Times.Once);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Aborted && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }

        [Test(Description = "Close execution context when thread was aborted and execution is in progress." +
                            "Job Execution item set to Aborted, Job Request item set Aborted.")]
        public void CloseContext_JobExecutionIsProcessing_ExecutionStatusToAborted()
        {
            // Arrange
            var commandThread = new Thread(() =>
            {
                var testCommand = new CommonTestCommand();
                testCommand.Run();
            });
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            commandThread.Start();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            commandThread.Abort();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.AddItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Aborted && j.EndTime != null)),
                Times.Once);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Aborted && jr.AttemptNumber == 1)),
                Times.AtLeastOnce);
        }
    }
}
