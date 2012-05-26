using System.Collections.Generic;
using System.Linq;

namespace Nebula.Infrastructure.Domain
{
    public abstract class ValueObject
    {
        private const int HashMultiplier = 31;
        protected abstract IEnumerable<object> EquatableValues();

        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return EquatableValues()
                .SequenceEqual(other.EquatableValues());
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ValueObject);
        }

        public static bool operator ==(ValueObject valueObject1, ValueObject valueObject2)
        {
            if ((object) valueObject1 == null)
            {
                return (object) valueObject2 == null;
            }

            return valueObject1.Equals(valueObject2);
        }

        public static bool operator !=(ValueObject valueObject1, ValueObject valueObject2)
        {
            return !(valueObject1 == valueObject2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var signatureValues = EquatableValues().ToList();

                var hashCode = GetType().GetHashCode();

                hashCode = signatureValues
                    .Where(value => value != null)
                    .Aggregate(hashCode, (current, value) => (current*HashMultiplier) ^ value.GetHashCode());

                return signatureValues.Any() ? hashCode : base.GetHashCode();
            }
        }
    }
}