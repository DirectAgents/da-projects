﻿using CakeExtracter.Common.JobExecutionManagement.JobExecution.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CakeExtracter.Common.JobExecutionManagement.JobExecution.Utils
{
    public static class ExecutionLoggingUtils
    {
        public static string AddAccountMessageToLogData(string sourceMessagesText, string message, int accountId)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            var accountLogData = EnsureAccountLogData(messagesObject, accountId);
            accountLogData.Messages.Add(message);
            return JsonConvert.SerializeObject(messagesObject);
        }

        public static string SetSingleAccountMessageInLogData(string sourceMessagesText, string message, int accountId)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            var accountLogData = EnsureAccountLogData(messagesObject, accountId);
            accountLogData.Messages = new List<string> { message };
            return JsonConvert.SerializeObject(messagesObject);
        }

        public static string AddCommonMessageToLogData(string sourceMessagesText, string message)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            messagesObject.CommonMessages.Add(message);
            return JsonConvert.SerializeObject(messagesObject);
        }

        public static string SetSingleCommonMessageInLogData(string sourceMessagesText, string message)
        {
            var messagesObject = GetJobExecutionLogDataFromMessageText(sourceMessagesText);
            messagesObject.CommonMessages = new List<string>() { message };
            return JsonConvert.SerializeObject(messagesObject);
        }

        private static JobExecutionLogData GetJobExecutionLogDataFromMessageText(string sourceMessagesText)
        {
            return string.IsNullOrEmpty(sourceMessagesText) ?
                new JobExecutionLogData()
                {
                    AccountsData = new List<AccountLogData>(),
                    CommonMessages = new List<string>() 
                }: JsonConvert.DeserializeObject<JobExecutionLogData>(sourceMessagesText);
        }

        private static AccountLogData EnsureAccountLogData(JobExecutionLogData logData, int accountId)
        {
            var accountLogData = logData.AccountsData.FirstOrDefault(accLogData => accLogData.AccountId == accountId);
            if (accountLogData == null)
            {
                accountLogData = new AccountLogData
                {
                    AccountId = accountId,
                    Messages = new List<string>()
                };
                logData.AccountsData.Add(accountLogData);
            }
            return accountLogData;
        }
    }
}
