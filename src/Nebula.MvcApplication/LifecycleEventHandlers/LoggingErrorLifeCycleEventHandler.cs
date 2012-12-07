using System;
using Castle.Core.Logging;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class LoggingErrorLifecycleEventHandler : IHttpApplicationLifecycleEventHandler<ErrorEvent>
    {
        private readonly ILogger logger;

        public LoggingErrorLifecycleEventHandler(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.Create("Nebula.Application");
        }

        public void Handle(ErrorEvent instance)
        {
            Exception e = instance.HttpContext.Server.GetLastError();
            logger.Error(e.Message, e);
        }
    }
}