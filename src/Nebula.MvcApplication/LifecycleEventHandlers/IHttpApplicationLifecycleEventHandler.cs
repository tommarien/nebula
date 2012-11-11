using Nebula.Infrastructure;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.LifecycleEventHandlers
{
    public interface IHttpApplicationLifecycleEventHandler<TEvent> : IHandle<TEvent>
        where TEvent : IHttpApplicationLifecycleEvent
    {
    }
}