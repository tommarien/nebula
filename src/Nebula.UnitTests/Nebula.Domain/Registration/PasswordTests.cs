using NUnit.Framework;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;

namespace Nebula.UnitTests.Nebula.Domain.Registration
{
    [TestFixture]
    public class PasswordTests
    {
        [Test]
        public void Equals_returns_false_if_other_is_null()
        {
            var password = new Password("secret");
            Assert.IsFalse(password.Equals((string) null));
        }

        [Test]
        public void Equals_returns_false_if_passwords_are_not_equal()
        {
            var password = new Password("secret");

            Assert.IsFalse(password.Equals("secret2"));
        }

        [Test]
        [TestCase("admin")]
        [TestCase("secret")]
        [TestCase("This is a super pass 45")]
        public void Equals_returns_true_if_passwords_are_equal(string value)
        {
            var password = new Password(value);

            Assert.IsTrue(password.Equals(value));
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public void Should_throw_businessException_if_invalid(string password)
        {
            Assert.Throws<BusinessException>(() => new Password(password));
        }
    }
}