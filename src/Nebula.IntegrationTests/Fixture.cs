using NHibernate;
using NHibernate.Cfg;
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

        protected static Configuration Configuration { get; private set; }

        protected static ISessionFactory SessionFactory { get; private set; }

        protected IStatistics Statistics
        {
            get { return SessionFactory.Statistics; }
        }

        private static void SetSessionFactory()
        {
            Configuration = NHibernateConfiguration.Build();
            SessionFactory = Configuration.BuildSessionFactory();
        }

        protected ISession CreateSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}