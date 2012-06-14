using System;
using Nebula.Infrastructure;

namespace Nebula.Domain.Registration
{
    public class Account : Entity<int>
    {
        protected Account()
        {
            IsActive = true;
        }

        public Account(string userName, Password password)
            : this()
        {
            if (string.IsNullOrWhiteSpace(userName)) throw new InvalidStateException("UserName cannot be null or empty.");
            if (password == null) throw new ArgumentNullException("password");

            UserName = userName;
            Password = password;
        }

        public virtual string UserName { get; protected set; }

        public virtual Password Password { get; protected set; }

        public virtual bool IsActive { get; set; }

        public virtual DateTime? LastLogOnDate { get; set; }

        public virtual void ChangePassword(Password password)
        {
            if (password == null) throw new ArgumentNullException("password");
            Password = password;
        }
    }
}