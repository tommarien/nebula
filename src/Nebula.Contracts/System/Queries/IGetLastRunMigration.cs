using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.System.Queries
{
    public interface IGetLastRunMigration : IQueryHandler<QuerySearch, long>
    {
    }
}