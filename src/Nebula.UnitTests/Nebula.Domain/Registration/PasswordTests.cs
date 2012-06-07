using NUnit.Framework;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;

namespace Nebula.UnitTests.Nebula.Domain.Registration
{
    [TestFixture]
    public class PasswordTests
    {
        [Test]
        public void Equality_Should_return_false_if_other_value()
        {
            var password1 = new Password("test");
            var password2 = new Password("tst");

            Assert.AreNotEqual(password1, password2);
        }

        [Test]
        public void Equality_Should_return_true_if_same_value()
        {
            var password1 = new Password("test");
            var password2 = new Password("test");

            Assert.AreEqual(password1, password2);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Should_throw_businessException_if_invalid(string password)
        {
            Assert.Throws<BusinessException>(() => new Password(password));
        }

        [Test]
        public void Verify_returns_false_if_other_is_null()
        {
            var password = new Password("secret");
            Assert.IsFalse(password.Verify(null));
        }

        [Test]
        public void Verify_returns_false_if_passwords_are_not_equal()
        {
            var password = new Password("secret");

            Assert.IsFalse(password.Verify("secret2"));
        }

        [Test]
        public void Verify_returns_true_if_passwords_are_equal()
        {
            var password = new Password("secret");

            Assert.IsTrue(password.Verify("secret"));
        }
    }
}