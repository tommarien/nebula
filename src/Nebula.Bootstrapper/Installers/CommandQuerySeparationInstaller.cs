﻿using Castle.Facilities.TypedFactory;
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
            container.Register(Component.For<ICommandHandlerFactory>().AsFactory().LifestylePerWebRequest());
            container.Register(Component.For<IMediator>().ImplementedBy<CommandMediator>().LifestylePerWebRequest());
            container.Register(Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof (NHibernateConfigurationBuilder))
                                      .BasedOn(typeof (ICommandHandler<>), typeof (ICommandHandler<,>))
                                      .WithService.Base()
                                      .Configure(c =>
                                          {
                                              c.Interceptors<AutoTxInterceptor>();
                                              c.Interceptors<TracingInterceptor>();
                                          })
                                      .LifestyleTransient());

            //Querying
            container.Register(Component.For<IQueryHandlerFactory>().AsFactory().LifestylePerWebRequest());

            container.Register(Classes.FromAssemblyContaining(typeof (NHibernateConfigurationBuilder))
                                      .BasedOn(typeof (IQueryHandler<,>))
                                      .WithService.Base()
                                      .Configure(c =>
                                          {
                                              c.Interceptors<AutoTxInterceptor>();
                                              c.Interceptors<TracingInterceptor>();
                                          })
                                      .LifestyleTransient());
        }
    }
}