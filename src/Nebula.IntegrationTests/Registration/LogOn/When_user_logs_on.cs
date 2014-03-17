using System;
using NSubstitute;
using NUnit.Framework;
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
        public void it_records_the_last_logon_date()
        {
            Act();

            var verify = Session.Get<Account>(Scenario.JohnDoe.Id);

            verify.LastLogOnDate.ShouldBe(ExpectedLogOnDate);
        }
    }
}