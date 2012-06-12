using System;

namespace Nebula.Domain.Registration
{
    public class Account : Entity<int>
    {
        public virtual string UserName { get; set; }
        public virtual Password Password { get; set; }
        public virtual DateTime? LastLogOnDate { get; set; }
    }
}