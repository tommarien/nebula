using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernate;
using Nebula.Services.Modules;

namespace Nebula.Bootstrapper.Installers
{
    public class NHibernateInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var sessionfactory = NHibernateConfiguration.Build().BuildSessionFactory();

            container.Register(Component.For<ISessionFactory>()
                                   .Instance(sessionfactory));

            container.Register(Component.For<ISessionManager>()
                                   .ImplementedBy<SessionManager>()
                                   .LifestylePerWebRequest());

            container.Register(Component.For<ISession>()
                                   .UsingFactoryMethod((kernel, context) =>
                                                           {
                                                               var sessionmanager = kernel.Resolve<ISessionManager>();
                                                               var session = sessionmanager.GetSession();
                                                               kernel.ReleaseComponent(sessionmanager);
                                                               return session;
                                                           })
                                   .LifestylePerWebRequest());
        }
    }
}