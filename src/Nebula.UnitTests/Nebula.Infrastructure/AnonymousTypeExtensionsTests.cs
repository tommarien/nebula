using NUnit.Framework;
using Nebula.Infrastructure;

namespace Nebula.UnitTests.Nebula.Infrastructure
{
    [TestFixture]
    public class AnonymousTypeExtensionsTests
    {
        [Test]
        public void ToDictionary_Should_behave_as_expected()
        {
            object anonymous = new
                                   {
                                       Name = "Doe",
                                       FirstName = "John"
                                   };

            var dictionary = anonymous.ToDictionary();

            Assert.IsNotNull(dictionary);
            Assert.AreEqual("John", dictionary["FirstName"]);
            Assert.AreEqual("Doe", dictionary["Name"]);
        }
    }
}