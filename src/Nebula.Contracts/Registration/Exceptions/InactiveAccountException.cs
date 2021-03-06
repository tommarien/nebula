using System;
using System.Security;

namespace Nebula.Contracts.Registration.Exceptions
{
    [Serializable]
    public class InactiveAccountException : SecurityException
    {
        public InactiveAccountException()
        {
        }

        public InactiveAccountException(string userName)
            : base(string.Format("The user with user name '{0}' is inactive.", userName))
        {
        }
    }
}