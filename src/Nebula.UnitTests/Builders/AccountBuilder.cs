using System.Collections.Generic;
using Nebula.Contracts.Registration;
using Nebula.Domain.Registration;

namespace Nebula.UnitTests.Builders
{
    public class AccountBuilder
    {
        public const string DefaultUserName = "jdoe";
        public const string DefaultPassword = "secret";

        private string userName = DefaultUserName;
        private string password = DefaultPassword;
        private List<Role> roles = new List<Role>();
        private bool active = true;

        public AccountBuilder Named(string name)
        {
            userName = name;
            return this;
        }

        public AccountBuilder WithPassword(string password)
        {
            this.password = password;
            return this;
        }

        public AccountBuilder WithRole(Role role)
        {
            roles.Add(role);
            return this;
        }

        public AccountBuilder AsInactive()
        {
            active = false;
            return this;
        }

        public Account Build()
        {
            var account = new Account(userName, new Password(password));

            foreach (var role in roles)
            {
                account.Grant(role);
            }

            account.IsActive = active;
            return account;
        }
    }
}