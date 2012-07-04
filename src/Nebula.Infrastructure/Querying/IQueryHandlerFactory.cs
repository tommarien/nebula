namespace Nebula.Infrastructure.Querying
{
    public interface IQueryHandlerFactory
    {
        TQuery CreateQuery<TQuery>() where TQuery : IQueryHandler;
        void Release(IQueryHandler query);
    }
}