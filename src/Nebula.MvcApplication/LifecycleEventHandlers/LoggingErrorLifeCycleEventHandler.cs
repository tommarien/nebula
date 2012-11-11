using System;
using Castle.Core.Logging;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class LoggingErrorLifecycleEventHandler : IHttpApplicationLifecycleEventHandler<ErrorEvent>
    {
        private ILogger logger;

        public ILogger Logger
        {
            get { return logger = logger ?? NullLogger.Instance; }
            set { logger = value; }
        }

        public void Handle(ErrorEvent instance)
        {
            Exception e = instance.HttpContext.Server.GetLastError();
            logger.Error(e.Message, e);
        }
    }
}