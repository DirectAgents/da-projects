using System;
using System.Collections.Generic;
using System.Configuration;
using DBM;
using DBM.Entities;

namespace DBM
{
    public class DBMUtility
    {
        private readonly string GoogleAPI_ServiceEmail = ConfigurationManager.AppSettings["GoogleAPI_ServiceEmail"];
        private readonly string GoogleAPI_Certificate = ConfigurationManager.AppSettings["GoogleAPI_Certificate"];

        // --- Logging ---
        private Action<string> _LogInfo;
        private Action<string> _LogError;

        private void LogInfo(string message)
        {
            if (_LogInfo == null)
                Console.WriteLine(message);
            else
                _LogInfo("[AdRollUtility] " + message);
        }

        private void LogError(string message)
        {
            if (_LogError == null)
                Console.WriteLine(message);
            else
                _LogError("[AdRollUtility] " + message);
        }

        // --- Constructors ---
        public DBMUtility()
        {
        }
        public DBMUtility(Action<string> logInfo, Action<string> logError)
        {
            _LogInfo = logInfo;
            _LogError = logError;
        }

        public Creative GetCreative(string adEid)
        {
            throw new NotImplementedException();
        }
    }
}
