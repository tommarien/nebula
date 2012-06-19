using Castle.Facilities.Logging;
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
                .Install(new CommandQuerySeparationInstaller())
                .Install(new NHibernateInstaller())
                .Install(new TracingInstaller());

            return container;
        }

        public static void ExplainFacilitiesToWindsor(IWindsorContainer container)
        {
            container.AddFacility<TypedFactoryFacility>();
            container.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
        }
    }
}