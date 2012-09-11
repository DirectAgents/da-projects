using System;
using System.Threading;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DirectAgents.Common.Aspects
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CountCallsAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            return new CountCallsHandler();
        }
    }

    public class CountCallsHandler : ICallHandler
    {
        private static int Initialized = 0;
        private static int Count = 0;

        public CountCallsHandler()
        {
            if (Interlocked.Exchange(ref Initialized, 1) == 0)
            {
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Logger.Write(Count, "Call Count", 1, 1, System.Diagnostics.TraceEventType.Information);
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            Interlocked.Increment(ref Count);
            return getNext()(input, getNext);
        }

        public int Order { get; set; }
    }
}
