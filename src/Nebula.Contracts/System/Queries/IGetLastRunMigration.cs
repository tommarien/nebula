using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.System.Queries
{
    public interface IGetLastRunMigration : IQuery<QuerySearch, long>
    {
    }
}