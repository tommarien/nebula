using Nebula.Domain.Base;

namespace Nebula.Domain.Registration
{
    public class UserAccount : Entity<int>
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}