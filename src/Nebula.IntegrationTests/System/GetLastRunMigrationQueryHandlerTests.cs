using NUnit.Framework;
using Nebula.Data.System.Queries;
using Nebula.Infrastructure.Querying;

namespace Nebula.IntegrationTests.System
{
    [TestFixture]
    public class GetLastRunMigrationQueryHandlerTests : AutoRollbackFixture
    {
        private GetLastRunMigrationQueryHandler handler;

        protected override void AfterSetUp()
        {
            handler = new GetLastRunMigrationQueryHandler(Session);
        }

        [Test]
        public void Should_always_return_a_number()
        {
            var version = handler.Execute(QuerySearch.Empty);

            Assert.Greater(version, 0);
        }
    }
}