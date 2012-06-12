using System;

namespace Nebula.Domain.Registration
{
    public class Account : Entity<int>
    {
        protected Account()
        {
        }

        public Account(UserName userName, Password password)
            : this()
        {
            if (userName == null) throw new ArgumentNullException("userName");
            if (password == null) throw new ArgumentNullException("password");

            UserName = userName;
            Password = password;
        }

        public virtual UserName UserName { get; protected set; }
        public virtual Password Password { get; protected set; }

        public virtual DateTime? LastLogOnDate { get; set; }

        public virtual void ChangePassword(Password password)
        {
            if (password == null) throw new ArgumentNullException("password");
            Password = password;
        }
    }
}