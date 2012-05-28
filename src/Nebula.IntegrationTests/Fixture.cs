using NHibernate;
using NHibernate.Stat;
using Nebula.Data;

namespace Nebula.IntegrationTests
{
    public abstract class Fixture
    {
        static Fixture()
        {
            SetSessionFactory();
        }

        protected static ISessionFactory SessionFactory { get; private set; }

        protected IStatistics Statistics
        {
            get { return SessionFactory.Statistics; }
        }

        private static void SetSessionFactory()
        {
            var configuration = NHibernateConfiguration.Build();
            SessionFactory = configuration.BuildSessionFactory();
        }

        protected ISession CreateSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}