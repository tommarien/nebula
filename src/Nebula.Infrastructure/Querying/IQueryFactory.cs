namespace Nebula.Infrastructure.Querying
{
    public interface IQueryFactory
    {
        IQuery<TSearch, TResult> CreateQuery<TSearch, TResult>();
        void Release(IQuery query);
    }
}