using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Nebula.Contracts.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Data.Registration.Commands
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, AuthenticationResult>
    {
        private readonly ISession session;

        public LogOnUserCommandHandler(ISession session)
        {
            this.session = session;
        }

        public AuthenticationResult Handle(LogOnUserCommand command)
        {
            var result = new AuthenticationResult();

            Account account = session.Query<Account>()
                                     .WithUserName(command.UserName)
                                     .SingleOrDefault();

            if (account != null)
            {
                result.UserId = account.Id;
                result.UserName = account.UserName;
                result.Roles = account.Roles.Select(x => x.ToString()).ToArray();

                result.Success = account.IsActive && account.LogOn(command.Password);
            }

            return result;
        }
    }
}