using NHibernate;
using Nebula.Contracts.Registration.Commands;
using Nebula.Contracts.Registration.Exceptions;
using Nebula.Domain.Registration;
using Nebula.Infrastructure.Commanding;
using NHibernate.Linq;
using System.Linq;

namespace Nebula.Data.Registration.Commands
{
    public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, bool>
    {
        private readonly ISession session;

        public ChangePasswordCommandHandler(ISession session)
        {
            this.session = session;
        }

        public bool Handle(ChangePasswordCommand command)
        {
            bool success = false;

            var account = session.Query<Account>()
                                 .WithUserName(command.UserName)
                                 .SingleOrDefault();

            if (account == null)
                throw new UnknownAccountException(command.UserName);

            if (account.Password.Equals(command.OldPassword))
            {
                account.ChangePassword(new Password(command.NewPassword));
                success = true;
            }

            return success;
        }
    }
}