using System.Collections.Generic;
using System.Linq;
using CakeExtracter.Common.JobExecutionManagement.JobExecution.Models;
using Newtonsoft.Json;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils
{
    /// <summary>
    /// Job execution history logging utils.
    /// </summary>
    public static class ExecutionLoggingUtils
    {
        /// <summary>
        /// Adds the message to log data with account specification.
        /// </summary>
        /// <param name="sourceMessagesText">The source messages text.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>Result JSON message.</returns>
        public static string AddAccountMessageToLogData(string sourceMessagesText, string message, int accountId)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            var accountLogData = EnsureAccountLogData(messagesObject, accountId);
            accountLogData.Messages.Add(message);
            return JsonConvert.SerializeObject(messagesObject);
        }

        /// <summary>
        /// Sets the single  message in log data for account. Replace all messages with new one.
        /// </summary>
        /// <param name="sourceMessagesText">The source messages text.</param>
        /// <param name="message">The message.</param>
        /// <param name="accountId">The account identifier.</param>
        /// <returns>Result JSON message.</returns>
        public static string SetSingleAccountMessageInLogData(string sourceMessagesText, string message, int accountId)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            var accountLogData = EnsureAccountLogData(messagesObject, accountId);
            accountLogData.Messages = new List<string> { message };
            return JsonConvert.SerializeObject(messagesObject);
        }

        /// <summary>
        /// Adds the common message to log data.
        /// </summary>
        /// <param name="sourceMessagesText">The source messages text.</param>
        /// <param name="message">The message.</param>
        /// <returns>Result JSON message.</returns>
        public static string AddCommonMessageToLogData(string sourceMessagesText, string message)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            messagesObject.CommonMessages.Add(message);
            return JsonConvert.SerializeObject(messagesObject);
        }

        /// <summary>
        /// Sets the single common message in log data.
        /// </summary>
        /// <param name="sourceMessagesText">The source messages text.</param>
        /// <param name="message">The message.</param>
        /// <returns>Result JSON message.</returns>
        public static string SetSingleCommonMessageInLogData(string sourceMessagesText, string message)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            messagesObject.CommonMessages = new List<string>() { message };
            return JsonConvert.SerializeObject(messagesObject);
        }

        /// <summary>
        /// Gets the job execution log data from message text.
        /// </summary>
        /// <param name="sourceMessagesText">The source messages text.</param>
        /// <returns>Log data model</returns>
        public static JobExecutionLogData GetJobExecutionLogDataFromMessageText(string sourceMessagesText)
        {
            return string.IsNullOrEmpty(sourceMessagesText)
                ? new JobExecutionLogData()
                {
                    AccountsData = new List<AccountLogData>(),
                    CommonMessages = new List<string>(),
                }
                : JsonConvert.DeserializeObject<JobExecutionLogData>(sourceMessagesText);
        }

        private static AccountLogData EnsureAccountLogData(JobExecutionLogData logData, int accountId)
        {
            var accountLogData = logData.AccountsData.FirstOrDefault(accLogData => accLogData.AccountId == accountId);
            if (accountLogData == null)
            {
                accountLogData = new AccountLogData
                {
                    AccountId = accountId,
                    Messages = new List<string>(),
                };
                logData.AccountsData.Add(accountLogData);
            }
            return accountLogData;
        }
    }
}
