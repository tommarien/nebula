using Nebula.Infrastructure;

namespace Nebula.Domain.Registration
{
    public class UserName
    {
        private readonly string value;

        private UserName()
        {
        }

        public UserName(string value)
            : this()
        {
            if (string.IsNullOrWhiteSpace(value)) throw new BusinessException("UserName should not be null or empty.");
            this.value = value;
        }

        public string Value
        {
            get { return value; }
        }

        public bool Equals(UserName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UserName);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static bool operator ==(UserName left, UserName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserName left, UserName right)
        {
            return !Equals(left, right);
        }

        public static implicit operator string(UserName userName)
        {
            return userName.Value;
        }
    }
}