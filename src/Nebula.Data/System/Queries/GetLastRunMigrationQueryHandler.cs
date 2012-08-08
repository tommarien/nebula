using NHibernate;
using NHibernate.Criterion;
using Nebula.Domain.System;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.System.Queries
{
    public class GetLastRunMigrationQueryHandler : IQueryHandler<Query, long>
    {
        private readonly ISession session;

        public GetLastRunMigrationQueryHandler(ISession session)
        {
            this.session = session;
        }

        public long Execute(Query search)
        {
            return session.QueryOver<SchemaVersionInfo>()
                .Select(Projections.Max<SchemaVersionInfo>(v => v.Version))
                .SingleOrDefault<long>();
        }
    }
}