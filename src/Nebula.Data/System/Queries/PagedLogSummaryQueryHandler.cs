using NHibernate;
using NHibernate.Transform;
using Nebula.Contracts.System.Queries;
using Nebula.Domain.System;
using Nebula.Infrastructure.Querying;
using Nebula.Infrastructure.Querying.QueryResults;

namespace Nebula.Data.System.Queries
{
    public class PagedLogSummaryQueryHandler : IQueryHandler<PagedLogSummaryQuery, PagedResult<LogSummary>>
    {
        private readonly ISession session;

        public PagedLogSummaryQueryHandler(ISession session)
        {
            this.session = session;
        }

        public PagedResult<LogSummary> Execute(PagedLogSummaryQuery query)
        {
            LogSummary summaryAlias = null;

            var queryOver = session.QueryOver<Log>()
                .SelectList(builder => builder
                                           .Select(e => e.Id).WithAlias(() => summaryAlias.Id)
                                           .Select(e => e.Date).WithAlias(() => summaryAlias.Date)
                                           .Select(e => e.Logger).WithAlias(() => summaryAlias.Logger)
                                           .Select(e => e.Level).WithAlias(() => summaryAlias.Level)
                                           .Select(e => e.Message).WithAlias(() => summaryAlias.Message))
                .TransformUsing(Transformers.AliasToBean<LogSummary>())
                .OrderBy(e => e.Id).Desc
                .Skip(query.Skip)
                .Take(query.Take);

            var count = queryOver.ToRowCountQuery().FutureValue<int>();
            var list = queryOver.Future<LogSummary>();
            return new PagedResult<LogSummary>(list, count.Value);
        }
    }
}