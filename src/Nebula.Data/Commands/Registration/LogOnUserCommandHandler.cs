using Nebula.Contracts.Commands.Registration;
using Nebula.Domain.Registration.Queries;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;

namespace Nebula.Data.Commands.Registration
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, OperationResult>
    {
        private readonly IGetUserAccountByUserNameQuery getUserAccountByUserNameQuery;

        public LogOnUserCommandHandler(IGetUserAccountByUserNameQuery getUserAccountByUserNameQuery)
        {
            this.getUserAccountByUserNameQuery = getUserAccountByUserNameQuery;
        }

        public OperationResult Handle(LogOnUserCommand command)
        {
            var account = getUserAccountByUserNameQuery.Execute(command.UserName);
            return account != null && account.Password == command.Password;
        }
    }
}