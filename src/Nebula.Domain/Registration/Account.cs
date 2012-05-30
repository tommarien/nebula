﻿using Nebula.Domain.Base;

namespace Nebula.Domain.Registration
{
    public class Account : Entity<int>
    {
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
    }
}