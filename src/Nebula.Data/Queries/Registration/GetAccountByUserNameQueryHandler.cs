using NHibernate;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;

namespace Nebula.Data.Queries.Registration
{
    public class GetAccountByUserNameQueryHandler : IGetAccountByUserNameQuery
    {
        private readonly ISession session;

        public GetAccountByUserNameQueryHandler(ISession session)
        {
            this.session = session;
        }

        public Account Execute(string parameters)
        {
            return session.QueryOver<Account>()
                .Where(ua => ua.UserName.Value == parameters)
                .SingleOrDefault();
        }
    }
}