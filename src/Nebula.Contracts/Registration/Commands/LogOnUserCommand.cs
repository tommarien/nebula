using Nebula.Infrastructure.Commanding;

namespace Nebula.Contracts.Registration.Commands
{
    public class LogOnUserCommand : ICommand
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}