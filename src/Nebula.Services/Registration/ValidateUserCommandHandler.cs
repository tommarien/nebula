using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;
using Nebula.Shared.Registration;

namespace Nebula.Services.Registration
{
    public class ValidateUserCommandHandler : ICommandHandler<ValidateUserCommand, OperationResult>
    {
        public OperationResult Handle(ValidateUserCommand command)
        {
            return command.UserName == "admin" && command.Password == "letmein";
        }
    }
}