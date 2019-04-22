using Facebook;
using System;
using System.Threading;

namespace FacebookAPI.Api
{
    public class ClientAndParams
    {
        public FacebookClient fbClient;
        public string path;
        public object parms;
        public string logMessage;

        private const int SecondsToWaitIfLimitReached = 61;
        private const int MaxRetries = 20; //??reduce??

        private string runId;

        public string GetRunId(int waitMillisecs = 0)
        {
            if (string.IsNullOrWhiteSpace(runId))
            {
                dynamic retObj = fbClient.Post(path, parms); // initial async call

                runId = retObj.report_run_id;
                Thread.Sleep(waitMillisecs);
            }
            return runId;
        }

        public string GetRunId_withRetry(Action<string> logInfo, Action<string> logError)
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
                        logInfo(String.Format("Waiting {0} seconds before trying again.", secondsToWait));
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
