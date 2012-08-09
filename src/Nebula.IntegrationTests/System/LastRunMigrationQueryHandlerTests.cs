using NUnit.Framework;
using Nebula.Contracts.System.Queries;
using Nebula.Data.System.Queries;

namespace Nebula.IntegrationTests.System
{
    [TestFixture]
    public class LastRunMigrationQueryHandlerTests : AutoRollbackFixture
    {
        private LastRunMigrationQueryHandler handler;

        protected override void AfterSetUp()
        {
            handler = new LastRunMigrationQueryHandler(Session);
        }

        [Test]
        public void Should_always_return_a_number_higher_then_zero()
        {
            var version = handler.Execute(new LastRunMigrationQuery());

            Assert.Greater(version, 0);
        }
    }
}