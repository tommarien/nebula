using Nebula.Infrastructure.Commanding;

namespace Nebula.Contracts.Registration.Commands
{
    public class ChangePasswordCommand : ICommand
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}