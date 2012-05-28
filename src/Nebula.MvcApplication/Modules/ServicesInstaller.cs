using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Bootstrapper.Interceptors;

namespace Nebula.MvcApplication.Modules
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                                   .InNamespace("Nebula.MvcApplication.Services")
                                   .WithServiceDefaultInterfaces()
                                   .Configure(c => c.Interceptors<TracingInterceptor>())
                                   .LifestylePerWebRequest());
        }
    }
}