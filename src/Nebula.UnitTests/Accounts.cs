using Nebula.Contracts.Registration;
using Nebula.UnitTests.Builders;

namespace Nebula.UnitTests
{
    public static class Accounts
    {
        public static AccountBuilder Administrator
        {
            get
            {
                return new AccountBuilder()
                    .Named("admin")
                    .WithPassword("secret")
                    .WithRole(Role.Administrator);
            }
        }
    }
}