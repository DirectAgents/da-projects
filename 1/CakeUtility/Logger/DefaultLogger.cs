using System;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DirectAgents.Common
{
    public class DefaultLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
