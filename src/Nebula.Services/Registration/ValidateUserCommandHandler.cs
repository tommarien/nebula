using Nebula.Domain.Registration.Queries;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Shared.Registration;

namespace Nebula.Services.Registration
{
    public class ValidateUserCommandHandler : ICommandHandler<ValidateUserCommand, OperationResult>
    {
        private readonly IGetUserAccountByUserNameQuery getUserAccountByUserNameQuery;

        public ValidateUserCommandHandler(IGetUserAccountByUserNameQuery getUserAccountByUserNameQuery)
        {
            this.getUserAccountByUserNameQuery = getUserAccountByUserNameQuery;
        }

        public OperationResult Handle(ValidateUserCommand command)
        {
            var userAccount = getUserAccountByUserNameQuery.Execute(command.UserName);

            return userAccount != null && userAccount.Password == command.Password;
        }
    }
}