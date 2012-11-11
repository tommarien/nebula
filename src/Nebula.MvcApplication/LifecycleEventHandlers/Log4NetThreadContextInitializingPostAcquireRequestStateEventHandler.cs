using System.Web.SessionState;
using Nebula.MvcApplication.Helpers;
using Nebula.MvcApplication.LifecycleEvents;
using log4net;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public class Log4NetThreadContextInitializingPostAcquireRequestStateEventHandler :
        IHttpApplicationLifecycleEventHandler<PostAcquireRequestStateEvent>
    {
        public void Handle(PostAcquireRequestStateEvent instance)
        {
            if (!(instance.HttpContext.Handler is IRequiresSessionState)) return;

            ThreadContext.Properties["sessionId"] = new SessionIdProvider().ToString();
            ThreadContext.Properties["UserName"] = new UserNameProvider().ToString();
        }
    }
}