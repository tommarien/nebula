using System;
using System.Diagnostics;
using Castle.Core.Logging;
using Castle.DynamicProxy;
using Nebula.Infrastructure;

namespace Nebula.Bootstrapper.Interceptors
{
    public class TracingInterceptor : IInterceptor
    {
        public TracingInterceptor(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.Create("Nebula.Trace");
        }

        public ILogger Logger { get; private set; }

        public void Intercept(IInvocation invocation)
        {
            string typeQualifiedMethodName = invocation.TypeQualifiedMethodName();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Logger.DebugFormat("{0} -- BEGIN", typeQualifiedMethodName);
            try
            {
                invocation.Proceed();
                stopWatch.Stop();
                Logger.DebugFormat("{0} -- END ({1} ms)", typeQualifiedMethodName, stopWatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                Logger.ErrorFormat(e, "{0} -- END ({1} ms) -- Threw an exception", typeQualifiedMethodName, stopWatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}