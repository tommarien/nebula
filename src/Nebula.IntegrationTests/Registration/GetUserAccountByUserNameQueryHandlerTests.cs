using NUnit.Framework;
using Nebula.Data.Queries.Registration;
using Nebula.Domain.Registration;

namespace Nebula.IntegrationTests.Registration
{
    [TestFixture]
    public class GetUserAccountByUserNameQueryHandlerTests : AutoRollbackFixture
    {
        private GetUserAccountByUserNameQueryHandler handler;
        private UserAccount user1;

        protected override void AfterSetUp()
        {
            handler = new GetUserAccountByUserNameQueryHandler(Session);

            // Basic user
            user1 = new UserAccount
                        {
                            UserName = "user1",
                            Password = "secret"
                        };

            Session.Save(user1);
            FlushAndClear();
        }

        [Test]
        public void Should_return_null_if_the_userAccount_does_not_exist()
        {
            var verify = handler.Execute("user2");

            Assert.IsNull(verify);
        }

        [Test]
        public void Should_return_the_expected_userAccount_if_exists()
        {
            var verify = handler.Execute(user1.UserName);

            Assert.AreEqual(user1.Id, verify.Id);
        }
    }
}