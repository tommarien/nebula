﻿using NHibernate;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class GetAccountQueryHandler : IQuery<string, Account>
    {
        private readonly ISession session;

        public GetAccountQueryHandler(ISession session)
        {
            this.session = session;
        }

        public Account Execute(string parameters)
        {
            return session.QueryOver<Account>()
                .Where(a => a.UserName == parameters)
                .Fetch(a => a.Roles).Eager
                .SingleOrDefault();
        }
    }
}