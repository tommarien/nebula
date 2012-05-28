using NHibernate;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;

namespace Nebula.Services.Registration
{
    public class GetUserAccountByUserNameQueryHandler : IGetUserAccountByUserNameQuery
    {
        private readonly ISession session;

        public GetUserAccountByUserNameQueryHandler(ISession session)
        {
            this.session = session;
        }

        public UserAccount Execute(string parameters)
        {
            return session.QueryOver<UserAccount>()
                .Where(ua => ua.UserName == parameters)
                .SingleOrDefault();
        }
    }
}