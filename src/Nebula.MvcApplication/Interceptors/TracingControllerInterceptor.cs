using System.Web.Mvc;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Nebula.MvcApplication.Interceptors
{
    public class TracingControllerInterceptor : IInterceptor
    {
        public TracingControllerInterceptor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.Create("Nebula.Trace");
        }

        public ILogger Logger { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            switch (invocation.Method.Name)
            {
                case "OnActionExecuting":
                    OnActionExecuting(invocation);
                    break;
                case "OnActionExecuted":
                    OnActionExecuted(invocation);
                    break;
            }

            invocation.Proceed();
        }

        private void OnActionExecuting(IInvocation invocation)
        {
            var context = invocation.Arguments[0] as ActionExecutingContext;
            if (context == null) return;
            Logger.DebugFormat("{0}.{1} -- BEGIN", invocation.TargetType.Name, context.ActionDescriptor.ActionName);
        }

        private void OnActionExecuted(IInvocation invocation)
        {
            var context = invocation.Arguments[0] as ActionExecutedContext;
            if (context == null) return;
            Logger.DebugFormat("{0}.{1} -- END", invocation.TargetType.Name, context.ActionDescriptor.ActionName);
        }
    }
}