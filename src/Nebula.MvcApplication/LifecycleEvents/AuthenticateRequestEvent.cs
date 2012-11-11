using System.Web;

namespace Nebula.MvcApplication.LifecycleEvents
{
    public class AuthenticateRequestEvent : IHttpApplicationLifecycleEvent
    {
        public HttpContextBase HttpContext { get; set; }
    }
}