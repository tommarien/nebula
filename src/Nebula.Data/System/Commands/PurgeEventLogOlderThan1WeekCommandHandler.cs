using NHibernate;
using Nebula.Contracts.System.Commands;
using Nebula.Infrastructure;
using Nebula.Infrastructure.Commanding;

namespace Nebula.Data.System.Commands
{
    public class PurgeEventLogOlderThan1WeekCommandHandler : ICommandHandler<PurgeEventLogOlderThan1WeekCommand>
    {
        private readonly ISession session;

        public PurgeEventLogOlderThan1WeekCommandHandler(ISession session)
        {
            this.session = session;
        }

        public void Handle(PurgeEventLogOlderThan1WeekCommand command)
        {
            // where are not asking for something but are doing, so this is a command, even if it only executes a query !
            var query = session.CreateQuery("delete from Log l where l.Date < :threshold");
            query.SetDateTime("threshold", SystemContext.Clock.Today().AddDays(-7));
            query.ExecuteUpdate();
        }
    }
}