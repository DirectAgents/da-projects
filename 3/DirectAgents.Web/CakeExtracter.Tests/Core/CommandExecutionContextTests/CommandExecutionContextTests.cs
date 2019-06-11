using System;
using System.Collections.Generic;
using CakeExtracter.Common;
using CakeExtracter.Common.JobExecutionManagement.JobRequests.Repositories;
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
        [Test(Description = "Creates new job request when request id not defined in command.")]
        public void ResetContext_JobCommandWithoutRequestId_JobRequestCreated()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand();
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(mock => mock.AddItem(It.IsAny<JobRequest>()), Times.Once);
        }

        [Test(Description = "Doesn't  create job request when job request id defined in command but fetching existing one.")]
        public void ResetContext_JobCommandWithRequestId_JobRequestNotCreatedButFetchedExisting()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest { Id = 15 });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand()
            {
                RequestId = 15,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(mock => mock.AddItem(It.IsAny<JobRequest>()), Times.Never);
            jobRequestsRepositoryMock.Verify(mock => mock.GetItem(15), Times.Once);
        }

        [Test(Description = "Creates job request execution items with processing state after each reset context.")]
        public void StartRequestExecution_CommandWithRequestId_ExecutionItemWasRecreated()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            jobRequestsRepositoryMock.Setup(repository => repository.GetItem(15)).Returns(() => new JobRequest { Id = 15 });
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new CommonTestCommand()
            {
                RequestId = 15,
            };
            testCommand.Run();

            // Assert
            jobRequestExecutionRepositoryMock.Verify(mock => mock.AddItem(It.IsAny<JobRequestExecution>()), Times.Once);
        }

        [Test(Description = "Increase Attempt number for command with existing request id exactly one time.")]
        public void StartRequestExecution_ExistingRequest_AttemptNumberIncreasedOnlyOneTime()
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
            var testCommand = new CommonTestCommand()
            {
                RequestId = 15,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.AttemptNumber != 4)), Times.Never);
        }

        [Test(Description = "After successful request execution requests execution and request should have completed state.")]
        public void CompleteRequestExecution_CommandWithoutErrors_RequestExecutionAndRequestHaveCompletedState()
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
            var testCommand = new CommonTestCommand()
            {
                RequestId = 15,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Completed)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed)), Times.AtLeastOnce);
        }

        [Test(Description = "Failing command. NoNeedToCreateRepeatRequests = true. Job Execution item set to Failed, Job Request item set to Failed.")]
        public void SetAsFailedRequestExecution_FailingCommandNoNeedToCreateRepeatRequests_RequestExecutionAndRequestHaveFailedState()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new FailingTestCommand()
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed)), Times.AtLeastOnce);
        }

        [Test(Description = "Failing command. NoNeedToCreateRepeatRequests = false. Job Execution item set to Failed, Job Request item set Scheduled.")]
        public void SetAsFailedRequestExecution_FailingCommandNeedToCreateRepeatRequests_RequestExecutionFailedRequestScheduledState()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new FailingTestCommand()
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Failed)), Times.AtLeastOnce);
        }

        [Test(Description = "Command with retries differ with parent. NoNeedToCreateRepeatRequests = false. Job execution item to Completed. Job request item to PendingRetries. Retries created.")]
        public void CompleteRequestExecution_CommandWithRetriesDifferWithParentCreatedNeedToCreateRepeatRequests_CreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand()
            {
                NoNeedToCreateRepeatRequests = false,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny< IEnumerable<JobRequest>>()), Times.AtLeastOnce);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.PendingRetries)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed)), Times.AtLeastOnce);
        }

        [Test(Description = "Command with unique retries. NoNeedToCreateRepeatRequests = true. Job Execution item to Completed. Job request item to Failed. Don't creates Retries.")]
        public void CompleteRequestExecution_CommandWithRetriesCreatedDifferWithParentNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedDifferWithParentTestCommand()
            {
                NoNeedToCreateRepeatRequests = true,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(-5),
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed)), Times.AtLeastOnce);
        }

        [Test(Description = "Command with retries equal to parent.NoNeedToCreateRepeatRequests = false. Job execution item to Completed. Job request item to Failed. Don't creates Retries.")]
        public void CompleteRequestExecution_CommandWithRetriesEqualToParentNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand()
            {
                NoNeedToCreateRepeatRequests = false,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Scheduled)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed)), Times.AtLeastOnce);
        }

        [Test(Description = "Command with retries equal to parent created  NoNeedToCreateRepeatRequests = true execution sets job execution item to Completed and job request item to Failed state and don't creates Retries.")]
        public void CompleteRequestExecution_CommandWithRetriesEqualToParentNoNeedToCreateRepeatRequests_DontCreatesRetriesAndStatusesCorrect()
        {
            // Arrange
            var jobRequestExecutionRepositoryMock = new Mock<IBaseRepository<JobRequestExecution>>();
            var jobRequestsRepositoryMock = new Mock<IJobRequestsRepository>();
            DIKernel.SetKernel(new TestExecutionContextDIModule(jobRequestExecutionRepositoryMock.Object, jobRequestsRepositoryMock.Object));

            // Act
            var testCommand = new WithRetriesCreatedEqualToParentTestCommand()
            {
                NoNeedToCreateRepeatRequests = true,
            };
            testCommand.Run();

            // Assert
            jobRequestsRepositoryMock.Verify(
               mock => mock.AddItems(It.IsAny<IEnumerable<JobRequest>>()), Times.Never);
            jobRequestsRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequest>(jr => jr.Status == JobRequestStatus.Failed)), Times.AtLeastOnce);
            jobRequestExecutionRepositoryMock.Verify(
                mock => mock.UpdateItem(It.Is<JobRequestExecution>(j => j.Status == JobExecutionStatus.Completed)), Times.AtLeastOnce);
        }
    }
}
