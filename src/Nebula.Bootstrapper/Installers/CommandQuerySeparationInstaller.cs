using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;
using Nebula.Services.Registration;

namespace Nebula.Bootstrapper.Installers
{
    public class CommandQuerySeparationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Commanding
            container.Register(Component.For<ICommandHandlerFactory>().AsFactory());
            container.Register(Component.For<ICommandExecutor>().ImplementedBy<CommandExecutor>());

            container.Register(Classes.FromAssemblyContaining<ValidateUserCommandHandler>()
                                   .BasedOn<ICommandHandler>()
                                   .Configure(c =>
                                                  {
                                                      c.Interceptors<AutoTransactionInterceptor>();
                                                      c.Interceptors<TracingInterceptor>();
                                                  })
                                   .WithServiceFirstInterface().LifestyleTransient());

            //Querying
            container.Register(Component.For<IQueryFactory>().AsFactory());

            container.Register(Classes.FromAssemblyContaining<ValidateUserCommandHandler>()
                                   .BasedOn<IQuery>()
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