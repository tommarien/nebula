using System.Collections.Generic;
using NUnit.Framework;
using Nebula.Infrastructure.Domain;

namespace Nebula.UnitTests.Nebula.Infrastructure.Domain
{
    [TestFixture]
    public class ValueObjectTests
    {
        public class AnotherDummyValueType : ValueObject
        {
            public int Id { get; set; }

            public string Name { get; set; }

            protected override IEnumerable<object> EquatableValues()
            {
                yield return Id;
                yield return Name;
            }
        }

        public class DummyValueType : ValueObject
        {
            public int Id { get; set; }

            public string Name { get; set; }

            protected override IEnumerable<object> EquatableValues()
            {
                yield return Id;
                yield return Name;
            }
        }

        [Test]
        public void Equality_DifferentReferences_SameValues_True()
        {
            var valueObject1 = new DummyValueType {Id = 1, Name = "Luis"};
            var valueObject2 = new DummyValueType {Id = 1, Name = "Luis"};
            Assert.That(valueObject1, Is.Not.SameAs(valueObject2));
            Assert.That(valueObject1, Is.EqualTo(valueObject2));
            Assert.That(valueObject1.Equals(valueObject2));
            Assert.That(valueObject1 == valueObject2);

            valueObject2.Name = "Billy";
            Assert.That(valueObject1 != valueObject2);
        }

        [Test]
        public void ShouldBeEqualSameReferenceWithNonNullValues()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};
            Assert.AreEqual(valType, valType);
        }

        [Test]
        public void ShouldBeEqualWithDifferentReferences()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};
            var anotherValType = new DummyValueType {Id = 1, Name = "Luis"};
            Assert.AreEqual(anotherValType, valType);
        }

        [Test]
        public void ShouldBeEqualWithSameReference()
        {
            var valType = new DummyValueType();
            Assert.AreEqual(valType, valType);
        }

        [Test]
        public void ShouldCompareAndReturnNotEqualWithOperators()
        {
            var valType = new DummyValueType {Id = 10, Name = "jose"};
            var anotherValType = new DummyValueType {Id = 20, Name = "Rui"};

            Assert.IsFalse(valType == anotherValType);
            Assert.IsTrue(valType != anotherValType);
        }

        [Test]
        public void ShouldGenerateSameHashcodeWhenEquals()
        {
            var valType = new DummyValueType {Id = 10, Name = "Miguel"};
            var anotherValType = new DummyValueType {Id = 10, Name = "Miguel"};
            Assert.AreEqual(valType.GetHashCode(), anotherValType.GetHashCode());
        }

        [Test]
        public void ShouldNotBeEqualToNull()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};
            Assert.AreNotEqual(null, valType);
            Assert.AreNotEqual(valType, null);
        }

        [Test]
        public void ShouldNotBeEqualToNullWithOperators()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};

            Assert.IsFalse(null == valType);
            Assert.IsFalse(valType == null);
            Assert.IsTrue(null != valType);
            Assert.IsTrue(valType != null);
        }

        [Test]
        public void ShouldNotBeEqualWhenComparingDifferentTypes()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};
            var anotherType = new AnotherDummyValueType {Id = 1, Name = "Luis"};
            Assert.IsFalse(anotherType.Equals(valType));
        }

        [Test]
        public void ShouldnotBeEqualWithDifferentReferencesAndDifferentIds()
        {
            var valType = new DummyValueType {Id = 1, Name = "Luis"};
            var anotherValType = new DummyValueType {Id = 10, Name = "Luis"};
            Assert.AreNotEqual(anotherValType, valType);
        }
    }
}