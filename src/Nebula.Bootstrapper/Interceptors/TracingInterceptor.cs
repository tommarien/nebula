using System;
using System.Diagnostics;
using Castle.Core.Logging;
using Castle.DynamicProxy;

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
            var method = string.Format("{0}.{1}", invocation.TargetType == null ? "" : invocation.TargetType.Name,
                                       invocation.Method.Name);
            var startMessage = string.Format("{0} started...", method);
            Logger.Info(startMessage);
            try
            {
                invocation.Proceed();
                stopWatch.Stop();
                var message = string.Format("{0} done. Took {1} ms.", method, stopWatch.ElapsedMilliseconds);
                Logger.Info(message);
            }
            catch (Exception e)
            {
                stopWatch.Stop();
                var message = string.Format("{0} threw an exception. Took {1} ms.", method, stopWatch.ElapsedMilliseconds);
                Logger.Error(message, e);
                throw;
            }
        }
    }
}