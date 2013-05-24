using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Bootstrapper.Interceptors;
using Nebula.Data;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Transactions;

namespace Nebula.Bootstrapper.Installers
{
    public class CommandQuerySeparationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Commanding
            container.Register(Component.For<ICommandHandlerFactory>().AsFactory().LifestylePerWebRequest());
            container.Register(Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof (NHibernateConfigurationBuilder))
                                   .BasedOn<ICommandHandler>()
                                   .Configure(c =>
                                                  {
                                                      c.Interceptors<AutomaticTransactionInterceptor>();
                                                      c.Interceptors<TracingInterceptor>();
                                                  })
                                   .WithServiceFirstInterface().LifestyleTransient());

            //Querying
            container.Register(Component.For<IQueryHandlerFactory>().AsFactory().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof(NHibernateConfigurationBuilder))
                                   .BasedOn<IQueryHandler>()
                                   .WithServiceFirstInterface()
                                   .Configure(c =>
                                                  {
                                                      c.Interceptors<AutomaticTransactionInterceptor>();
                                                      c.Interceptors<TracingInterceptor>();
                                                  })
                                   .LifestyleTransient());
        }
    }
}