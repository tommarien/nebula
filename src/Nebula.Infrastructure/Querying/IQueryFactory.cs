namespace Nebula.Infrastructure.Querying
{
    public interface IQueryFactory
    {
        TQuery CreateQuery<TQuery>() where TQuery : IQuery;
        void Release(IQuery query);
    }
}