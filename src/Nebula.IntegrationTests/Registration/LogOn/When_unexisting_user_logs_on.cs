using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
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
        public void it_indicates_authentication_failed()
        {
            AuthenticationResult result = Act();

            result.Success.ShouldBe(false);
        }
    }
}