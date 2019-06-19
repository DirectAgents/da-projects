﻿using System;
using System.Threading;
using Facebook;

namespace FacebookAPI.Api
{
    /// <summary>
    /// Facebook API Client Job Requests Utility
    /// </summary>
    internal class FacebookJobRequest
    {
        public FacebookClient fbClient;
        public string path;
        public object parms;
        public string logMessage;

        private const int SecondsToWaitIfLimitReached = 61;
        private const int MaxRetries = 20; //??reduce??

        private string runId;

        /// <summary>
        /// Gets the job request identifier.
        /// </summary>
        /// <param name="waitMillisecs">The wait milliseconds.</param>
        /// <returns>Asynch job initialization.</returns>
        public string GetRunId(int waitMillisecs = 0)
        {
            if (string.IsNullOrWhiteSpace(runId))
            {
                dynamic retObj = fbClient.Post(path, parms); // initial asynch call
                runId = retObj.report_run_id;
                Thread.Sleep(waitMillisecs);
            }
            return runId;
        }

        /// <summary>
        /// Resets Run id if exists. Gets the job request identifier with retry.
        /// </summary>
        /// <param name="logInfo">The log information.</param>
        /// <param name="logError">The log error.</param>
        /// <returns> Updated run Id.</returns>
        public string ResetAndGetRunId_withRetry(Action<string> logInfo, Action<string> logError)
        {
            int tryNumber = 0;
            runId = null;
            do
            {
                try
                {
                    runId = GetRunId();
                    tryNumber = 0; // mark as call succeeded (no exception)
                }
                catch (Exception ex)
                {
                    logError(ex.Message);
                    int secondsToWait = 2;
                    if (ex.Message.Contains("request limit") || ex.Message.Contains("rate limit"))
                        secondsToWait = SecondsToWaitIfLimitReached;

                    tryNumber++;
                    if (tryNumber < MaxRetries)
                    {
                        logInfo(string.Format("Waiting {0} seconds before trying again.", secondsToWait));
                        Thread.Sleep(secondsToWait * 1000);
                    }
                }
            } while (tryNumber > 0 && tryNumber < MaxRetries);
            if (tryNumber >= MaxRetries)
                throw new Exception(String.Format("Tried {0} times. Aborting GetRunId_withRetry.", tryNumber));
            return runId;
        }
    }
}
