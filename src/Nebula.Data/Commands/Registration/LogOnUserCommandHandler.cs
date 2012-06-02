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
            if (account == null) throw new UnknownAccountException(command.UserName);

            if (account.Password != command.Password) return false;

            account.LastLogOnDate = SystemClock.Now;

            return true;
        }
    }
}