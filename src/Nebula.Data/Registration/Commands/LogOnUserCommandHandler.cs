using Nebula.Contracts.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, OperationResult>
    {
        private readonly IQueryHandler<string, Account> getAccountQueryHandler;

        public LogOnUserCommandHandler(IQueryHandler<string, Account> getAccountQueryHandler)
        {
            this.getAccountQueryHandler = getAccountQueryHandler;
        }

        public OperationResult Handle(LogOnUserCommand command)
        {
            var account = getAccountQueryHandler.Execute(command.UserName);
            return account != null && account.LogOn(command.Password);
        }
    }
}