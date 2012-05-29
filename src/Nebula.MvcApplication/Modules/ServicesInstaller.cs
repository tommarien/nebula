using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.MvcApplication.Services;

namespace Nebula.MvcApplication.Modules
{
    public class ServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IFormsAuthenticationService>()
                                   .ImplementedBy<FormsAuthenticationService>());
        }
    }
}