using Nebula.Infrastructure.Querying;

namespace Nebula.Domain.Registration.Queries
{
    public interface IGetAccountByUserNameQuery : IQuery<string, Account>
    {
    }
}