using System;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.Practices.Unity;

namespace DirectAgents.Common.Aspects
{
    public class LogMethodBoundaryAttribute : HandlerAttribute
    {
        public LogMethodBoundaryAttribute()
        {
        }

        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            return container.Resolve<LogMethodBoundaryHandler>();
        }
    }

    public class LogMethodBoundaryHandler : ICallHandler 
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn methodReturn = null;
            string methodName = input.MethodBase.Name;
            string targetName = input.Target.GetType().Name;
            string messageFormat = "{0} {1}::{2}";
            Log(string.Format(messageFormat, "Before", targetName, methodName));
            methodReturn = getNext()(input, getNext);
            Log(string.Format(messageFormat, "After", targetName, methodName));
            return methodReturn;
        }

        private void Log(string message)
        {
            if (this.Logger != null)
            {
                this.Logger.Log(message);
            }
            else
            {
                Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write(message);
            }
        }

        public int Order { get; set; }

        [OptionalDependency("MethodBoundaryLogger")]
        public ILogger Logger { get; set; }
    }
}
