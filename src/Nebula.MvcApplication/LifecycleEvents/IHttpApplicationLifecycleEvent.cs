using System.Web;

namespace Nebula.MvcApplication.LifecycleEvents
{
    public interface IHttpApplicationLifecycleEvent
    {
        HttpContextBase HttpContext { get; }
    }
}