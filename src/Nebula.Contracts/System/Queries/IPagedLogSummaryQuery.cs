using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Querying.QueryResults;

namespace Nebula.Contracts.System.Queries
{
    public interface IPagedLogSummaryQuery : IQuery<LogSummarySearch, PagedResult<LogSummary>>
    {
    }
}