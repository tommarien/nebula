using System;
using NSubstitute;
using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    [TestFixture]
    public class When_user_logs_on : LogOnFixture
    {
        private readonly DateTime ExpectedLogOnDate = new DateTime(2014, 1, 1, 11, 00, 59);

        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            FlushAndClear();

            SystemContext.Clock = Substitute.For<ISystemClock>();
            SystemContext.Clock.Now().Returns(ExpectedLogOnDate);
        }

        [Test]
        public void it_indicates_the_authentication_succeeded()
        {
            AuthenticationResult result = Act();

            result.Success.ShouldBe(true);
        }

        [Test]
        public void it_records_the_last_logon_date()
        {
            Act();

            var verify = Session.Get<Account>(Scenario.JohnDoe.Id);

            verify.LastLogOnDate.ShouldBe(ExpectedLogOnDate);
        }

        [Test]
        public void it_returns_the_roles()
        {
            AuthenticationResult result = Act();

            result.Roles.ShouldBe(new[] {"Administrator"});
        }

        [Test]
        public void it_returns_the_user_id()
        {
            AuthenticationResult result = Act();

            result.UserId.ShouldBe(Scenario.JohnDoe.Id);
        }


        [Test]
        public void it_returns_the_user_name()
        {
            AuthenticationResult result = Act();

            result.UserName.ShouldBe(Scenario.JohnDoe.UserName);
        }
    }
}