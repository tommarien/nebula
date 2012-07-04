using System.Linq;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class GetAccountRolesQueryHandler : IGetAccountRolesQueryHandler
    {
        private readonly IQueryHandler<string, Account> getAccountQueryHandler;

        public GetAccountRolesQueryHandler(IQueryHandler<string, Account> getAccountQueryHandler)
        {
            this.getAccountQueryHandler = getAccountQueryHandler;
        }

        public Role[] Execute(string search)
        {
            var account = getAccountQueryHandler.Execute(search);
            if (account == null) return new Role[] {};
            return account.Roles.ToArray();
        }
    }
}