using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.System.Queries
{
    public interface ILogEntryDetailsQueryHandler : IQueryHandler<int, LogEntry>
    {
    }
}