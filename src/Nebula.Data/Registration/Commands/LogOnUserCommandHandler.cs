using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand>
    {
        private readonly IQueryHandler<AccountQuery, Account> accountQueryHandler;

        public LogOnUserCommandHandler(IQueryHandler<AccountQuery, Account> accountQueryHandler)
        {
            this.accountQueryHandler = accountQueryHandler;
        }

        public void Handle(LogOnUserCommand command)
        {
            Account account = accountQueryHandler.Execute(new AccountQuery {UserName = command.UserName});

            if (account == null || !account.LogOn(command.Password))
                throw new AuthenticationFailedException(command.UserName);
        }
    }
}