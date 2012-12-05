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
            ISessionFactory sessionfactory = NHibernateConfiguration.Build().BuildSessionFactory();

            container.Register(Component.For<ISessionFactory>()
                                        .Instance(sessionfactory));

            container.Register(Component.For<ISession>()
                                        .UsingFactoryMethod((kernel, context) =>
                                            {
                                                var sessionFactory = kernel.Resolve<ISessionFactory>();
                                                ISession session = sessionFactory.OpenSession();
                                                kernel.ReleaseComponent(sessionFactory);

                                                // Set session properties
                                                session.FlushMode = FlushMode.Commit;

                                                return session;
                                            })
                                        .LifestylePerWebRequest());

            container.Register(Component.For<ITransactionManager>()
                                        .ImplementedBy<NHibernateTransactionManager>()
                                        .LifestyleTransient());

            container.Register(Component.For<AutoTransactionInterceptor>()
                                        .LifestyleTransient());

            container.Register(Component.For<AutomaticTransactionInterceptor>()
                                        .LifestyleTransient());
        }
    }
}