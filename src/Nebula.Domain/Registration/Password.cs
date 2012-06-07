using System.Collections.Generic;
using Nebula.Domain.Base;
using Nebula.Infrastructure;

namespace Nebula.Domain.Registration
{
    public class Password : ValueObject
    {
        private readonly string value;

        private Password()
        {
        }

        public Password(string value)
            : this()
        {
            if (!IsValid(value))
                throw new BusinessException("Password cannot be null or empty.");

            this.value = value;
        }

        public virtual string Value
        {
            get { return value; }
        }

        protected override IEnumerable<object> EquatableValues()
        {
            yield return Value;
        }

        public static bool IsValid(string password)
        {
            return !string.IsNullOrWhiteSpace(password);
        }

        public bool Verify(string other)
        {
            if (other == null) return false;

            return Value.Equals(other);
        }
    }
}