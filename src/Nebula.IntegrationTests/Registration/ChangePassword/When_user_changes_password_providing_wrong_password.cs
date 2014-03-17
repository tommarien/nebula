using NUnit.Framework;
using Nebula.Domain.Registration;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.ChangePassword
{
    [TestFixture]
    public class When_user_changes_password_providing_wrong_password : ChangePasswordFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            FlushAndClear();

            Command.OldPassword = "invalid";
        }

        [Test]
        public void it_indicates_the_operation_failed()
        {
            Act().ShouldBe(false);
        }

        [Test]
        public void it_retained_the_password()
        {
            Act();

            var verify = Session.Get<Account>(Scenario.JohnDoe.Id);

            verify.Password.Equals("superDuper12").ShouldBe(true);
        }
    }
}