using Nebula.Infrastructure.Commanding;
using Nebula.Shared.Registration;

namespace Nebula.Services.Registration
{
    public class ValidateUserCommandHandler : ICommandHandler<ValidateUserCommand>
    {
        public ICommandResult Handle(ValidateUserCommand command)
        {
            return new SimpleCommandResult<bool>(command.UserName == "admin" && command.Password == "letmein");
        }
    }
}