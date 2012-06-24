using System;
using NUnit.Framework;
using Nebula.Contracts.Registration;
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
            var account = new Account("jdoe", new Password("secret"));

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

        [Test]
        public void Ctor_Should_set_IsActive_to_true_by_default()
        {
            var account = new Account("jdoe", new Password("secret"));
            Assert.IsTrue(account.IsActive);
        }

        [Test]
        public void Grant_Should_add_expected_role()
        {
            var account = new Account("jdoe", new Password("secret"));

            account.Grant(Role.Administrator);

            CollectionAssert.Contains(account.Roles, Role.Administrator);
        }

        [Test]
        public void LogOn_Returns_true_if_everything_is_ok()
        {
            var account = new Account("jdoe", new Password("secret"));

            Assert.IsTrue(account.LogOn("secret"));
        }

        [Test]
        public void LogOn_Sets_LastLogonDate_if_goes_as_expected()
        {
            var account = new Account("jdoe", new Password("secret"));
            var januariTheFirst2011 = new DateTime(2011, 1, 1);
            SystemClock.Mock(januariTheFirst2011);

            account.LogOn("secret");

            Assert.AreEqual(januariTheFirst2011, account.LastLogOnDate);
        }

        [Test]
        public void LogOn_Should_return_false_if_password_is_different()
        {
            var account = new Account("jdoe", new Password("secret"));

            Assert.IsFalse(account.LogOn("wrong"));
        }

        [Test]
        public void LogOn_Throws_InactiveAccountException_if_account_is_inactive()
        {
            var account = new Account("jdoe", new Password("secret")) {IsActive = false};

            Assert.Throws<InactiveAccountException>(() => account.LogOn("secret"));
        }
    }
}