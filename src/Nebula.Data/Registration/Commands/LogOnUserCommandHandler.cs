using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, OperationResult>
    {
        private readonly IQueryHandler<AccountQuery, Account> accountQueryHandler;

        public LogOnUserCommandHandler(IQueryHandler<AccountQuery, Account> accountQueryHandler)
        {
            this.accountQueryHandler = accountQueryHandler;
        }

        public OperationResult Handle(LogOnUserCommand command)
        {
            var account = accountQueryHandler.Execute(new AccountQuery { UserName = command.UserName });
            return account != null && account.LogOn(command.Password);
        }
    }
}