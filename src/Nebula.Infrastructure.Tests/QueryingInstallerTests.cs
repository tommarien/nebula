using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Tests.Querying;

namespace Nebula.Infrastructure.Tests
{
    [TestFixture]
    public class QueryingInstallerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            windsorContainer = new WindsorContainer();
            windsorContainer.Install(new QueryingInstaller());
            windsorContainer.Register(Component.For<IQuery<int, MyPrecious>>().ImplementedBy<MyPreciousQueryById>().LifestyleTransient());
        }

        [TearDown]
        public void TearDown()
        {
            windsorContainer.Dispose();
        }

        #endregion

        private IWindsorContainer windsorContainer;

        [Test]
        public void Should_be_able_to_createquery()
        {
            var factory = windsorContainer.Resolve<IQueryFactory>();

            Assert.IsInstanceOf<MyPreciousQueryById>(factory.CreateQuery<int, MyPrecious>());
        }

        [Test]
        public void Should_be_able_to_resolve_queryexecutor()
        {
            Assert.IsInstanceOf<QueryExecutor>(windsorContainer.Resolve<IQueryExecutor>());
        }
    }
}