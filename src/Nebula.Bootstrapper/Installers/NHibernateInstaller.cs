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
            var sessionfactory = NHibernateConfiguration.Build().BuildSessionFactory();

            container.Register(Component.For<ISessionFactory>()
                                   .Instance(sessionfactory));

            container.Register(Component.For<ISession>()
                                   .UsingFactoryMethod((kernel, context) =>
                                                           {
                                                               var sessionFactory = kernel.Resolve<ISessionFactory>();
                                                               var session = sessionFactory.OpenSession();
                                                               kernel.ReleaseComponent(sessionFactory);

                                                               // Set session properties
                                                               session.FlushMode = FlushMode.Commit;

                                                               return session;
                                                           })
                                   .LifestylePerWebRequest());

            container.Register(Component.For<AutoTransactionInterceptor>()
                                   .LifestyleTransient());
        }
    }
}