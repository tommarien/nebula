using Nebula.Infrastructure.Querying;

namespace Nebula.Domain.Registration.Queries
{
    public interface IGetUserAccountByUserNameQuery : IQuery<string, UserAccount>
    {
    }
}