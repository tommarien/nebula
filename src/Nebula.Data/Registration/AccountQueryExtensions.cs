using System.Linq;
using Nebula.Domain.Registration;

namespace Nebula.Data.Registration
{
    public static class AccountQueryExtensions
    {
        public static IQueryable<Account> WithUserName(this IQueryable<Account> queryable, string userName)
        {
            return queryable.Where(x => x.UserName == userName);
        }
    }
}