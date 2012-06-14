﻿using Nebula.Contracts.Registration;
using Nebula.Domain.Registration.Queries;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Commanding;
using Nebula.Infrastructure.Commanding.CommandResults;

namespace Nebula.Data.Commands.Registration
{
    public class LogOnUserCommandHandler : ICommandHandler<LogOnUserCommand, OperationResult>
    {
        private readonly IGetAccountByUserNameQuery getAccountByUserNameQuery;

        public LogOnUserCommandHandler(IGetAccountByUserNameQuery getAccountByUserNameQuery)
        {
            this.getAccountByUserNameQuery = getAccountByUserNameQuery;
        }

        public OperationResult Handle(LogOnUserCommand command)
        {
            var account = getAccountByUserNameQuery.Execute(command.UserName);
            if (account == null) return false;
            if (!account.IsActive) throw new InactiveAccountException(command.UserName);

            if (!account.Password.Equals(command.Password)) return false;

            account.LastLogOnDate = SystemClock.Now;

            return true;
        }
    }
}