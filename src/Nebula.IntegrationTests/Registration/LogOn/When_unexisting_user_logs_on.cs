using NUnit.Framework;
using Nebula.Contracts.Registration.Exceptions;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    [TestFixture]
    public class When_unexisting_user_logs_on : LogOnFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            FlushAndClear();

            Command.UserName = "Unknown";
        }

        [Test]
        public void it_throws_authentication_failed_exception()
        {
            Should.Throw<AuthenticationFailedException>(() => Act());
        }
    }
}