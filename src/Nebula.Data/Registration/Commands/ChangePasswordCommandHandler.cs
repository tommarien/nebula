using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Data.Registration.Queries;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, OperationResult>
    {
        private readonly IQueryHandler<AccountQuery, Account> accountQueryHandler;

        public ChangePasswordCommandHandler(IQueryHandler<AccountQuery, Account> accountQueryHandler)
        {
            this.accountQueryHandler = accountQueryHandler;
        }

        public OperationResult Handle(ChangePasswordCommand command)
        {
            var account = accountQueryHandler.Execute(new AccountQuery { UserName = command.UserName });
            if (account == null) throw new UnknownAccountException(command.UserName);

            if (!account.Password.Equals(command.OldPassword)) return false;

            account.ChangePassword(new Password(command.NewPassword));

            return true;
        }
    }
}