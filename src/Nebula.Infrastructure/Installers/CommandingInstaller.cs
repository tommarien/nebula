using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Infrastructure.Installers
{
    public class CommandingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacilityIfNeeded<TypedFactoryFacility>();

            container.Register(Component.For<ICommandHandlerFactory>().AsFactory());
            container.Register(Component.For<ICommandDispatcher>().ImplementedBy<DefaultCommandDispatcher>());
        }
    }
}