using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.Registration.Queries
{
    public interface IGetAccountRolesQuery : IQuery<string, Role[]>
    {
    }
}