namespace Nebula.Infrastructure.Querying
{
    public interface IQueryExecutor
    {
        TResult Execute<TSearch, TResult>(TSearch search);
    }
}