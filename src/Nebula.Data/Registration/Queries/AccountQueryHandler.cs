using NHibernate;
using Nebula.Contracts.Registration;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class AccountQueryHandler : IQueryHandler<AccountQuery, Account>
    {
        private readonly ISession session;

        public AccountQueryHandler(ISession session)
        {
            this.session = session;
        }

        public Account Execute(AccountQuery query)
        {
            return session.QueryOver<Account>()
                .Where(a => a.UserName == query.UserName)
                .Fetch(a => a.Roles).Eager
                .SingleOrDefault();
        }
    }
}