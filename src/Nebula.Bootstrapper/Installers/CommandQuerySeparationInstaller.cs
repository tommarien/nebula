using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Bootstrapper.Interceptors;
using Nebula.Data;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

namespace Nebula.Bootstrapper.Installers
{
    public class CommandQuerySeparationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Commanding
            container.Register(Component.For<ICommandHandlerFactory>().AsFactory().LifestylePerWebRequest());
            container.Register(Component.For<ICommandDispatcher>().ImplementedBy<CommandDispatcher>().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof (NHibernateConfiguration))
                                   .BasedOn<ICommandHandler>()
                                   .Configure(c =>
                                                  {
                                                      c.Interceptors<AutoTransactionInterceptor>();
                                                      c.Interceptors<TracingInterceptor>();
                                                  })
                                   .WithServiceFirstInterface().LifestyleTransient());

            //Querying
            container.Register(Component.For<IQueryHandlerFactory>().AsFactory().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof (NHibernateConfiguration))
                                   .BasedOn<IQueryHandler>()
                                   .WithServiceFirstInterface()
                                   .Configure(c =>
                                                  {
                                                      c.Interceptors<AutoTransactionInterceptor>();
                                                      c.Interceptors<TracingInterceptor>();
                                                  })
                                   .LifestyleTransient());
        }
    }
}