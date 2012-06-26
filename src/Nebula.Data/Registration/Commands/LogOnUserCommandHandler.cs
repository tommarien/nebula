using Nebula.Contracts.Registration.Commands;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Commands.Registration
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, OperationResult>
    {
        private readonly IQuery<string, Account> getAccountQueryHandler;

        public LogOnUserCommandHandler(IQuery<string, Account> getAccountQueryHandler)
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