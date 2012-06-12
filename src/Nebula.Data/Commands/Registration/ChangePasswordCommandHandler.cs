using Nebula.Contracts.Registration;
using Nebula.Domain.Registration;
using Nebula.Domain.Registration.Queries;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;

namespace Nebula.Data.Commands.Registration
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, OperationResult>
    {
        private readonly IGetAccountByUserNameQuery accountByUserNameQuery;

        public ChangePasswordCommandHandler(IGetAccountByUserNameQuery accountByUserNameQuery)
        {
            this.accountByUserNameQuery = accountByUserNameQuery;
        }

        public OperationResult Handle(ChangePasswordCommand command)
        {
            var account = accountByUserNameQuery.Execute(command.UserName);
            if (account == null) throw new UnknownAccountException(command.UserName);

            if (!account.Password.Equals(command.OldPassword)) return false;

            account.ChangePassword(new Password(command.NewPassword));

            return true;
        }
    }
}