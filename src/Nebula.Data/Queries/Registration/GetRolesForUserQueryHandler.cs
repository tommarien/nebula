using System.Linq;
using Nebula.Contracts.Registration;
using Nebula.Domain.Registration.Queries;

namespace Nebula.Data.Queries.Registration
{
    public class GetRolesForUserQueryHandler : IGetRolesForUserQuery
    {
        private readonly IGetAccountByUserNameQuery getAccountByUserNameQuery;

        public GetRolesForUserQueryHandler(IGetAccountByUserNameQuery getAccountByUserNameQuery)
        {
            this.getAccountByUserNameQuery = getAccountByUserNameQuery;
        }

        public Role[] Execute(string parameters)
        {
            var account = getAccountByUserNameQuery.Execute(parameters);
            if (account == null) return new Role[] {};
            return account.Roles.ToArray();
        }
    }
}