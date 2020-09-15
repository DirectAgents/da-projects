using System;
using System.Threading;
using Facebook;

using FacebookAPI.Exceptions;

namespace FacebookAPI.Api
{
    /// <summary>
    /// Facebook API Client Job Requests Utility.
    /// </summary>
    public class FacebookJobRequest
    {
        private readonly Action<string> logInfo;

        private readonly Action<string> logError;

        public FacebookClient fbClient;
        public string path;
        public object parms;
        public string logMessage;

        private const int UnsupportedRequestCode = 100;
        private const int PermissionsDeniedSubCode = 33;
        private const int SecondsToWaitIfLimitReached = 61;
        private const int MaxRetries = 20; //??reduce??

        private string runId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookJobRequest"/> class.
        /// </summary>
        /// <param name="logInfo"></param>
        /// <param name="logError"></param>
        public FacebookJobRequest(Action<string> logInfo, Action<string> logError)
        {
            this.logInfo = logInfo;
            this.logError = logError;
        }

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
        /// <returns> Updated run Id.</returns>
        public string ResetAndGetRunIdWithRetry()
        {
            var tryNumber = 0;
            runId = null;

            while (!TryGetRunId(out runId))
            {
                tryNumber++;
                if (tryNumber >= MaxRetries)
                {
                    throw new Exception($"Tried {tryNumber} times. Aborting GetRunId_withRetry.");
                }
            }

            return runId;
        }

        private bool TryGetRunId(out string id)
        {
            id = null;

            try
            {
                id = GetRunId();
                return true;
            }
            catch (Exception ex)
            {
                return ProcessException(ex);
            }
        }

        private bool ProcessException(Exception ex)
        {
            logError(ex.Message);
            var secondsToWait = 2;
            var apiException = ex as FacebookApiException;

            if (apiException?.ErrorCode == UnsupportedRequestCode && apiException.ErrorSubcode == PermissionsDeniedSubCode)
            {
                throw new FbPermissionDeniedException(apiException.Message);
            }

            if (ex.Message.Contains("request limit") || ex.Message.Contains("rate limit"))
            {
                secondsToWait = SecondsToWaitIfLimitReached;
            }

            logInfo($"Waiting {secondsToWait} seconds before trying again.");
            Thread.Sleep(secondsToWait * 1000);
            return false;
        }
    }
}
