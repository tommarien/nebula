using NHibernate;
using NHibernate.Transform;
using Nebula.Contracts.System.Queries;
using Nebula.Domain.System;
using Nebula.Infrastructure.Querying.QueryResults;

namespace Nebula.Data.System.Queries
{
    public class PagedLogSummaryQueryHandler : IPagedLogSummaryQuery
    {
        private readonly ISession session;

        public PagedLogSummaryQueryHandler(ISession session)
        {
            this.session = session;
        }

        public PagedResult<LogSummary> Execute(LogSummarySearch search)
        {
            LogSummary summaryAlias = null;

            var query = session.QueryOver<Log>()
                .SelectList(builder => builder
                                           .Select(e => e.Id).WithAlias(() => summaryAlias.Id)
                                           .Select(e => e.Date).WithAlias(() => summaryAlias.Date)
                                           .Select(e => e.Logger).WithAlias(() => summaryAlias.Logger)
                                           .Select(e => e.Level).WithAlias(() => summaryAlias.Level)
                                           .Select(e => e.Message).WithAlias(() => summaryAlias.Message))
                .TransformUsing(Transformers.AliasToBean<LogSummary>())
                .OrderBy(e => e.Id).Desc
                .Skip(search.Skip)
                .Take(search.Take);

            var count = query.ToRowCountQuery().FutureValue<int>();
            var list = query.Future<LogSummary>();
            return new PagedResult<LogSummary>(list, count.Value);
        }
    }
}