using NUnit.Framework;
using Nebula.Contracts.Registration.Exceptions;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.ChangePassword
{
    [TestFixture]
    public class When_unknown_user_changes_password : ChangePasswordFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            FlushAndClear();

            Command.UserName = "Unknown";
        }

        [Test]
        public void it_throws_unknown_account_exception()
        {
            Should.Throw<UnknownAccountException>(() => Act());
        }
    }
}