using System.Security;

namespace Nebula.Contracts.Registration.Exceptions
{
    public class AuthenticationFailedException : SecurityException
    {
        public AuthenticationFailedException()
        {
        }

        public AuthenticationFailedException(string username)
            : base(string.Format("Authentication failed for user {0}.", username))
        {
        }
    }
}