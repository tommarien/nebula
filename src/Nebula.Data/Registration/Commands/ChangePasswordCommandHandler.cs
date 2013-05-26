using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Domain.Registration;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.Registration.Commands
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
    {
        private readonly IQueryHandler<AccountQuery, Account> accountQueryHandler;

        public ChangePasswordCommandHandler(IQueryHandler<AccountQuery, Account> accountQueryHandler)
        {
            this.accountQueryHandler = accountQueryHandler;
        }

        public void Handle(ChangePasswordCommand command)
        {
            Account account = accountQueryHandler.Execute(new AccountQuery {UserName = command.UserName});
            if (account == null) throw new UnknownAccountException(command.UserName);

            if (!account.Password.Equals(command.OldPassword))
                throw new BusinessException("The password provided did not match our records.");

            account.ChangePassword(new Password(command.NewPassword));
        }
    }
}