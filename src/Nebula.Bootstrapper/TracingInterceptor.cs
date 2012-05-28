using System;
using Castle.Core.Logging;
using Castle.DynamicProxy;

namespace Nebula.Bootstrapper
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
            var start = DateTime.Now;
            var method = string.Format("{0}.{1}", invocation.TargetType == null ? "" : invocation.TargetType.Name,
                                       invocation.Method.Name);
            var startMessage = string.Format("{0}: {1} started...", start, method);
            Logger.Info(startMessage);
            try
            {
                invocation.Proceed();
                var message = string.Format("{0}: {1} done. Took {2} ms.", DateTime.Now, method,
                                            (DateTime.Now - start).TotalMilliseconds);
                Logger.Info(message);
            }
            catch (Exception e)
            {
                var message = string.Format("{0}: {1} threw an exception. Took {2} ms.", DateTime.Now, method,
                                            (DateTime.Now - start).TotalMilliseconds);
                Logger.Error(message, e);
                throw;
            }
        }
    }
}