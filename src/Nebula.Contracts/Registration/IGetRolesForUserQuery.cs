using Nebula.Infrastructure.Querying;

namespace Nebula.Contracts.Registration
{
    public interface IGetRolesForUserQuery : IQuery<string, Role[]>
    {
    }
}