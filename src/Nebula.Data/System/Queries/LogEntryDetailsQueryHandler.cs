using NHibernate;
using Nebula.Contracts.System.Queries;
using Nebula.Domain.System;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.System.Queries
{
    public class LogEntryDetailsQueryHandler : IQueryHandler<int, LogEntry>
    {
        private readonly ISession session;

        public LogEntryDetailsQueryHandler(ISession session)
        {
            this.session = session;
        }

        public LogEntry Execute(int search)
        {
            var log = session.Get<Log>(search);
            return log == null
                       ? null
                       : new LogEntry
                           {
                               Id = log.Id,
                               Date = log.Date,
                               Level = log.Level,
                               Logger = log.Logger,
                               Exception = log.Exception,
                               HostName = log.HostName,
                               Message = log.Message,
                               SessionId = log.SessionId,
                               UserName = log.UserName
                           };
        }
    }
}