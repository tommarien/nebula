using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using Nebula.Bootstrapper.Interceptors;
using Nebula.Data;
using Nebula.Infrastructure.Transactions;

namespace Nebula.Bootstrapper.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ISessionFactory sessionfactory = new NHibernateConfigurationBuilder()
                .Build()
                .BuildSessionFactory();

            container.Register(Component.For<ISessionFactory>()
                                        .Instance(sessionfactory));

            container.Register(Component.For<ISession>()
                                        .UsingFactoryMethod((kernel, context) =>
                                            {
                                                var sessionFactory = kernel.Resolve<ISessionFactory>();
                                                return sessionFactory.OpenSession();
                                            })
                                        .OnCreate(session => session.FlushMode = FlushMode.Commit)
                                        .LifestylePerWebRequest());

            container.Register(Component.For<ITransactionManager>()
                                        .ImplementedBy<NHibernateTransactionManager>()
                                        .Interceptors<TracingInterceptor>()
                                        .LifestyleTransient());

            container.Register(Component.For<AutomaticTransactionInterceptor>()
                                        .LifestyleTransient());
        }
    }
}