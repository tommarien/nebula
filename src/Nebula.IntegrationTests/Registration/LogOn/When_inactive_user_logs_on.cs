﻿using NUnit.Framework;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Shouldly;

namespace Nebula.IntegrationTests.Registration.LogOn
{
    [TestFixture]
    public class When_inactive_user_logs_on : LogOnFixture
    {
        protected override void AfterSetUp()
        {
            base.AfterSetUp();

            Scenario.JohnDoe.Deactivate();

            FlushAndClear();
        }

        [Test]
        public void it_indicates_authentication_failed()
        {
            AuthenticationResult result = Act();

            result.Success.ShouldBe(false);
        }
    }
}