﻿using System;
using System.Collections.Generic;
using Iesi.Collections.Generic;
using Nebula.Contracts.Registration;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Infrastructure;

namespace Nebula.Domain.Registration
{
    public class Account : Entity<int>
    {
        protected Iesi.Collections.Generic.ISet<Role> roles;

        protected Account()
        {
            IsActive = true;
            roles = new HashedSet<Role>();
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

        public virtual bool IsActive { get; protected set; }

        public virtual bool IsSuperAdmin
        {
            get { return "admin" == UserName; }
        }

        public virtual DateTime? LastLogOnDate { get; protected set; }

        public virtual IEnumerable<Role> Roles
        {
            get { return roles; }
        }

        public virtual void ChangePassword(Password password)
        {
            if (password == null) throw new ArgumentNullException("password");
            Password = password;
        }

        public virtual bool LogOn(string password)
        {
            GuardAgainstInactivity();

            if (!Password.Equals(password)) return false;

            LastLogOnDate = SystemContext.Clock.Now();

            return true;
        }

        public virtual void Grant(Role role)
        {
            roles.Add(role);
        }

        public virtual void Deactivate()
        {
            GuardAgainstSuperAdmin();

            IsActive = false;
        }

        private void GuardAgainstInactivity()
        {
            if (!IsActive) throw new InactiveAccountException(UserName);
        }

        private void GuardAgainstSuperAdmin()
        {
            if (IsSuperAdmin) throw new BusinessException("Invalid operation on Super Admin account!");
        }
    }
}