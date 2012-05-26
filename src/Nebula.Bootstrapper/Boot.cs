using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Nebula.Bootstrapper.Installers;

namespace Nebula.Bootstrapper
{
    public static class Boot
    {
        public static IWindsorContainer Container()
        {
            var container = new WindsorContainer();

            ExplainFacilitiesToWindsor(container);

            container
                .Install(new CommandQuerySeparationInstaller());

            return container;
        }

        public static void ExplainFacilitiesToWindsor(IWindsorContainer container)
        {
            container.AddFacility<TypedFactoryFacility>();
        }
    }
}