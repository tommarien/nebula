using NHibernate;
using NHibernate.Criterion;
using Nebula.Contracts.System.Queries;
using Nebula.Domain.System;
using Nebula.Infrastructure.Querying;

namespace Nebula.Data.System.Queries
{
    public class LastRunMigrationQueryHandler : IQueryHandler<LastRunMigrationQuery, long>
    {
        private readonly ISession session;

        public LastRunMigrationQueryHandler(ISession session)
        {
            this.session = session;
        }

        public long Execute(LastRunMigrationQuery query)
        {
            return session.QueryOver<SchemaVersionInfo>()
                .Select(Projections.Max<SchemaVersionInfo>(v => v.Version))
                .SingleOrDefault<long>();
        }
    }
}