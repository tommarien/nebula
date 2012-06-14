using System;
using NUnit.Framework;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;

namespace Nebula.UnitTests.Nebula.Domain.Registration
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void ChangePassword_Should_not_allow_null_password()
        {
            var account = ObjectMother.CreateAccount("userx", "secret");

            Assert.Throws<ArgumentNullException>(() => account.ChangePassword(null));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Ctor_Should_not_allow_for_username(string username)
        {
            Assert.Throws<InvalidStateException>(() => new Account(username, new Password("secret")));
        }

        [Test]
        public void Ctor_Should_not_allow_null_password()
        {
            Assert.Throws<ArgumentNullException>(() => new Account("johnDoe", null));
        }
    }
}