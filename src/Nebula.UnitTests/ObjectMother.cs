using Nebula.Domain.Registration;

namespace Nebula.UnitTests
{
    public static class ObjectMother
    {
        public static Account CreateAccount(string userName, string password)
        {
            return new Account(new UserName(userName), new Password(password));
        }
    }
}