using NUnit.Framework;
using Nebula.Data.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;
using Nebula.UnitTests.Builders;

namespace Nebula.IntegrationTests.Registration
{
    [TestFixture]
    public class GetAccountQueryHandlerTests : AutoRollbackFixture
    {
        private IQuery<string, Account> handler;
        private Account user1;

        protected override void AfterSetUp()
        {
            handler = new GetAccountQueryHandler(Session);

            // Basic user
            user1 = new AccountBuilder().Build();

            Session.Save(user1);
            FlushAndClear();
        }

        [Test]
        public void Should_return_null_if_the_account_does_not_exist()
        {
            var verify = handler.Execute("user2");

            Assert.IsNull(verify);
        }

        [Test]
        public void Should_return_the_expected_account_if_exists()
        {
            var verify = handler.Execute(user1.UserName);

            Assert.AreEqual(user1.Id, verify.Id);
        }
    }
}