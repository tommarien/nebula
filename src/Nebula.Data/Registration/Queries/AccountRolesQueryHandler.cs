using System.Linq;
using Nebula.Contracts.Registration;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class AccountRolesQueryHandler : IQueryHandler<AccountQuery, Role[]>
    {
        private readonly IQueryHandler<AccountQuery, Account> accountQueryHandler;

        public AccountRolesQueryHandler(IQueryHandler<AccountQuery, Account> accountQueryHandler)
        {
            this.accountQueryHandler = accountQueryHandler;
        }

        public Role[] Execute(AccountQuery query)
        {
            var account = accountQueryHandler.Execute(query);
            if (account == null) return new Role[] { };
            return account.Roles.ToArray();
        }
    }
}