using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    [TestFixture]
    public class When_user_logs_on_providing_wrong_password : LogOnFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();
            FlushAndClear();

            Command.Password = "incorrect";
        }

        [Test]
        public void it_indicates_authentication_failed()
        {
            AuthenticationResult result = Act();

            result.Success.ShouldBe(false);
        }
    }
}