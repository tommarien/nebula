namespace Nebula.Contracts.Registration.Commands
{
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
    }
}