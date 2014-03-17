using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Data.Registration.Commands
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand>
    {
        private readonly ISession session;

        public LogOnUserCommandHandler(ISession session)
        {
            this.session = session;
        }

        public void Handle(LogOnUserCommand command)
        {
            Account account = session.Query<Account>()
                                     .WithUserName(command.UserName)
                                     .SingleOrDefault();

            if (account == null || !account.LogOn(command.Password))
                throw new AuthenticationFailedException(command.UserName);
        }
    }
}