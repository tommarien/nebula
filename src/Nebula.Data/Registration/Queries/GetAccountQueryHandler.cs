using NHibernate;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Queries
{
    public class GetAccountQueryHandler : IQueryHandler<string, Account>
    {
        private readonly ISession session;

        public GetAccountQueryHandler(ISession session)
        {
            this.session = session;
        }

        public Account Execute(string search)
        {
            return session.QueryOver<Account>()
                .Where(a => a.UserName == search)
                .Fetch(a => a.Roles).Eager
                .SingleOrDefault();
        }
    }
}