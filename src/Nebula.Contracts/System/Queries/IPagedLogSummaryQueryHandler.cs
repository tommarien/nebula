using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Querying.QueryResults;

namespace Nebula.Contracts.System.Queries
{
    public interface IPagedLogSummaryQueryHandler : IQueryHandler<LogSummarySearch, PagedResult<LogSummary>>
    {
    }
}