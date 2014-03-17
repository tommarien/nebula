using NUnit.Framework;
using Nebula.Domain.Registration;
using Nebula.IntegrationTests.Registration.ChangePassword;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.ChangePasswordp
{
    [TestFixture]
    public class When_user_changes_password : ChangePasswordFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            FlushAndClear();
        }

        [Test]
        public void it_indicates_the_operation_succeeded()
        {
            Act().ShouldBe(true);
        }

        [Test]
        public void it_changed_the_password()
        {
            Act();

            var verify = Session.Get<Account>(Scenario.JohnDoe.Id);

            verify.Password.Equals(Command.NewPassword).ShouldBe(true);
        }
    }
}