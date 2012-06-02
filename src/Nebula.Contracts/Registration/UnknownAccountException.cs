using System.Security;

namespace Nebula.Contracts.Registration
{
    public class UnknownAccountException : SecurityException
    {
        public UnknownAccountException()
        {
        }

        public UnknownAccountException(string userName)
            : base(string.Format("No known user with user name '{0}'", userName))
        {
        }
    }
}