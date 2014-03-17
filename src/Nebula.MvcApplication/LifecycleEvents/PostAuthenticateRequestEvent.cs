using System.Web;

namespace Nebula.MvcApplication.LifecycleEvents
{
    public class PostAuthenticateRequestEvent : IHttpApplicationLifecycleEvent
    {
        public HttpContextBase HttpContext { get; set; }
    }
}