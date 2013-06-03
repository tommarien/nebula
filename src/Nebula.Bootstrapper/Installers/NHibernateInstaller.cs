using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using Nebula.Bootstrapper.Interceptors;
using Nebula.Data;

namespace Nebula.Bootstrapper.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ISessionFactory>()
                                        .UsingFactoryMethod(
                                            () => new NHibernateConfigurationBuilder().Build().BuildSessionFactory()));

            container.Register(Component.For<ISession>()
                                        .UsingFactory<ISessionFactory, ISession>(factory => factory.OpenSession())
                                        .LifestylePerWebRequest());

            container.Register(Component.For<AutoTxInterceptor>()
                                        .LifestyleTransient());
        }
    }
}