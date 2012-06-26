﻿using System.Linq;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class GetAccountRolesQueryHandler : IGetAccountRolesQuery
    {
        private readonly IQuery<string, Account> getAccountQueryHandler;

        public GetAccountRolesQueryHandler(IQuery<string, Account> getAccountQueryHandler)
        {
            this.getAccountQueryHandler = getAccountQueryHandler;
        }

        public Role[] Execute(string parameters)
        {
            var account = getAccountQueryHandler.Execute(parameters);
            if (account == null) return new Role[] {};
            return account.Roles.ToArray();
        }
    }
}