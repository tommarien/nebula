using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Infrastructure;

namespace Nebula.Services.Installers
{
    public class DomainServicesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterCommandHandlers(GetType().Assembly);
        }
    }
}