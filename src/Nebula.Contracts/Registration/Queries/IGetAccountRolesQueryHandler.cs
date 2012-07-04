using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.Registration.Queries
{
    public interface IGetAccountRolesQueryHandler : IQueryHandler<string, Role[]>
    {
    }
}