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
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            Logger.DebugFormat("{0} -- BEGIN", invocation.TypeQualifiedMethodName());
            try
            {
                invocation.Proceed();
                stopWatch.Stop();
                Logger.DebugFormat("{0} -- END ({1} ms)", invocation.TypeQualifiedMethodName(), stopWatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                Logger.ErrorFormat(e, "{0} -- END ({1} ms) -- Threw an exception", invocation.TypeQualifiedMethodName(), stopWatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}