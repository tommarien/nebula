using System.Web;

namespace Nebula.MvcApplication.LifecycleEvents
{
    public class ErrorEvent : IHttpApplicationLifecycleEvent
    {
        public HttpContextBase HttpContext { get; set; }
    }
}