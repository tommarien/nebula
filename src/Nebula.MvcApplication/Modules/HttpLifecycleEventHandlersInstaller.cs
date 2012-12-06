using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Bootstrapper.Interceptors;
using Nebula.MvcApplication.LifecycleEventHandlers;

namespace Nebula.MvcApplication.Modules
{
    public class HttpLifecycleEventHandlersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                   .BasedOn(typeof (IHttpApplicationLifecycleEventHandler<>))
                                   .WithServiceFirstInterface()
                                   .Configure(r=>r.Interceptors<TracingInterceptor>())
                                   .LifestyleScoped());
        }
    }
}