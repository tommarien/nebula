using NUnit.Framework;
using Nebula.Data.Queries.Registration;
using Nebula.Domain.Registration;

namespace Nebula.IntegrationTests.Registration
{
    [TestFixture]
    public class GetAccountByUserNameQueryHandlerTests : AutoRollbackFixture
    {
        private GetAccountByUserNameQueryHandler handler;
        private Account user1;

        protected override void AfterSetUp()
        {
            handler = new GetAccountByUserNameQueryHandler(Session);

            // Basic user
            user1 = new Account
                        {
                            UserName = "user1",
                            Password = "secret"
                        };

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