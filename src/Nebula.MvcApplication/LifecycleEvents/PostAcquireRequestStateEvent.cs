using System.Web;

namespace Nebula.MvcApplication.LifecycleEvents
{
    public class PostAcquireRequestStateEvent : IHttpApplicationLifecycleEvent
    {
        public HttpContextBase HttpContext { get; set; }
    }
}