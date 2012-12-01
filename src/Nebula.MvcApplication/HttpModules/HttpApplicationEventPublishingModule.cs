using System;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Nebula.MvcApplication.LifecycleEventHandlers;
using Nebula.MvcApplication.LifecycleEvents;

namespace Nebula.MvcApplication.HttpModules
{
    public class HttpApplicationEventPublishingModule : IHttpModule
    {
        private IKernel kernel;

        public void Init(HttpApplication context)
        {
            var containerAccessor = context as IContainerAccessor;
            if (containerAccessor == null)
                throw new InvalidOperationException("Your HttpApplication should implement IContainerAccessor");
            kernel = containerAccessor.Container.Kernel;

            context.Error += OnError;
            context.AuthenticateRequest += OnAuthenticateRequest;
            context.PostAuthenticateRequest += OnPostAuthenticateRequest;
            context.PostAcquireRequestState += OnPostAcquireRequestState;
        }

        public void Dispose()
        {
            // NO OP
        }

        private void OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContextBase context = ExtractContext(sender);
            if (context == null) return;
            Publish(new PostAuthenticateRequestEvent {HttpContext = context});
        }

        private void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            HttpContextBase context = ExtractContext(sender);
            if (context == null) return;
            Publish(new PostAcquireRequestStateEvent {HttpContext = context});
        }

        private void OnAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContextBase context = ExtractContext(sender);
            if (context == null) return;
            Publish(new AuthenticateRequestEvent {HttpContext = context});
        }

        private void OnError(object sender, EventArgs e)
        {
            HttpContextBase context = ExtractContext(sender);
            if (context == null) return;
            Publish(new ErrorEvent {HttpContext = context});
        }

        private HttpContextBase ExtractContext(object sender)
        {
            var application = sender as HttpApplication;
            return application == null ? null : new HttpContextWrapper(application.Context);
        }

        private void Publish<T>(T @event) where T : IHttpApplicationLifecycleEvent
        {
            using (kernel.BeginScope())
            {
                IHttpApplicationLifecycleEventHandler<T>[] handlers =
                    kernel.ResolveAll<IHttpApplicationLifecycleEventHandler<T>>();
                foreach (var handler in handlers) handler.Handle(@event);
            }
        }
    }
}