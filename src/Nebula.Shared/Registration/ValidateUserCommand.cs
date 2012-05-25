using Nebula.Infrastructure.Commanding;

namespace Nebula.Shared.Registration
{
    public class ValidateUserCommand : ICommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}