using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, OperationResult>
    {
        private readonly IQuery<string, Account> getAccountQueryHandler;

        public ChangePasswordCommandHandler(IQuery<string, Account> getAccountQueryHandler)
        {
            this.getAccountQueryHandler = getAccountQueryHandler;
        }

        public OperationResult Handle(ChangePasswordCommand command)
        {
            var account = getAccountQueryHandler.Execute(command.UserName);
            if (account == null) throw new UnknownAccountException(command.UserName);

            if (!account.Password.Equals(command.OldPassword)) return false;

            account.ChangePassword(new Password(command.NewPassword));

            return true;
        }
    }
}