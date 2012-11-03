using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.MvcApplication.Interceptors;

namespace Nebula.MvcApplication.Modules
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<TracingControllerInterceptor>().LifestyleTransient());

            container.Register(Classes.FromThisAssembly()
                                   .BasedOn<IController>()
                                   .Configure(c => c.Interceptors<TracingControllerInterceptor>())
                                   .LifestyleTransient());
        }
    }
}